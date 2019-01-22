#region Dapplo 2016-2019 - GNU Lesser General Public License

//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2016-2019 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log
// 
//  Dapplo.Log is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Log.LogFile.Impl;

namespace Dapplo.Log.LogFile
{
    /// <summary>
    ///     This implements a logger which writes to a log file in the background
    ///     Filename and directory are configurable, also rolling filename and compression can be activated
    /// </summary>
    public class FileLogger : AbstractLogger<IFileLoggerConfiguration>, IDisposable
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        ///     This take care of specifying a logger, to prevent the internal LogSource to write to it's own file.
        ///     The code should still work if the mapping was already available before (which only works if the registration is
        ///     done by name)
        /// </summary>
        static FileLogger()
        {
            // Make sure this class doesn't log into it's own file
            Log.LogTo(new NullLogger());
        }

        private readonly ConcurrentQueue<Tuple<LogInfo, string, object[]>> _logItems = new ConcurrentQueue<Tuple<LogInfo, string, object[]>>();

        private readonly CancellationTokenSource _backgroundCancellationTokenSource = new CancellationTokenSource();
        private string _previousFilePath;
        private Dictionary<string, object> _previousVariables;
        private readonly Task<bool> _backgroundTask;
        private readonly List<Task> _archiveTaskList = new List<Task>();

        /// <summary>
        ///     default Constructor which starts the background task
        /// </summary>
        public FileLogger()
        {
            // Start the processing in the background
            _backgroundTask = Task.Run(async () => await BackgroundAsync(_backgroundCancellationTokenSource.Token).ConfigureAwait(false));

            LoggerConfiguration = new SimpleFileLoggerConfiguration();
            SetProcessName(LoggerConfiguration);
        }

        private static void SetProcessName(IFileLoggerConfiguration fileLoggerConfiguration)
        {
            if (fileLoggerConfiguration == null)
            {
                return;
            }
            using (var process = Process.GetCurrentProcess())
            {
                fileLoggerConfiguration.ProcessName = Path.GetFileNameWithoutExtension(process.MainModule.FileName);
            }
        }

        /// <summary>
        ///     Configure this logger
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        public override void Configure(IFileLoggerConfiguration loggerConfiguration)
        {
            // Copy all values from the ILoggerConfiguration
            base.Configure(loggerConfiguration);

            // Copy all values from the IFileLoggerConfiguration
            if (string.IsNullOrEmpty(loggerConfiguration.ProcessName))
            {
#if !_PCL_
                SetProcessName(loggerConfiguration);
#else
				throw new ArgumentNullException(nameof(IFileLoggerConfiguration.ProcessName));
#endif
            }
        }

        /// <summary>
        ///     Enqueue the current information so it can be written to the file, formatting is done later.. (improves performance for the UI)
        ///     Preferably do NOT pass huge objects which need to be garbage collected
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string</param>
        /// <param name="logParameters">params</param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            if (_backgroundCancellationTokenSource.IsCancellationRequested)
            {
                throw new OperationCanceledException("FileLogger has been disposed!", _backgroundCancellationTokenSource.Token);
            }
            if (LoggerConfiguration.PreFormat)
            {
                _logItems.Enqueue(new Tuple<LogInfo, string, object[]>(logInfo, Format(logInfo, messageTemplate, logParameters), null));
            }
            else
            {
                _logItems.Enqueue(new Tuple<LogInfo, string, object[]>(logInfo, messageTemplate, logParameters));
            }
        }

        #region FormatWith

        /// <summary>
        ///     A simple FormatWith
        /// </summary>
        /// <param name="source"></param>
        /// <param name="variables"></param>
        /// <returns>Formatted string</returns>
        private static string SimpleFormatWith(string source, Dictionary<string, object> variables)
        {
            var stringToFormat = source;
            var arguments = new List<object>();
            foreach (var key in variables.Keys)
            {
                var index = arguments.Count;
                if (!stringToFormat.Contains(key))
                {
                    continue;
                }

                // Replace the key with the index, so we can use normal formatting
                stringToFormat = stringToFormat.Replace(key, index.ToString());
                // Add the argument to the index, so the normal formatting can find this
                arguments.Add(variables[key]);
            }

            return string.Format(stringToFormat, arguments.ToArray());
        }

        #endregion

        #region Background processing

        /// <summary>
        ///     This is the implementation of the background task
        /// </summary>
        /// <returns>Task</returns>
        private async Task<bool> BackgroundAsync(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // ReSharper disable once MethodSupportsCancellation
                await ProcessLinesAsync().ConfigureAwait(false);
                // Wait a while before we process the next items
                await Task.Delay(LoggerConfiguration.WriteInterval, cancellationToken).ConfigureAwait(false);
            }
            return true;
        }


        /// <summary>
        ///     Process the lines
        /// </summary>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task</returns>
        private async Task ProcessLinesAsync(CancellationToken cancellationToken = default)
        {
            if (_logItems.IsEmpty)
            {
                return;
            }
            var variables = new Dictionary<string, object>
            {
                {"ProcessName", LoggerConfiguration.ProcessName},
                {"Timestamp", DateTimeOffset.Now},
                {"Extension", LoggerConfiguration.Extension}
            };
            var expandedFilename = Environment.ExpandEnvironmentVariables(LoggerConfiguration.FilenamePattern);
            var directory = SimpleFormatWith(Environment.ExpandEnvironmentVariables(LoggerConfiguration.DirectoryPath), variables);

            // Filename of the file to write to.
            var filename = SimpleFormatWith(expandedFilename, variables);
            var filepath = Path.Combine(directory, filename);

            var isFilepathChange = !filepath.Equals(_previousFilePath);

            if (_previousFilePath != null && isFilepathChange)
            {
                // Archive!!
                try
                {
                    // Create the archive task
                    var archiveTask = ArchiveFileAsync(_previousFilePath, _previousVariables, cancellationToken);
                    // Add it to the list of current archive tasks
                    lock (_archiveTaskList)
                    {
                        _archiveTaskList.Add(archiveTask);
                    }
                    // Create a continue, so the task can be removed from the list, we do not use a CancellationToken or use the variable.
                    // ReSharper disable once MethodSupportsCancellation
                    // ReSharper disable once UnusedVariable
                    var ignoreThis = archiveTask.ContinueWith(async x =>
                    {
                        await x.ConfigureAwait(false);
                        lock (_archiveTaskList)
                        {
                            _archiveTaskList.Remove(x);
                        }
                    });
                }
                catch (Exception ex)
                {
                    Log.Error().WriteLine(ex, "Error archiving {0}", _previousFilePath);
                }
            }

            // check if the last file we wrote to is not the same.
            if (_previousFilePath is null || isFilepathChange)
            {
                // Store the current variables, so we can use them next time for the archiving
                _previousFilePath = filepath;
                _previousVariables = variables;
            }

            using (var streamWriter = new StreamWriter(new MemoryStream(), Encoding.UTF8))
            {
                streamWriter.AutoFlush = true;
                // Loop as long as there are items available
                while (_logItems.TryDequeue(out var logItem))
                {
                    try
                    {
                        var line = LoggerConfiguration.PreFormat ? logItem.Item2 : Format(logItem.Item1, logItem.Item2, logItem.Item3);
                        await streamWriter.WriteAsync(line).ConfigureAwait(false);
                        // Check if we exceeded our buffer
                        if (streamWriter.BaseStream.Length > LoggerConfiguration.MaxBufferSize)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Couldn't format passed log information, maybe this was owned by the UI?", null);
                        Log.Warn().WriteLine("LogInfo and messagetemplate for the problematic log information: {0} {1}", logItem.Item1, logItem.Item2);
                    }
                }
                // Check if we wrote anything, if so store it to the file
                if (streamWriter.BaseStream.Length > 0)
                {
                    try
                    {
                        if (!Directory.Exists(directory))
                        {
                            Log.Info().WriteLine("Created directory {0}", directory);
                            Directory.CreateDirectory(directory);
                        }
                        streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                        using (var fileStream = new FileStream(filepath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                        {
                            await streamWriter.BaseStream.CopyToAsync(fileStream).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error().WriteLine(ex, "Error writing to logfile {0}", filepath);
                    }
                }
            }
        }


        /// <summary>
        ///     Archive the finished file
        /// </summary>
        /// <param name="oldFile">string with the filename as is</param>
        /// <param name="oldVariables">Dictionary</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Task to await for</returns>
        private async Task ArchiveFileAsync(string oldFile, Dictionary<string, object> oldVariables, CancellationToken cancellationToken = default)
        {
            var expandedArchiveFilename = Environment.ExpandEnvironmentVariables(LoggerConfiguration.ArchiveFilenamePattern);
            oldVariables["Extension"] = LoggerConfiguration.ArchiveExtension;
            var archiveDirectory = SimpleFormatWith(Environment.ExpandEnvironmentVariables(LoggerConfiguration.ArchiveDirectoryPath), oldVariables);

            // Filename of the file to write to.
            var archiveFilename = SimpleFormatWith(expandedArchiveFilename, oldVariables);
            var archiveFilepath = Path.Combine(archiveDirectory, archiveFilename);

            Log.Info().WriteLine("Archiving {0} to {1}", oldFile, archiveFilepath);

            if (!Directory.Exists(archiveDirectory))
            {
                Directory.CreateDirectory(archiveDirectory);
            }
            LoggerConfiguration.ArchiveHistory.Add(archiveFilepath);
            if (!LoggerConfiguration.ArchiveCompress)
            {
                await Task.Run(() => File.Move(oldFile, archiveFilepath), cancellationToken).ConfigureAwait(false);
            }
            else
            {
                using (var targetFileStream = new FileStream(archiveFilepath + ".tmp", FileMode.CreateNew, FileAccess.Write, FileShare.Read))
                using (var sourceFileStream = new FileStream(oldFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var compressionStream = new GZipStream(targetFileStream, CompressionMode.Compress))
                    {
                        await sourceFileStream.CopyToAsync(compressionStream).ConfigureAwait(false);
                    }
                }
                // As the previous code didn't throw, we can now safely delete the old file
                File.Delete(oldFile);
                // And rename the .tmp file.
                File.Move(archiveFilepath + ".tmp", archiveFilepath);
            }

            while (LoggerConfiguration.ArchiveHistory.Count > LoggerConfiguration.ArchiveCount)
            {
                var fileToRemove = LoggerConfiguration.ArchiveHistory[0];
                LoggerConfiguration.ArchiveHistory.RemoveAt(0);
                File.Delete(fileToRemove);
            }
        }

        #endregion

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _backgroundCancellationTokenSource.Cancel();
                try
                {
                    _backgroundTask.GetAwaiter().GetResult();
                }
                catch (TaskCanceledException)
                {
                    // Expected!
                }
                catch (Exception ex)
                {
                    Log.Error().WriteLine(ex, "Exception in background task.", null);
                }

                // Process leftovers
                try
                {
                    ProcessLinesAsync().Wait();
                }
                catch (Exception ex)
                {
                    Log.Error().WriteLine(ex, "Exception in cleanup.", null);
                }
                // Wait for archiving
                try
                {
                    Task[] archiveTasksToWaitFor;
                    lock (_archiveTaskList)
                    {
                        archiveTasksToWaitFor = _archiveTaskList.ToArray();
                    }
                    Task.WhenAll(archiveTasksToWaitFor).Wait();
                }
                catch (Exception ex)
                {
                    Log.Error().WriteLine(ex, "Exception in archiving.", null);
                }
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log.Facade
// 
//  Dapplo.Log.Facade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log.Facade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log.Facade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapplo.Log.Facade;
using Dapplo.Log.Facade.Loggers;
using Dapplo.Utils.Extensions;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;

namespace Dapplo.Log.LogFile
{
	/// <summary>
	/// This implements a logger which writes to a log file in the background
	/// Filename and directory are configurable, also rolling filename and compression can be activated
	/// </summary>
    public class FileLogger : AbstractLogger, IDisposable
	{
		private static readonly LogSource Log = new LogSource();

		static FileLogger()
		{
			// Make sure this class doesn't log into it's own file
			LogSettings.RegisterLoggerFor(Log, new IgnoreLogger());
		}

		private readonly ConcurrentQueue<Tuple<LogInfo, string>> _logItems = new ConcurrentQueue<Tuple<LogInfo, string>>();
		private readonly CancellationTokenSource _backgroundCancellationTokenSource = new CancellationTokenSource();
		private string _previousFilePath;
		private IDictionary<string, object> _previousVariables;
		private readonly IFileLoggerConfiguration _fileLoggerConfiguration;
		private readonly Task<bool> _backgroundTask;
		
		public FileLogger(IFileLoggerConfiguration fileLoggerConfiguration = null)
		{
			

			_fileLoggerConfiguration = fileLoggerConfiguration ?? new FileLoggerConfiguration();
			if (string.IsNullOrEmpty(_fileLoggerConfiguration.Processname))
			{
				var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
				_fileLoggerConfiguration.Processname = Path.GetFileNameWithoutExtension(assembly.Location);
			}
			// Start the processing in the background
			_backgroundTask = Task.Run(async () => await BackgroundAsync(_backgroundCancellationTokenSource.Token));
		}

		/// <summary>
		/// This is the implementation of the background task
		/// </summary>
		/// <returns>Task</returns>
		private async Task<bool> BackgroundAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				// ReSharper disable once MethodSupportsCancellation
				await ProcessLinesAsync().ConfigureAwait(false);
				// Wait a while before we process the next items
				await Task.Delay(_fileLoggerConfiguration.WriteInterval, cancellationToken).ConfigureAwait(false);
			}
			return true;
		}

		/// <summary>
		/// Process the lines
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		/// <returns>Task</returns>
		private async Task ProcessLinesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (_logItems.IsEmpty)
			{
				return;
			}
			IDictionary<string, object> variables = new Dictionary<string, object>()
			{
				{ "Processname", _fileLoggerConfiguration.Processname},
				{ "Timestamp" , DateTimeOffset.Now},
				{ "Extension", _fileLoggerConfiguration.Extension }
			};
			var expandedFilename = Environment.ExpandEnvironmentVariables(_fileLoggerConfiguration.FilenamePattern);
			var directory = Environment.ExpandEnvironmentVariables(_fileLoggerConfiguration.DirectoryPath).FormatWith(variables);

			// Filename of the file to write to.
			string filename = expandedFilename.FormatWith(variables);
			var filepath = Path.Combine(directory, filename);

			bool isFilepathChange = !filepath.Equals(_previousFilePath);

			if (_previousFilePath != null && isFilepathChange)
			{
				// Archive, and await... we will hopefully catch up??
				// TODO: Check if this is the case...
				try
				{
					await ArchiveFileAsync(_previousFilePath, _previousVariables, cancellationToken);
				}
				catch (Exception ex)
				{
					Log.Error().WriteLine(ex, "Error archiving {0}", _previousFilePath);
				}
			}

			// check if the last file we wrote to is not the same.
			if (_previousFilePath == null || isFilepathChange)
			{
				// Store the current variables, so we can use them next time for the archiving
				_previousFilePath = filepath;
				_previousVariables = variables;
			}

			using (var streamWriter = new StreamWriter(new MemoryStream(), Encoding.UTF8))
			{
				streamWriter.AutoFlush = true;
				// Item to process
				Tuple<LogInfo, string> logItem;
				// Loop as long as there are items available
				while (_logItems.TryDequeue(out logItem))
				{
					await streamWriter.WriteAsync(logItem.Item2).ConfigureAwait(false);
					// Check if we exceeded our buffer
					if (streamWriter.BaseStream.Length > _fileLoggerConfiguration.MaxBufferSize)
					{
						break;
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
						// TODO: handle the exception?
						Log.Error().WriteLine(ex, "Error writing to logfile {0}", filepath);
					}
				}
			}
	    }


		/// <summary>
		/// Archive the finished file
		/// </summary>
		/// <returns>Task to await for</returns>
		private async Task ArchiveFileAsync(string oldFile, IDictionary<string, object> oldVariables, CancellationToken cancellationToken = default(CancellationToken))
		{
			var expandedArchiveFilename = Environment.ExpandEnvironmentVariables(_fileLoggerConfiguration.ArchiveFilenamePattern);
			oldVariables["Extension"] = _fileLoggerConfiguration.ArchiveExtension;
			var archiveDirectory = Environment.ExpandEnvironmentVariables(_fileLoggerConfiguration.ArchiveDirectoryPath).FormatWith(oldVariables);

			// Filename of the file to write to.
			string archiveFilename = expandedArchiveFilename.FormatWith(oldVariables);
			var archiveFilepath = Path.Combine(archiveDirectory, archiveFilename);

			Log.Info().WriteLine("Archiving {0} to {1}", oldFile, archiveFilepath);

			if (!Directory.Exists(archiveDirectory))
			{
				Directory.CreateDirectory(archiveDirectory);
			}
			_fileLoggerConfiguration.ArchiveHistory.Add(archiveFilepath);
			if (!_fileLoggerConfiguration.ArchiveCompress)
			{
				await Task.Run(() => File.Move(oldFile, archiveFilepath), cancellationToken).ConfigureAwait(false);
			}
			else
			{
				using (var targetFileStream = new FileStream(archiveFilepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read))
				using (var sourceFileStream = new FileStream(oldFile, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					using (var compressionStream = new GZipStream(targetFileStream, CompressionMode.Compress))
					{
						await sourceFileStream.CopyToAsync(compressionStream);
					}
				}
				File.Delete(oldFile);
			}

			while (_fileLoggerConfiguration.ArchiveHistory.Count > _fileLoggerConfiguration.ArchiveCount)
			{
				var fileToRemove = _fileLoggerConfiguration.ArchiveHistory[0];
				_fileLoggerConfiguration.ArchiveHistory.RemoveAt(0);
				File.Delete(fileToRemove);
			}
		}

		/// <summary>
		/// Enqueue the current information so it can be written to the file
		/// The formatting of the message with the parameters needs to be done here, to prevent issues with disposed objects
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
			_logItems.Enqueue(new Tuple<LogInfo, string>(logInfo, Format(logInfo, messageTemplate, logParameters)));
		}

		/// <summary>
		/// This takes care of cancelling the background task which is writing the items
		/// </summary>
		public void Dispose()
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
				Log.Error().WriteLine(ex, "Exception in background task.");
			}

			// Process leftovers
			try
			{
				ProcessLinesAsync().Wait();
			}
			catch (Exception ex)
			{
				Log.Error().WriteLine(ex, "Exception in cleanup.");
			}

		}
	}
}

#region Dapplo 2016-2019 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016-2019 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Log
// 
// Dapplo.Log is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Log is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Log. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

using System;
using System.Collections.Generic;
using Dapplo.Log.Impl;

namespace Dapplo.Log.LogFile.Impl
{
    /// <summary>
    /// This is a simple container for the IFileLoggerConfiguration
    /// </summary>
    public class SimpleFileLoggerConfiguration : SimpleLoggerConfiguration, IFileLoggerConfiguration
    {
        #region Implementation of IFileLoggerConfiguration

        /// <summary>
        ///     Setting this to true will format the message in the context of the write call.
        ///     If this is set to false, the default, the formatting is done when writing to the file.
        ///     First makes the call slower, last could introduce problems with UI owned objects.
        /// </summary>
        public bool PreFormat { get; set; }

        /// <summary>
        ///     Limit the internal StringBuilder size
        /// </summary>
        public int MaxBufferSize { get; set; } = 512 * 1024;

        /// <summary>
        ///     Specify how long the background task can wait until it starts writing log entries
        /// </summary>
        public int WriteInterval { get; set; } = (int)TimeSpan.FromMilliseconds(500).TotalMilliseconds;

        /// <summary>
        ///     Name of the application, if null it will be created
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        ///     The extension of log file, default this is ".log"
        /// </summary>
        public string Extension { get; set; } = ".log";

        /// <summary>
        ///     Change the format for the filename, as soon as the filename changes, the previous is archived.
        /// </summary>
        public string FilenamePattern { get; set; } = "{ProcessName}-{Timestamp:yyyyMMdd}{Extension}";

        /// <summary>
        ///     Change the format for the filename, the possible arguments are documented in the .
        ///     Environment variables are also expanded.
        /// </summary>
#if _PCL_
		public string DirectoryPath { get; set; } = string.Empty;
#else
        public string DirectoryPath { get; set; } = @"%LOCALAPPDATA%\{ProcessName}";
#endif

        /// <summary>
        ///     Change the format for the archived filename
        /// </summary>
        public string ArchiveFilenamePattern { get; set; } = "{ProcessName}-{Timestamp:yyyyMMdd}{Extension}";

        /// <summary>
        ///     The path of the archived file
        /// </summary>
#if _PCL_
		public string ArchiveDirectoryPath { get; set; } = string.Empty;
#else
        public string ArchiveDirectoryPath { get; set; } = @"%LOCALAPPDATA%\{ProcessName}";
#endif

        /// <summary>
        ///     The extension of archived file, default this is ".log.gz"
        /// </summary>
        public string ArchiveExtension { get; set; } = ".log.gz";

        /// <summary>
        ///     Compress the archive
        /// </summary>
        public bool ArchiveCompress { get; set; } = true;

        /// <summary>
        ///     The amount of archived files which are allowed. The oldest is removed.
        /// </summary>
        public int ArchiveCount { get; set; } = 2;

        /// <summary>
        ///     The history of archived files, this could e.g. be stored in a configuration
        /// </summary>
        public IList<string> ArchiveHistory { get; set; } = new List<string>();


        #endregion
    }
}

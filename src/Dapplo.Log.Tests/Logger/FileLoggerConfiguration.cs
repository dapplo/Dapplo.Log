// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Log.LogFile;
using System.Collections.Generic;

namespace Dapplo.Log.Tests.Logger
{
    internal class FileLoggerConfiguration : IFileLoggerConfiguration
    {
        public bool PreFormat { get; set; }
        public int MaxBufferSize { get; set; }
        public int WriteInterval { get; set; }
        public string Processname { get; set; }
        public string Extension { get; set; }
        public string FilenamePattern { get; set; }
        public string DirectoryPath { get; set; }
        public string ArchiveFilenamePattern { get; set; }
        public string ArchiveDirectoryPath { get; set; }
        public string ArchiveExtension { get; set; }
        public bool ArchiveCompress { get; set; }
        public int ArchiveCount { get; set; }
        public IList<string> ArchiveHistory { get; set; }
        public LogLevels LogLevel { get; set; }
        public bool UseShortSource { get; set; }
        public string DateTimeFormat { get; set; }
        public string LogLineFormat { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Dapplo.Log.LogFile
{
	/// <summary>
	/// Configuration implementation for the IFileLoggerConfiguration
	/// </summary>
	public class FileLoggerConfiguration : IFileLoggerConfiguration
    {
		/// <summary>
		/// Limit the internal stringbuilder size, 
		/// </summary>
		public int MaxBufferSize { get; set; } = 512 * 1024;

		/// <summary>
		/// Specify how long the background task can wait until it starts writing log entries
		/// </summary>
		public int WriteInterval { get; set; } = (int)TimeSpan.FromMilliseconds(500).TotalMilliseconds;

		/// <summary>
		/// Name of the application, if null it will be created
		/// </summary>
		public string Processname { get; set; }

		/// <summary>
		/// The extension of log file, default this is ".log"
		/// </summary>
		public string Extension { get; set; } = ".log";

		/// <summary>
		/// Change the format for the filename, as soon as the filename changes, the previous is archived.
		/// </summary>
		public string FilenamePattern { get; set; } = "{Processname}-{Timestamp:yyyyMMdd}{Extension}";

		/// <summary>
		/// Change the format for the filename, the possible arguments are documented in the .
		/// Environment variablen are also expanded.
		/// </summary>
#if _PCL_
		public string DirectoryPath { get; set; } = @"";
#else
		public string DirectoryPath { get; set; } = @"%LOCALAPPDATA%\{Processname}";
#endif

		/// <summary>
		/// Change the format for the archived filename
		/// </summary>
		public string ArchiveFilenamePattern { get; set; } = "{Processname}-{Timestamp:yyyyMMdd}{Extension}";

		/// <summary>
		/// The path of the archived file
		/// </summary>
#if _PCL_
		public string ArchiveDirectoryPath { get; set; } = @"";
#else
		public string ArchiveDirectoryPath { get; set; } = @"%LOCALAPPDATA%\{Processname}";
#endif
		
		/// <summary>
		/// The extension of archived file, default this is ".log.gz"
		/// </summary>
		public string ArchiveExtension { get; set; } = ".log.gz";

		/// <summary>
		/// Compress the archive
		/// </summary>
		public bool ArchiveCompress { get; set; } = true;

		/// <summary>
		/// The amount of archived files which are allowed. The oldest is removed.
		/// </summary>
		public int ArchiveCount { get; set; } = 2;

		/// <summary>
		/// The history of archived files, this could e.g. be stored in a configuration
		/// </summary>
		public IList<string> ArchiveHistory { get; set; } = new List<string>();
	}
}

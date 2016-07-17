#region Dapplo 2016 - GNU Lesser General Public License

//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2016 Dapplo
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

#region Usings

using System.Collections.Generic;
using System.ComponentModel;
using Dapplo.Log.Facade;

#endregion

namespace Dapplo.Log.LogFile
{
	/// <summary>
	/// Interface for the logfile configuration
	/// </summary>
	public interface IFileLoggerConfiguration : ILoggerConfiguration
	{
		/// <summary>
		/// Setting this to true will format the message in the context of the write call.
		/// If this is set to false, the default, the formatting is done when writing to the file.
		/// First makes the call slower, last could introduce problems with UI owned objects.
		/// </summary>
		[DefaultValue(false)]
		bool Preformat { get; set; }

		/// <summary>
		/// Limit the internal stringbuilder size, 
		/// </summary>
		[DefaultValue(512*1024)]
		int MaxBufferSize { get; set; }

		/// <summary>
		/// Specify how long the background task can wait until it starts writing log entries
		/// </summary>
		[DefaultValue(500)]
		int WriteInterval { get; set; }

		/// <summary>
		/// Name of the application, if null it will be created
		/// </summary>
		string Processname { get; set; }

		/// <summary>
		/// The extension of log file, default this is ".log"
		/// </summary>
		[DefaultValue(".log")]
		string Extension { get; set; }

		/// <summary>
		/// Change the format for the filename, as soon as the filename changes, the previous is archived.
		/// </summary>
		[DefaultValue("{Processname}-{Timestamp:yyyyMMdd}{Extension}")]
		string FilenamePattern { get; set; }

		/// <summary>
		/// Change the format for the filename, the possible arguments are documented in the .
		/// Environment variablen are also expanded.
		/// </summary>
		[DefaultValue(@"%LOCALAPPDATA%\{Processname}")]
		string DirectoryPath { get; set; }

		/// <summary>
		/// Change the format for the archived filename
		/// </summary>
		[DefaultValue("{Processname}-{Timestamp:yyyyMMdd}{Extension}")]
		string ArchiveFilenamePattern { get; set; }

		/// <summary>
		/// The path of the archived file
		/// </summary>
		[DefaultValue(@"%LOCALAPPDATA%\{Processname}")]
		string ArchiveDirectoryPath { get; set; }

		/// <summary>
		/// The extension of archived file, default this is .log.gz
		/// </summary>
		[DefaultValue(".log.gz")]
		string ArchiveExtension { get; set; }

		/// <summary>
		/// Compress the archive
		/// </summary>
		[DefaultValue(true)]
		bool ArchiveCompress { get; set; }

		/// <summary>
		/// The amount of archived files which are allowed. The oldest is removed.
		/// </summary>
		[DefaultValue(3)]
		int ArchiveCount { get; set; }

		/// <summary>
		/// The history of archived files, this could e.g. be stored in a configuration
		/// </summary>
		IList<string> ArchiveHistory { get; set; }
	}
}
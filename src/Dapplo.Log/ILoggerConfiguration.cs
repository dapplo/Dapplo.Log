#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
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

#region Usings

using System.ComponentModel;

#endregion

namespace Dapplo.Log
{
    /// <summary>
    ///     Interface for the LoggerConfiguration
    /// </summary>
    public interface ILoggerConfiguration
    {
        /// <summary>
        ///     The LogLevels enum a logger uses
        /// </summary>
        [DefaultValue(LogLevels.Info)]
        LogLevels LogLevel { get; set; }

        /// <summary>
        ///     Defines if the Source is written like d.l.LoggerTest (default) or Dapplo.Log.Facade.LoggerTest
        /// </summary>
        [DefaultValue(true)]
        bool UseShortSource { get; set; }

        /// <summary>
        ///     Timestamp format which is used in the output, when outputting the LogInfo details
        /// </summary>
        [DefaultValue("yyyy-MM-dd HH:mm:ss.fff")]
        string DateTimeFormat { get; set; }

        /// <summary>
        ///     Default line format for loggers which use the DefaultFormatter.
        ///     The first argument is the LogInfo, the second the message + parameters formatted
        /// </summary>
        [DefaultValue("{0} - {1}")]
        string LogLineFormat { get; set; }
    }
}
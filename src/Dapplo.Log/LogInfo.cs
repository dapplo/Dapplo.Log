#region Dapplo 2016-2018 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016-2018 Dapplo
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

using System;

#endregion

namespace Dapplo.Log
{
    /// <summary>
    ///     A simple wrapper for some information which is passed to the logger
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        ///     The LogLevels enum for the log
        /// </summary>
        public LogLevels LogLevel { get; set; }

        /// <summary>
        ///     The line of the log
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        ///     Method in the Caller (class) from where the log statement came
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        ///     Class from where the log statement came
        /// </summary>
        public LogSource Source { get; set; }

        /// <summary>
        ///     Timestamp for the log
        /// </summary>
        public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

        /// <summary>
        ///     Create a string representation of the LogInfo, this by default has a timestamp, level, source, method and line.
        ///     If the format needs to be changed, LogSettings.LogInfoFormatter can be assigned with your custom formatter Func
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return
                $"{Timestamp.ToString(LogSettings.DefaultLoggerConfiguration.DateTimeFormat)} {LogLevel} {Source.Source}:{Method}({Line})";
        }
    }
}
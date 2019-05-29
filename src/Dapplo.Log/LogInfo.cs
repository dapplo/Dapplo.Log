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
using System.Text;

namespace Dapplo.Log
{
    /// <summary>
    ///     A simple wrapper for some information which is passed to the logger
    /// </summary>
    public sealed class LogInfo
    {
        /// <summary>
        /// Create a LogInfo
        /// </summary>
        /// <param name="source">LogSource</param>
        /// <param name="method">string with the name of the method</param>
        /// <param name="line">int with the line number</param>
        /// <param name="logLevel">LogLevels</param>
        public LogInfo(LogSource source, string method, int line, LogLevels logLevel = LogLevels.Info)
        {
            Timestamp = DateTimeOffset.Now;
            Source = source;
            Method = method;
            Line = line;
            LogLevel = logLevel;
        }

        /// <summary>
        ///     The LogLevels enum for the log
        /// </summary>
        public LogLevels LogLevel { get; }

        /// <summary>
        ///     The line of the log
        /// </summary>
        public int Line { get; }

        /// <summary>
        ///     Method in the Caller (class) from where the log statement came
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     Class from where the log statement came
        /// </summary>
        public LogSource Source { get; }

        /// <summary>
        ///     Timestamp for the log
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        ///     Create a string representation of the LogInfo, this by default has a timestamp, level, source, method and line.
        ///     If the format needs to be changed, LogSettings.LogInfoFormatter can be assigned with your custom formatter Func
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return ToString(LogSettings.DefaultLoggerConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggerConfiguration"></param>
        /// <returns></returns>
        public string ToString(ILoggerConfiguration loggerConfiguration)
        {
            var stringBuilder = new StringBuilder(Timestamp.ToString(loggerConfiguration?.DateTimeFormat ?? "yyyy-MM-dd HH:mm:ss.fff"));
            stringBuilder
                .Append(' ')
                .Append(((int)LogLevel).ToString())
                .Append(' ')
                .Append(Source.Source)
                .Append(':')
                .Append(Method)
                .Append('(')
                .Append(Line.ToString())
                .Append(')');
            return stringBuilder.ToString();
        }
    }
}
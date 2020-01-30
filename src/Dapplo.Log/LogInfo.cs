// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// ToString
        /// </summary>
        /// <param name="loggerConfiguration">ILoggerConfiguration</param>
        /// <returns></returns>
        public string ToString(ILoggerConfiguration loggerConfiguration)
        {
            var stringBuilder = new StringBuilder(Timestamp.ToString(loggerConfiguration?.DateTimeFormat ?? "yyyy-MM-dd HH:mm:ss.fff"));
            stringBuilder
                .Append(' ')
                .Append(LogLevel)
                .Append(' ')
                .Append(loggerConfiguration.UseShortSource ? Source.ShortSource : Source.Source)
                .Append(':')
                .Append(Method)
                .Append('(')
                .Append(Line.ToString())
                .Append(')');
            return stringBuilder.ToString();
        }
    }
}
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

#define DEBUG

#region Usings

using System;

#endregion

namespace Dapplo.Log
{
    /// <summary>
    ///     Abstract implementation for ILogger, which safes some time.
    ///     In general you will only need to implement Write (without exception) as:
    ///     1) WriteLine in calls Write after appending a newline.
    ///     2) WriteLine with Exception calls WriteLine with the template/parameters and again with the Exception.ToString()
    /// </summary>
    public class AbstractLogger : ILogger
    {
        /// <summary>
        ///     Configure the logger with the supplied configuration, values are copied.
        /// </summary>
        /// <param name="configuration">ILoggerConfiguration</param>
        public virtual void Configure(ILoggerConfiguration configuration)
        {
            if (configuration == null)
            {
                return;
            }
            LogLevel = configuration.LogLevel;
            UseShortSource = configuration.UseShortSource;
            LogLineFormat = configuration.LogLineFormat;
            DateTimeFormat = configuration.DateTimeFormat;
        }

        /// <summary>
        ///     This function can be changed to format the line message differently
        ///     First argument is the LogInfo, second the messageTemplate, third the parameters
        /// </summary>
        public virtual string Format(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            var message = messageTemplate;
            // Test if there are parameters, if not there is no need to format it!
            if (logParameters != null && logParameters.Length > 0)
            {
                message = string.Format(messageTemplate, logParameters);
            }
            // Format the LogInfo
            var source = UseShortSource ? logInfo.Source.ShortSource : logInfo.Source.Source;
            var logInfoString = $"{logInfo.Timestamp.ToString(DateTimeFormat)} {logInfo.LogLevel} {source}:{logInfo.Method}({logInfo.Line})";
            return string.Format(LogLineFormat, logInfoString, message);
        }

        /// <summary>
        ///     Use this to specify the LogLevels enum
        /// </summary>
        public virtual LogLevels LogLevel { get; set; } = LogLevels.Info;

        /// <summary>
        ///     Defines if the Source is written like d.l.LoggerTest (default) or Dapplo.Log.LoggerTest
        /// </summary>
        public bool UseShortSource { get; set; } = true;

        /// <summary>
        ///     Timestamp format which is used in the output, when outputting the LogInfo details
        /// </summary>
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        ///     Default line format for loggers which use the DefaultFormatter.
        ///     The first argument is the LogInfo, the second the message + parameters formatted
        /// </summary>
        public string LogLineFormat { get; set; } = "{0} - {1}";

        /// <summary>
        ///     Test if a certain LogLevels enum is enabled, optionally the LogSource for which this is requested is supplied and
        ///     can be used.
        /// </summary>
        /// <param name="logLevel">LogLevels enum</param>
        /// <param name="logSource">optional LogSource</param>
        /// <returns>true if this is enabled</returns>
        public virtual bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            return logLevel != LogLevels.None && logLevel >= LogLevel;
        }

        /// <summary>
        ///     Override this if you need to move your content into a different logger which replaces "you"
        /// </summary>
        /// <param name="newLogger">ILogger</param>
        public virtual void ReplacedWith(ILogger newLogger)
        {
            // This does nothing.
        }

        /// <summary>
        ///     Write a message with parameters to the logger
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string with the message template</param>
        /// <param name="logParameters">object array with the parameters for the template</param>
        public virtual void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            // This does nothing.
        }

        /// <summary>
        ///     WriteLine, default wraps to write, passes a message with parameters to the logger and appends a newline
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string with the message template</param>
        /// <param name="logParameters">object array with the parameters for the template</param>
        public virtual void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Write(logInfo, messageTemplate + Environment.NewLine, logParameters);
        }

        /// <summary>
        ///     WriteLine with Exception calls WriteLine with messageTemplate / logParameters and WriteLine with the exception as
        ///     string
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="exception">Exception</param>
        /// <param name="messageTemplate">string with the message template</param>
        /// <param name="logParameters">object array with the parameters for the template</param>
        public virtual void WriteLine(LogInfo logInfo, Exception exception, string messageTemplate = null,
            params object[] logParameters)
        {
            if (messageTemplate != null)
            {
                WriteLine(logInfo, messageTemplate, logParameters);
            }
            if (exception != null)
            {
                WriteLine(logInfo, exception.ToString());
            }
        }
    }
}
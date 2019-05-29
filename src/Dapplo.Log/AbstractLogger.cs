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

#define DEBUG

using System;

namespace Dapplo.Log
{

    /// <summary>
    ///     Abstract implementation for ILogger, which safes some time.
    ///     In general you will only need to implement Write (without exception) as:
    ///     1) WriteLine in calls Write after appending a newline.
    ///     2) WriteLine with Exception calls WriteLine with the template/parameters and again with the Exception.ToString()
    /// </summary>
    public class AbstractLogger : ILogger, ILoggerConfiguration
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractLogger()
        {

        }

        /// <summary>
        /// Constructor with LogLevels
        /// </summary>
        /// <param name="logLevels">LogLevels</param>
        public AbstractLogger(LogLevels logLevels)
        {
            LogLevel = logLevels;
        }

        /// <inheritdoc />
        public virtual void Configure(ILoggerConfiguration loggerConfiguration)
        {
            if (loggerConfiguration == null)
            {
                return;
            }
            LogLevel = loggerConfiguration.LogLevel;
            UseShortSource = loggerConfiguration.UseShortSource;
            LogLineFormat = loggerConfiguration.LogLineFormat;
            DateTimeFormat = loggerConfiguration.DateTimeFormat;
        }

        /// <inheritdoc />
        public virtual string Format(LogInfo logInfo, string messageTemplate, params object[] parameters)
        {
            if (logInfo is null)
            {
                throw new ArgumentNullException(nameof(logInfo));
            }
            if (messageTemplate is null)
            {
                throw new ArgumentNullException(nameof(messageTemplate));
            }

            // Test if there are parameters, if not there is no need to format it!
            if (parameters != null && parameters.Length > 0)
            {
                messageTemplate = string.Format(messageTemplate, parameters);
            }

            // Format the LogInfo
            var logInfoString = logInfo.ToString(this);
            return string.Format(LogLineFormat, logInfoString, messageTemplate);
        }

        /// <inheritdoc />
        public virtual LogLevels LogLevel { get; set; } = LogLevels.Info;

        /// <inheritdoc />
        public bool UseShortSource { get; set; } = true;

        /// <inheritdoc />
        public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

        /// <inheritdoc />
        public string LogLineFormat { get; set; } = "{0} - {1}";

        /// <inheritdoc />
        public virtual bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            return logLevel != LogLevels.None && logLevel >= LogLevel;
        }

        /// <inheritdoc />
        public virtual void ReplacedWith(ILogger newLogger)
        {
            // This does nothing.
        }

        /// <inheritdoc />
        public virtual void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            // This does nothing.
        }

        /// <inheritdoc />
        public virtual void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Write(logInfo, messageTemplate + Environment.NewLine, logParameters);
        }

        /// <inheritdoc />
        public virtual void WriteLine(LogInfo logInfo, Exception exception, string messageTemplate = null, params object[] logParameters)
        {
            if (messageTemplate != null)
            {
                WriteLine(logInfo, messageTemplate, logParameters);
            }
            if (exception != null)
            {
                WriteLine(logInfo, LogSettings.ExceptionToStacktrace(exception), null);
            }
        }
    }
}
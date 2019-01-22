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

using log4net;

namespace Dapplo.Log.Tests.Logger
{
    /// <summary>
    ///     A log4net logger, this can be copied into your code
    /// </summary>
    public class Log4NetLogger : AbstractLogger
    {
        private static ILog GetLogger(LogSource logSource)
        {
            return logSource.SourceType != null ? LogManager.GetLogger(logSource.SourceType) : LogManager.GetLogger(logSource.Source);
        }

        /// <summary>
        ///     Write the supplied information to a log4net.ILog
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string</param>
        /// <param name="logParameters">params object[]</param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            var log = GetLogger(logInfo.Source);

            switch (logInfo.LogLevel)
            {
                case LogLevels.Verbose:
                case LogLevels.Debug:
                    if (logParameters != null)
                    {
                        log.DebugFormat(messageTemplate, logParameters);
                    }
                    else
                    {
                        log.Debug(messageTemplate);
                    }
                    break;
                case LogLevels.Error:
                    if (logParameters != null)
                    {
                        log.ErrorFormat(messageTemplate, logParameters);
                    }
                    else
                    {
                        log.Error(messageTemplate);
                    }
                    break;
                case LogLevels.Fatal:
                    if (logParameters != null)
                    {
                        log.FatalFormat(messageTemplate, logParameters);
                    }
                    else
                    {
                        log.Fatal(messageTemplate);
                    }
                    break;
                case LogLevels.Info:
                    if (logParameters != null)
                    {
                        log.InfoFormat(messageTemplate, logParameters);
                    }
                    else
                    {
                        log.Info(messageTemplate);
                    }
                    break;
                case LogLevels.Warn:
                    if (logParameters != null)
                    {
                        log.WarnFormat(messageTemplate, logParameters);
                    }
                    else
                    {
                        log.Warn(messageTemplate);
                    }
                    break;
            }
        }

        /// <summary>
        ///     Make sure there are no newlines passed, log4net already writes those
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="logParameters"></param>
        public override void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Write(logInfo, messageTemplate, logParameters);
        }

        /// <summary>
        ///     Test if a certain LogLevels enum is enabled
        /// </summary>
        /// <param name="logLevel">LogLevels value</param>
        /// <param name="logSource">LogSource to check for</param>
        /// <returns>bool true if the LogLevels enum is enabled</returns>
        public override bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            var log = GetLogger(logSource);
            switch (logLevel)
            {
                case LogLevels.Verbose:
                case LogLevels.Debug:
                    return log.IsDebugEnabled;
                case LogLevels.Error:
                    return log.IsErrorEnabled;
                case LogLevels.Fatal:
                    return log.IsFatalEnabled;
                case LogLevels.Info:
                    return log.IsInfoEnabled;
                case LogLevels.Warn:
                    return log.IsWarnEnabled;
            }
            return false;
        }
    }
}
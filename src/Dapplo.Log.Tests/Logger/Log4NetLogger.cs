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

using log4net;

#endregion

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
        /// <param name="propertyValues">params object[]</param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] propertyValues)
        {
            var log = GetLogger(logInfo.Source);

            switch (logInfo.LogLevel)
            {
                case LogLevels.Verbose:
                case LogLevels.Debug:
                    if (propertyValues != null)
                    {
                        log.DebugFormat(messageTemplate, propertyValues);
                    }
                    else
                    {
                        log.Debug(messageTemplate);
                    }
                    break;
                case LogLevels.Error:
                    if (propertyValues != null)
                    {
                        log.ErrorFormat(messageTemplate, propertyValues);
                    }
                    else
                    {
                        log.Error(messageTemplate);
                    }
                    break;
                case LogLevels.Fatal:
                    if (propertyValues != null)
                    {
                        log.FatalFormat(messageTemplate, propertyValues);
                    }
                    else
                    {
                        log.Fatal(messageTemplate);
                    }
                    break;
                case LogLevels.Info:
                    if (propertyValues != null)
                    {
                        log.InfoFormat(messageTemplate, propertyValues);
                    }
                    else
                    {
                        log.Info(messageTemplate);
                    }
                    break;
                case LogLevels.Warn:
                    if (propertyValues != null)
                    {
                        log.WarnFormat(messageTemplate, propertyValues);
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
        /// <param name="level">LogLevels value</param>
        /// <param name="logSource">LogSource to check for</param>
        /// <returns>bool true if the LogLevels enum is enabled</returns>
        public override bool IsLogLevelEnabled(LogLevels level, LogSource logSource = null)
        {
            var log = GetLogger(logSource);
            switch (level)
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
// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            return logLevel switch
            {
                LogLevels.Verbose => log.IsDebugEnabled,
                LogLevels.Debug => log.IsDebugEnabled,
                LogLevels.Error => log.IsErrorEnabled,
                LogLevels.Fatal => log.IsFatalEnabled,
                LogLevels.Info => log.IsInfoEnabled,
                LogLevels.Warn => log.IsWarnEnabled,
                _ => false
            };
        }
    }
}
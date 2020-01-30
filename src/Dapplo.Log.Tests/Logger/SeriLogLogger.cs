// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Serilog.Events;

namespace Dapplo.Log.Tests.Logger
{
    /// <summary>
    ///     Dapplo.Log.ILogger implementation for Serilog
    /// </summary>
    public class SeriLogLogger : AbstractLogger
    {
        // Default Serilog.ILogger for when no type is supplied
        private static readonly Serilog.ILogger Log = Serilog.Log.Logger.ForContext<SeriLogLogger>();

        /// <summary>
        ///     Write the supplied information to a Serilog.ILogger
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string</param>
        /// <param name="logParameters">params object[]</param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Serilog.ILogger seriLogger;
            if (logInfo.Source.SourceType != null)
            {
                seriLogger = Log.ForContext(logInfo.Source.SourceType);
            }
            else
            {
                seriLogger = Log.ForContext("SourceContext", logInfo.Source.Source);
            }
            seriLogger.Write(ConvertLevel(logInfo.LogLevel), messageTemplate, logParameters);
        }

        /// <summary>
        ///     Test if a certain LogLevels enum is enabled
        /// </summary>
        /// <param name="logLevel">LogLevels value</param>
        /// <param name="logSource">LogSource to check for</param>
        /// <returns>bool true if the LogLevels enum is enabled</returns>
        public override bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            Serilog.ILogger seriLogger = Log;
            if (logSource?.SourceType != null)
            {
                seriLogger = Log.ForContext(logSource.SourceType);
            }
            return logLevel != LogLevels.None && seriLogger.IsEnabled(ConvertLevel(logLevel));
        }

        /// <summary>
        ///     Convert the Dapplo.Log.LogLevels to a Serilog.Events.LogEventLevel
        /// </summary>
        /// <param name="logLevel">LogLevels</param>
        /// <returns>LogEventLevel</returns>
        private static LogEventLevel ConvertLevel(LogLevels logLevel)
        {
            var seriLogLevel = LogEventLevel.Debug;
            switch (logLevel)
            {
                case LogLevels.Verbose:
                    seriLogLevel = LogEventLevel.Verbose;
                    break;
                case LogLevels.Debug:
                    seriLogLevel = LogEventLevel.Debug;
                    break;
                case LogLevels.Info:
                    seriLogLevel = LogEventLevel.Information;
                    break;
                case LogLevels.Warn:
                    seriLogLevel = LogEventLevel.Warning;
                    break;
                case LogLevels.Error:
                    seriLogLevel = LogEventLevel.Error;
                    break;
                case LogLevels.Fatal:
                    seriLogLevel = LogEventLevel.Fatal;
                    break;
            }
            return seriLogLevel;
        }
    }
}
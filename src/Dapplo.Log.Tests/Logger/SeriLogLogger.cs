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
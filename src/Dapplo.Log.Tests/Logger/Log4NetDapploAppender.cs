// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using log4net.Appender;
using log4net.Core;
using System.Linq;
// ReSharper disable ExplicitCallerInfoArgument

namespace Dapplo.Log.Tests.Logger
{
    /// <summary>
    /// An implementation of a Log4NET appender which logs using Dapplo.Log
    /// </summary>
    public class Log4NetDapploAppender : AppenderSkeleton
    {
        private readonly Level _minimalLog4NetLevel = Level.Info;
        /// <summary>
        /// </summary>
        /// <param name="minimalLog4NetLevel"></param>
        public Log4NetDapploAppender(Level minimalLog4NetLevel = null)
        {
            if (minimalLog4NetLevel != null)
            {
                _minimalLog4NetLevel = minimalLog4NetLevel;
            }
        }

        /// <summary>
        /// Helper method to compare levels
        /// </summary>
        /// <param name="level"></param>
        /// <param name="possibilities"></param>
        /// <returns></returns>
        private static bool LevelIn(Level level, params Level[] possibilities)
        {
            if (level is null || possibilities == null)
            {
                return false;
            }
            return possibilities.Any(possibility => level.Value == possibility.Value);
        }

        /// <summary>
        /// Append the information to the Dapplo log
        /// </summary>
        /// <param name="loggingEvent">LoggingEvent</param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (loggingEvent.Level < _minimalLog4NetLevel)
            {
                // Ignore log
                return;
            }
            var locationInformation = loggingEvent.LocationInformation;
            var log = new LogSource(locationInformation.FullInfo);

            // Map the Log4NET level, including the method and line number, to Dapplo.Log.LogInfo
            var number = int.Parse(locationInformation.LineNumber);
            var logInfo = log.Info(number, locationInformation.MethodName);
            if (LevelIn(loggingEvent.Level, Level.Error))
            {
                logInfo = log.Error(number, locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Fatal, Level.Critical, Level.Alert, Level.Emergency))
            {
                logInfo = log.Fatal(number, locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Warn, Level.Severe))
            {
                logInfo = log.Warn(number, locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Debug))
            {
                logInfo = log.Debug(number, locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Verbose, Level.Fine, Level.Finer, Level.Finest, Level.Trace))
            {
                logInfo = log.Verbose(number, locationInformation.MethodName);
            }
            // Log the actual message, use Log4Net for the rendering
            logInfo.WriteLine(loggingEvent.RenderedMessage);
        }
    }
}

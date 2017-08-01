#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
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

using System.Diagnostics.CodeAnalysis;
using log4net.Appender;
using log4net.Core;
using System.Linq;

namespace Dapplo.Log.Tests.Logger
{
    /// <summary>
    /// An implementation of a Log4NET appender which logs using Dapplo.Log
    /// </summary>
    [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument", Justification = "Framework code")]
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
            if (level == null || possibilities == null)
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
            LogInfo logInfo = null;
            if (LevelIn(loggingEvent.Level, Level.Error))
            {
                logInfo = log.Error(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Fatal, Level.Critical, Level.Alert, Level.Emergency))
            {
                logInfo = log.Fatal(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Warn, Level.Severe))
            {
                logInfo = log.Warn(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Info, Level.Notice))
            {
                logInfo = log.Info(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Debug))
            {
                logInfo = log.Debug(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            if (LevelIn(loggingEvent.Level, Level.Verbose, Level.Fine, Level.Finer, Level.Finest, Level.Trace))
            {
                logInfo = log.Verbose(int.Parse(locationInformation.LineNumber), locationInformation.MethodName);
            }
            // Log the actual message, use Log4Net for the rendering
            logInfo?.WriteLine(loggingEvent.RenderedMessage);
        }
    }
}

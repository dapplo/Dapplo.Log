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

using System;
using System.Linq;
using Dapplo.Log.Loggers;
using Xunit;

#endregion

namespace Dapplo.Log.Tests
{
    public class LoggerTests
    {
        private static readonly LogSource Log = new LogSource();

        /// <summary>
        ///     Test Constructor
        /// </summary>
        [Fact]
        public void TestConstructor()
        {
            Assert.Equal(GetType().FullName, Log.Source);
        }

        /// <summary>
        ///     Test ConsoleLogger
        /// </summary>
        [Fact]
        public void TestConsoleLogger()
        {
            LoggerTestSupport.TestAllLogMethods(new ConsoleLogger());
        }

        /// <summary>
        ///     Test DebugLogger
        /// </summary>
        [Fact]
        public void TestDebugLogger()
        {
            LoggerTestSupport.TestAllLogMethods(new DebugLogger());
        }

        /// <summary>
        ///     Test to check if the Logger doesn't write when the level isn't set
        /// </summary>
        [Fact]
        public void TestLoggerVisibility()
        {
            var stringwriterLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>();

            Assert.NotNull(stringwriterLogger);
            Log.Verbose().WriteLine("This is a test, should NOT be visisble");
            Log.Debug().WriteLine("This is a test, should NOT be visisble");
            Log.Info().WriteLine("This is a test");
            Log.Warn().WriteLine("This is a test");
            Log.Error().WriteLine("This is a test");
            Log.Fatal().WriteLine("This is a test");

            Log.Error().WriteLine(new Exception(nameof(stringwriterLogger)), "This is a test exception");

            Assert.DoesNotContain("should NOT be visisble", stringwriterLogger.Output);

            var lines = stringwriterLogger.Output.Count(x => x.ToString() == Environment.NewLine);
            // Info + Warn + Error + Fatal = 4
            Assert.False(lines == 4);
        }

        /// <summary>
        ///     Test RegisterLoggerFor and DeregisterLoggerFor
        /// </summary>
        [Fact]
        public void TestMapping()
        {
            var defaultLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>();

            var differentLogSource = LogSource.ForCustomSource("Test");
            var logger = new StringWriterLogger();

            LoggerMapper.RegisterLoggerFor("Test", logger);

            const string visibleMessage = "Should be visisble";
            const string notVisibleMessage = "Should be NOT visisble in logger, but arrive in the defaultLogger";
            differentLogSource.Info().WriteLine(visibleMessage);
            Log.Info().WriteLine(notVisibleMessage);
            Assert.Contains(visibleMessage, logger.Output);
            Assert.DoesNotContain(notVisibleMessage, logger.Output);
            Assert.Contains(notVisibleMessage, defaultLogger.Output);

            defaultLogger.Clear();
            LoggerMapper.DeregisterLoggerFor("Test", logger);
            differentLogSource.Info().WriteLine(notVisibleMessage);
            Assert.DoesNotContain(notVisibleMessage, logger.Output);
            Assert.Contains(notVisibleMessage, defaultLogger.Output);
        }

        /// <summary>
        ///     Test TraceLogger
        /// </summary>
        [Fact]
        public void TestTraceLogger()
        {
            LoggerTestSupport.TestAllLogMethods(new TraceLogger());
        }


        /// <summary>
        ///     Test formatting without arguments
        /// </summary>
        [Fact]
        public void TestFormat()
        {
            const string testString = "{\"valueNormal\":\"normal\",\"valueNotReadOnly\":\"notReadonly\"}";
            var logger = new AbstractLogger();

            var logInfo = new LogInfo
            {
                Source = new LogSource(),
                Method = nameof(TestFormat),
                Line = 1,
                LogLevel = LogLevels.Debug
            };
            logger.Format(logInfo, testString);
        }
    }
}
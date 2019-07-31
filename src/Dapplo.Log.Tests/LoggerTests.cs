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

using System;
using System.Linq;
using Dapplo.Log.Loggers;
using Xunit;

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

        [Fact]
        public void TestConstructorWithLinuxPath()
        {
            var log = new LogSource("/Some/Path/To/Namespace/Type.Name.dll");
            Assert.Equal("Namespace.Type.Name", log.Source);
        }
        
        [Fact]
        public void TestConstructorWithWindowsPath()
        {
            var log = new LogSource(@"c:\Some\Path\To\Namespace\Type.Name.dll");
            Assert.Equal("Namespace.Type.Name", log.Source);
        }
        
        [Fact]
        public void TestConstructorWithShortLinuxPath()
        {
            var log = new LogSource("/Namespace/Type.Name.dll");
            Assert.Equal("Namespace.Type.Name", log.Source);
        }
        
        [Fact]
        public void TestConstructorWithShortWindowsPath()
        {
            var log = new LogSource(@"c:\Namespace\Type.Name.dll");
            Assert.Equal("Namespace.Type.Name", log.Source);
        }

        [Fact]
        public void TestRegisterDefaultLogger()
        {
            var defaultLogger = LogSettings.DefaultLogger;
            // Initialize a debug logger for Dapplo packages
            LogSettings.RegisterDefaultLogger<DebugLogger>(LogLevels.Verbose);
            Assert.True(LogSettings.DefaultLogger is DebugLogger);
            LogSettings.DefaultLogger = defaultLogger;
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
            var stringWriterLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>();

            Assert.NotNull(stringWriterLogger);
            Log.Verbose().WriteLine("This is a test, should NOT be visible");
            Log.Debug().WriteLine("This is a test, should NOT be visible");
            Log.Info().WriteLine("This is a test");
            Log.Warn().WriteLine("This is a test");
            Log.Error().WriteLine("This is a test");
            Log.Fatal().WriteLine("This is a test");

            Log.Error().WriteLine(new Exception(nameof(stringWriterLogger)), "This is a test exception");

            Assert.DoesNotContain("should NOT be visible", stringWriterLogger.Output);

            var lines = stringWriterLogger.Output.Count(x => x.ToString() == Environment.NewLine);
            // Info + Warn + Error + Fatal = 4
            Assert.False(lines == 4);
        }

        /// <summary>
        ///     Test RegisterLoggerFor and DeRegisterLoggerFor
        /// </summary>
        [Fact]
        public void TestMapping()
        {
            var defaultLogger = LogSettings.RegisterDefaultLogger<StringWriterLogger>();

            var differentLogSource = LogSource.ForCustomSource("Test");
            var logger = new StringWriterLogger();

            LoggerMapper.RegisterLoggerFor("Test", logger);

            const string visibleMessage = "Should be visible";
            const string notVisibleMessage = "Should be NOT visible in logger, but arrive in the defaultLogger";
            differentLogSource.Info().WriteLine(visibleMessage);
            Log.Info().WriteLine(notVisibleMessage);
            Assert.Contains(visibleMessage, logger.Output);
            Assert.DoesNotContain(notVisibleMessage, logger.Output);
            Assert.Contains(notVisibleMessage, defaultLogger.Output);

            defaultLogger.Clear();
            LoggerMapper.DeRegisterLoggerFor("Test", logger);
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

            var logInfo = new LogInfo(new LogSource(), nameof(TestFormat), 1, LogLevels.Debug);
            logger.Format(logInfo, testString);
        }
    }
}
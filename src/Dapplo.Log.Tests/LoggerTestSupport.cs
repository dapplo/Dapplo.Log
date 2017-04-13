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

#region Usings

using System;
using Xunit;

#endregion

namespace Dapplo.Log.Tests
{
    public static class LoggerTestSupport
    {
        /// <summary>
        ///     This will call all available methods on the ILogger
        /// </summary>
        public static void TestAllLogMethods(ILogger loggerUnderTest)
        {
            var logSource = LogSource.ForCustomSource(Guid.NewGuid().ToString());
            logSource.LogTo(loggerUnderTest);
            try
            {
                AssertLogLevels(logSource, loggerUnderTest);
                AssertWriteLines(logSource);
            }
            finally
            {
                LoggerMapper.DeregisterLoggerFor(logSource, loggerUnderTest);
            }
        }

        /// <summary>
        ///     This will assert that the log levels work
        /// </summary>
        private static void AssertLogLevels(LogSource logSource, ILogger loggerUnderTest)
        {
            var initialLevel = loggerUnderTest.LogLevel;
            try
            {
                // Assert the log levels
                loggerUnderTest.LogLevel = LogLevels.None;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.False(logSource.IsDebugEnabled());
                Assert.False(logSource.IsInfoEnabled());
                Assert.False(logSource.IsWarnEnabled());
                Assert.False(logSource.IsErrorEnabled());
                Assert.False(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Verbose;
                Assert.True(logSource.IsVerboseEnabled());
                Assert.True(logSource.IsDebugEnabled());
                Assert.True(logSource.IsInfoEnabled());
                Assert.True(logSource.IsWarnEnabled());
                Assert.True(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Debug;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.True(logSource.IsDebugEnabled());
                Assert.True(logSource.IsInfoEnabled());
                Assert.True(logSource.IsWarnEnabled());
                Assert.True(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Info;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.False(logSource.IsDebugEnabled());
                Assert.True(logSource.IsInfoEnabled());
                Assert.True(logSource.IsWarnEnabled());
                Assert.True(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Warn;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.False(logSource.IsDebugEnabled());
                Assert.False(logSource.IsInfoEnabled());
                Assert.True(logSource.IsWarnEnabled());
                Assert.True(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Error;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.False(logSource.IsDebugEnabled());
                Assert.False(logSource.IsInfoEnabled());
                Assert.False(logSource.IsWarnEnabled());
                Assert.True(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());

                loggerUnderTest.LogLevel = LogLevels.Fatal;
                Assert.False(logSource.IsVerboseEnabled());
                Assert.False(logSource.IsDebugEnabled());
                Assert.False(logSource.IsInfoEnabled());
                Assert.False(logSource.IsWarnEnabled());
                Assert.False(logSource.IsErrorEnabled());
                Assert.True(logSource.IsFatalEnabled());
            }
            finally
            {
                loggerUnderTest.LogLevel = initialLevel;
            }
        }

        /// <summary>
        ///     This will assert that the write lines work
        /// </summary>
        private static void AssertWriteLines(LogSource logSource)
        {
            // Now call all write lines
            string message = "Test log line";
            string messageWithArguments = "Test log line with argument {0}";
            var exception = new Exception("Test");

            logSource.Verbose().WriteLine(message);
            logSource.Verbose().WriteLine(messageWithArguments, "One");
            logSource.Verbose().WriteLine(exception, message);
            logSource.Verbose().WriteLine(exception, messageWithArguments, "One");

            logSource.Debug().WriteLine(message);
            logSource.Debug().WriteLine(messageWithArguments, "One");
            logSource.Debug().WriteLine(exception, message);
            logSource.Debug().WriteLine(exception, messageWithArguments, "One");

            logSource.Info().WriteLine(message);
            logSource.Info().WriteLine(messageWithArguments, "One");
            logSource.Info().WriteLine(exception, message);
            logSource.Info().WriteLine(exception, messageWithArguments, "One");

            logSource.Warn().WriteLine(message);
            logSource.Warn().WriteLine(messageWithArguments, "One");
            logSource.Warn().WriteLine(exception, message);
            logSource.Warn().WriteLine(exception, messageWithArguments, "One");

            logSource.Error().WriteLine(message);
            logSource.Error().WriteLine(messageWithArguments, "One");
            logSource.Error().WriteLine(exception, message);
            logSource.Error().WriteLine(exception, messageWithArguments, "One");

            logSource.Fatal().WriteLine(message);
            logSource.Fatal().WriteLine(messageWithArguments, "One");
            logSource.Fatal().WriteLine(exception, message);
            logSource.Fatal().WriteLine(exception, messageWithArguments, "One");
        }
    }
}
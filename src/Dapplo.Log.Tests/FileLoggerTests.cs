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

using System.Threading.Tasks;
using Dapplo.Log.LogFile;
using Dapplo.Log.Tests.Logger;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

namespace Dapplo.Log.Tests
{
    /// <summary>
    /// Tests for the FileLogger
    /// </summary>
    public class FileLoggerTests
    {
        public FileLoggerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        [Fact]
        public void Test_FileLogger_Default()
        {
            var fileLoggerConfiguration = new FileLoggerConfiguration();

            var fileLogger = LogSettings.RegisterDefaultLogger<FileLogger>(fileLoggerConfiguration);

            Assert.Equal(fileLoggerConfiguration.FilenamePattern, fileLogger.FilenamePattern);

            Assert.Equal(fileLogger, LogSettings.DefaultLogger);
        }

        [Fact]
        public async Task TestFileLogger()
        {
            var xUnitLogger = new XUnitLogger(_testOutputHelper)
            {
                LogLevel = LogLevels.Verbose
            };
            LoggerMapper.RegisterLoggerFor<FileLogger>(xUnitLogger);

            // Define a pattern with seconds in it...
            const string filenamePattern = "{Processname}-{Timestamp:yyyyMMddHHmmss}{Extension}";

            using (var forwardingLogger = new ForwardingLogger {LogLevel = LogLevels.Verbose})
            {
                LoggerTestSupport.TestAllLogMethods(forwardingLogger);
                using (var fileLogger = new FileLogger())
                {
                    fileLogger.FilenamePattern = filenamePattern;
                    fileLogger.ArchiveFilenamePattern = filenamePattern;
                    forwardingLogger.ReplacedWith(fileLogger);
                    // Force archiving, as the filename changes
                    await Task.Delay(2000);
                    LoggerTestSupport.TestAllLogMethods(fileLogger);
                }
            }
        }
    }
}
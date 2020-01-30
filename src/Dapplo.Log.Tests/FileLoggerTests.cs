// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

            using var forwardingLogger = new ForwardingLogger { LogLevel = LogLevels.Verbose };
            LoggerTestSupport.TestAllLogMethods(forwardingLogger);
            using var fileLogger = new FileLogger
            {
                FilenamePattern = filenamePattern,
                ArchiveFilenamePattern = filenamePattern
            };
            forwardingLogger.ReplacedWith(fileLogger);
            // Force archiving, as the filename changes
            await Task.Delay(2000);
            LoggerTestSupport.TestAllLogMethods(fileLogger);
        }
    }
}
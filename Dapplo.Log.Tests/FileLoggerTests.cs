//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log.Facade
// 
//  Dapplo.Log.Facade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log.Facade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log.Facade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System.Threading;
using Dapplo.Log.Facade;
using Dapplo.Log.LogFile;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

namespace Dapplo.Log.Tests
{
	public class FileLoggerTests
	{
		private readonly ITestOutputHelper _testOutputHelper;
		public FileLoggerTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void TestFileLogger()
		{
			var xunitLogger = new XUnitLogger(_testOutputHelper)
			{
				LogLevel = LogLevels.Verbose
			};
			LogSettings.RegisterLoggerFor<FileLogger>(xunitLogger);

			// Define a pattern with seconds in it...
			var filenamePattern = "{Processname}-{Timestamp:yyyyMMddHHmmss}{Extension}";
			var fileLoggerConfiguration = new FileLoggerConfiguration
			{
				FilenamePattern = filenamePattern,
				ArchiveFilenamePattern = filenamePattern
			};

			using (var forwardingLogger = new ForwardingLogger() { LogLevel = LogLevels.Verbose })
			{
				LoggerTestSupport.TestAllLogMethods(forwardingLogger);
				using (var fileLogger = new FileLogger(fileLoggerConfiguration) { LogLevel = LogLevels.Verbose })
				{
					forwardingLogger.ReplacedWith(fileLogger);
					//LoggerTestSupport.TestAllLogMethods(fileLogger);
					// Force archiving, as the filename changes
					Thread.Sleep(2000);
					LoggerTestSupport.TestAllLogMethods(fileLogger);
				}
			}
		}
	}
}
 
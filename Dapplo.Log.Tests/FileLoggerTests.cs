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

using System.Threading.Tasks;
using Dapplo.Log.LogFile;
using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.Log.Tests
{
	public class FileLoggerTests
	{
		public FileLoggerTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		private readonly ITestOutputHelper _testOutputHelper;

		[Fact]
		public async Task TestFileLogger()
		{
			var xunitLogger = new XUnitLogger(_testOutputHelper)
			{
				LogLevel = LogLevels.Verbose
			};
			LoggerMapper.RegisterLoggerFor<FileLogger>(xunitLogger);

			// Define a pattern with seconds in it...
			var filenamePattern = "{Processname}-{Timestamp:yyyyMMddHHmmss}{Extension}";

			using (var forwardingLogger = new ForwardingLogger {LogLevel = LogLevels.Verbose})
			{
				LoggerTestSupport.TestAllLogMethods(forwardingLogger);
				using (var fileLogger = new FileLogger())
				{
					fileLogger.FilenamePattern = filenamePattern;
					fileLogger.ArchiveFilenamePattern = filenamePattern;
					forwardingLogger.ReplacedWith(fileLogger);
					//LoggerTestSupport.TestAllLogMethods(fileLogger);
					// Force archiving, as the filename changes
					await Task.Delay(2000);
					LoggerTestSupport.TestAllLogMethods(fileLogger);
				}
			}
		}
	}
}
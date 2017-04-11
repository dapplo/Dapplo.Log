//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.LogFacade
// 
//  Dapplo.LogFacade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.LogFacade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.LogFacade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#region Usings

using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.Log.Tests
{
    public class XUnitLoggerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public XUnitLoggerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        ///     Test XUnitLogger
        /// </summary>
        [Fact]
        public void TestXUnitLogger()
        {
            LoggerTestSupport.TestAllLogMethods(new XUnitLogger(_testOutputHelper));
        }
    }
}
// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapplo.Log.XUnit;
using Xunit;
using Xunit.Abstractions;

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
// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using Xunit;

namespace Dapplo.Log.XUnit
{
    /// <summary>
    ///     xUnit will have tests run parallel, and due to this it won't capture trace output correctly.
    ///     This is where their ITestOutputHelper comes around, but Dapplo.Log can only have one logger.
    ///     This class solves the problem by registering the ITestOutputHelper in the CallContext.
    ///     Every log statement will retrieve the ITestOutputHelper from the context and use it to log.
    /// </summary>
    public class XUnitLogger : AbstractLogger
    {
        private static readonly AsyncLocal<ITestOutputHelper> TestOutputHelperAsyncLocal = new AsyncLocal<ITestOutputHelper>();
        private static readonly AsyncLocal<LogLevels> LogLevelAsyncLocal = new AsyncLocal<LogLevels>();

        /// <summary>
        ///     Prevent the constructor from being use elsewhere, this also makes it impossible to use
        ///     LogSettings.RegisterDefaultLogger
        /// </summary>
        public XUnitLogger(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelperAsyncLocal.Value = testOutputHelper;
            LogLevelAsyncLocal.Value = LogLevels.Info;
        }

        /// <summary>
        ///     LogLevel, this can give a different result pro xUnit test...
        ///     It will depend on the RegisterLogger value which was used in the current xUnit test
        /// </summary>
        public override LogLevels LogLevel
        {
            get => LogLevelAsyncLocal.Value;
            set => LogLevelAsyncLocal.Value = value;
        }

        /// <summary>
        ///     If the level is enabled, this returns true
        ///     The level depends on what the xUnit test used in the RegisterLogger
        /// </summary>
        /// <param name="logLevel">LogLevels enum</param>
        /// <param name="logSource">optional LogSource</param>
        /// <returns>true if the level is enabled</returns>
        public override bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            return logLevel != LogLevels.None && logLevel >= LogLevel;
        }

        /// <summary>
        ///     There is not Write for the ITestOutputHelper
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="messageTemplate"></param>
        /// <param name="logParameters"></param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            throw new InvalidOperationException("ITestOutputHelper doesn't provide a Write(), so this is illegal to use");
        }

        /// <summary>
        ///     Writes the output to the testOutputHelper
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string</param>
        /// <param name="logParameters">params object</param>
        public override void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            var testOutputHelper = TestOutputHelperAsyncLocal.Value;
            if (testOutputHelper is null)
            {
                throw new ArgumentNullException(nameof(testOutputHelper), "Couldn't find a ITestOutputHelper in the CallContext, maybe you are trying to log outside your testcase?");
            }
            testOutputHelper.WriteLine(Format(logInfo, messageTemplate, logParameters));
        }
    }
}
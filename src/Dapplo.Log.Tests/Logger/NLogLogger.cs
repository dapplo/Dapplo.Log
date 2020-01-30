// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NLog;

namespace Dapplo.Log.Tests.Logger
{
    /// <summary>
    ///     Wrapper for Dapplo.Log.ILogger -> NLog.Logger
    /// </summary>
    public class NLogLogger : AbstractLogger
    {
        /// <inheritdoc />
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            LogManager.GetLogger(logInfo.Source.Source).Log(Convert(logInfo.LogLevel), messageTemplate, logParameters);
        }

        private static LogLevel Convert(LogLevels logLevel) => logLevel switch
        {
            LogLevels.Info => NLog.LogLevel.Info,
            LogLevels.Warn => NLog.LogLevel.Warn,
            LogLevels.Error => NLog.LogLevel.Error,
            LogLevels.Fatal => NLog.LogLevel.Fatal,
            _ => NLog.LogLevel.Debug,
        };
    }
}
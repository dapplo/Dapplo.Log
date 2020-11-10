// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.Log
{
    /// <summary>
    ///     A null logger, doesn't log anything!
    ///     This can be used to silence certain LogSources
    /// </summary>
    public class NullLogger : AbstractLogger
    {
        /// <summary>
        ///     Always returns false, there is no logging
        /// </summary>
        /// <param name="logLevel">LogLevel</param>
        /// <param name="logSource">optional LogSource</param>
        /// <returns>false</returns>
        public override bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null)
        {
            return false;
        }
    }
}
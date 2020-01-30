// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dapplo.Log
{
    /// <summary>
    ///     Log level for the log, default(LogLevel) gives None which doesn't log anything
    ///     LogLevels.None is actually checked internally, before IsLogLevelEnabled.
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        ///     Default, no logging
        /// </summary>
        None,

        /// <summary>
        ///     Verbose logs pretty much everything
        /// </summary>
        Verbose,

        /// <summary>
        ///     Debugging information, usually needed when troubleshooting
        /// </summary>
        Debug,

        /// <summary>
        ///     Informational logging
        /// </summary>
        Info,

        /// <summary>
        ///     Warn that something didn't went well
        /// </summary>
        Warn,

        /// <summary>
        ///     Used for logging real errors
        /// </summary>
        Error,

        /// <summary>
        ///     Used for unrecoverable problems
        /// </summary>
        Fatal
    }
}
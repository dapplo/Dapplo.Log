// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#define DEBUG

using System.Diagnostics;

namespace Dapplo.Log.Loggers
{
    /// <summary>
    ///     A debug logger, the simplest implementation for logging debug messages
    /// </summary>
    public class DebugLogger : AbstractLogger
    {
#if !PCL
        /// <inheritdoc />
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Debug.Write(Format(logInfo, messageTemplate, logParameters));
        }
#else
        /// <inheritdoc />
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            // This outputs a Write to a new line, which is unfortunate but better than nothing...
            Debug.WriteLine(Format(logInfo, messageTemplate, logParameters));
        }

        /// <inheritdoc />
        public override void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Debug.WriteLine(Format(logInfo, messageTemplate, logParameters));
        }

#endif
    }
}
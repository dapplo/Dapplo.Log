#region Dapplo 2016-2019 - GNU Lesser General Public License

// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#endregion

#if !PCL

using System.Diagnostics;

namespace Dapplo.Log.Loggers
{
    /// <summary>
    ///     A trace logger, the simplest implementation for logging trace messages
    /// </summary>
    public class TraceLogger : AbstractLogger
    {
        /// <summary>
        ///     Write a message with parameters to the Trace
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string with the message template</param>
        /// <param name="logParameters">object array with the parameters for the template</param>
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Trace.Write(Format(logInfo, messageTemplate, logParameters));
        }
    }
}
#endif
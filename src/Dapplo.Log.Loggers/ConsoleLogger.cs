#region Dapplo 2016-2019 - GNU Lesser General Public License

// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#endregion

#if !PCL

using System;

namespace Dapplo.Log.Loggers
{
    /// <summary>
    ///     A console logger, the simplest implementation for logging messages to a console
    /// </summary>
    public class ConsoleLogger : AbstractLogger
    {
        /// <inheritdoc />
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            Console.Write(Format(logInfo, messageTemplate, logParameters));
        }
    }
}
#endif
// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Log
{
    /// <summary>
    ///     The extensions for making logging easy and flexible
    /// </summary>
    public static class LogInfoExtensions
    {
        /// <summary>
        ///     This extension method passes the messageTemplate and parameters to the loggers Write method.
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string with formatting</param>
        /// <param name="logParameters">parameters for the formatting</param>
        public static void Write(this LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            if (logInfo is null)
            {
                return;
            }

            ILogger[] array = LogSettings.LoggerLookup(logInfo.Source);
            for (int i = 0; i < array.Length; i++)
            {
                ILogger logger = array[i];
                logger.Write(logInfo, messageTemplate, logParameters);
            }
        }

        /// <summary>
        ///     This extension method passes the messageTemplate and parameters to the loggers WriteLine method.
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string with formatting</param>
        /// <param name="logParameters">parameters for the formatting</param>
        public static void WriteLine(this LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            if (logInfo is null)
            {
                return;
            }

            ILogger[] array = LogSettings.LoggerLookup(logInfo.Source);
            for (int i = 0; i < array.Length; i++)
            {
                ILogger logger = array[i];
                logger.WriteLine(logInfo, messageTemplate, logParameters);
            }
        }

        /// <summary>
        ///     This extension method passes the Exception, messageTemplate and parameters to the loggers WriteLine method.
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="exception">Exception to log</param>
        /// <param name="messageTemplate">string with formatting</param>
        /// <param name="logParameters">parameters for the formatting</param>
        public static void WriteLine(this LogInfo logInfo, Exception exception, string messageTemplate = null, params object[] logParameters)
        {
            if (logInfo is null)
            {
                return;
            }

            ILogger[] array = LogSettings.LoggerLookup(logInfo.Source);
            for (int i = 0; i < array.Length; i++)
            {
                ILogger logger = array[i];
                logger.WriteLine(logInfo, exception, messageTemplate, logParameters);
            }
        }
    }
}
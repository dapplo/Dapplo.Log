// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Log
{
    /// <summary>
    ///     This is the interface used for internal logging.
    ///     The idea is that you can implement a small wrapper for you favorite logger which implements this interface.
    ///     Assign it to the HttpExtensionsGlobals.Logger and Dapplo.HttpExtensions will start logger with your class.
    ///     A TraceLogger implementation is supplied, so you can see some output while your project is in development.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Configure the logger with the specified ILoggerConfiguration
        /// </summary>
        /// <param name="loggerConfiguration">ILoggerConfiguration</param>
        void Configure(ILoggerConfiguration loggerConfiguration);

        /// <summary>
        /// The LogLevel, initially it takes the ILoggerConfiguration.DefaultLogLevel
        /// </summary>
        LogLevels LogLevel { get; set; }

        /// <summary>
        ///     This can be overriden to format the line message differently
        /// </summary>
        /// <param name="logInfo">LogInfo</param>
        /// <param name="messageTemplate">string</param>
        /// <param name="parameters">object array with the parameters</param>
        /// <returns>string formatted</returns>
        string Format(LogInfo logInfo, string messageTemplate, object[] parameters);

        /// <summary>
        ///     This is called when your logger was the default, and is replaced with a different one.
        ///     In this method a buffer logger could place all it's content into the new logger.
        /// </summary>
        /// <param name="newLogger">ILogger</param>
        void ReplacedWith(ILogger newLogger);

        /// <summary>
        ///     A simple test, to see if the log level is enabled.
        ///     Note: level == LogLevels.None should always return false
        ///     Level == LogLevels.None is actually checked in the extension
        ///     Optionally the LogSource for which this is requested is supplied and can be used.
        /// </summary>
        /// <param name="logLevel">LogLevels</param>
        /// <param name="logSource">Optional LogSource</param>
        /// <returns>true if enabled</returns>
        bool IsLogLevelEnabled(LogLevels logLevel, LogSource logSource = null);

        /// <summary>
        ///     This writes the LogInfo, messageTemplate and the log parameters to the log
        /// </summary>
        /// <param name="logInfo">LogInfo with source, timestamp, level etc</param>
        /// <param name="messageTemplate">Message (template) with formatting</param>
        /// <param name="logParameters">Parameters for the template</param>
        void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters);

        /// <summary>
        ///     This writes the LogInfo, messageTemplate and the log parameters to the log, finishing with a newline
        /// </summary>
        /// <param name="logInfo">LogInfo with source, timestamp, level etc</param>
        /// <param name="messageTemplate">Message (template) with formatting</param>
        /// <param name="logParameters">Parameters for the template</param>
        void WriteLine(LogInfo logInfo, string messageTemplate, params object[] logParameters);

        /// <summary>
        ///     This writes the LogInfo, exception, messageTemplate and the log parameters to the log, finishing with a newline
        /// </summary>
        /// <param name="logInfo">LogInfo with source, timestamp, level etc</param>
        /// <param name="exception">exception to log</param>
        /// <param name="messageTemplate">Message (template) with formatting</param>
        /// <param name="logParameters">Parameters for the template</param>
        void WriteLine(LogInfo logInfo, Exception exception, string messageTemplate = null, params object[] logParameters);
    }
}
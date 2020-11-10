// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Dapplo.Log
{
    /// <summary>
    ///     The extensions for making logging easy and flexible
    /// </summary>
    public static class LogSourceExtensions
    {
        /// <summary>
        ///     Test if LogLevels.Debug is enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsDebugEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Debug);
        }

        /// <summary>
        ///     Test if LogLevels.Error enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsErrorEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Error);
        }

        /// <summary>
        ///     Test if LogLevels.Fatal is enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsFatalEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Fatal);
        }

        /// <summary>
        ///     Test if LogLevels.Info is enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsInfoEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Info);
        }

        /// <summary>
        ///     Test if LogLevels.Verbose is enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsVerboseEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Verbose);
        }

        /// <summary>
        ///     Test if LogLevels.Warn is enabled
        /// </summary>
        /// <returns>true if enabled</returns>
        public static bool IsWarnEnabled(this LogSource logSource)
        {
            return IsLogLevelEnabled(logSource, LogLevels.Warn);
        }

        /// <summary>
        ///     A simple test, to see if the log level is enabled.
        ///     Note: logLevel == LogLevels.None should always return false
        ///     logLevel == LogLevels.None is actually checked in the extension
        /// </summary>
        /// <param name="logSource">LogSource</param>
        /// <param name="logLevel">LogLevels</param>
        /// <returns>true if there are loggers which will log for the LogSource</returns>
        private static bool IsLogLevelEnabled(LogSource logSource, LogLevels logLevel)
        {
            // "Fail-fast"
            if (logSource is null || logLevel == LogLevels.None)
            {
                return false;
            }

            // Check if there are any loggers
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var x in LogSettings.LoggerLookup(logSource))
            {
                if (x.LogLevel != LogLevels.None && x.IsLogLevelEnabled(logLevel, logSource))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     This helper method will create LogInfo, if there is anything to log to
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="logLevel">LogLevels enum for the LogInfo</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        private static LogInfo CreateLogInfo(LogSource logSource, LogLevels logLevel, int lineNumber, string methodName)
        {
            if (logSource is null || !IsLogLevelEnabled(logSource, logLevel))
            {
                return null;
            }
            return new LogInfo(logSource, methodName, lineNumber, logLevel);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Debug
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Debug(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Debug, lineNumber, methodName);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Error
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Error(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Error, lineNumber, methodName);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Fatal
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Fatal(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Fatal, lineNumber, methodName);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Info
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Info(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Info, lineNumber, methodName);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Verbose
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Verbose(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Verbose, lineNumber, methodName);
        }

        /// <summary>
        ///     This extension will create LogInfo, for LogLevels.Warn
        /// </summary>
        /// <param name="logSource">LogContext is the context (source) from where the log entry came</param>
        /// <param name="lineNumber">Should be set by the compiler, int lineNumber of the log statement</param>
        /// <param name="methodName">Should be set by the compiler, is the calling method</param>
        /// <returns>LogInfo</returns>
        public static LogInfo Warn(this LogSource logSource, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
        {
            return CreateLogInfo(logSource, LogLevels.Warn, lineNumber, methodName);
        }
    }
}
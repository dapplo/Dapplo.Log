// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Dapplo.Log
{
    /// <summary>
    ///     This is to specify global settings for the Log "framework"
    /// </summary>
    public static class LogSettings
    {
        /// <summary>
        ///     Default ILoggerConfiguration
        /// </summary>
        public static ILoggerConfiguration DefaultLoggerConfiguration { get; set; }

        /// <summary>
        ///     The default logger used, if the logger implements IDisposable it will be disposed if another logger is assigned
        /// </summary>
        public static ILogger DefaultLogger {
            get => DefaultLoggerArray.Length > 0 ? DefaultLoggerArray[0] : null;
            set => DefaultLoggerArray = new[] {value};
        }

        /// <summary>
        ///     The constructed default logger array
        /// </summary>
        internal static ILogger[] DefaultLoggerArray { get; private set; } =
#if NETSTANDARD1_3 || NETSTANDARD2_0
            Array.Empty<ILogger>();
#else
            {};
#endif

        /// <summary>
        ///     This function is responsible for finding the right loggers for a LogSource.
        ///     Default implementation is from the LoggerMapper.
        /// </summary>
        public static Func<LogSource, ILogger[]> LoggerLookup { get; set; } = x => x.Loggers();

        /// <summary>
        /// This function converts an exception to a stacktrace string.
        /// This can come in handy if replaced with exception => exception.ToStringDemystified() from the NuGet package Ben.Demystifier
        /// </summary>
        public static Func<Exception, string> ExceptionToStacktrace { get; set; } = exception => exception.ToString();

        /// <summary>
        ///     Takes care of registering the default logger with a logger, configuration and arguments
        /// </summary>
        /// <typeparam name="TLogger">Type for the logger</typeparam>
        /// <param name="loggerConfiguration">ILoggerConfiguration to configure the logger with</param>
        /// <param name="arguments">params</param>
        /// <returns>The newly created logger, this might be needed elsewhere</returns>
        public static TLogger RegisterDefaultLogger<TLogger>(ILoggerConfiguration loggerConfiguration = default, params object[] arguments) where TLogger : class, ILogger
        {
            var newLogger = (TLogger) Activator.CreateInstance(typeof(TLogger), arguments);
            if (loggerConfiguration != null)
            {
                newLogger.Configure(loggerConfiguration);
            }
            ReplaceDefaultLogger(newLogger);
            return newLogger;
        }

        /// <summary>
        ///     Takes care of registering the default logger with a logger and arguments
        /// </summary>
        /// <typeparam name="TLogger">Type for the logger</typeparam>
        /// <param name="arguments">params</param>
        /// <returns>The newly created logger, this might be needed elsewhere</returns>
        public static TLogger RegisterDefaultLogger<TLogger>(params object[] arguments) where TLogger : class, ILogger
        {
            var newLogger = (TLogger)Activator.CreateInstance(typeof(TLogger), arguments);
            ReplaceDefaultLogger(newLogger);
            return newLogger;
        }

        /// <summary>
        ///     Takes care of registering the default logger with a logger, LogLevel and arguments
        /// </summary>
        /// <typeparam name="TLogger">Type for the logger</typeparam>
        /// <param name="logLevel"></param>
        /// <param name="arguments">params</param>
        /// <returns>The newly created logger, this might be needed elsewhere</returns>
        public static TLogger RegisterDefaultLogger<TLogger>(LogLevels logLevel, params object[] arguments) where TLogger : class, ILogger
        {
            var newLogger = (TLogger) Activator.CreateInstance(typeof(TLogger), arguments);
            newLogger.LogLevel = logLevel;
            ReplaceDefaultLogger(newLogger);
            return newLogger;
        }

        /// <summary>
        ///     Helper method to replace the default logger
        /// </summary>
        /// <param name="newLogger">ILogger</param>
        private static void ReplaceDefaultLogger<TLogger>(TLogger newLogger) where TLogger : class, ILogger
        {
            var previousDefaultLogger = DefaultLogger;

            DefaultLogger = newLogger;
            previousDefaultLogger?.ReplacedWith(newLogger);

            // Call Dispose if the logger implements IDisposable
            var previousDefaultLoggerAsDisposable = previousDefaultLogger as IDisposable;
            previousDefaultLoggerAsDisposable?.Dispose();
        }
    }
}
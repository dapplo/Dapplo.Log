#region Dapplo 2016-2019 - GNU Lesser General Public License

// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Linq;
using System.Collections;

#if NETSTANDARD2_0 || NETCOREAPP1_6 || NET45
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif

namespace Dapplo.Log
{
    /// <summary>
    ///     The logic for the mapping of loggers
    /// </summary>
    public static class LoggerMapper
    {
        private static readonly ILogger[] EmptyLoggerArray = {};
        /// <summary>
        ///     The lookup table for finding the loggers for a LogSource.Source
        /// </summary>
#if NETSTANDARD2_0 || NETCOREAPP1_6 || NET45
		private static ConcurrentDictionary<string, ILogger[]> LoggerMap { get; } = new ConcurrentDictionary<string, ILogger[]>();
#else
        private static Dictionary<string, ILogger[]> LoggerMap { get; } = new Dictionary<string, ILogger[]>();
#endif

        /// <summary>
        ///     The default lookup implementation
        /// </summary>
        /// <param name="logSource">LogSource to find loggers for</param>
        /// <returns>enumerable with loggers</returns>
        public static ILogger[] Loggers(this LogSource logSource)
        {
            if (logSource is null)
            {
                return EmptyLoggerArray;
            }

            if (LoggerMap.TryGetValue(logSource.Source, out var possibleLoggers) && possibleLoggers.Length > 0)
            {
                return possibleLoggers;
            }

            return LogSettings.DefaultLoggerArray;
        }

        /// <summary>
        ///     Takes care of registering the supplied logger for a certain source
        /// </summary>
        /// <param name="source">string for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void RegisterLoggerFor(string source, ILogger logger)
        {
            if (source is null)
            {
                return;
            }
            if (!LoggerMap.TryGetValue(source, out var loggersForSource))
            {
                loggersForSource = new[]{ logger };
#if NETSTANDARD2_0 || NETCOREAPP1_6 || NET45
                LoggerMap.TryAdd(source, loggersForSource);
#else
                LoggerMap.Add(source, loggersForSource);
#endif
            }

            // Only add if it's not yet set
            if (loggersForSource.Contains(logger))
            {
                return;
            }

            // Take a small penalty for creating a new list, vs having the need to lock when evaluating
            var newLoggersForSource = loggersForSource.Concat(new[]{ logger }).ToArray();
            LoggerMap[source] = newLoggersForSource;
        }

        /// <summary>
        ///     Takes care of registering the supplied logger for a LogSource
        /// </summary>
        /// <param name="logSource">LogSource to register for</param>
        /// <param name="logger">ILogger to register</param>
        public static void LogTo(this LogSource logSource, ILogger logger)
        {
            RegisterLoggerFor(logSource.Source, logger);
        }

        /// <summary>
        ///     Takes care of registering the supplied logger for a certain source
        /// </summary>
        /// <param name="type">Type for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void RegisterLoggerFor(Type type, ILogger logger)
        {
            RegisterLoggerFor(type.FullName, logger);
        }

        /// <summary>
        ///     Takes care of registering the supplied logger for a certain source
        /// </summary>
        /// <typeparam name="TType">Type for the source</typeparam>
        /// <param name="logger">ILogger to register</param>
        public static void RegisterLoggerFor<TType>(ILogger logger)
        {
            RegisterLoggerFor(typeof(TType).FullName, logger);
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a certain source
        /// </summary>
        /// <param name="source">string for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void DeRegisterLoggerFor(string source, ILogger logger)
        {
            if (!LoggerMap.TryGetValue(source, out var loggersForSource))
            {
                return;
            }
            // Take penalty of creating a new list vs locking
            var newLoggersForSource = loggersForSource.Where(l => l != logger).ToArray();
            LoggerMap[source] = newLoggersForSource;

            if (newLoggersForSource.Length == 0)
            {
#if NETSTANDARD2_0 || NETCOREAPP1_6 || NET45
                LoggerMap.TryRemove(source, out _);
#else
                LoggerMap.Remove(source);
#endif
            }
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a LogSource
        /// </summary>
        /// <param name="logSource">LogSource for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void DeRegisterLoggerFor(LogSource logSource, ILogger logger)
        {
            DeRegisterLoggerFor(logSource.Source, logger);
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a certain source
        /// </summary>
        /// <param name="type">Type for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void DeRegisterLoggerFor(Type type, ILogger logger)
        {
            DeRegisterLoggerFor(type.FullName, logger);
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a certain source
        /// </summary>
        /// <typeparam name="TType">Type for the source</typeparam>
        /// <param name="logger">ILogger to register</param>
        public static void DeRegisterLoggerFor<TType>(ILogger logger)
        {
            DeRegisterLoggerFor(typeof(TType).FullName, logger);
        }
    }
}
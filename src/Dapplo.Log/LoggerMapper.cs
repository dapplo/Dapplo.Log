#region Dapplo 2016-2018 - GNU Lesser General Public License

//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2016-2018 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log
// 
//  Dapplo.Log is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Collections.Generic;
#if NETCOREAPP1_6 || NET45
using System.Collections.Concurrent;
#endif

#endregion

namespace Dapplo.Log
{
    /// <summary>
    ///     The logic for the mapping of loggers
    /// </summary>
    public static class LoggerMapper
    {
        /// <summary>
        ///     The lookup table for finding the loggers for a LogSource.Source
        /// </summary>
#if NETCOREAPP1_6 || NET45
		private static IDictionary<string, IList<ILogger>> LoggerMap { get; } = new ConcurrentDictionary<string, IList<ILogger>>();
#else
        private static IDictionary<string, IList<ILogger>> LoggerMap { get; } = new Dictionary<string, IList<ILogger>>();
#endif

        /// <summary>
        ///     The default lookup implementation
        /// </summary>
        /// <param name="logSource">LogSource to find loggers for</param>
        /// <returns>enumerable with loggers</returns>
        public static IEnumerable<ILogger> Loggers(this LogSource logSource)
        {
            if (logSource == null)
            {
                yield break;
            }
            IList<ILogger> loggers;
            var foundLogger = false;
            if (LoggerMap.TryGetValue(logSource.Source, out loggers))
            {
                foreach (var logger in loggers)
                {
                    if (logger != null)
                    {
                        foundLogger = true;
                        yield return logger;
                    }
                }
            }
            var defaultLogger = LogSettings.DefaultLogger;
            if (!foundLogger && defaultLogger != null)
            {
                yield return defaultLogger;
            }
        }

        /// <summary>
        ///     Takes care of registering the supplied logger for a certain source
        /// </summary>
        /// <param name="source">string for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void RegisterLoggerFor(string source, ILogger logger)
        {
            IList<ILogger> loggersForSource;
            if (!LoggerMap.TryGetValue(source, out loggersForSource))
            {
                loggersForSource = new List<ILogger>();
                LoggerMap.Add(source, loggersForSource);
            }
            if (!loggersForSource.Contains(logger))
            {
                loggersForSource.Add(logger);
            }
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
        public static void DeregisterLoggerFor(string source, ILogger logger)
        {
            IList<ILogger> loggersForSource;
            if (LoggerMap.TryGetValue(source, out loggersForSource))
            {
                loggersForSource.Remove(logger);
                if (loggersForSource.Count == 0)
                {
                    LoggerMap.Remove(source);
                }
            }
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a LogSource
        /// </summary>
        /// <param name="logSource">LogSource for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void DeregisterLoggerFor(LogSource logSource, ILogger logger)
        {
            DeregisterLoggerFor(logSource.Source, logger);
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a certain source
        /// </summary>
        /// <param name="type">Type for the source</param>
        /// <param name="logger">ILogger to register</param>
        public static void DeregisterLoggerFor(Type type, ILogger logger)
        {
            DeregisterLoggerFor(type.FullName, logger);
        }

        /// <summary>
        ///     Takes care of de-registering the supplied logger for a certain source
        /// </summary>
        /// <typeparam name="TType">Type for the source</typeparam>
        /// <param name="logger">ILogger to register</param>
        public static void DeregisterLoggerFor<TType>(ILogger logger)
        {
            DeregisterLoggerFor(typeof(TType).FullName, logger);
        }
    }
}
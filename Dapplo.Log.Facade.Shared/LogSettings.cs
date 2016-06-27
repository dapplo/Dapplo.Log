//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2015-2016 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Log.Facade
// 
//  Dapplo.Log.Facade is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Log.Facade is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Log.Facade. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Dapplo.Log.Facade
{
	/// <summary>
	///     This is to specify global settings for the LogFacade "framework"
	/// </summary>
	public static class LogSettings
	{
		/// <summary>
		/// The lookup table for finding the loggers for a LogSource.Source
		/// </summary>
		private static IDictionary<string, IList<ILogger>> LoggerMap { get; } = new ConcurrentDictionary<string, IList<ILogger>>();

		/// <summary>
		/// The default lookup implementation
		/// </summary>
		/// <param name="logSource">LogSource to find loggers for</param>
		/// <returns>enumerable with loggers</returns>
		public static IEnumerable<ILogger> LookupLoggersFor(LogSource logSource)
		{
			IList<ILogger> loggers;
			bool foundLogger = false;
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
			if (!foundLogger && DefaultLogger != null)
			{
				yield return DefaultLogger;
			}
		}

		/// <summary>
		/// This function is responsible for finding the right loggers for a LogSource.
		/// </summary>
		public static Func<LogSource, IEnumerable<ILogger>> LoggerLookup { get; set; } = x => LookupLoggersFor(x).ToList();

		/// <summary>
		///     A default LogLevel, which loggers use when nothing is specified
		/// </summary>
		public static LogLevels DefaultLogLevel { get; set; } = LogLevels.Info;

		/// <summary>
		///     The default logger used, if the logger implements IDisposable it will be disposed if another logger is assigned
		/// </summary>
		public static ILogger DefaultLogger { get; set; }

		/// <summary>
		///     Defines if the Source is written like d.l.LoggerTest (default) or Dapplo.Log.Facade.LoggerTest
		/// </summary>
		public static bool UseShortSource { get; set; } = true;

		/// <summary>
		/// Timestamp format which is used in the output, when outputting the LogInfo details
		/// </summary>
		public static string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

		/// <summary>
		/// Default line format for loggers which use the DefaultFormatter.
		/// The first argument is the LogInfo, the second the message + parameters formatted
		/// </summary>
		public static string LogLineFormat { get; set; } = "{0} - {1}";

		/// <summary>
		/// This function can be set to have the LogInfo output with custom formatting
		/// </summary>
		public static Func<LogInfo, string> LogInfoFormatter { get; set; }

		/// <summary>
		/// This function can be changed to change the way the message is formatted 
		/// </summary>
		public static Func<string, object[], string> DefaultMessageFormatter { get; set; } = (messageTemplate, logParameters) =>
		{
			string message = messageTemplate;
			// Test if there are parameters, if not there is no need to format it!
			if (logParameters != null && logParameters.Length >= 0)
			{
				message = string.Format(messageTemplate, logParameters);
			}
			return message;
		};

		/// <summary>
		/// This function can be changed to change the default formatter, and output the line differently.
		/// First argument is the LogInfo, second the messageTemplate, third the parameters.
		/// </summary>
		public static Func<LogInfo, string, object[], string> DefaultLineFormatter { get; set; } = (logInfo, messageTemplate, logParameters) => string.Format(LogLineFormat, logInfo, DefaultMessageFormatter(messageTemplate, logParameters));

		/// <summary>
		/// Takes care of registering the default logger
		/// </summary>
		/// <typeparam name="TLogger">Type for the logger</typeparam>
		/// <param name="logLevel">LogLevels or if none the default level is taken</param>
		/// <returns>The newly created logger, this might be needed elsewhere</returns>
		public static TLogger RegisterDefaultLogger<TLogger>(LogLevels logLevel = default(LogLevels)) where TLogger : ILogger, new()
		{
			var newLogger = new TLogger {LogLevel = logLevel == LogLevels.None ? DefaultLogLevel : logLevel};
			ReplaceDefaultLogger(newLogger);
			return newLogger;
		}

		/// <summary>
		/// Takes care of registering the default logger with a logger which needs arguments
		/// </summary>
		/// <typeparam name="TLogger">Type for the logger</typeparam>
		/// <param name="logLevel">LogLevels or if none the default level is taken</param>
		/// <param name="arguments">params</param>
		/// <returns>The newly created logger, this might be needed elsewhere</returns>
		public static TLogger RegisterDefaultLogger<TLogger>(LogLevels logLevel = default(LogLevels), params object[] arguments) where TLogger : ILogger
		{
			var newLogger = (TLogger)Activator.CreateInstance(typeof(TLogger), arguments);
			newLogger.LogLevel = logLevel == LogLevels.None ? DefaultLogLevel : logLevel;
			ReplaceDefaultLogger(newLogger);
			return newLogger;
		}

		/// <summary>
		/// Assign the new default logger, but make sure the previous DefaultLogger is disposed (if IDisposable is implemented)
		/// </summary>
		/// <param name="newLogger">ILogger</param>
		private static void ReplaceDefaultLogger(ILogger newLogger)
		{
			var previousDefaultLogger = DefaultLogger;
			DefaultLogger = newLogger;
			previousDefaultLogger?.ReplacedWith(newLogger);

			// Call Dispose if the logger implements IDisposable
			IDisposable previousDefaultLoggerAsDisposable = previousDefaultLogger as IDisposable;
			previousDefaultLoggerAsDisposable?.Dispose();
		}

		/// <summary>
		/// Takes care of registering the supplied logger for a certain source
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
		/// Takes care of registering the supplied logger for a LogSource
		/// </summary>
		/// <param name="logSource">LogSource to register for</param>
		/// <param name="logger">ILogger to register</param>
		public static void RegisterLoggerFor(LogSource logSource, ILogger logger)
		{
			RegisterLoggerFor(logSource.Source, logger);
		}

		/// <summary>
		/// Takes care of registering the supplied logger for a certain source
		/// </summary>
		/// <param name="type">Type for the source</param>
		/// <param name="logger">ILogger to register</param>
		public static void RegisterLoggerFor(Type type, ILogger logger)
		{
			RegisterLoggerFor(type.FullName, logger);
		}

		/// <summary>
		/// Takes care of registering the supplied logger for a certain source
		/// </summary>
		/// <typeparam name="TType">Type for the source</typeparam>
		/// <param name="logger">ILogger to register</param>
		public static void RegisterLoggerFor<TType>(ILogger logger)
		{
			RegisterLoggerFor(typeof(TType).FullName, logger);
		}

		/// <summary>
		/// Takes care of de-registering the supplied logger for a certain source
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
		/// Takes care of de-registering the supplied logger for a LogSource
		/// </summary>
		/// <param name="logSource">LogSource for the source</param>
		/// <param name="logger">ILogger to register</param>
		public static void DeregisterLoggerFor(LogSource logSource, ILogger logger)
		{
			DeregisterLoggerFor(logSource.Source, logger);
		}

		/// <summary>
		/// Takes care of de-registering the supplied logger for a certain source
		/// </summary>
		/// <param name="type">Type for the source</param>
		/// <param name="logger">ILogger to register</param>
		public static void DeregisterLoggerFor(Type type, ILogger logger)
		{
			DeregisterLoggerFor(type.FullName, logger);
		}

		/// <summary>
		/// Takes care of de-registering the supplied logger for a certain source
		/// </summary>
		/// <typeparam name="TType">Type for the source</typeparam>
		/// <param name="logger">ILogger to register</param>
		public static void DeregisterLoggerFor<TType>(ILogger logger)
		{
			DeregisterLoggerFor(typeof(TType).FullName, logger);
		}
	}
}
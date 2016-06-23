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

#region using

using System;

#endregion

namespace Dapplo.Log.Facade
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
		///     The LogLevels enum this logger uses, default can be taken from the LogSettings.DefaultLevel
		/// </summary>
		LogLevels LogLevel { get; set; }

		/// <summary>
		/// This function can be changed to format the line message differently
		/// First argument is the LogInfo, second the messageTemplate, third the parameters
		/// </summary>
		Func<LogInfo, string, object[], string> Format { get; set; }

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
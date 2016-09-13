#region Dapplo 2016 - GNU Lesser General Public License

//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2016 Dapplo
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

#endregion

namespace Dapplo.Log
{
	/// <summary>
	///     The extensions for making logging easy and flexible
	/// </summary>
	public static class LogInfoExtensions
	{
		/// <summary>
		/// This extension method passes the messageTemplate and parameters to the loggers Write method.
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="messageTemplate">string with formatting</param>
		/// <param name="logParameters">parameters for the formatting</param>
		public static void Write(this LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			if (logInfo == null)
			{
				return;
			}
			foreach (var logger in LogSettings.LoggerLookup(logInfo.Source))
			{
				logger.Write(logInfo, messageTemplate, logParameters);
			}
		}

		/// <summary>
		/// This extension method passes the messageTemplate and parameters to the loggers WriteLine method.
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="messageTemplate">string with formatting</param>
		/// <param name="logParameters">parameters for the formatting</param>
		public static void WriteLine(this LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			if (logInfo == null)
			{
				return;
			}
			foreach (var logger in LogSettings.LoggerLookup(logInfo.Source))
			{
				logger.WriteLine(logInfo, messageTemplate, logParameters);
			}
		}

		/// <summary>
		/// This extension method passes the Exception, messageTemplate and parameters to the loggers WriteLine method.
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="exception">Exception to log</param>
		/// <param name="messageTemplate">string with formatting</param>
		/// <param name="logParameters">parameters for the formatting</param>
		public static void WriteLine(this LogInfo logInfo, Exception exception, string messageTemplate = null,
			params object[] logParameters)
		{
			if (logInfo == null)
			{
				return;
			}
			foreach (var logger in LogSettings.LoggerLookup(logInfo.Source))
			{
				logger.WriteLine(logInfo, exception, messageTemplate, logParameters);
			}
		}
	}
}
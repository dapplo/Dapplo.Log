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
using System.Collections.Generic;

#endregion

namespace Dapplo.Log.Loggers
{
	/// <summary>
	///     A console logger with colors, an implementation for logging messages to a console where every log level has a different color.
	/// </summary>
	public class ColorConsoleLogger : AbstractLogger
	{
		/// <summary>
		/// Used to log the writes, so the colors and lines don't mix when used from multiple threads.
		/// </summary>
		private readonly object _lock = new object();

		/// <summary>
		/// Maps the LogLevels level to a foreground color, levels that are not available map to white.
		/// </summary>
		public IDictionary<LogLevels, ConsoleColor> ForegroundColors { get; set; } = new Dictionary<LogLevels, ConsoleColor>
		{
			{LogLevels.Verbose, ConsoleColor.DarkGray},
			{LogLevels.Debug, ConsoleColor.Gray},
			{LogLevels.Info, ConsoleColor.White},
			{LogLevels.Warn, ConsoleColor.Yellow},
			{LogLevels.Error, ConsoleColor.Red},
			{LogLevels.Fatal, ConsoleColor.DarkRed}
		};

		/// <summary>
		/// Maps the LogLevels level to a background color, levels that are not available map to black.
		/// </summary>
		public IDictionary<LogLevels, ConsoleColor> BackgroundColors { get; set; } = new Dictionary<LogLevels, ConsoleColor>()
		{
			{LogLevels.Fatal, ConsoleColor.White}
		};

		/// <summary>
		/// Write a message with parameters to the Console
		/// </summary>
		/// <param name="logInfo">LogInfo</param>
		/// <param name="messageTemplate">string with the message template</param>
		/// <param name="logParameters">object array with the parameters for the template</param>
		public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			ConsoleColor backgroundColor;
			if (!BackgroundColors.TryGetValue(logInfo.LogLevel, out backgroundColor))
			{
				backgroundColor = ConsoleColor.Black;
			}
			ConsoleColor foregroundColor;
			if (!ForegroundColors.TryGetValue(logInfo.LogLevel, out foregroundColor))
			{
				foregroundColor = ConsoleColor.White;
			}
			// Make sure the colors don't mix, as the write is not atomic.
			lock (_lock)
			{
				Console.BackgroundColor = backgroundColor;
				Console.ForegroundColor = foregroundColor;
				Console.Write(Format(logInfo, messageTemplate, logParameters));
				Console.ResetColor();
			}
		}
	}
}
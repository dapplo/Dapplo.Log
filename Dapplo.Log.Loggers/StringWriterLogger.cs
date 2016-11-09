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
using System.IO;

#endregion

namespace Dapplo.Log.Loggers
{
	/// <summary>
	///     This logger will just append the content to a StringWriter
	///     Use the Output property to get the "log" (result)
	/// </summary>
	public class StringWriterLogger : AbstractLogger, IDisposable
	{
		private readonly StringWriter _writer = new StringWriter();

		/// <summary>
		///     Use this to collect the current string content
		/// </summary>
		/// <returns>string</returns>
		public string Output => _writer.ToString();

		/// <summary>
		/// Use this to clear the content
		/// </summary>
		public void Clear()
		{
			_writer.GetStringBuilder().Length = 0;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			_writer?.Dispose();
		}

		/// <inheritdoc />
		public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			_writer.Write(Format(logInfo, messageTemplate, logParameters));
		}
	}
}
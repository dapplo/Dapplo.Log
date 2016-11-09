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

#define DEBUG

#region Usings

using System.Diagnostics;

#endregion

namespace Dapplo.Log.Loggers
{
	/// <summary>
	///     A debug logger, the simplest implementation for logging debug messages
	/// </summary>
	public class DebugLogger : AbstractLogger
	{
		/// <inheritdoc />
		public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
		{
			Debug.Write(Format(logInfo, messageTemplate, logParameters));
		}
	}
}
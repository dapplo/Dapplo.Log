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

#define DEBUG

#region using



#endregion

namespace Dapplo.Log.Facade
{
	/// <summary>
	///     An ignoring logger, doesn't log anything!
	///     This can be used to shut-up certain LogSources
	/// </summary>
	public class DummyLogger : AbstractLogger
	{
		/// <summary>
		/// Always returns false, there is no logging
		/// </summary>
		/// <param name="level">LogLevel</param>
		/// <param name="logSource">optional LogSource</param>
		/// <returns>false</returns>
		public override bool IsLogLevelEnabled(LogLevels level, LogSource logSource = null)
		{
			return false;
		}
	}
}
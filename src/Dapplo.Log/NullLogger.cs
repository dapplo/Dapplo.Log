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


#define DEBUG

#region using

#endregion

namespace Dapplo.Log
{
    /// <summary>
    ///     An null logger, doesn't log anything!
    ///     This can be used to shut-up certain LogSources
    /// </summary>
    public class NullLogger : AbstractLogger
    {
        /// <summary>
        ///     Always returns false, there is no logging
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
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

namespace Dapplo.Log
{
    /// <summary>
    ///     Log level for the log facade, default(LogLevel) gives None which doesn't log anything
    ///     LogLevels.None is actually checked internally, before IsLogLevelEnabled.
    /// </summary>
    public enum LogLevels
    {
        /// <summary>
        ///     Default, no logging
        /// </summary>
        None,

        /// <summary>
        ///     Verbose logs pretty much everything
        /// </summary>
        Verbose,

        /// <summary>
        ///     Debugging information, usually needed when troubleshooting
        /// </summary>
        Debug,

        /// <summary>
        ///     Informational logging
        /// </summary>
        Info,

        /// <summary>
        ///     Warn that something didn't went well
        /// </summary>
        Warn,

        /// <summary>
        ///     Used for logging real errors
        /// </summary>
        Error,

        /// <summary>
        ///     Used for unrecoverable problems
        /// </summary>
        Fatal
    }
}
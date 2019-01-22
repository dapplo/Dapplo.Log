#region Dapplo 2016-2019 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016-2019 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Log
// 
// Dapplo.Log is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Log is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Log. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

using System;
using Dapplo.Log.Loggers;

namespace Dapplo.Log.ConsoleExample
{
    internal static class Program
    {
        private static void Main()
        {
            var log = new LogSource();
            var logger = LogSettings.RegisterDefaultLogger<ColorConsoleLogger>();
            logger.LogLevel = LogLevels.Verbose;
            log.Verbose().WriteLine("This is {0}", LogLevels.Verbose);
            log.Debug().WriteLine("This is {0}", LogLevels.Debug);
            log.Info().WriteLine("This is {0}", LogLevels.Info);
            log.Warn().WriteLine("This is {0}", LogLevels.Warn);
            log.Error().WriteLine("This is {0}", LogLevels.Error);
            log.Fatal().WriteLine("This is {0}", LogLevels.Fatal);
            Console.ReadLine();
        }
    }
}
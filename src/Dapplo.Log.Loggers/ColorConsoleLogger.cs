// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if !PCL

using System;
using System.Collections.Generic;

namespace Dapplo.Log.Loggers
{
    /// <summary>
    ///     A console logger with colors, an implementation for logging messages to a console where every log level has a different color.
    /// </summary>
    public class ColorConsoleLogger : AbstractLogger
    {
        /// <summary>
        ///     Used to log the writes, so the colors and lines don't mix when used from multiple threads.
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        ///     Maps the LogLevels level to a foreground color, levels that are not available map to white.
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
        ///     Maps the LogLevels level to a background color, levels that are not available map to black.
        /// </summary>
        public IDictionary<LogLevels, ConsoleColor> BackgroundColors { get; set; } = new Dictionary<LogLevels, ConsoleColor>
        {
            {LogLevels.Fatal, ConsoleColor.White}
        };

        /// <inheritdoc />
        public override void Write(LogInfo logInfo, string messageTemplate, params object[] logParameters)
        {
            if (!BackgroundColors.TryGetValue(logInfo.LogLevel, out var backgroundColor))
            {
                backgroundColor = ConsoleColor.Black;
            }

            if (!ForegroundColors.TryGetValue(logInfo.LogLevel, out var foregroundColor))
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
#endif
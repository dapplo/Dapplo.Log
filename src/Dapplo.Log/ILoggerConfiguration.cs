// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Dapplo.Log
{
    /// <summary>
    ///     Interface for the LoggerConfiguration
    /// </summary>
    public interface ILoggerConfiguration
    {
        /// <summary>
        ///     The LogLevels enum a logger uses
        /// </summary>
        [DefaultValue(LogLevels.Info)]
        LogLevels LogLevel { get; set; }

        /// <summary>
        ///     Defines if the Source is written like d.l.LoggerTest (default) or Dapplo.Log.LoggerTest
        /// </summary>
        [DefaultValue(true)]
        bool UseShortSource { get; set; }

        /// <summary>
        ///     Timestamp format which is used in the output, when outputting the LogInfo details
        /// </summary>
        [DefaultValue("yyyy-MM-dd HH:mm:ss.fff")]
        string DateTimeFormat { get; set; }

        /// <summary>
        ///     Default line format for loggers which use the DefaultFormatter.
        ///     The first argument is the LogInfo, the second the message + parameters formatted
        /// </summary>
        [DefaultValue("{0} - {1}")]
        string LogLineFormat { get; set; }
    }
}
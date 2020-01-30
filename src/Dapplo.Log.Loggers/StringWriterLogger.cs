// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

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
        ///     Use this to clear the content
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
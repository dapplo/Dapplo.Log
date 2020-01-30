// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dapplo.Log
{
    /// <summary>
    ///     This defines the "source" (origin) for log statements, it should have a Type or a identifier (string) so it's clear
    ///     where the log
    ///     entries come from. In general this should be instanciated with the default constructor without arguments, which
    ///     takes care of initiating it.
    ///     For normal .NET 4.5 this uses the Stack to find the type which called the constructor.
    ///     For other platforms this uses the CallerFilePath, which supplies the source-file.
    /// </summary>
    public sealed class LogSource
    {
        private static readonly char[] Dot = new[] { '.' };
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSource"/> class.
        ///     The constructor for specifying the type manually
        /// </summary>
        /// <param name="callerType">Type for the LogSource, not null</param>
        public LogSource(Type callerType)
        {
            if (callerType is null)
            {
                throw new ArgumentNullException(nameof(callerType));
            }
            SetSourceFromType(callerType);
        }

        /// <summary>
        ///     Private constructor used internally, to differentiate from the empty constructor
        /// </summary>
        // ReSharper disable once UnusedParameter.Local
        private LogSource(bool ignore)
        {
            // Nothing here, the properties are filled from the factory methods
        }

        private static string GetFilenameWithoutExtension(IEnumerable<string> filePath)
        {
            var filenameParts = GetFilename(filePath).Split(Dot).ToList();
            filenameParts.RemoveAt(filenameParts.Count - 1);
            return string.Join(".", filenameParts);
        }

        private static string GetFilename(IEnumerable<string> filePath)
        {
            return filePath.Last();
        }

        private static IEnumerable<string> GetDirectory(IEnumerable<string> filePath)
        {
            var fileParts = filePath.ToList();
            fileParts.RemoveAt(fileParts.Count - 1);
            return fileParts;
        }

        /// <summary>
        ///     The NON desktop default constructor which should be called without an argument.
        ///     It will use the CallerFilePath attribute, to make sure the source file is passed as an argument.
        /// </summary>
        public LogSource([CallerFilePath] string sourceFilePath = null)
        {
            if (sourceFilePath is null)
            {
                throw new ArgumentNullException(nameof(sourceFilePath));
            }

            var directorySeparatorChar = sourceFilePath.IndexOf('/') >= 0 ? '/' : '\\';

            var pathParts = sourceFilePath.Split(directorySeparatorChar);

            var typeName = GetFilenameWithoutExtension(pathParts);

            var nameSpace = GetFilename(GetDirectory(pathParts));

            var fqTypeName = $"{nameSpace}.{typeName}";
#if NETSTANDARD2_0 || NET45
            var type = Type.GetType(fqTypeName, false, true);
#else
            var type = Type.GetType(fqTypeName, false);
#endif
            if (type != null)
            {
                SetSourceFromType(type);
            }
            else
            {
                SetSourceFromString(fqTypeName);
            }
        }

        /// <summary>
        ///     Factory method where you can specify the type manually
        /// </summary>
        /// <param name="source">A custom identifier for the LogSource</param>
        /// <returns>LogSource</returns>
        public static LogSource ForCustomSource(string source)
        {
            var result = new LogSource(false);
            result.SetSourceFromString(source);
            return result;
        }

        /// <summary>
        ///     Use a string to set the source information
        /// </summary>
        /// <param name="source">Source to se</param>
        private void SetSourceFromString(string source)
        {
            Source = source;
            var parts = Source.Split(Dot);
            if (parts.Length > 0)
            {
                ShortSource = string.Join(".", parts.Take(parts.Length - 1).Select(s => s.Substring(0, 1).ToLowerInvariant()).Concat(new[] {parts.Last()}));
            }
            else
            {
                ShortSource = Source;
            }
        }

        /// <summary>
        ///     Use a type to set the source information
        /// </summary>
        /// <param name="sourceType"></param>
        private void SetSourceFromType(Type sourceType)
        {
            SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
            SetSourceFromString(sourceType.FullName);
        }

        /// <summary>
        ///     The Type where this LogSource was created
        /// </summary>
        public Type SourceType { get; private set; }

        /// <summary>
        ///     The Type, as string, where this LogSource was created
        ///     Every part of the namespace is shortened to one letter.
        ///     e.g. this class would return d.l.LogSource
        /// </summary>
        public string ShortSource { get; set; }

        /// <summary>
        ///     The Type, as string, where this LogSource was created
        ///     e.g. this class would return Dapplo.Log.LogSource
        /// </summary>
        public string Source { get; set; }
    }
}

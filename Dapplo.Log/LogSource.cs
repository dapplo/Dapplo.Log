#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2016 Dapplo
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

#region Usings

using System;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

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
	public class LogSource
	{
		/// <summary>
		///     The constructor for specifying the type manually
		/// </summary>
		/// <param name="callerType">Type for the LogSource, not null</param>
		public LogSource(Type callerType)
		{
			if (callerType == null)
			{
				throw new ArgumentNullException(nameof(callerType));
			}
			SetSourceFromType(callerType);
		}

		/// <summary>
		///     Private constructor used internally, to differenciate from the empty constructor
		/// </summary>
		// ReSharper disable once UnusedParameter.Local
		private LogSource(bool ignore)
		{
			// Nothing here, the properties are filled from the factory methods
		}

		/// <summary>
		///     The NON desktop default constructor which should be called without an argument.
		///     It will use the CallerFilePath attribute, to make sure the source file is passed as an argument.
		/// </summary>
		public LogSource([CallerFilePath] string sourceFilePath = null)
		{
			if (sourceFilePath == null)
			{
				throw new ArgumentNullException(nameof(sourceFilePath));
			}
#if NETSTANDARD1_3 || NET45
			var directorySeparatorChar = Path.DirectorySeparatorChar;
#else
			var directorySeparatorChar = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? '\\' : '/';
#endif
			var pathParts = sourceFilePath.Split(directorySeparatorChar);

			var typeName = Path.GetFileNameWithoutExtension(pathParts.Last());
			var leftovers = sourceFilePath.Substring(0, sourceFilePath.LastIndexOf(typeName, StringComparison.Ordinal)-1);
			var nameSpace = Path.GetFileName(leftovers);

			var fqTypeName = $"{nameSpace}.{typeName}";
#if NET45
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
			var parts = Source.Split('.');
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
			if (sourceType == null)
			{
				throw new ArgumentNullException(nameof(sourceType));
			}
			SourceType = sourceType;
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
		///     e.g. this class would return Dapplo.Log.Facade.LogSource
		/// </summary>
		public string Source { get; set; }
	}
}
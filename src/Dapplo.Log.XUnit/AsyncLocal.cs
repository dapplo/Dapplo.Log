#region Dapplo 2016-2019 - GNU Lesser General Public License

//  Dapplo - building blocks for .NET applications
//  Copyright (C) 2016-2019 Dapplo
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

#if NET45

using System;
using System.Runtime.Remoting.Messaging;

// ReSharper disable once CheckNamespace
namespace Nito.AsyncEx.AsyncLocal
{
    /// <summary>
    ///     Data that is "local" to the current async method. This is the async near-equivalent of <c>ThreadLocal&lt;T&gt;</c>.
    /// </summary>
    /// <typeparam name="TImmutableType">The type of the data. This must be an immutable type.</typeparam>
    public sealed class AsyncLocal<TImmutableType>
    {
        /// <summary>
        ///     Our unique slot name.
        /// </summary>
        private readonly string _slotName = Guid.NewGuid().ToString("N");

        /// <summary>
        ///     The default value when none has been set.
        /// </summary>
        private readonly TImmutableType _defaultValue;

        /// <summary>
        ///     Creates a new async-local variable with the default value of <typeparamref name="TImmutableType" />.
        /// </summary>
        public AsyncLocal() : this(default)
        {
        }

        /// <summary>
        ///     Creates a new async-local variable with the specified default value.
        /// </summary>
        public AsyncLocal(TImmutableType defaultValue)
        {
            _defaultValue = defaultValue;
        }

        /// <summary>
        ///     Returns a value indicating whether the value of this async-local variable has been set for the local context.
        /// </summary>
        public bool IsValueSet => CallContext.LogicalGetData(_slotName) != null;

        /// <summary>
        ///     Gets or sets the value of this async-local variable for the local context.
        /// </summary>
        public TImmutableType Value
        {
            get
            {
                var ret = CallContext.LogicalGetData(_slotName);
                if (ret is null)
                {
                    return _defaultValue;
                }
                return (TImmutableType) ret;
            }

            set => CallContext.LogicalSetData(_slotName, value);
        }

        /// <summary>
        ///     Clears the value of this async-local variable for the local context.
        /// </summary>
        public void ClearValue()
        {
            CallContext.FreeNamedDataSlot(_slotName);
        }
    }
}

#endif
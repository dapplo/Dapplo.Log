// Copyright (c) Dapplo and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
﻿#region © Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
// Copyright (c) 2015, Web Applications UK Ltd
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Web Applications UK Ltd nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL WEB APPLICATIONS UK LTD BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Threading
{
    /// <summary>
    ///   Holds a stack of objects against a logical call stack on a thread.
    /// </summary>
    /// <remarks>
    ///   <para>Although it can take any object type, it is never safe to modify objects that exist on the stack,
    ///   as they are shared across many threads. Only the stack structure itself is thread-safe. When used with value
    ///   types it is entirely safe.</para>
    ///   <para>Unlike previous systems, this only stores <see cref="string"/>s against the thread call context, the
    ///   data of which is merely an index into a lookup dictionary. This means that it will always serialize (for
    ///   example when passing across an app domain) without any issues.</para>
    ///   <para>This works because it stores a string against the thread context, which is a value type, as such
    ///   when a new thread is spun up, the string value is copied to the new thread - acting as a snapshot of the
    ///   thread state at the point the new thread was created.</para>
    /// </remarks>
    /// <typeparam name="T">The type of objects in the stack.</typeparam>
    [PublicAPI]
    [Obsolete("Context stacks can cause problems in asynchronous code (particular Rx) and should therefore be avoided.")
    ]
    public class ContextStack<T>
    {
        /// <summary>
        ///   This holds a unique random key (<see cref="System.Guid"/>), that makes it impossible to guess
        ///   where the object is stored in the <see cref="System.Runtime.Remoting.Messaging.CallContext">call
        ///   context</see>, so the only access point is this instance.
        /// </summary>
        [NotNull]
        private readonly string _contextKey = Guid.NewGuid().ToString();

        /// <summary>
        ///   Holds all active objects by their index.
        /// </summary>
        [NotNull]
        private readonly ConcurrentDictionary<long, T> _objects = new ConcurrentDictionary<long, T>();

        /// <summary>
        ///   Caches the current stack against the Thread Local Storage
        ///   (http://msdn.microsoft.com/en-us/library/windows/desktop/ms686749).
        /// </summary>
        [NotNull]
        private readonly ThreadLocal<KeyValuePair<string, IEnumerable<T>>> _stackCache =
            new ThreadLocal<KeyValuePair<string, IEnumerable<T>>>();

        /// <summary>
        ///   The counter, which is used to generate unique indices for objects (more memory efficient and
        ///   quicker than GUIDs, but requires more thought to prevent collision).
        /// </summary>
        private long _counter;

        /// <summary>
        ///   Gets the current stack.
        /// </summary>
        [NotNull]
        public IEnumerable<T> CurrentStack
        {
            get
            {
                // Get the stack of long keys.
                string stack = CallContext.LogicalGetData(_contextKey) as string;
                if (string.IsNullOrEmpty(stack))
                    return Enumerable.Empty<T>();

                // Try to get the stack out of the current TLS.
                KeyValuePair<string, IEnumerable<T>> kvp = _stackCache.Value;
                if (kvp.Key == stack)
                {
                    Debug.Assert(kvp.Value != null);
                    return kvp.Value;
                }

                // Look up actual objects.
                string[] objectKeys = stack.Split('|');
                List<T> objects = new List<T>(objectKeys.Length);
                foreach (string objectKey in objectKeys)
                {
                    long key;
                    if (!long.TryParse(objectKey, out key))
                        continue;
                    T value;
                    if (!_objects.TryGetValue(key, out value))
                        continue;
                    objects.Add(value);
                }

                // Cache the stack against this thread.
                _stackCache.Value = new KeyValuePair<string, IEnumerable<T>>(stack, objects);

                return objects;
            }
        }

        /// <summary>
        ///   Gets the current top of the stack.
        /// </summary>
        [CanBeNull]
        public T Current
        {
            get { return CurrentStack.LastOrDefault(); }
        }

        /// <summary>
        ///   Adds the entry to the top of the stack.
        /// </summary>
        /// <param name="entry">The entry.</param>
        [NotNull]
        public IDisposable Region(T entry)
        {
            return new Disposer(this, entry);
        }

        /// <summary>
        ///   Creates a region in which the stack does not exist.
        ///   This is useful for security when passing off to a set of untrusted code.
        /// </summary>
        [NotNull]
        public IDisposable Clean()
        {
            return new Cleaner(this);
        }

        #region Nested type: Cleaner
        /// <summary>
        ///   Used to remove stack from the logical call context for a period.
        /// </summary>
        private class Cleaner : IDisposable
        {
            /// <summary>
            ///   The stack value before we started.
            /// </summary>
            private readonly string _oldStack;

            /// <summary>
            ///   The owner stack.
            /// </summary>
            [NotNull]
            private readonly ContextStack<T> _stack;

            /// <summary>
            ///   The managed thread id.
            /// </summary>
            private readonly int _threadId;

            /// <summary>
            ///   The value held in the TLS.
            /// </summary>
            private readonly KeyValuePair<string, IEnumerable<T>> _tls;

            /// <summary>
            ///   Initializes a new instance of the <see cref="object"/> class.
            /// </summary>
            public Cleaner([NotNull] ContextStack<T> stack)
            {
                _stack = stack;
                _threadId = Thread.CurrentThread.ManagedThreadId;

                _oldStack = CallContext.LogicalGetData(_stack._contextKey) as string;
                if (_oldStack != null)
                    CallContext.FreeNamedDataSlot(_stack._contextKey);
                _tls = _stack._stackCache.Value;

                // Clear the stack cache.
                _stack._stackCache.Value = default(KeyValuePair<string, IEnumerable<T>>);
            }

            #region IDisposable Members
            /// <summary>
            ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            /// <exception cref="InvalidOperationException">
            ///   Cannot close the cleaner region as it was created on another thread.
            /// </exception>
            public void Dispose()
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (threadId != _threadId)
                    throw new InvalidOperationException(
                        string.Format(
                            // ReSharper disable once AssignNullToNotNullAttribute
                            Resources.ContextStack_Dispose_CannotCloseCleanerRegion,
                            _threadId,
                            threadId));

                if (_oldStack != null)
                    CallContext.LogicalSetData(_stack._contextKey, _oldStack);

                // Restore the stack cache.
                _stack._stackCache.Value = _tls;
            }
            #endregion
        }
        #endregion

        #region Nested type: Disposer
        /// <summary>
        ///   Used to start and end a region.
        /// </summary>
        private class Disposer : IDisposable
        {
            /// <summary>
            ///   The key.
            /// </summary>
            private readonly long _key;

            /// <summary>
            ///   The stack value before we started.
            /// </summary>
            private readonly string _oldStack;

            /// <summary>
            ///   The owner stack.
            /// </summary>
            [NotNull]
            private readonly ContextStack<T> _stack;

            /// <summary>
            ///   The managed thread id.
            /// </summary>
            private readonly int _threadId;

            /// <summary>
            ///   Initializes a new instance of the <see cref="ContextStack&lt;T&gt;.Disposer"/> class.
            /// </summary>
            /// <param name="stack">The stack.</param>
            /// <param name="value">The value.</param>
            public Disposer([NotNull] ContextStack<T> stack, T value)
            {
                _stack = stack;
                _threadId = Thread.CurrentThread.ManagedThreadId;
                // We use Interlocked.Increment, as it's thread safe and never overflows (wraps around).
                // by the time it wraps back to itself it's highly likely that the objects are no longer in use.
                // However, the while loop ensures we get a nice blank spot.
                T used;
                do
                {
                    _key = Interlocked.Increment(ref stack._counter);
                } while (stack._objects.TryGetValue(_key, out used));

                stack._objects.AddOrUpdate(_key, value, (k, v) => value);

                _oldStack = CallContext.LogicalGetData(stack._contextKey) as string;
                CallContext.LogicalSetData(
                    stack._contextKey,
                    (_oldStack == null ? string.Empty : _oldStack + "|") + _key);
            }

            #region IDisposable Members
            /// <summary>
            ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            /// <filterpriority>2</filterpriority>
            /// <exception cref="InvalidOperationException">
            ///   Cannot close the region as it was created on another thread.
            /// </exception>
            public void Dispose()
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                if (threadId != _threadId)
                    throw new InvalidOperationException(
                        string.Format(
                            // ReSharper disable once AssignNullToNotNullAttribute
                            Resources.ContextStack_Dispose_CannotCloseRegion,
                            _threadId,
                            threadId));

                // Try to clear the object from the stack.
                T value;
                _stack._objects.TryRemove(_key, out value);

                if (_oldStack == null)
                    CallContext.FreeNamedDataSlot(_stack._contextKey);
                else
                    CallContext.LogicalSetData(_stack._contextKey, _oldStack);

                // Clear the stack cache.
                _stack._stackCache.Value = default(KeyValuePair<string, IEnumerable<T>>);
            }
            #endregion
        }
        #endregion
    }
}
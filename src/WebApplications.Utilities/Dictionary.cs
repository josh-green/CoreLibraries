#region � Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Reflect;

namespace WebApplications.Utilities
{
    /// <summary>
    /// Dictionary extension and helper methods.
    /// </summary>
    [PublicAPI]
    public static class Dictionary
    {
        // ReSharper disable AssignNullToNotNullAttribute, PossibleNullReferenceException

        /// <summary>
        /// The name object entry type (this is an internal framework type).
        /// </summary>
        [NotNull]
        private static readonly ExtendedType _nameObjectEntryType =
            ExtendedType.Get("System.Collections.Specialized.NameObjectCollectionBase+NameObjectEntry, System");
        
        /// <summary>
        /// The function to efficiently get the entries array from a <see cref="NameObjectCollectionBase"/>.
        /// </summary>
        [NotNull]
        private static readonly Func<NameObjectCollectionBase, ArrayList> _getEntriesFunc =
            ExtendedType.Get(typeof(NameObjectCollectionBase))
                .GetField("_entriesArray")
                .Getter<NameObjectCollectionBase, ArrayList>();

        /// <summary>
        /// Gets the key from a NameObjectEntry. 
        /// </summary>
        [NotNull]
        private static readonly Func<object, string> _getNameObjectEntryKeyFunc =
            _nameObjectEntryType
                .GetField("Key")
                .Getter<object, string>();

        /// <summary>
        /// Gets the value from a NameObjectEntry. 
        /// </summary>
        [NotNull]
        private static readonly Func<object, object> _getNameObjectEntryValueFunc =
            _nameObjectEntryType
                .GetField("Value")
                .Getter<object, object>();

        /// <summary>
        /// Gets a collection of keys from any <see cref="NameObjectCollectionBase"/> descendant.
        /// </summary>
        /// <param name="nameObjectCollection">The name object collection.</param>
        /// <returns>A read only collection of keys.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameObjectCollection"/> is <see langword="null" />.</exception>
        [NotNull]
        public static IReadOnlyCollection<string> Keys(
            [NotNull] this NameObjectCollectionBase nameObjectCollection)
        {
            if (nameObjectCollection == null) throw new ArgumentNullException(nameof(nameObjectCollection));
            return nameObjectCollection.Keys.Cast<string>().ToArray();
        }
        /// <summary>
        /// Gets a collection of values from any <see cref="NameObjectCollectionBase"/> descendant.
        /// </summary>
        /// <param name="nameObjectCollection">The name object collection.</param>
        /// <returns>A read only collection of keys.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameObjectCollection"/> is <see langword="null" />.</exception>
        [NotNull]
        public static IReadOnlyCollection<object> Values(
            [NotNull] this NameObjectCollectionBase nameObjectCollection)
        {
            if (nameObjectCollection == null) throw new ArgumentNullException(nameof(nameObjectCollection));
            // ReSharper disable EventExceptionNotDocumented
            return _getEntriesFunc(nameObjectCollection)
                .Cast<object>()
                .Select(_getNameObjectEntryValueFunc)
                .ToArray();
            // ReSharper restore EventExceptionNotDocumented
        }

        /// <summary>
        /// Converts any <see cref="NameObjectCollectionBase"/> descendant to a dictionary.
        /// </summary>
        /// <param name="nameObjectCollection">The name object collection.</param>
        /// <returns>A read only dictionary</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nameObjectCollection"/> is <see langword="null" />.</exception>
        [NotNull]
        public static IReadOnlyDictionary<string, object> ToDictionary(
            [NotNull] this NameObjectCollectionBase nameObjectCollection)
        {
            if (nameObjectCollection == null) throw new ArgumentNullException(nameof(nameObjectCollection));
            // ReSharper disable EventExceptionNotDocumented
            return _getEntriesFunc(nameObjectCollection)
                .Cast<object>()
                .ToDictionary(_getNameObjectEntryKeyFunc, _getNameObjectEntryValueFunc);
            // ReSharper restore EventExceptionNotDocumented
        }

        // ReSharper restore AssignNullToNotNullAttribute, PossibleNullReferenceException

        /// <summary>
        /// Returns an empty <see cref="IReadOnlyDictionary{T,T}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns></returns>
        [NotNull]
        public static IReadOnlyDictionary<TKey, TValue> Empty<TKey, TValue>()
        {
            return EmptyDictionary<TKey, TValue>.Instance;
        }

        /// <summary>
        /// An empty <see cref="IReadOnlyDictionary{T,T}"/>
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        private class EmptyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
        {
            /// <summary>
            /// The empty dictionary.
            /// </summary>
            [NotNull]
            public static readonly IReadOnlyDictionary<TKey, TValue> Instance = new EmptyDictionary<TKey, TValue>();

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                yield break;
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                yield break;
            }

            /// <summary>
            /// Gets the number of elements in the collection.
            /// </summary>
            /// <returns>The number of elements in the collection. </returns>
            public int Count
            {
                get { return 0; }
            }

            /// <summary>
            /// Determines whether the read-only dictionary contains an element that has the specified key.
            /// </summary>
            /// <param name="key">The key to locate.</param>
            /// <returns>
            /// true if the read-only dictionary contains an element that has the specified key; otherwise, false.
            /// </returns>
            public bool ContainsKey(TKey key)
            {
                return false;
            }

            /// <summary>
            /// Gets the value that is associated with the specified key.
            /// </summary>
            /// <param name="key">The key to locate.</param>
            /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed uninitialized.</param>
            /// <returns>
            /// true if the object that implements the <see cref="T:System.Collections.Generic.IReadOnlyDictionary`2" /> interface contains an element that has the specified key; otherwise, false.
            /// </returns>
            public bool TryGetValue(TKey key, out TValue value)
            {
                value = default(TValue);
                return false;
            }

            /// <summary>
            /// Gets the element that has the specified key in the read-only dictionary.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            /// <exception cref="System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
            public TValue this[TKey key]
            {
                get { throw new KeyNotFoundException(); }
            }

            /// <summary>
            /// Gets an enumerable collection that contains the keys in the read-only dictionary.
            /// </summary>
            /// <returns>An enumerable collection that contains the keys in the read-only dictionary.</returns>
            [NotNull]
            public IEnumerable<TKey> Keys
            {
                get { return Enumerable.Empty<TKey>(); }
            }

            /// <summary>
            /// Gets an enumerable collection that contains the values in the read-only dictionary.
            /// </summary>
            /// <returns>An enumerable collection that contains the values in the read-only dictionary.</returns>
            [NotNull]
            public IEnumerable<TValue> Values
            {
                get { return Enumerable.Empty<TValue>(); }
            }
        }
    }
}
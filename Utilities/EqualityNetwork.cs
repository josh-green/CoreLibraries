﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;

namespace WebApplications.Utilities
{
    /// <summary>
    /// Allows the normalization of equalities.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class EqualityNetwork<T> : IEnumerable<KeyValuePair<T, T>>
        where T : IComparable<T>
    {
        [NotNull]
        private readonly List<HashSet<T>> _sets;

        /// <summary>
        /// The equality comparer
        /// </summary>
        [NotNull]
        private readonly EqualityComparer<T> _equalityComparer;

        /// <summary>
        /// The comparator
        /// </summary>
        [NotNull]
        private readonly Comparer<T> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EqualityNetwork{T}"/> class.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="comparer">The comparer.</param>
        public EqualityNetwork(
            int count = 0,
            [CanBeNull] EqualityComparer<T> equalityComparer = null,
            [CanBeNull] Comparer<T> comparer = null)
        {
            _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            _comparer = comparer ?? Comparer<T>.Default;
            _sets = new List<HashSet<T>>(count);
        }

        /// <summary>
        /// Adds a set of values that have to be equal to each other.
        /// </summary>
        /// <param name="values">The values.</param>
        public void Add([NotNull] IEnumerable<T> values)
        {
            Add(values.ToArray());
        }

        /// <summary>
        /// Adds a set of values that have to be equal to each other.
        /// </summary>
        /// <param name="values">The values.</param>
        [UsedImplicitly]
        public void Add([NotNull] params T[] values)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            HashSet<T>[] sets = _sets.Where(set => set.Intersect(values).Any()).ToArray();
            if (sets.Length == 0)
                _sets.Add(new HashSet<T>(values, _equalityComparer));
            else
            {
                Contract.Assert(sets[0] != null);
                sets[0].UnionWith(values);
                for (int i = 1; i < sets.Length; i++)
                {
                    Contract.Assert(sets[i] != null);
                    sets[0].UnionWith(sets[i]);
                    _sets.Remove(sets[i]);
                }
            }
        }

        /// <summary>
        /// Returns an enumeration of equalities.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<T, T>> GetEnumerator()
        {
            return _sets.SelectMany(
                equalitySet =>
                {
                    Contract.Assert(equalitySet != null);
                    T target = equalitySet.Min(_comparer);
                    // ReSharper disable once PossibleNullReferenceException
                    return equalitySet.Where(value => !value.Equals(target))
                        .Select(value => new KeyValuePair<T, T>(value, target));
                }).GetEnumerator();
        }

        /// <summary>
        /// Given a value tells you the value 
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target.</param>
        /// <returns><c>true</c> if an equality was found, <c>false</c> otherwise</returns>
        [UsedImplicitly]
        public bool TryGetEquivalent([NotNull] T value, out T target)
        {
            Contract.Requires(!ReferenceEquals(value, null));

            // ReSharper disable once PossibleNullReferenceException
            HashSet<T> equalitySet = _sets.FirstOrDefault(es => es.Contains(value));
            if (equalitySet != null)
            {
                target = equalitySet.Min(_comparer);
                Contract.Assert(!ReferenceEquals(target, null));
                return !target.Equals(value);
            }
            target = default(T);
            return false;
        }

        /// <summary>
        /// Gets the equivalent value for the current value.
        /// If value has not been set equal to anything, this method returns value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The equivalent value.</returns>
        [NotNull]
        public T GetEquivalent([NotNull] T value)
        {
            Contract.Requires(!ReferenceEquals(value, null));

            // ReSharper disable once PossibleNullReferenceException
            HashSet<T> equalitySet = _sets.FirstOrDefault(es => es.Contains(value));
            // ReSharper disable once AssignNullToNotNullAttribute
            return equalitySet != null
                       ? equalitySet.Min(_comparer)
                       : value;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Tests whether two objects are set to be equal.
        /// </summary>
        /// <param name="a">One object</param>
        /// <param name="b">The other object</param>
        /// <returns>true if they are equal, false otherwise.</returns>
        [UsedImplicitly]
        public bool AreEqual([NotNull]T a, [NotNull]T b)
        {
            Contract.Requires(!ReferenceEquals(a, null));
            Contract.Requires(!ReferenceEquals(b, null));

            foreach (HashSet<T> hashSet in _sets)
            {
                Contract.Assert(hashSet != null);
                if (hashSet.Contains(a))
                    return hashSet.Contains(b);
            }
            return false;
        }
    }
}

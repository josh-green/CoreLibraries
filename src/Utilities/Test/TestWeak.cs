﻿#region © Copyright Web Applications (UK) Ltd, 2014.  All rights reserved.
// Copyright (c) 2014, Web Applications UK Ltd
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Testing;
using WebApplications.Utilities.Caching;

namespace WebApplications.Utilities.Test
{
    [TestClass]
    public class TestWeak
    {
        [TestMethod]
        [Timeout(5000)]
        public void TestWeakConcurrentDictionaryReferences()
        {
            const int elements = 100000;
            Random random = new Random();
            WeakConcurrentDictionary<int, TestClass> weakConcurrentDictionary =
                new WeakConcurrentDictionary<int, TestClass>(allowResurrection: false);
            ConcurrentDictionary<int, TestClass> referenceDictionary = new ConcurrentDictionary<int, TestClass>();
            int nullCount = 0;
            int unreferencedNullCount = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(
                0,
                elements,
                l =>
                {
                    // Include nulls ~25% of the time.
                    TestClass t;
                    if (random.Next(4) < 3)
                        t = new TestClass(random.Next(int.MinValue, int.MaxValue));
                    else
                    {
                        t = null;
                        Interlocked.Increment(ref nullCount);
                    }

                    weakConcurrentDictionary.Add(l, t);

                    // Only keep references ~33% of the time.
                    if (random.Next(3) == 0)
                        referenceDictionary.AddOrUpdate(l, t, (k, v) => t);
                    else if (t == null)
                        Interlocked.Increment(ref unreferencedNullCount);
                });
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Populating dictionaries with {0} elements", elements));

            //GC.WaitForFullGCComplete(5000);
            stopwatch.Restart();
            long bytes = GC.GetTotalMemory(true);
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Garbage collection"));
            Trace.WriteLine($"Memory: {bytes / 1024}K");

            // Check that we have l
            Assert.IsTrue(referenceDictionary.Count <= elements);

            int refCount = referenceDictionary.Count;

            stopwatch.Restart();
            int weakCount = weakConcurrentDictionary.Count;
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Counting '{0}' elements", weakCount));

            stopwatch.Restart();
            weakCount = weakConcurrentDictionary.Count;
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Counting '{0}' elements again", weakCount));

            int floor = refCount + unreferencedNullCount;

            Trace.WriteLine(
                string.Format(
                    "Referenced Dictionary Count: {1}{0}Weak Dictionary Count: {2}{0}Null values: {3} (unreferenced: {4}){0}Garbage Collected: {5}{0}Pending Collection: {6}{0}",
                    Environment.NewLine,
                    refCount,
                    weakCount,
                    nullCount,
                    unreferencedNullCount,
                    elements - weakCount,
                    weakCount - floor
                    ));

            // Check we only have references to referenced elements.
            Assert.AreEqual(refCount + unreferencedNullCount, weakCount);

            // Check everything that's still referenced is available.
            stopwatch.Restart();
            Parallel.ForEach(
                referenceDictionary,
                kvp =>
                {
                    TestClass value;
                    Assert.IsTrue(weakConcurrentDictionary.TryGetValue(kvp.Key, out value));
                    Assert.AreEqual(kvp.Value, value);
                });
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Checking '{0}' elements", weakCount));
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestWeakConcurrentDictionaryKeysAndValues()
        {
            WeakConcurrentDictionary<string, string> weakConcurrentDictionary =
                new WeakConcurrentDictionary<string, string>
                {
                    { "A", "a" },
                    { "B", "b" }
                };

            CollectionAssert.AreEquivalent(new[] { "A", "B" }, (ICollection)weakConcurrentDictionary.Keys, "Keys property failed to return values.");
            CollectionAssert.AreEquivalent(new[] {"a", "b"}, (ICollection) weakConcurrentDictionary.Values, "Values property failed to return values.");
        }

        [TestMethod]
        [Timeout(5000)]
        public void TestWeakConcurrentDictionaryReferencesObservable()
        {
            const int elements = 100000;
            Random random = new Random();
            WeakConcurrentDictionary<int, ObservableTestClass> weakConcurrentDictionary =
                new WeakConcurrentDictionary<int, ObservableTestClass>(allowResurrection: false);
            ConcurrentDictionary<int, ObservableTestClass> referenceDictionary =
                new ConcurrentDictionary<int, ObservableTestClass>();
            int nullCount = 0;
            int unreferencedNullCount = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(
                0,
                elements,
                l =>
                {
                    // Include nulls ~25% of the time.
                    ObservableTestClass t;
                    if (random.Next(4) < 3)
                        t = new ObservableTestClass(random.Next(int.MinValue, int.MaxValue));
                    else
                    {
                        t = null;
                        Interlocked.Increment(ref nullCount);
                    }

                    weakConcurrentDictionary.Add(l, t);

                    // Only keep references ~33% of the time.
                    if (random.Next(3) == 0)
                        referenceDictionary.AddOrUpdate(l, t, (k, v) => t);
                    else if (t == null)
                        Interlocked.Increment(ref unreferencedNullCount);
                });
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Populating dictionaries with {0} elements", elements));

            //GC.WaitForFullGCComplete(5000);
            stopwatch.Restart();
            long bytes = GC.GetTotalMemory(true);
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Garbage collection"));
            Trace.WriteLine(string.Format("Memory: {0}K", bytes / 1024));

            // Check that we have l
            Assert.IsTrue(referenceDictionary.Count <= elements);

            int refCount = referenceDictionary.Count;

            stopwatch.Restart();
            int weakCount = weakConcurrentDictionary.Count;
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Counting '{0}' elements", weakCount));

            stopwatch.Restart();
            weakCount = weakConcurrentDictionary.Count;
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Counting '{0}' elements again", weakCount));

            int floor = refCount + unreferencedNullCount;

            Trace.WriteLine(
                string.Format(
                    "Referenced Dictionary Count: {1}{0}Weak Dictionary Count: {2}{0}Null values: {3} (unreferenced: {4}){0}Garbage Collected: {5}{0}Pending Collection: {6}{0}",
                    Environment.NewLine,
                    refCount,
                    weakCount,
                    nullCount,
                    unreferencedNullCount,
                    elements - weakCount,
                    weakCount - floor
                    ));

            // Check we only have references to referenced elements.
            Assert.AreEqual(refCount + unreferencedNullCount, weakCount);

            // Check everything that's still referenced is available.
            stopwatch.Restart();
            Parallel.ForEach(
                referenceDictionary,
                kvp =>
                {
                    ObservableTestClass value;
                    Assert.IsTrue(weakConcurrentDictionary.TryGetValue(kvp.Key, out value));
                    Assert.AreEqual(kvp.Value, value);
                });
            stopwatch.Stop();
            Trace.WriteLine(stopwatch.ToString("Checking '{0}' elements", weakCount));
        }

        #region Nested type: ObservableTestClass
        /// <summary>
        /// A test class for placing in a weak dictionary.
        /// </summary>
        /// <remarks></remarks>
        public class ObservableTestClass : IObservableFinalize
        {
            public readonly int? Value;

            private EventHandler _finalized;

            /// <summary>
            /// Initializes a new instance of the <see cref="TestClass"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <remarks></remarks>
            public ObservableTestClass(int? value)
            {
                Value = value;
            }

            #region IObservableFinalize Members
            /// <inheritdoc />
            public event EventHandler Finalized
            {
                add
                {
                    if (_finalized == null)
                        GC.ReRegisterForFinalize(this);

                    _finalized += value;
                }

                remove
                {
                    _finalized -= value;

                    if (_finalized == null)
                        GC.SuppressFinalize(this);
                }
            }
            #endregion

            /// <inheritdoc />
            ~ObservableTestClass()
            {
                if (_finalized != null)
                    _finalized(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Nested type: TestClass
        /// <summary>
        /// A test class for placing in a weak dictionary.
        /// </summary>
        /// <remarks></remarks>
        public class TestClass
        {
            public readonly int? Value;

            /// <summary>
            /// Initializes a new instance of the <see cref="TestClass"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <remarks></remarks>
            public TestClass(int? value)
            {
                Value = value;
            }
        }
        #endregion
    }
}
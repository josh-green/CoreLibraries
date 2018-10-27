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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Difference;

namespace WebApplications.Utilities.Test
{
    /// <summary>
    /// Test difference methods.
    /// </summary>
    [TestClass]
    public class TestDifference
    {

        /// <summary>
        /// Allows the building of the expected result of a difference operation.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class DiffResult<T> : IReadOnlyList<Tuple<bool, IEnumerable<T>, IEnumerable<T>>>
        {
            /// <summary>
            /// The underlying chunks.
            /// </summary>
            [NotNull]
            private readonly List<Tuple<bool, IEnumerable<T>, IEnumerable<T>>> _chunks =
                new List<Tuple<bool, IEnumerable<T>, IEnumerable<T>>>();

            /// <summary>
            /// Adds a chunk to both.
            /// </summary>
            /// <param name="enumerationA">The "A" enumeration.</param>
            /// <param name="enumerationB">The "B" enumeration.</param>
            public void AddEqual([NotNull] IEnumerable<T> enumerationA, IEnumerable<T> enumerationB = null)
                => _chunks.Add(
                    new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(true, enumerationA, enumerationB ?? enumerationA));

            /// <summary>
            /// Adds a chunk to both.
            /// </summary>
            /// <param name="array">The array.</param>
            public void AddEqual([NotNull] params T[] array)
                            => _chunks.Add(
                                new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(true, array, array));

            /// <summary>
            /// Adds a chunk to both.
            /// </summary>
            /// <param name="enumerationA">The "A" enumeration.</param>
            /// <param name="enumerationB">The "B" enumeration.</param>
            public void AddNotEqual([NotNull] IEnumerable<T> enumerationA, IEnumerable<T> enumerationB = null)
                => _chunks.Add(
                    new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(false, enumerationA, enumerationB ?? enumerationA));
            
            /// <summary>
            /// Adds a chunk to A.
            /// </summary>
            /// <param name="enumeration">The enumeration.</param>
            public void AddA([NotNull] IEnumerable<T> enumeration)
                            => _chunks.Add(
                                new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(false, enumeration, null));

            /// <summary>
            /// Adds a chunk to A.
            /// </summary>
            /// <param name="array">The array.</param>
            public void AddA([NotNull] params T[] array)
                            => _chunks.Add(
                                new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(false, array, null));

            /// <summary>
            /// Adds a chunk to B.
            /// </summary>
            /// <param name="enumeration">The enumeration.</param>
            public void AddB([NotNull] IEnumerable<T> enumeration)
                            => _chunks.Add(
                                new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(false, null, enumeration));

            /// <summary>
            /// Adds a chunk to B.
            /// </summary>
            /// <param name="array">The array.</param>
            public void AddB([NotNull] params T[] array)
                            => _chunks.Add(
                                new Tuple<bool, IEnumerable<T>, IEnumerable<T>>(false, null, array));

            /// <inheritdoc/>
            public IEnumerator<Tuple<bool, IEnumerable<T>, IEnumerable<T>>> GetEnumerator()
                            => _chunks.GetEnumerator();

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator()
                => _chunks.GetEnumerator();

            /// <inheritdoc/>
            public int Count => _chunks.Count;

            /// <inheritdoc/>
            public Tuple<bool, IEnumerable<T>, IEnumerable<T>> this[int index] => _chunks[index];
        }

        /// <summary>
        /// Tests the difference between to enumerations.
        /// </summary>
        /// <typeparam name="T">The item type</typeparam>
        /// <param name="a">The 'A' collection.</param>
        /// <param name="b">The 'B' collection.</param>
        /// <param name="expectedResult">The expected result.</param>
        /// <param name="comparer">The optional equality comparer for the <typeparamref name="T"/> type.</param>
        private static void TestDiff<T>(
                    [NotNull] IReadOnlyList<T> a,
                    [NotNull] IReadOnlyList<T> b,
                    [NotNull] DiffResult<T> expectedResult,
                    IEqualityComparer<T> comparer = null)
        {
            if (comparer == null) comparer = EqualityComparer<T>.Default;

            Assert.IsNotNull(a, "The 'A' collection cannot be null.");
            Assert.IsNotNull(b, "The 'B' collection cannot be null.");
            Assert.IsNotNull(expectedResult, "The expected result cannot be null.");

            Tuple<bool, IEnumerable<T>, IEnumerable<T>>[] er = expectedResult.ToArray();
            CollectionAssert.AllItemsAreNotNull(er, "Expected result contains null.");

            Differences<T> actualResult = a.Diff(b, comparer);
            Assert.IsNotNull(actualResult, "The diff operation returned null.");

            Chunk<T>[] ar = actualResult.ToArray();
            CollectionAssert.AllItemsAreNotNull(ar, "Actual result contains null.");

            Trace.WriteLine($"Chunks:\r\n{actualResult}");

            Assert.AreEqual(er.Length, ar.Length, $"{ar.Length} actual results returned instead of the expected {er.Length}.");
            for (int i = 0; i < er.Length; i++)
            {
                Tuple<bool, IEnumerable<T>, IEnumerable<T>> eri = er[i];
                Chunk<T> ari = ar[i];
                Assert.IsNotNull(ari, $"The actual chunk of the #{i} difference was null.");

                Assert.AreEqual(eri.Item1, ari.AreEqual, $"The actual chunk of the #{i} difference was {(ari.AreEqual ? "equal" : "not equal")}, when the expected result was {(eri.Item1 ? "equal" : "not equal")}.");

                if (ari.A == null)
                    Assert.IsNull(
                        eri.Item2,
                        $"The actual chunk A of the #{i} difference was null, when the expected chunk A is not null.");
                else
                {
                    Assert.IsNotNull(
                        eri.Item2,
                        $"The actual chunk A of the #{i} difference was not null, when the expected chunk A is null.");

                    IEnumerator<T> eriae = eri.Item2.GetEnumerator();
                    IEnumerator<T> ariae = ari.A.GetEnumerator();

                    int j = 0;
                    while (eriae.MoveNext())
                    {
                        Assert.IsTrue(
                            ariae.MoveNext(),
                            $"The actual chunk A of the #{i} difference only had {j} items, whilst more were expected.");

                        Assert.IsTrue(
                            comparer.Equals(eriae.Current, ariae.Current),
                            $"The #{j} item '{ariae.Current}' of the A #{i} difference did not match the expected item '{eriae.Current}'.");
                        j++;
                    }
                    Assert.IsFalse(
                        ariae.MoveNext(),
                        $"The actual chunk A of the #{i} difference has more than the {j} expected items.");
                }

                if (ari.B == null)
                    Assert.IsNull(
                        eri.Item3,
                        $"The actual chunk B of the #{i} difference was null, when the expected chunk B is not null.");
                else
                {
                    Assert.IsNotNull(
                        eri.Item3,
                        $"The actual chunk B of the #{i} difference was not null, when the expected chunk B is null.");

                    IEnumerator<T> eribe = eri.Item3.GetEnumerator();
                    IEnumerator<T> aribe = ari.B.GetEnumerator();

                    int j = 0;
                    while (eribe.MoveNext())
                    {
                        Assert.IsTrue(
                            aribe.MoveNext(),
                            $"The actual chunk B of the #{i} difference only had {j} items, whilst more were expected.");

                        Assert.IsTrue(
                            comparer.Equals(eribe.Current, aribe.Current),
                            $"The #{j} item '{aribe.Current}' of the B #{i} difference did not match the expected item '{eribe.Current}'.");
                        j++;
                    }
                    Assert.IsFalse(
                        aribe.MoveNext(),
                        $"The actual chunk B of the #{i} difference has more than the {j} expected items.");
                }
            }
        }

        [TestMethod]
        public void TestNoChanges()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 1, 2, 3, 4, 5 };
            result.AddEqual(a);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestAllChanged()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 6, 7, 8 };
            result.AddNotEqual(a, b);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestOverlap()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 2, 3, 4, 5, 6 };
            result.AddA(1);
            result.AddEqual(2,3,4,5);
            result.AddB(6);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestHead()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 2, 3, 4, 5 };
            result.AddA(1);
            result.AddEqual(2, 3, 4, 5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestTail()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4 };
            int[] b = { 1, 2, 3, 4, 5 };
            result.AddEqual(1, 2, 3, 4);
            result.AddB(5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestEmptyA()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = Array<int>.Empty;
            int[] b = { 1, 2, 3, 4, 5 };
            result.AddB(b);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestEmptyB()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = Array<int>.Empty;
            result.AddA(a);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestBothEmpty()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = Array<int>.Empty;
            int[] b = Array<int>.Empty;
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestInsertion()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 4, 5 };
            int[] b = { 1, 2, 3, 4, 5 };
            result.AddEqual(1,2);
            result.AddB(3);
            result.AddEqual(4, 5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestDeletion()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 1, 2, 4, 5 };
            result.AddEqual(1, 2);
            result.AddA(3);
            result.AddEqual(4, 5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestMove()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 1, 2, 4, 3, 5 };
            result.AddEqual(1, 2);
            result.AddB(4);
            result.AddEqual(3);
            result.AddA(4);
            result.AddEqual(5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestBigMove()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5, 6 };
            int[] b = { 1, 4, 5, 2, 3, 6 };
            result.AddEqual(1);
            result.AddB(4, 5);
            result.AddEqual(2, 3);
            result.AddA(4, 5);
            result.AddEqual(6);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestSurround()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 2 };
            int[] b = { 1, 2, 3, 4 };
            result.AddB(1);
            result.AddEqual(2);
            result.AddB(3,4);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestExcise()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4 };
            int[] b = { 2 };
            result.AddA(1);
            result.AddEqual(2);
            result.AddA(3, 4);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestMultipleChanges()
        {
            DiffResult<int> result = new DiffResult<int>();
            int[] a = { 1, 2, 3, 4, 5 };
            int[] b = { 1, 2, 6, 2, 3, 7, 5 };
            result.AddEqual(1, 2);
            result.AddB(6, 2);
            result.AddEqual(3);
            result.AddNotEqual(new[] { 4 }, new[] { 7 });
            result.AddEqual(5);
            TestDiff(a, b, result);
        }

        [TestMethod]
        public void TestStringDifferenceIgnore1()
        {
            StringDifferences differences = "Test   difference".Diff("Test Difference", TextOptions.IgnoreWhiteSpace, CharComparer.CurrentCultureIgnoreCase);
            Assert.IsTrue(differences.AreEqual);
        }

        [TestMethod]
        public void TestStringDifference2()
        {
            StringDifferences differences = "Test   difference".Diff("Test Difference", TextOptions.None, CharComparer.CurrentCultureIgnoreCase);
            Assert.IsFalse(differences.AreEqual);
        }

        [TestMethod]
        public void TestStringDifference3()
        {
            StringDifferences differences = "Test  a difference ".Diff("Test Difference  ", TextOptions.IgnoreWhiteSpace, CharComparer.CurrentCultureIgnoreCase);
            Assert.IsFalse(differences.AreEqual);
        }

        [TestMethod]
        public void TestStringDifference4()
        {
            string loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            string loremIpsumMod = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. This bit is changed, quis nostrud exercitation ullamco this bit is added laboris nisi ut aliquip. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            StringDifferences differences = loremIpsum.Diff(loremIpsumMod);
            Assert.IsFalse(differences.AreEqual);
        }
    }
}
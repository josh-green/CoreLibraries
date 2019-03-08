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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Testing;
using WebApplications.Utilities.Caching;

namespace WebApplications.Utilities.Test.Caching
{
    [TestClass]
    public class ConcurrentLookupTests : UtilitiesTestBase
    {


        [TestMethod]
        public void Count_InitialCollectionContainsNoDuplicates_MatchesInitialCollection()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(20))).ToList();
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.AreEqual(collection.Count, concurrentLookup.Count);
        }

        [TestMethod]
        public void Count_InitialCollectionContainsDuplicatesKeys_MatchesNumberOfUniqueKeys()
        {
            List<Guid> keys = Enumerable.Range(1, Random.Next(10, 100)).Select(n => Guid.NewGuid()).ToList();
            int numberOfDuplicates = Random.Next(1, keys.Count / 2);
            List<KeyValuePair<Guid, string>> collection = keys.Concat(keys.Take(numberOfDuplicates)).Select(
                key => new KeyValuePair<Guid, string>(key, Random.RandomString(20))
                ).ToList();
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.AreEqual(collection.Count - numberOfDuplicates, concurrentLookup.Count);
        }

        [TestMethod]
        public void GetEnumerator_InitialCollectionContainsDuplicatesKeys_GroupingKeysMatchUniqueCollectionKeys()
        {
            List<Guid> keys = Enumerable.Range(1, Random.Next(10, 100)).Select(n => Guid.NewGuid()).ToList();
            int numberOfDuplicates = Random.Next(1, keys.Count / 2);
            List<KeyValuePair<Guid, string>> collection = keys.Concat(keys.Take(numberOfDuplicates)).Select(
                key => new KeyValuePair<Guid, string>(key, Random.RandomString(20))
                ).ToList();
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            CollectionAssert.AreEquivalent(keys, concurrentLookup.Select(grouping => grouping.Key).ToList());
        }

        [TestMethod]
        public void GetEnumerator_InitialCollectionContainsDuplicatesKeys_GroupingsMatchCollectionEntriesMatchingKey()
        {
            List<Guid> keys = Enumerable.Range(1, Random.Next(10, 100)).Select(n => Guid.NewGuid()).ToList();
            int numberOfDuplicates = Random.Next(1, keys.Count / 2);
            List<KeyValuePair<Guid, string>> collection = keys.Concat(keys.Take(numberOfDuplicates)).Select(
                key => new KeyValuePair<Guid, string>(key, Random.RandomString(20))
                ).ToList();
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            foreach (IGrouping<Guid, string> grouping in concurrentLookup)
            {
                Assert.IsNotNull(grouping);
                CollectionAssert.AreEquivalent(
                    collection.Where(e => e.Key == grouping.Key).Select(e => e.Value).ToList(),
                    grouping.ToList());
            }
        }

        [TestMethod]
        public void This_ExistingKey_EnumerationMatchesAllCollectionEntriesMatchingKey()
        {
            Guid matchingKey = Guid.NewGuid();
            List<string> matchingValues =
                Enumerable.Range(1, Random.Next(10, 100)).Select(n => Random.RandomString(20)).ToList();
            IEnumerable<KeyValuePair<Guid, string>> otherEntries =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            IEnumerable<KeyValuePair<Guid, string>> collection = matchingValues.Select(
                value => new KeyValuePair<Guid, string>(matchingKey, value))
                .Concat(otherEntries);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            CollectionAssert.AreEquivalent(matchingValues, concurrentLookup[matchingKey].ToList());
        }

        [TestMethod]
        public void This_AbsentKey_ReturnsEmptyEnumeration()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            CollectionAssert.AreEquivalent(Enumerable.Empty<string>().ToList(), concurrentLookup[absentKey].ToList());
        }

        [TestMethod]
        public void TryGet_ExistingKey_OutputEnumerationMatchesAllCollectionEntriesMatchingKey()
        {
            Guid matchingKey = Guid.NewGuid();
            List<string> matchingValues =
                Enumerable.Range(1, Random.Next(10, 100)).Select(n => Random.RandomString(20)).ToList();
            IEnumerable<KeyValuePair<Guid, string>> otherEntries =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            IEnumerable<KeyValuePair<Guid, string>> collection = matchingValues.Select(
                value => new KeyValuePair<Guid, string>(matchingKey, value))
                .Concat(otherEntries);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            IGrouping<Guid, String> output;
            concurrentLookup.TryGet(matchingKey, out output);
            Assert.IsNotNull(output);
            CollectionAssert.AreEquivalent(matchingValues, output.ToList());
        }

        [TestMethod]
        public void TryGet_ExistingKey_OutputKeyMatchesKey()
        {
            Guid matchingKey = Guid.NewGuid();
            List<string> matchingValues =
                Enumerable.Range(1, Random.Next(10, 100)).Select(n => Random.RandomString(20)).ToList();
            IEnumerable<KeyValuePair<Guid, string>> otherEntries =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            IEnumerable<KeyValuePair<Guid, string>> collection = matchingValues.Select(
                value => new KeyValuePair<Guid, string>(matchingKey, value))
                .Concat(otherEntries);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            IGrouping<Guid, String> output;
            concurrentLookup.TryGet(matchingKey, out output);
            Assert.IsNotNull(output);
            Assert.AreEqual(matchingKey, output.Key);
        }

        [TestMethod]
        public void TryGet_AbsentKey_OutputsNull()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            IGrouping<Guid, String> output;
            concurrentLookup.TryGet(absentKey, out output);
            Assert.IsNull(output);
        }

        [TestMethod]
        public void Contains_ExistingKey_ReturnsTrue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid existingKey = collection[Random.Next(0, collection.Count)].Key;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsTrue(concurrentLookup.Contains(existingKey));
        }

        [TestMethod]
        public void Contains_AbsentKey_ReturnsFalse()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsFalse(concurrentLookup.Contains(absentKey));
        }

        [TestMethod]
        public void TryGet_ExistingKey_ReturnsTrue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid existingKey = collection[Random.Next(0, collection.Count)].Key;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            IGrouping<Guid, String> output;
            Assert.IsTrue(concurrentLookup.TryGet(existingKey, out output));
        }

        [TestMethod]
        public void TryGet_AbsentKey_ReturnsFalse()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            IGrouping<Guid, String> output;
            Assert.IsFalse(concurrentLookup.TryGet(absentKey, out output));
        }

        [TestMethod]
        public void Remove_ExistingKey_ReturnsTrue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid existingKey = collection[Random.Next(0, collection.Count)].Key;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsTrue(concurrentLookup.Remove(existingKey));
        }

        [TestMethod]
        public void Remove_AbsentKey_ReturnsFalse()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsFalse(concurrentLookup.Remove(absentKey));
        }

        [TestMethod]
        public void Contains_AfterRemovingKey_ReturnsFalse()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid existingKey = collection[Random.Next(0, collection.Count)].Key;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Remove(existingKey);
            Assert.IsFalse(concurrentLookup.Contains(existingKey));
        }

        [TestMethod]
        public void Remove_ExistingKeyAndValue_ReturnsTrue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            KeyValuePair<Guid, string> existing = collection[Random.Next(0, collection.Count)];
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsTrue(concurrentLookup.Remove(existing.Key, existing.Value));
        }

        [TestMethod]
        public void Remove_AbsentKeyAndValue_ReturnsFalse()
        {
            Guid absentKey = Guid.NewGuid();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)));
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsFalse(concurrentLookup.Remove(absentKey, Random.RandomString(15)));
        }

        [TestMethod]
        public void Remove_WrongKeyForValue_ReturnsFalse()
        {
            Guid absentKey = Guid.NewGuid();
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            string existingValue = collection[Random.Next(0, collection.Count)].Value;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsFalse(concurrentLookup.Remove(absentKey, existingValue));
        }

        [TestMethod]
        public void Remove_WrongValueForKey_ReturnsFalse()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid existingKey = collection[Random.Next(0, collection.Count)].Key;
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            Assert.IsFalse(concurrentLookup.Remove(existingKey, Random.RandomString(50)));
        }

        [TestMethod]
        public void Contains_AfterRemovingLastValueInKey_ReturnsFalse()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            KeyValuePair<Guid, string> existing = collection[Random.Next(0, collection.Count)];
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Remove(existing.Key, existing.Value);
            Assert.IsFalse(concurrentLookup.Contains(existing.Key));
        }

        [TestMethod]
        public void Contains_AfterRemovingButValuesStillExistForKey_ReturnsTrue()
        {
            Guid existingKeyWithDuplicates = Guid.NewGuid();
            List<KeyValuePair<Guid, string>> existingValues =
                Enumerable.Range(1, Random.Next(2, 10)).Select(
                    n => new KeyValuePair<Guid, string>(existingKeyWithDuplicates, Random.RandomString(20))).ToList();
            IEnumerable<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10)))
                    .Concat(existingValues);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Remove(existingKeyWithDuplicates, existingValues.First().Value);
            Assert.IsTrue(concurrentLookup.Contains(existingKeyWithDuplicates));
        }

        [TestMethod]
        public void Contains_AfterAddingKey_ReturnsTrue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid newKey = Guid.NewGuid();
            String newValue = Random.RandomString(20);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Add(newKey, newValue);
            Assert.IsTrue(concurrentLookup.Contains(newKey));
        }

        [TestMethod]
        public void This_AfterAddingKey_ContainsAddedValue()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid newKey = Guid.NewGuid();
            String newValue = Random.RandomString(20);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Add(newKey, newValue);
            CollectionAssert.Contains(concurrentLookup[newKey].ToList(), newValue);
        }

        [TestMethod]
        public void This_AfterAddingDuplicateValue_ContainsBothAddedValues()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid newKey = Guid.NewGuid();
            String newValue = Random.RandomString(20);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Add(newKey, newValue);
            concurrentLookup.Add(newKey, newValue);
            CollectionAssert.AreEqual(new List<String> {newValue, newValue}, concurrentLookup[newKey].ToList());
        }

        [TestMethod]
        public void This_AfterRemovingDuplicateValue_StillContainsOneAddedValues()
        {
            List<KeyValuePair<Guid, string>> collection =
                Enumerable.Range(1, Random.Next(10, 100)).Select(
                    n => new KeyValuePair<Guid, string>(Guid.NewGuid(), Random.RandomString(10))).ToList();
            Guid newKey = Guid.NewGuid();
            String newValue = Random.RandomString(20);
            ConcurrentLookup<Guid, String> concurrentLookup = new ConcurrentLookup<Guid, string>(collection);
            concurrentLookup.Add(newKey, newValue);
            concurrentLookup.Add(newKey, newValue);
            concurrentLookup.Remove(newKey, newValue);
            CollectionAssert.AreEqual(new List<String> {newValue}, concurrentLookup[newKey].ToList());
        }
    }
}
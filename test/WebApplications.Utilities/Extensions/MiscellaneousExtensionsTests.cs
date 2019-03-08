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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using WebApplications.Testing;

namespace WebApplications.Utilities.Test.Extensions
{
    [TestClass]
    public class MiscellaneousExtensionsTests : UtilitiesTestBase
    {
        // ReSharper disable InconsistentNaming
#pragma warning disable 618

        [TestMethod]
        public void EpochStart_Value_IsUnixEpoch()
        {
            Assert.AreEqual(
                new DateTime(1970, 1, 1),
                UtilityExtensions.EpochStart,
                "The value of EpochStart should be the UNIX epoch: 1st Jan 1970.");
        }

        [TestMethod]
        public void DefaultSplitCharacters_Value_AreAsGivenHere()
        {
            char[] expectedValue = new[] {' ', ',', '\t', '\r', '\n', '|'};
            Assert.AreEqual(
                expectedValue.Length,
                UtilityExtensions.DefaultSplitChars.Length,
                "The value of the DefaultSplitChars array should be space, comma, tab, carriage return, newline, and pipe.");
            foreach (Char value in expectedValue)
                Assert.IsTrue(
                    UtilityExtensions.DefaultSplitChars.Contains(value),
                    String.Format(
                        "The character '{0}' should be included in the DefaultSplitChars array",
                        value));
        }

        [TestMethod]
        public void ToOrdinal_Int_SameAsGetOrdinalWithValuePrepended()
        {
            int value = Random.Next();
            Assert.AreEqual(
                String.Format("{0}{1}", value.ToString(), value.GetOrdinal()),
                value.ToOrdinal(),
                "The result of int.ToOrdinal should be the same as prepending the value to the result of int.GetOrdinal.");
        }

        [TestMethod]
        public void GetOrdinal_ValueInTeens_OrdinalIsTh()
        {
            int hundreds = Random.Next(0, 1000);
            int tens = 1;
            int units = Random.Next(0, 10);
            int value = hundreds * 100 + tens * 10 + units;
            Assert.AreEqual(
                "th",
                value.GetOrdinal(),
                "The ordinal of any number where the next to last digit is 1 (e.g. 3219 and 12) is 'th'.");
        }

        [TestMethod]
        public void GetOrdinal_ValueEndsInOne_OrdinalIsSt()
        {
            int hundreds = Random.Next(0, 1000);
            // rule out the case of tens=1
            int tens;
            do
            {
                tens = Random.Next(0, 10);
            } while (tens == 1);
            int units = 1;
            int value = hundreds * 100 + tens * 10 + units;
            Assert.AreEqual("st", value.GetOrdinal(), "The ordinal of any number ending in a 1 (e.g. 21) is 'st'.");
        }

        [TestMethod]
        public void GetOrdinal_ValueEndsInTwo_OrdinalIsNd()
        {
            int hundreds = Random.Next(0, 1000);
            // rule out the case of tens=1
            int tens;
            do
            {
                tens = Random.Next(0, 10);
            } while (tens == 1);
            int units = 2;
            int value = hundreds * 100 + tens * 10 + units;
            Assert.AreEqual("nd", value.GetOrdinal(), "The ordinal of any number ending in a 2 (e.g. 32) is 'nd'.");
        }

        [TestMethod]
        public void GetOrdinal_ValueEndsInThree_OrdinalIsRd()
        {
            int hundreds = Random.Next(0, 1000);
            // rule out the case of tens=1
            int tens;
            do
            {
                tens = Random.Next(0, 10);
            } while (tens == 1);
            int units = 3;
            int value = hundreds * 100 + tens * 10 + units;
            Assert.AreEqual("rd", value.GetOrdinal(), "The ordinal of any number ending in a 1 (e.g. 453) is 'rd'.");
        }

        [TestMethod]
        public void GetOrdinal_ValueEndsInfourOrGreater_OrdinalIsth()
        {
            int hundreds = Random.Next(0, 1000);
            // rule out the case of tens=1
            int tens;
            do
            {
                tens = Random.Next(0, 10);
            } while (tens == 1);
            int units = Random.Next(4, 10);
            int value = hundreds * 100 + tens * 10 + units;
            Assert.AreEqual(
                "th",
                value.GetOrdinal(),
                "The ordinal of any number ending in a 4 or greater (e.g. 454 or 929) is 'th'.");
        }

        [TestMethod]
        public void ToEnglish_Int_SameResultAsForDouble()
        {
            int value = Random.Next();
            Assert.AreEqual(
                ((double) value).ToEnglish(),
                value.ToEnglish(),
                "The result of int.ToEnglish should be the same as with double.ToEnglish.");
        }

        [TestMethod]
        public void ToEnglish_Long_SameResultAsForDouble()
        {
            int value = Random.Next();
            Assert.AreEqual(
                ((double) value).ToEnglish(),
                value.ToEnglish(),
                "The result of long.ToEnglish should be the same as with double.ToEnglish.");
        }

        [TestMethod]
        public void ToEnglish_NegativeNumber_PrefixedByNegative()
        {
            long value = -Random.Next();
            Assert.IsTrue(
                value.ToEnglish().StartsWith("Negative "),
                "For values less than zero, the result of ToEnglish should begin with 'Negative '.");
        }

        [TestMethod]
        public void ToEnglish_NegativeNumber_SameAsPositivePrefixedByWordNegative()
        {
            long value = Random.Next();
            Assert.AreEqual(
                String.Format("Negative {0}", value.ToEnglish()),
                (-value).ToEnglish(),
                "The result of ToEnglish for a negative number should be the same as its positive equivilant plus a prefix of 'Negative '.");
        }

        [TestMethod]
        public void ToEnglish_NegativeFloatingPointNumber_PrefixedByNegative()
        {
            double value = -Random.Next() + Random.NextDouble();
            Assert.IsTrue(
                value.ToEnglish().StartsWith("Negative "),
                "For values less than zero, the result of ToEnglish should begin with 'Negative '.");
        }

        [TestMethod]
        public void ToEnglish_NegativeFloatingPointNumber_SameAsPositivePrefixedByWordNegative()
        {
            double value = Random.Next() + Random.NextDouble();
            Assert.AreEqual(
                String.Format("Negative {0}", value.ToEnglish()),
                (-value).ToEnglish(),
                "The result of ToEnglish for a negative number should be the same as its positive equivilant plus a prefix of 'Negative '.");
        }

        [TestMethod]
        public void ToEnglish_Zero_GivesZero()
        {
            Assert.AreEqual("Zero", 0.ToEnglish(), "The result of ToEnglish for the number 0 should be 'Zero'.");
        }

        [TestMethod]
        public void ToEnglish_FloatingPointZero_GivesZero()
        {
            Assert.AreEqual("Zero", 0.0.ToEnglish(), "The result of ToEnglish for the number 0 should be 'Zero'.");
        }

        [TestMethod]
        public void ToEnglish_ValueBetweenOneAndTwoHundred_ProvidesWordAndAfterHundred()
        {
            int value = Random.Next(1, 100);
            Assert.AreEqual(
                String.Format("One Hundred And {0}", value.ToEnglish()),
                (100 + value).ToEnglish(),
                "The result of ToEnglish for a number between 101 and 199 should start with 'One Hundred And'.");
        }

        [TestMethod]
        public void ToEnglish_ExactlyOneHundred_DoesNotContainAndAfterHundred()
        {
            Assert.AreEqual(
                "One Hundred",
                100.ToEnglish(),
                "The result of ToEnglish for a 100 should be 'One Hundred'.");
        }

        [TestMethod]
        public void ToEnglish_FractionalValue_ContainsPoint()
        {
            double value;
            do
            {
                value = Random.Next() * Math.Pow(10, -Random.Next(1, 10));
            } while (value - Math.Floor(value) < 0.0001);
            
            Assert.IsTrue(
                value.ToEnglish().Contains(" Point "),
                "Where values contain a fractional component, the result of ToEnglish should contain ' Point '.");
        }

        [TestMethod]
        public void ToEnglish_FractionalValue_WordsAfterPointMatchesNumberOfDecimalPlaces()
        {
            // Note that we cannot guarantee this due to floating point errors, so the random generation has been stripped out
            const double value = 1.355d;
            string result = value.ToEnglish();
            string wordsAfterPoint = result.Split(new[] {" Point "}, StringSplitOptions.None)[1];
            string[] numberOfWordsAfterPoint = wordsAfterPoint.Split(' ').ToArray();

            Assert.AreEqual(3, numberOfWordsAfterPoint.Length);
        }

        [TestMethod]
        public void EqualsByRuntimeType_BothInputsNull_ReturnsTrue()
        {
            Assert.IsTrue(
                ((object) null).EqualsByRuntimeType(null),
                "Using EqualsByRuntimeType to compare two nulls should return true.");
        }

        [TestMethod]
        public void EqualsByRuntimeType_FirstInputOnlyNull_ReturnsFalse()
        {
            Assert.IsFalse(
                ((object) null).EqualsByRuntimeType(Random.Next()),
                "Using EqualsByRuntimeType to compare a null with anything not null should return false.");
        }

        [TestMethod]
        public void EqualsByRuntimeType_SecondInputOnlyNull_ReturnsFalse()
        {
            Assert.IsFalse(
                Random.Next().EqualsByRuntimeType(null),
                "Using EqualsByRuntimeType to compare a null with anything not null should return false.");
        }

        [TestMethod]
        public void EqualsByRuntimeType_InputsOfDifferentTypes_ReturnsFalse()
        {
            int value = Random.Next();
            Assert.IsFalse(
                value.EqualsByRuntimeType((long) value),
                "Using EqualsByRuntimeType to compare values of different types should return false.");
        }

        [TestMethod]
        public void EqualsByRuntimeType_InputsOfSameTypesButNotEqual_ReturnsFalse()
        {
            int value = Random.Next();
            Assert.IsFalse(
                value.EqualsByRuntimeType(value - Random.Next(1, 20)),
                "Using EqualsByRuntimeType to compare different values of the same type should return false.");
        }

        [TestMethod]
        public void EqualsByRuntimeType_InputsOfSameTypesAndEqual_ReturnsTrue()
        {
            int value = Random.Next();
            Assert.IsTrue(
                value.EqualsByRuntimeType(value),
                "Using EqualsByRuntimeType to compare equal values of the same type should return true.");
        }

        [TestMethod]
        public void DeepEquals_IdenticalLists_ReturnsTrue()
        {
            List<int> list = new List<int> {1, 2, 3, 4, 5, 6};
            Assert.IsTrue(list.DeepEquals(list), "DeepEquals should be true for identical lists.");
        }

        [TestMethod]
        public void DeepEquals_BothNull_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(
                list.DeepEquals(null),
                "DeepEquals should be true for identical lists, even if they are both null.");
        }

        [TestMethod]
        public void DeepEquals_OneNull_ReturnsFalse()
        {
            List<int> list = new List<int> {1, 2, 3, 4, 5, 6};
            Assert.IsFalse(list.DeepEquals(null), "DeepEquals should be false if only one list is null.");
            Assert.IsFalse(((List<int>) null).DeepEquals(list), "DeepEquals should be false if only one list is null.");
        }

        [TestMethod]
        public void DeepEquals_ListsOfSameSizeButOneDifferentValue_ReturnsFalse()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 5, 6};
            List<int> listB = new List<int> {1, 2, 3, 99, 5, 6};
            Assert.IsFalse(
                listA.DeepEquals(listB),
                "DeepEquals should be false for lists of identical length but with different values.");
        }

        [TestMethod]
        public void DeepEquals_EquivilantListsWithDuplicatedValues_ReturnsTrue()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 3, 6};
            List<int> listB = new List<int> {1, 4, 3, 3, 2, 6};
            Assert.IsTrue(
                listA.DeepEquals(listB),
                "DeepEquals should be true for lists whose contents contains duplicates, but the same number of each .");
        }

        [TestMethod]
        public void DeepEquals_ListsWithVaryingNumberOfDuplicatedValues_ReturnsFalse()
        {
            List<int> listA = new List<int> {1, 2, 3, 3, 5, 6};
            List<int> listB = new List<int> {1, 2, 3, 5, 5, 6};
            Assert.IsFalse(
                listA.DeepEquals(listB),
                "DeepEquals should be false for lists containing the same values, but with different amounts of each duplicated.");
        }

        [TestMethod]
        public void DeepEquals_ListsOfDifferentSize_ReturnsFalse()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 5, 6};
            List<int> listB = new List<int> {1, 2, 3, 5, 6};
            Assert.IsFalse(listA.DeepEquals(listB), "DeepEquals should be false for lists of different length.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerIdenticalLists_ReturnsTrue()
        {
            List<string> list = new List<string> {"a", "b", "c", "d", "e"};
            Assert.IsTrue(
                list.DeepEquals(list, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be true for identical lists.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerEquivilantLists_ReturnsTrue()
        {
            List<string> list1 = new List<string> {"a", "b", "C", "D", "e"};
            List<string> list2 = new List<string> {"a", "B", "c", "d", "e"};
            Assert.IsTrue(
                list1.DeepEquals(list2, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be true for list where all items are deemed equivilant by the comparer supplied.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerBothNull_ReturnsTrue()
        {
            List<string> list = null;
            Assert.IsTrue(
                list.DeepEquals(null, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be true for identical lists, even if they are both null.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerOneNull_ReturnsFalse()
        {
            List<string> list = new List<string> {"a", "b", "c", "d", "e"};
            Assert.IsFalse(
                list.DeepEquals(null, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be false if only one list is null.");
            Assert.IsFalse(
                ((List<string>) null).DeepEquals(list, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be false if only one list is null.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerListsOfSameSizeButOneDifferentValue_ReturnsFalse()
        {
            List<string> listA = new List<string> {"a", "b", "c", "d", "e"};
            List<string> listB = new List<string> {"a", "b", "c", "z", "e"};
            Assert.IsFalse(
                listA.DeepEquals(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be false for lists of identical length but with different values.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerEquivilantListsWithDuplicatedValues_ReturnsTrue()
        {
            List<string> listA = new List<string> {"a", "b", "B", "d", "e"};
            List<string> listB = new List<string> {"a", "B", "d", "B", "e"};
            Assert.IsTrue(
                listA.DeepEquals(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be true for lists whose contents contains duplicates, but the same number of each .");
        }

        [TestMethod]
        public void DeepEquals_WithComparerListsWithVaryingNumberOfDuplicatedValues_ReturnsFalse()
        {
            List<string> listA = new List<string> {"a", "b", "d", "d", "e"};
            List<string> listB = new List<string> {"a", "b", "b", "d", "e"};
            Assert.IsFalse(
                listA.DeepEquals(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be false for lists containing the same values, but with different amounts of each duplicated.");
        }

        [TestMethod]
        public void DeepEquals_WithComparerListsOfDifferentSize_ReturnsFalse()
        {
            List<string> listA = new List<string> {"a", "b", "c", "d", "e"};
            List<string> listB = new List<string> {"a", "b", "c", "d", "e", "f"};
            Assert.IsFalse(
                listA.DeepEquals(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEquals should be false for lists of different length.");
        }

        [TestMethod]
        public void DeepEqualsSimple_IdenticalLists_ReturnsTrue()
        {
            List<int> list = new List<int> {1, 2, 3, 4, 5, 6};
            Assert.IsTrue(list.DeepEqualsSimple(list), "DeepEqualsSimple should be true for identical lists.");
        }

        [TestMethod]
        public void DeepEqualsSimple_BothNull_ReturnsTrue()
        {
            List<int> list = null;
            Assert.IsTrue(
                list.DeepEqualsSimple(null),
                "DeepEqualsSimple should be true for identical lists, even if they are both null.");
        }

        [TestMethod]
        public void DeepEqualsSimple_OneNull_ReturnsFalse()
        {
            List<int> list = new List<int> {1, 2, 3, 4, 5, 6};
            Assert.IsFalse(list.DeepEqualsSimple(null), "DeepEqualsSimple should be false if only one list is null.");
            Assert.IsFalse(
                ((List<int>) null).DeepEqualsSimple(list),
                "DeepEqualsSimple should be false if only one list is null.");
        }

        [TestMethod]
        public void DeepEqualsSimple_ListsIdenticalValuesInDifferentOrder_ReturnsTrue()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 5, 6};
            List<int> listB = new List<int> {2, 3, 1, 6, 5, 4};
            Assert.IsTrue(
                listA.DeepEqualsSimple(listB),
                "DeepEqualsSimple should be true for lists of identical content but with different orders.");
        }

        [TestMethod]
        public void DeepEqualsSimple_ListsOfSameSizeButOneDifferentValue_ReturnsFalse()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 5, 6};
            List<int> listB = new List<int> {1, 2, 3, 99, 5, 6};
            Assert.IsFalse(
                listA.DeepEqualsSimple(listB),
                "DeepEqualsSimple should be false for lists of identical length but with different values.");
        }

        [TestMethod]
        public void DeepEqualsSimple_ListsOfDifferentSize_ReturnsFalse()
        {
            List<int> listA = new List<int> {1, 2, 3, 4, 5, 6};
            List<int> listB = new List<int> {1, 2, 3, 5, 6};
            Assert.IsFalse(
                listA.DeepEqualsSimple(listB),
                "DeepEqualsSimple should be false for lists of different length.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerIdenticalLists_ReturnsTrue()
        {
            List<string> list = new List<string> {"a", "b", "c", "d", "e"};
            Assert.IsTrue(
                list.DeepEqualsSimple(list, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be true for identical lists.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerEquivilantLists_ReturnsTrue()
        {
            List<string> list1 = new List<string> {"a", "b", "C", "D", "e"};
            List<string> list2 = new List<string> {"a", "B", "c", "d", "e"};
            Assert.IsTrue(
                list1.DeepEqualsSimple(list2, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be true for list where all items are deemed equivilant by the comparer supplied.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerBothNull_ReturnsTrue()
        {
            List<string> list = null;
            Assert.IsTrue(
                list.DeepEqualsSimple(null, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be true for identical lists, even if they are both null.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerOneNull_ReturnsFalse()
        {
            List<string> list = new List<string> {"a", "b", "c", "d", "e"};
            Assert.IsFalse(
                list.DeepEqualsSimple(null, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be false if only one list is null.");
            Assert.IsFalse(
                ((List<string>) null).DeepEqualsSimple(list, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be false if only one list is null.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerListsOfSameSizeButOneDifferentValue_ReturnsFalse()
        {
            List<string> listA = new List<string> {"a", "b", "c", "d", "e"};
            List<string> listB = new List<string> {"a", "b", "c", "z", "e"};
            Assert.IsFalse(
                listA.DeepEqualsSimple(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be false for lists of identical length but with different values.");
        }

        [TestMethod]
        public void DeepEqualsSimple_WithComparerListsOfDifferentSize_ReturnsFalse()
        {
            List<string> listA = new List<string> {"a", "b", "c", "d", "e"};
            List<string> listB = new List<string> {"a", "b", "c", "d", "e", "f"};
            Assert.IsFalse(
                listA.DeepEqualsSimple(listB, StringComparer.OrdinalIgnoreCase),
                "DeepEqualsSimple should be false for lists of different length.");
        }

        [TestMethod]
        public void ToDictionary_EmptyNameValueCollection_GivesEmptyDict()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            Dictionary<string, string> dict = nameValueCollection.ToDictionary();
            Assert.AreEqual(
                0,
                dict.Count,
                "Converting an empty nameValueCollection with ToDictionary should result in an empty dictionary");
        }

        [TestMethod]
        public void ToDictionary_NameValueCollection_HasSameItemCount()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            int count = Random.Next(1, 20);
            for (int i = 0; i < count; i++)
                nameValueCollection.Set(
                    String.Format("{1} {0}", i, Random.Next()),
                    Random.Next().ToString(CultureInfo.InvariantCulture));
            Dictionary<string, string> dict = nameValueCollection.ToDictionary();
            Assert.AreEqual(
                nameValueCollection.Count,
                dict.Count,
                "Converting a nameValueCollection with ToDictionary should preserve item count");
        }

        [TestMethod]
        public void ToDictionary_NameValueCollection_ValuesMatch()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            int count = Random.Next(5, 10);
            for (int i = 0; i < count; i++)
                nameValueCollection.Set(
                    String.Format("{1} {0}", i, Random.Next()),
                    Random.Next().ToString(CultureInfo.InvariantCulture));
            Dictionary<string, string> dict = nameValueCollection.ToDictionary();
            foreach (string key in nameValueCollection)
                Assert.AreEqual(
                    nameValueCollection.Get(key),
                    dict[key],
                    "After converting a nameValueCollection with ToDictionary, the values of each key should be preserved.");
        }

        [TestMethod]
        public void ToDictionary_NameValueCollection_CaseInsensitiveKeys()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            // Add the values using mixed case and upper case keys even though NameValueCollections are case insensitive
            nameValueCollection.Set("Keyname", Random.Next().ToString(CultureInfo.InvariantCulture));
            nameValueCollection.Set("KEYNAME", Random.Next().ToString(CultureInfo.InvariantCulture));
            Dictionary<string, string> dict = nameValueCollection.ToDictionary();
            Assert.IsTrue(
                dict.ContainsKey("keyname"),
                "After converting a NameValueCollection with ToDictionary, the keys should be case insensitive.");
            Assert.AreEqual(
                nameValueCollection.Get("keyname"),
                dict["keyname"],
                "After converting a nameValueCollection with ToDictionary, the keys should be case insensitive.");
            Assert.IsTrue(
                dict.ContainsKey("Keyname"),
                "After converting a NameValueCollection with ToDictionary, the keys should be case insensitive.");
            Assert.IsTrue(
                dict.ContainsKey("KEYNAME"),
                "After converting a NameValueCollection with ToDictionary, the keys should be case insensitive.");
        }

        [TestMethod]
        public void XmlEscape_Object_ConvertsToString()
        {
            object obj = Random.Next();
            Assert.AreEqual(obj.ToString(), obj.XmlEscape());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void XmlEscape_NullObject_ThrowsArgumentNullException()
        {
            Assert.IsNull(((object) null).XmlEscape());
        }

        [TestMethod]
        public void XmlEscape_NullString_ReturnsNull()
        {
            Assert.IsNull(((String) null).XmlEscape());
        }

        [TestMethod]
        public void XmlEscape_EmptyString_ReturnsEmptyString()
        {
            Assert.AreEqual(String.Empty, String.Empty.XmlEscape());
        }

        [TestMethod]
        public void XmlEscape_UnicodeString_OutputContainsValidXmlText()
        {
            String output = Random.RandomString().XmlEscape();
            XmlDocument xml = new XmlDocument();
            // The following line throws an exception if the output is not valid xml
            xml.LoadXml(String.Format("<xml>{0}</xml>", output));
        }

        [TestMethod]
        public void XmlEscape_AlphaNumericString_ReturnsStringUnchanged()
        {
            const String input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            String output = input.XmlEscape();
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetEmbeddedXml_NullAssembly_ThrowsInvalidoperationException()
        {
            Assembly assembly = null;
            // ReSharper disable ExpressionIsAlwaysNull
            assembly.GetEmbeddedXml("filename");
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetEmbeddedXml_NullFilename_ThrowsInvalidoperationException()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            assembly.GetEmbeddedXml(null);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetEmbeddedXml_WhitespaceFilename_ThrowsInvalidoperationException()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            assembly.GetEmbeddedXml("       ");
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void GetEmbeddedXml_FilenameNotMatchingAnyManifest_ThrowsInvalidoperationException()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            assembly.GetEmbeddedXml("this filepath is assumed not to exist");
        }

        [TestMethod]
        public void GetEpochTime_UnixEpoch_ReturnsZero()
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            Assert.AreEqual(0, dateTime.GetEpochTime(), "Performing GetEpochTime on 1st Jan 1970 should return 0.");
        }

        [TestMethod]
        public void GetEpochTime_UnixEpochPlusSomeHours_ReturnsNumberOfHoursMultipliedBy3600000()
        {
            int hours = Random.Next(1, 100);
            DateTime dateTime = new DateTime(1970, 1, 1).AddHours(hours);
            Assert.AreEqual(
                hours * 3600000,
                dateTime.GetEpochTime(),
                "Performing GetEpochTime on (1st Jan 1970 + x hours) should return x*3600000.");
        }

        [TestMethod]
        public void GetEpochTime_DateTimeWithLocalTimeOutsideDaylightSavingTime_ReturnsMillisecondsPastUnixEpoch()
        {
            long seconds;
            DateTime dateTime;
            do
            {
                seconds = Random.Next();
                dateTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Local).AddSeconds(seconds);
            } while (dateTime.IsDaylightSavingTime());
            Assert.AreEqual(
                seconds * 1000,
                dateTime.GetEpochTime(),
                "Performing GetEpochTime outside of daylight saving time should return the number of milliseconds past the unix epoch.");
        }

        [TestMethod]
        public void GetEpochTime_DateTimeWithLocalTimeInDaylightSavingTime_ReturnsMillisecondsPastUnixEpoch()
        {
            long seconds;
            DateTime dateTime;
            do
            {
                seconds = Random.Next();
                dateTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Local).AddSeconds(seconds);
            } while (!dateTime.IsDaylightSavingTime());
            Assert.AreEqual(
                seconds * 1000,
                dateTime.GetEpochTime(),
                "Performing GetEpochTime in daylight saving time should return the number of milliseconds past the unix epoch.");
        }

        [TestMethod]
        public void GetEpochTime_DateTimeWithUTC_ReturnsMillisecondsPastUnixEpoch()
        {
            long seconds = Random.Next();
            DateTime dateTime = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc).AddSeconds(seconds);
            Assert.AreEqual(
                seconds * 1000,
                dateTime.GetEpochTime(),
                "Performing GetEpochTime should return the number of milliseconds past the unix epoch.");
        }

        [TestMethod]
        public void GetDateTime_ResultFromGetEpochTime_ReturnsOriginalDateTimeToWithinMillisecond()
        {
            DateTime dateTime = DateTime.Now.AddTicks(Random.Next()).AddSeconds(-Random.Next());
            Assert.AreEqual(
                0,
                (dateTime - UtilityExtensions.GetDateTime(dateTime.GetEpochTime())).TotalMilliseconds,
                1.0);
        }

        [TestMethod]
        public void StripHTML_Tag_StripsTag()
        {
            const string testString = "Surrounding <htmltag> Text";
            Assert.IsFalse(
                testString.StripHTML().Contains("htmltag"),
                "StripHTML should remove all HTML tags from '{0}'.",
                testString);
        }

        [TestMethod]
        public void StripHTML_TagWithAttributes_StripsTag()
        {
            const string testString = "Surrounding <htmltag attribute=\"value\"> Text";
            Assert.IsFalse(
                testString.StripHTML().Contains("htmltag"),
                "StripHTML should remove all HTML tags from '{0}'.",
                testString);
        }

        [TestMethod]
        public void StripHTML_TagSplitAcrossTwoLines_StripsTag()
        {
            const string testString = "Surrounding <htmltag\n> Text";
            Assert.IsFalse(
                testString.StripHTML().Contains("htmltag"),
                "StripHTML should remove all HTML tags from '{0}'.",
                testString);
        }

        [TestMethod]
        public void StripHTML_TagsWithBracketsWithin_StripsEntireTag()
        {
            const string testString = "Surrounding <htmltag<>> Text";
            Assert.IsFalse(
                testString.StripHTML().Contains("htmltag"),
                "StripHTML should remove all HTML tags from '{0}'.",
                testString);
        }

        [TestMethod]
        public void StripHTML_EntityEncodedTags_TagsIgnored()
        {
            const string testString = "Surrounding &lt;htmltag&gt; Text";
            Assert.IsTrue(
                testString.StripHTML().Contains("htmltag"),
                "StripHTML should not remove the entity encoded (and thus safe) tags from '{0}'.",
                testString);
        }

        [TestMethod]
        public void ToRadians_ValuesInRangeZeroToThreeSixty__ConvertsFromDegreesToRadians()
        {
            double degrees = Random.NextDouble() * 360;
            Assert.AreEqual(
                (degrees * Math.PI) / 180,
                degrees.ToRadians(),
                1e-10,
                "ToRadians should convert a value in degrees to the value in radians (i.e. multiply by pi/180)");
        }

        [TestMethod]
        public void ToDegrees_ValuesInRangeZeroToTwoPi__ConvertsFromRadiansToDegrees()
        {
            double radians = Random.NextDouble() * Math.PI * 2;
            Assert.AreEqual(
                (radians * 180) / Math.PI,
                radians.ToDegrees(),
                1e-10,
                "ToDegrees should convert a value in radians to the value in degrees (i.e. divide by pi/180)");
        }

        [TestMethod]
        public void GetDateTime_StandardGuid_GivesSameResultAsCombGuidGetDateTime()
        {
            Guid standardGuid = Guid.NewGuid();
            Assert.AreEqual(CombGuid.GetDateTime(standardGuid), standardGuid.GetDateTime());
        }

        [TestMethod]
        public void GetDateTime_CombGuid_GivesSameResultAsCombGuidGetDateTime()
        {
            Guid combGuid = CombGuid.NewCombGuid();
            Assert.AreEqual(CombGuid.GetDateTime(combGuid), combGuid.GetDateTime());
        }

        [TestMethod]
        public void UnWrap_AfterPerformingWrap_ReturnsWrappedObject()
        {
            string wrappedObject = Random.RandomString(10);
            IAsyncResult initial = new Mock<IAsyncResult>().Object;
            IAsyncResult wrapped = initial.Wrap(wrappedObject);
            string unwrappedObject = wrapped.Unwrap<string>();
            Assert.AreEqual(wrappedObject, unwrappedObject);
        }

        [TestMethod]
        public void UnWrap_AfterPerformingWrapThenUnwrapped_OriginalCanBeUnwrappedAgain()
        {
            string wrappedObject = Random.RandomString(10);
            IAsyncResult initial = new Mock<IAsyncResult>().Object;
            IAsyncResult wrapped = initial.Wrap(wrappedObject);
            string unwrappedObject = wrapped.Unwrap<string>();
            Assert.AreEqual(wrappedObject, wrapped.Unwrap<string>());
        }

        [TestMethod]
        public void UnWrap_AfterPerformingWrapThenUnwrappedWithIAsyncResultOutputted_ReturnsWrappedObject()
        {
            string wrappedObject = Random.RandomString(10);
            IAsyncResult initial = new Mock<IAsyncResult>().Object;
            IAsyncResult wrapped = initial.Wrap(wrappedObject);
            IAsyncResult unwrapped;
            string unwrappedObject = wrapped.Unwrap<string>(out unwrapped);
            Assert.AreEqual(wrappedObject, unwrappedObject);
        }

        [TestMethod]
        public void UnWrap_AfterPerformingWrapThenUnwrappedWithIAsyncResultOutputted_OriginalCanBeUnwrappedAgain()
        {
            string wrappedObject = Random.RandomString(10);
            IAsyncResult initial = new Mock<IAsyncResult>().Object;
            IAsyncResult wrapped = initial.Wrap(wrappedObject);
            IAsyncResult unwrapped;
            string unwrappedObject = wrapped.Unwrap<string>(out unwrapped);
            Assert.AreEqual(wrappedObject, wrapped.Unwrap<string>());
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void
            UnWrap_AfterPerformingWrapThenUnwrappedWithIAsyncResultOutputtedThenUnwrappingOutput_ThrowsArgumentException
            ()
        {
            string wrappedObject = Random.RandomString(10);
            IAsyncResult initial = new Mock<IAsyncResult>().Object;
            IAsyncResult wrapped = initial.Wrap(wrappedObject);
            IAsyncResult unwrapped;
            string unwrappedObject = wrapped.Unwrap<string>(out unwrapped);
            unwrapped.Unwrap<string>();
        }

        [TestMethod]
        public void Mod_PositiveInt_ReturnsModulus()
        {
            int number = Random.Next();
            int mod = Random.Next(2, 100);
            int result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_NegativeInt_ReturnsPositiveModulus()
        {
            int number = -Random.Next();
            int mod = Random.Next(2, 100);
            int result = number.Mod(mod);
            Assert.AreEqual(((number % mod) + mod) % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveIntWithExactMultiple_ReturnsZero()
        {
            int mod = Random.Next(2, 100);
            int number = Random.Next(1, int.MaxValue / mod) * mod;
            int result = number.Mod(mod);
            Assert.AreEqual(0, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_NegativeIntWithExactMultiple_ReturnsZero()
        {
            int mod = Random.Next(2, 100);
            int number = -Random.Next(1, int.MaxValue / mod) * mod;
            int result = number.Mod(mod);
            Assert.AreEqual(0, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveUInt_ReturnsModulus()
        {
            uint number = (uint) Random.Next();
            uint mod = (uint) Random.Next(2, 100);
            uint result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveLong_ReturnsModulus()
        {
            long number = Random.Next();
            long mod = Random.Next(2, 100);
            long result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_NegativeLong_ReturnsPositiveModulus()
        {
            long number = -Random.Next();
            long mod = Random.Next(2, 100);
            long result = number.Mod(mod);
            Assert.AreEqual(((number % mod) + mod) % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveULong_ReturnsModulus()
        {
            ulong number = (ulong) Random.Next();
            ulong mod = (ulong) Random.Next(2, 100);
            ulong result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveShort_ReturnsModulus()
        {
            short number = (short) Random.Next(0, short.MaxValue);
            short mod = (short) Random.Next(2, 100);
            long result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_NegativeShort_ReturnsPositiveModulus()
        {
            short number = (short) -Random.Next(0, short.MaxValue);
            short mod = (short) Random.Next(2, 100);
            short result = number.Mod(mod);
            Assert.AreEqual(((number % mod) + mod) % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void Mod_PositiveUShort_ReturnsModulus()
        {
            ushort number = (ushort) Random.Next(0, ushort.MaxValue);
            ushort mod = (ushort) Random.Next(2, 100);
            long result = number.Mod(mod);
            Assert.AreEqual(number % mod, result, "Expected result is {0}%{1}", number, mod);
        }

        [TestMethod]
        public void IsNull_ValueType_ReturnsFalse()
        {
            int value = Random.Next();
            Assert.IsFalse(value.IsNull());
        }

        [TestMethod]
        public void IsNull_NotNullNullableValueType_ReturnsFalse()
        {
            int? value = Random.Next();
            Assert.IsFalse(value.IsNull());
        }

        [TestMethod]
        public void IsNull_NullNullableValueType_ReturnsTrue()
        {
            int? value = null;
            Assert.IsTrue(value.IsNull());
        }

        [TestMethod]
        public void IsNull_Null_ReturnsTrue()
        {
            Assert.IsTrue(((object) null).IsNull());
        }

        [TestMethod]
        public void IsNull_DBNull_ReturnsTrue()
        {
            Assert.IsTrue(DBNull.Value.IsNull());
        }

        [TestMethod]
        public void IsNull_NullReference_ReturnsTrue()
        {
            MiscellaneousExtensionsTests reference = null;
            Assert.IsTrue(reference.IsNull());
        }

        [TestMethod]
        public void IsNull_ReferenceType_ReturnsFalse()
        {
            MiscellaneousExtensionsTests reference = new MiscellaneousExtensionsTests();
            Assert.IsFalse(reference.IsNull());
        }

        [TestMethod]
        public void Join()
        {
            string[] stringsToJoin = {"string1", null, " ", string.Empty, "string2"};

            Assert.AreEqual(
                "string1, ,,string2",
                stringsToJoin.JoinNotNull(","),
                "JoinNotNull as string extension has failed.");
            Assert.AreEqual(
                "string1, ,string2",
                stringsToJoin.JoinNotNullOrEmpty(","),
                "JoinNotNullOrEmpty as string extension has failed.");
            Assert.AreEqual(
                "string1,string2",
                stringsToJoin.JoinNotNullOrWhiteSpace(","),
                "JoinNotNullOrWhitespace as string extension has failed.");
        }


        [TestMethod]
        [Owner("craig.dean@webappuk.com")]
        public void CreateSet_MultipleValues()
        {
            HashCollection<string> set = (HashCollection<string>) typeof (string).CreateSet(new[] {"a", "b"});
            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains("a"));
            Assert.IsTrue(set.Contains("b"));
        }

        [TestMethod]
        [Owner("craig.dean@webappuk.com")]
        public void CreateSet_NullValue()
        {
            HashCollection<string> set = (HashCollection<string>) typeof (string).CreateSet(null);
            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        [Owner("craig.dean@webappuk.com")]
        public void CreateSet_EmptyValue()
        {
            HashCollection<string> set = (HashCollection<string>) typeof (string).CreateSet(Enumerable.Empty<string>());
            Assert.AreEqual(0, set.Count);
        }

        [TestMethod]
        [Owner("craig.dean@webappuk.com")]
        public void CreateSet_MultipleValuesCast()
        {
            HashCollection<string> set = (HashCollection<string>) typeof (string).CreateSet(new object[] {"a", "b"});
            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains("a"));
            Assert.IsTrue(set.Contains("b"));
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void IsUnassigned_Unassigned_ReturnsTrue()
        {
            object o = new Optional<int?>();
            Assert.IsTrue(o.IsUnassigned(), "Unassigned int was not detected.");
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void IsUnassigned_Assigned_ReturnsFalse()
        {
            object o = new Optional<int?>(6);
            Assert.IsFalse(o.IsUnassigned(), "Assigned Optional<int> was not detected.");
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void IsUnassigned_NotOptional_ReturnsFalse()
        {
            object o = 12;
            Assert.IsFalse(o.IsUnassigned(), "Non-optional int was detected as unassigned.");
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void SimplifiedFullName_NoAssemblyNameSupplied()
        {
            Assert.AreEqual(
                "WebApplications.Utilities.Test.Extensions.MiscellaneousExtensionsTests,WebApplications.Utilities.Test",
                typeof (MiscellaneousExtensionsTests).SimplifiedTypeFullName());
        }

        [TestMethod]
        [Owner("andrew.billings@webappuk.com")]
        public void SimplifiedFullName_AssemblyNameSupplied()
        {
            Assert.AreEqual(
                "WebApplications.Utilities.Test.Extensions.MiscellaneousExtensionsTests",
                typeof (MiscellaneousExtensionsTests).SimplifiedTypeFullName("WebApplications.Utilities.Test"));
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void DefaultAssigned()
        {
            Assert.AreEqual(0, typeof (int).DefaultAssigned());
            Assert.AreEqual(new Optional<int>(0), typeof (Optional<int>).DefaultAssigned());
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void Expression_Unblockify()
        {
            Expression expression = Expression.Multiply(
                Expression.Constant(1),
                Expression.Constant(2));

            Assert.AreSame(expression, expression.UnBlockify().Single());

            IEnumerable<ParameterExpression> variables;
            Assert.AreSame(expression, expression.UnBlockify(out variables).Single());
            Assert.IsFalse(variables.Any());

            Assert.AreSame(
                expression,
                Expression.Block(new[] {expression}).UnBlockify().Single());

            Assert.AreSame(
                expression,
                Expression.Block(new[] {expression}).UnBlockify(out variables).Single());
            Assert.IsFalse(variables.Any());
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void Expression_AddExpressions()
        {
            BlockExpression expression = Expression.Block(
                new Expression[]
                {
                    Expression.Multiply(Expression.Constant(1), Expression.Constant(2))
                });
            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(1, expression.Expressions.Count);

            Assert.AreSame(expression, expression.AddExpressions());

            expression = expression.AddExpressions(
                new Expression[]
                {
                    Expression.Multiply(Expression.Constant(2), Expression.Constant(3))
                }) as BlockExpression;
            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Expressions);
            Assert.AreEqual(2, expression.Expressions.Count);
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void Expression_AddVariables()
        {
            BlockExpression expression = Expression.Block(
                new Expression[]
                {
                    Expression.Multiply(Expression.Constant(1), Expression.Constant(2))
                });
            Assert.IsNotNull(expression.Variables);
            Assert.AreEqual(0, expression.Variables.Count);

            Assert.AreSame(expression, expression.AddVariables());

            expression = expression.AddVariables(
                new[]
                {
                    Expression.Parameter(typeof (int), "x")
                }) as BlockExpression;
            Assert.IsNotNull(expression);
            Assert.IsNotNull(expression.Variables);
            Assert.AreEqual(1, expression.Variables.Count);
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_BasicSignedIntTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (short).IsNumeric());
            Assert.IsTrue(typeof (int).IsNumeric());
            Assert.IsTrue(typeof (long).IsNumeric());

            Assert.IsTrue(typeof (short?).IsNumeric());
            Assert.IsTrue(typeof (int?).IsNumeric());
            Assert.IsTrue(typeof (long?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_BasicUnsignedIntTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (ushort).IsNumeric());
            Assert.IsTrue(typeof (uint).IsNumeric());
            Assert.IsTrue(typeof (ulong).IsNumeric());

            Assert.IsTrue(typeof (ushort?).IsNumeric());
            Assert.IsTrue(typeof (uint?).IsNumeric());
            Assert.IsTrue(typeof (ulong?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_NonBasicSignedIntTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (Int16).IsNumeric());
            Assert.IsTrue(typeof (Int32).IsNumeric());
            Assert.IsTrue(typeof (Int64).IsNumeric());

            Assert.IsTrue(typeof (Int16?).IsNumeric());
            Assert.IsTrue(typeof (Int32?).IsNumeric());
            Assert.IsTrue(typeof (Int64?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_NonBasicUnsignedIntTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (UInt16).IsNumeric());
            Assert.IsTrue(typeof (UInt32).IsNumeric());
            Assert.IsTrue(typeof (UInt64).IsNumeric());

            Assert.IsTrue(typeof (UInt16?).IsNumeric());
            Assert.IsTrue(typeof (UInt32?).IsNumeric());
            Assert.IsTrue(typeof (UInt64?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_BasicFloatTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (float).IsNumeric());
            Assert.IsTrue(typeof (double).IsNumeric());

            Assert.IsTrue(typeof (float?).IsNumeric());
            Assert.IsTrue(typeof (double?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_NonBasicFloatTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (Single).IsNumeric());
            Assert.IsTrue(typeof (Double).IsNumeric());

            Assert.IsTrue(typeof (Single?).IsNumeric());
            Assert.IsTrue(typeof (Double?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_BasicByteTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (sbyte).IsNumeric());
            Assert.IsTrue(typeof (byte).IsNumeric());

            Assert.IsTrue(typeof (sbyte?).IsNumeric());
            Assert.IsTrue(typeof (byte?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_NonBasicByteTypes_ReturnTrue()
        {
            Assert.IsTrue(typeof (SByte).IsNumeric());
            Assert.IsTrue(typeof (Byte).IsNumeric());

            Assert.IsTrue(typeof (SByte?).IsNumeric());
            Assert.IsTrue(typeof (Byte?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_NonNumericNumberTypes_ReturnFalse()
        {
            Assert.IsFalse(typeof (char).IsNumeric());
            Assert.IsFalse(typeof (Char).IsNumeric());
            Assert.IsFalse(typeof (char?).IsNumeric());
            Assert.IsFalse(typeof (Char?).IsNumeric());

            Assert.IsFalse(typeof (decimal).IsNumeric());
            Assert.IsFalse(typeof (Decimal).IsNumeric());
            Assert.IsFalse(typeof (decimal?).IsNumeric());
            Assert.IsFalse(typeof (Decimal?).IsNumeric());

            Assert.IsFalse(typeof (Enum).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_BooleanTypes_ReturnFalse()
        {
            Assert.IsFalse(typeof (bool).IsNumeric());
            Assert.IsFalse(typeof (bool?).IsNumeric());
            Assert.IsFalse(typeof (Boolean).IsNumeric());
            Assert.IsFalse(typeof (Boolean?).IsNumeric());
        }

        [TestMethod]
        [Owner("giles.smart@webappuk.com")]
        public void IsNumeric_ObjectTypes_ReturnFalse()
        {
            Assert.IsFalse(typeof (string).IsNumeric());
            Assert.IsFalse(typeof (String).IsNumeric());

            Assert.IsFalse(typeof (object).IsNumeric());
            Assert.IsFalse(typeof (Object).IsNumeric());
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        [ExpectedException(typeof (ArgumentException))]
        public void ForEach_FailsWithNonEnumerableExpression()
        {
            Expression.Constant(4).ForEach(x => x);
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void ForEach_ReturnsEmptyExpressionWithEmptyEnumerables()
        {
            Expression expression =
                Expression.Constant(Array<int>.Empty, typeof(IEnumerable<int>))
                    .ForEach(x => Array<Expression>.Empty);
            Assert.AreEqual("default(Void)", expression.ToString());
        }

        [TestMethod]
        [Owner("andrew.taylor@webappuk.com")]
        public void ValueComparer()
        {
            ValueComparer<int, string> defaultComparer = ValueComparer<int, string>.Default;
            KeyValuePair<int, string> a10 = new KeyValuePair<int, string>(10, "A");
            KeyValuePair<int, string> b1 = new KeyValuePair<int, string>(1, "B");
            KeyValuePair<int, string> b10 = new KeyValuePair<int, string>(10, "B");
            KeyValuePair<int, string> null10 = new KeyValuePair<int, string>(10, null);

            Assert.IsFalse(defaultComparer.Equals(a10, b1));
            Assert.IsTrue(defaultComparer.Equals(b10, b1));

            Assert.IsFalse(defaultComparer.Equals(a10, null10));
            Assert.IsTrue(defaultComparer.Equals(null10, null10));
            Assert.IsFalse(defaultComparer.Equals(null10, b10));

            Assert.AreEqual(defaultComparer.GetHashCode(b1), defaultComparer.GetHashCode(b10));
            Assert.AreNotEqual(defaultComparer.GetHashCode(a10), defaultComparer.GetHashCode(null10));
        }
    }
}
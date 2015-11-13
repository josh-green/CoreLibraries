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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using WebApplications.Utilities.Difference;

namespace WebApplications.Utilities.Test
{
    [TestClass]
    public class TestToMappedCharArray
    {
        /// <summary>
        /// Tests the string mapping.
        /// </summary>
        /// <param name="failures">The number of failures.</param>
        /// <param name="successes">The number of successes.</param>
        /// <param name="expected">The expected output mapped string.</param>
        /// <param name="input">The input string.</param>
        /// <param name="options">The options.</param>
        private static void TestMap(ref int failures, ref int successes, string expected, string input, TextOptions options)
        {
            Trace.WriteLine(new string('=', 80));
            Trace.WriteLine($"Options:'{options}' String:'{input.Escape()}' Map:");

            // Generate map
            Mapping[] map = input.ToMappedCharArray(options).ToArray();

            // Display map
            Trace.WriteLine(
                string.Join(
                    Environment.NewLine,
                    map.Select(
                        m =>
                            $"{m.Offset,4} {m.Length,4} '{input.Substring(m.Offset, m.Length).Escape()}'")));

            string output = string.Concat(map.Select(m => input.Substring(m.Offset, m.Length)));

            if (!string.Equals(expected, output, StringComparison.Ordinal))
            {
                failures++;
                Trace.WriteLine($"FAILED - Expected a mapped string of '{expected.Escape()}' - got '{output.Escape()}'");
            }
            else successes++;


            Trace.WriteLine(string.Empty);
        }

        [TestMethod]
        public void TestNone()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.None);
            TestMap(ref failures, ref successes, " ", " ", TextOptions.None);
            TestMap(ref failures, ref successes, "\r", "\r", TextOptions.None);
            TestMap(ref failures, ref successes, "\r\n", "\r\n", TextOptions.None);
            TestMap(ref failures, ref successes, "\r\n ", "\r\n ", TextOptions.None);
            TestMap(ref failures, ref successes, " \r\n ", " \r\n ", TextOptions.None);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.None);
            TestMap(ref failures, ref successes, " ab ", " ab ", TextOptions.None);
            TestMap(ref failures, ref successes, "a b", "a b", TextOptions.None);
            TestMap(ref failures, ref successes, " a b ", " a b ", TextOptions.None);
            TestMap(ref failures, ref successes, " a  b ", " a  b ", TextOptions.None);
            TestMap(ref failures, ref successes, " a\rb ", " a\rb ", TextOptions.None);
            TestMap(ref failures, ref successes, " a\r\nb ", " a\r\nb ", TextOptions.None);
            TestMap(ref failures, ref successes, "\ra\r\nb\n", "\ra\r\nb\n", TextOptions.None);
            TestMap(ref failures, ref successes, "\r a \r\n b \n", "\r a \r\n b \n", TextOptions.None);
            TestMap(ref failures, ref successes, "\r ab \r\n cd \n", "\r ab \r\n cd \n", TextOptions.None);
            TestMap(ref failures, ref successes, "\ra b\r\n c d \n", "\ra b\r\n c d \n", TextOptions.None);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }

        [TestMethod]
        public void TestTrim()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.Trim);
            TestMap(ref failures, ref successes, "", " ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\r", "\r", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\r\n", "\r\n", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\r\n", "\r\n ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\r\n", " \r\n ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.Trim);
            TestMap(ref failures, ref successes, "ab", " ab ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "a b", "a b", TextOptions.Trim);
            TestMap(ref failures, ref successes, "a b", " a b ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "a  b", " a  b ", TextOptions.None);
            TestMap(ref failures, ref successes, "a\rb", " a\rb ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "a\r\nb", " a\r\nb ", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\ra\r\nb\n", "\ra\r\nb\n", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\ra\r\nb\n", "\r a \r\n b \n", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\rab\r\ncd\n", "\r ab \r\n cd \n", TextOptions.Trim);
            TestMap(ref failures, ref successes, "\ra b\r\nc d\n", "\ra b\r\n c d \n", TextOptions.Trim);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }

        [TestMethod]
        public void TestNormalizeLineEndings()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " ", " ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", "\r", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", "\r\n", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r ", "\r\n ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " \r ", " \r\n ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " ab ", " ab ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a b", "a b", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " a b ", " a b ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " a  b ", " a  b ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " a\rb ", " a\rb ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, " a\rb ", " a\r\nb ", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\ra\rb\n", "\ra\r\nb\n", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r a \r b \n", "\r a \r\n b \n", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r ab \r cd \n", "\r ab \r\n cd \n", TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\ra b\r c d \n", "\ra b\r\n c d \n", TextOptions.NormalizeLineEndings);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }

        [TestMethod]
        public void TestTrimNormalizeLineEndings()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "", " ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", "\r", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", "\r\n", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", "\r\n ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\r", " \r\n ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "ab", " ab ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a b", "a b", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a b", " a b ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a  b", " a  b ", TextOptions.None);
            TestMap(ref failures, ref successes, "a\rb", " a\rb ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "a\rb", " a\r\nb ", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\ra\rb\n", "\ra\r\nb\n", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\ra\rb\n", "\r a \r\n b \n", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\rab\rcd\n", "\r ab \r\n cd \n", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            TestMap(ref failures, ref successes, "\ra b\rc d\n", "\ra b\r\n c d \n", TextOptions.Trim | TextOptions.NormalizeLineEndings);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }

        [TestMethod]
        public void TestCollapseWhiteSpace()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " ", " ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\r", "\r", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\r", "\r\n", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\r ", "\r\n ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " ", " \r\n ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " ab ", " ab ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "a b", "a b", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " a b ", " a b ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " a b ", " a  b ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " a\rb ", " a\rb ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, " a\rb ", " a\r\nb ", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\ra\rb\n", "\ra\r\nb\n", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\r a \r b \n", "\r a \r\n b \n", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\rab \r cd \n", "\r ab \r\n cd \n", TextOptions.CollapseWhiteSpace);
            TestMap(ref failures, ref successes, "\ra b\r c d \n", "\ra b\r\n c d \n", TextOptions.CollapseWhiteSpace);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }

        [TestMethod]
        public void TestIgnoreWhiteSpace()
        {
            int failures = 0;
            int successes = 0;
            TestMap(ref failures, ref successes, "", "", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "", " ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "", "\r", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "", "\r\n", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "", "\r\n ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "", " \r\n ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "a", "a", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", " ab ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", "a b", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", " a b ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", " a  b ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", " a\rb ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", " a\r\nb ", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", "\ra\r\nb\n", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "ab", "\r a \r\n b \n", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "abcd", "\r ab \r\n cd \n", TextOptions.IgnoreWhiteSpace);
            TestMap(ref failures, ref successes, "abcd", "\ra b\r\n c d \n", TextOptions.IgnoreWhiteSpace);
            if (failures > 0)
                Assert.Fail($"{failures} failures out of {failures + successes}.");
            else
                Trace.WriteLine($"{successes} Successes");
        }
    }
}
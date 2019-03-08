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

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Utilities.Formatting;

namespace WebApplications.Utilities.Test.Formatting
{
    [TestClass]
    public class TestFormatChunks
    {
        [TestMethod]
        public void TestEmpty()
        {
            FormatChunk[] chunks = FormatChunk.Parse(string.Empty).ToArray();
            Assert.AreEqual(0, chunks.Length);
        }

        [TestMethod]
        public void TestNoFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse(" ").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual(" ", chunk.Value);
            Assert.IsNull(chunk.Tag);
        }

        [TestMethod]
        public void TestSimpleFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(0, chunk.Alignment);
            Assert.IsNull(chunk.Format);
        }

        [TestMethod]
        public void TestAlignedFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,-1}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0,-1}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(-1, chunk.Alignment);
            Assert.IsNull(chunk.Format);
        }

        [TestMethod]
        public void TestInvalidAlignedFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("0", chunk.Tag);
            Assert.IsNull(chunk.Value);
        }

        [TestMethod]
        public void TestInvalidIntAlignedFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,a}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("0", chunk.Tag);
            Assert.IsNull(chunk.Value);
        }

        [TestMethod]
        public void TestFormatFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0:G}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0:G}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(0, chunk.Alignment);
            Assert.AreEqual("G", chunk.Format);
        }

        [TestMethod]
        public void TestCommaFormatFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0:,}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0:,}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(0, chunk.Alignment);
            Assert.AreEqual(",", chunk.Format);
        }

        [TestMethod]
        public void TestInvalidFormatFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0:}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("0", chunk.Tag);
            Assert.IsNull(chunk.Value);
        }

        [TestMethod]
        public void TestFullFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,-1:G}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0,-1:G}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(-1, chunk.Alignment);
            Assert.AreEqual("G", chunk.Format);
        }

        [TestMethod]
        public void TestInvalidFullFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,:}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("0", chunk.Tag);
            Assert.IsNull(chunk.Value);
        }

        [TestMethod]
        public void TestInvalidIntFullFillPoint()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,a:G}").ToArray();
            Assert.AreEqual(1, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual("G", chunk.Format);
            Assert.IsNull(chunk.Value);
        }

        [TestMethod]
        public void TestLeadingSpace()
        {
            FormatChunk[] chunks = FormatChunk.Parse(" {0,-1:G}").ToArray();
            Assert.AreEqual(2, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual(" ", chunk.Value);
            Assert.IsNull(chunk.Tag);

            chunk = chunks[1];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0,-1:G}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(-1, chunk.Alignment);
            Assert.AreEqual("G", chunk.Format);
        }

        [TestMethod]
        public void TestTrailingSpace()
        {
            FormatChunk[] chunks = FormatChunk.Parse("{0,-1:G} ").ToArray();
            Assert.AreEqual(2, chunks.Length);
            FormatChunk chunk = chunks[0];
            Assert.IsNotNull(chunk);
            Assert.AreEqual("{0,-1:G}", chunk.ToString("F", null));
            Assert.IsNull(chunk.Value);
            Assert.IsNotNull(chunk.Tag);
            Assert.AreEqual("0", chunk.Tag);
            Assert.AreEqual(-1, chunk.Alignment);
            Assert.AreEqual("G", chunk.Format);

            chunk = chunks[1];
            Assert.IsNotNull(chunk);
            Assert.AreEqual(" ", chunk.Value);
            Assert.IsNull(chunk.Tag);
        }

        [TestMethod]
        public void TestNestedChunks()
        {
            // Any character that appears after a '\' will always be added. This only has an effect for the '{', '}' and '\' characters, all others do not need to be escaped
            FormatChunk[] chunks = FormatChunk.Parse(@"This: {Tag:Is a very {complicated: {nested} format string with \{{escaped}\} tags and other \\ \'\characters\}}} and some trailing text").ToArray();

            // ReSharper disable PossibleNullReferenceException
            Assert.AreEqual(3, chunks.Length);
            Assert.AreEqual("This: ", chunks[0].Value);
            Assert.AreEqual("Tag", chunks[1].Tag);
            Assert.AreEqual(" and some trailing text", chunks[2].Value);

            chunks = chunks[1].Children.ToArray();
            Assert.AreEqual(2, chunks.Length);
            Assert.AreEqual("Is a very ", chunks[0].Value);
            Assert.AreEqual("complicated", chunks[1].Tag);

            chunks = chunks[1].Children.ToArray();
            Assert.AreEqual(5, chunks.Length);
            Assert.AreEqual(" ", chunks[0].Value);
            Assert.AreEqual("nested", chunks[1].Tag);
            Assert.AreEqual(@" format string with {", chunks[2].Value);
            Assert.AreEqual("escaped", chunks[3].Tag);
            Assert.AreEqual(@"} tags and other \ 'characters}", chunks[4].Value);
            // ReSharper restore PossibleNullReferenceException
        }
    }
}
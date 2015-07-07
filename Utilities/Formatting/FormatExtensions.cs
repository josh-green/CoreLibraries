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
using System.Collections.Generic;
using System.IO;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Threading;

namespace WebApplications.Utilities.Formatting
{
    /// <summary>
    /// Formattign extension methods.
    /// </summary>
    [PublicAPI]
    public static class FormatExtensions
    {
        /// <summary>
        /// A safe <see cref="string" /> format.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="parameters">The values used in the format string.</param>
        /// <returns>
        /// Returns the formatted <see cref="string" /> if successful; otherwise returns the <paramref name="format" /> string.
        /// </returns>
        [CanBeNull]
        [StringFormatMethod("format")]
        public static string SafeFormat([CanBeNull] this string format, [CanBeNull] params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(format)) return null;
            if (parameters == null ||
                parameters.Length < 1)
                return format;
            return new FormatBuilder().AppendFormat(format, parameters).ToString();
        }

        /// <summary>
        /// A safe <see cref="string" /> format.
        /// </summary>
        /// <param name="format">The format string.</param>
        /// <param name="formatOptions">The format options string. <seealso cref="FormatBuilder.ToString(String, IFormatProvider)"/></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="parameters">The values used in the format string.</param>
        /// <returns>Returns the formatted <see cref="string" /> if successful; otherwise returns the <paramref name="format" /> string.</returns>
        [CanBeNull]
        [StringFormatMethod("format")]
        public static string SafeFormat(
            [CanBeNull] this string format,
            [CanBeNull] string formatOptions,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(format)) return null;
            if (parameters == null ||
                parameters.Length < 1)
                return format;
            return new FormatBuilder().AppendFormat(format, parameters).ToString(formatOptions, formatProvider);
        }

        /// <summary>
        /// Produces a serialized version of the specified writer, using <see cref="SerializingSynchronizationContext" />, that
        /// will write output serially.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <returns>A synchronized TextWriter.</returns>
        [NotNull]
        public static TextWriter Serialize([NotNull] this TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            ISerialTextWriter stw = writer as ISerialTextWriter;
            return stw != null
                ? writer
                : new SerialTextWriter(writer);
        }

        /// <summary>
        /// Produces a formatted version of the specified writer, using <see cref="FormatTextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns>A laid out TextWriter.</returns>
        [NotNull]
        public static FormatTextWriter Format(
            [NotNull] this TextWriter writer,
            [CanBeNull] Layout layout = null,
            ushort startPosition = 0)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            FormatTextWriter ltw = writer as FormatTextWriter;
            if (ltw == null) return new FormatTextWriter(writer, layout, startPosition);

            ltw.ApplyLayout(layout);
            return ltw;
        }

        /// <summary>
        /// Produces a formatted version of the specified writer, using <see cref="FormatTextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="width">The width.</param>
        /// <param name="indentSize">Size of the indent.</param>
        /// <param name="rightMarginSize">Size of the right margin.</param>
        /// <param name="indentChar">The indent character.</param>
        /// <param name="firstLineIndentSize">First size of the line indent.</param>
        /// <param name="tabStops">The tab stops.</param>
        /// <param name="tabSize">Size of the tab.</param>
        /// <param name="tabChar">The tab character.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="splitLength">The split length.</param>
        /// <param name="hyphenate">The hyphenate.</param>
        /// <param name="hyphenChar">The hyphen character.</param>
        /// <param name="wrapMode">The wrap mode.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns>A laid out TextWriter.</returns>
        [NotNull]
        public static FormatTextWriter Format(
            [NotNull] this TextWriter writer,
            Optional<int> width,
            Optional<int> indentSize = default(Optional<int>),
            Optional<int> rightMarginSize = default(Optional<int>),
            Optional<char> indentChar = default(Optional<char>),
            Optional<int> firstLineIndentSize = default(Optional<int>),
            Optional<IEnumerable<int>> tabStops = default(Optional<IEnumerable<int>>),
            Optional<byte> tabSize = default(Optional<byte>),
            Optional<char> tabChar = default(Optional<char>),
            Optional<Alignment> alignment = default(Optional<Alignment>),
            Optional<byte> splitLength = default(Optional<byte>),
            Optional<bool> hyphenate = default(Optional<bool>),
            Optional<char> hyphenChar = default(Optional<char>),
            Optional<LayoutWrapMode> wrapMode = default(Optional<LayoutWrapMode>),
            ushort startPosition = 0)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            FormatTextWriter ltw = writer as FormatTextWriter;
            if (ltw == null)
                return new FormatTextWriter(
                    writer,
                    width,
                    indentSize,
                    rightMarginSize,
                    indentChar,
                    firstLineIndentSize,
                    tabStops,
                    tabSize,
                    tabChar,
                    alignment,
                    splitLength,
                    hyphenate,
                    hyphenChar,
                    wrapMode,
                    startPosition);

            ltw.ApplyLayout(
                width,
                indentSize,
                rightMarginSize,
                indentChar,
                firstLineIndentSize,
                tabStops,
                tabSize,
                tabChar,
                alignment,
                splitLength,
                hyphenate,
                hyphenChar,
                wrapMode);
            return ltw;
        }
    }
}
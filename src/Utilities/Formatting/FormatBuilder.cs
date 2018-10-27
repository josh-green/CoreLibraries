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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Formatting
{
    /// <summary>
    /// Build a formatted string, which can be used to enumerate FormatChunks
    /// </summary>
    [TypeConverter(typeof(FormatBuilderConverter))]
    [PublicAPI]
    public sealed partial class FormatBuilder : IFormattable, IWriteable, IEquatable<FormatBuilder>,
        IEnumerable<FormatChunk>
    {
        /// <summary>
        /// The first character of a fill point.
        /// </summary>
        public const char OpenChar = '{';

        /// <summary>
        /// The last character of a fill point.
        /// </summary>
        public const char CloseChar = '}';

        /// <summary>
        /// The control character precedes a tag to indicate it is a control chunk.
        /// </summary>
        public const char ControlChar = '!';

        /// <summary>
        /// The alignment character separates the tag from an alignment
        /// </summary>
        public const char AlignmentChar = ',';

        /// <summary>
        /// The splitter character separates the tag/alignment from the format.
        /// </summary>
        public const char FormatChar = ':';

        /// <summary>
        /// The root chunk.
        /// </summary>
        [NotNull]
        public readonly FormatChunk RootChunk = new FormatChunk(null);

        /// <summary>
        /// Whether this builder is read only
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// Gets the initial layout to use when resetting the layout.
        /// </summary>
        /// <value>
        /// The initial layout.
        /// </value>
        [NotNull]
        public Layout InitialLayout { get; set; }

        #region Constructor overloads
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBuilder"/> class.
        /// </summary>
        public FormatBuilder()
        {
            InitialLayout = Layout.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBuilder" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="isReadOnly">if set to <see langword="true" /> is read only.</param>
        [StringFormatMethod("format")]
        public FormatBuilder([CanBeNull] string format, bool isReadOnly = false)
        {
            InitialLayout = Layout.Default;
            if (!string.IsNullOrEmpty(format))
                AppendFormat(format);
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBuilder" /> class.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="isReadOnly">if set to <see langword="true" /> is read only.</param>
        public FormatBuilder([CanBeNull] Layout layout, bool isReadOnly = false)
        {
            InitialLayout = Layout.Default.Apply(layout);
            Debug.Assert(InitialLayout.IsFull);
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBuilder" /> class.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="format">The format.</param>
        /// <param name="isReadOnly">if set to <see langword="true" /> is read only.</param>
        [StringFormatMethod("format")]
        public FormatBuilder([CanBeNull] Layout layout, [CanBeNull] string format, bool isReadOnly = false)
        {
            InitialLayout = Layout.Default.Apply(layout);
            Debug.Assert(InitialLayout.IsFull);
            if (!string.IsNullOrEmpty(format))
                AppendFormat(format);
            _isReadOnly = isReadOnly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatBuilder" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
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
        /// <param name="isReadOnly">if set to <see langword="true" /> is read only.</param>
        [StringFormatMethod("format")]
        public FormatBuilder(
            Optional<int> width = default(Optional<int>),
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
            [CanBeNull] string format = null,
            bool isReadOnly = false)
        {
            InitialLayout = Layout.Default.Apply(
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
            Debug.Assert(InitialLayout.IsFull);
            if (!string.IsNullOrEmpty(format))
                AppendFormat(format);
            _isReadOnly = isReadOnly;
        }
        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        [NotNull]
        public FormatBuilder Clear()
        {
            RootChunk.ChildrenInternal = null;
            return this;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><see langword="true" /> if this instance is empty; otherwise, <see langword="false" />.</value>
        public bool IsEmpty
        {
            get { return RootChunk.ChildrenInternal == null || RootChunk.ChildrenInternal.Count < 1; }
        }

        /// <summary>
        /// Gets a value indicating whether this builder is read only.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this builder is read only; otherwise, <see langword="false" />.
        /// </value>
        /// <remarks>A read only builder cannot have any more chunks appended, but fill points can still be resolved.</remarks>
        public bool IsReadOnly
        {
            get { return _isReadOnly; }
        }

        /// <summary>
        /// Makes this builder read only.
        /// </summary>
        [NotNull]
        public FormatBuilder MakeReadOnly()
        {
            _isReadOnly = true;
            return this;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <param name="readOnly">If set to <see langword="true" />, the returned builder will be read only.</param>
        /// <returns>
        /// A shallow copy of this builder.
        /// </returns>
        [NotNull]
        public FormatBuilder Clone(bool readOnly = false)
        {
            if (_isReadOnly && readOnly)
                return this;

            FormatBuilder formatBuilder = new FormatBuilder(InitialLayout);
            if (readOnly)
            {
                formatBuilder.RootChunk.ChildrenInternal = RootChunk.ChildrenInternal;
                formatBuilder.MakeReadOnly();
            }
            else if (RootChunk.ChildrenInternal != null &&
                     RootChunk.ChildrenInternal.Count > 0)
                FormatChunk.DeepCopyChunks(RootChunk, formatBuilder.RootChunk);

            return formatBuilder;
        }

        #region Append overloads
        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(bool value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(sbyte value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(byte value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(char value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(short value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(int value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(long value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(float value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(double value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(decimal value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(ushort value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(uint value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(ulong value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] object value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            if (value == null) return this;

            string str = value as string;
            if (str != null)
            {
                RootChunk.AppendChunk(new FormatChunk(str));
                return this;
            }

            FormatChunk chunk = value as FormatChunk;
            if (chunk != null)
            {
                RootChunk.AppendChunk(chunk);
                return this;
            }

            IEnumerable<FormatChunk> chunks = value as IEnumerable<FormatChunk>;
            if (chunks != null)
            {
                if (RootChunk.ChildrenInternal == null)
                    RootChunk.ChildrenInternal = new List<FormatChunk>(chunks);
                else
                    RootChunk.ChildrenInternal.AddRange(chunks);
                return this;
            }

            RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] char[] value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if ((value != null) &&
                (value.Length > 0))
                RootChunk.AppendChunk(new FormatChunk(new string(value)));
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="charCount">The character count.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] char[] value, int startIndex, int charCount)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if ((value != null) &&
                (value.Length > 0) &&
                (startIndex >= 0) &&
                (charCount >= 0))
            {
                if (startIndex + charCount > value.Length)
                    charCount = value.Length - startIndex;
                RootChunk.AppendChunk(new FormatChunk(new string(value, startIndex, charCount)));
            }
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="repeatCount">The repeat count.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append(char value, int repeatCount)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (repeatCount > 0)
                RootChunk.AppendChunk(new FormatChunk(new string(value, repeatCount)));
            return this;
        }

        /// <summary>
        /// Appends the string, without additional formatting.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] string value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(value))
                RootChunk.AppendChunk(new FormatChunk(value));
            return this;
        }

        /// <summary>
        /// Appends the chunks.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] FormatChunk chunk)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (chunk != null)
                RootChunk.AppendChunk(chunk);
            return this;
        }

        /// <summary>
        /// Appends the chunks.
        /// </summary>
        /// <param name="chunks">The chunks.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] [InstantHandle] IEnumerable<FormatChunk> chunks)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (chunks == null) return this;

            FormatChunk[] cArr = chunks.ToArray();
            if (cArr.Length > 0)
                if (RootChunk.ChildrenInternal == null)
                    RootChunk.ChildrenInternal = new List<FormatChunk>(cArr);
                else
                    RootChunk.ChildrenInternal.AddRange(cArr);
            return this;
        }

        /// <summary>
        /// Appends the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder Append([CanBeNull] FormatBuilder builder)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            if (builder == null ||
                builder.IsEmpty) return this;

            Debug.Assert(builder.RootChunk.ChildrenInternal != null);
            Debug.Assert(builder.RootChunk.ChildrenInternal.Count > 0);

            // ReSharper disable AssignNullToNotNullAttribute
            if (RootChunk.ChildrenInternal == null)
                RootChunk.ChildrenInternal = new List<FormatChunk>(builder.RootChunk.ChildrenInternal);
            else
                RootChunk.ChildrenInternal.AddRange(builder.RootChunk.ChildrenInternal);
            // ReSharper restore AssignNullToNotNullAttribute
            return this;
        }
        #endregion

        #region AppendLine overloads
        /// <summary>
        /// Appends a line.
        /// </summary>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine()
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(bool value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(sbyte value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(byte value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(char value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(short value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(int value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(long value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(float value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(double value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(decimal value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(ushort value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(uint value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(ulong value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] object value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            if (value == null)
            {
                RootChunk.AppendChunk(NewLineChunk);
                return this;
            }

            string str = value as string;
            if (str != null)
            {
                RootChunk.AppendChunk(new FormatChunk(str));
                RootChunk.AppendChunk(NewLineChunk);
                return this;
            }

            FormatChunk chunk = value as FormatChunk;
            if (chunk != null)
            {
                RootChunk.AppendChunk(chunk);
                RootChunk.AppendChunk(NewLineChunk);
                return this;
            }

            IEnumerable<FormatChunk> chunks = value as IEnumerable<FormatChunk>;
            if (chunks != null)
            {
                if (RootChunk.ChildrenInternal == null)
                    RootChunk.ChildrenInternal = new List<FormatChunk>(chunks);
                else
                    RootChunk.ChildrenInternal.AddRange(chunks);
                RootChunk.AppendChunk(NewLineChunk);
                return this;
            }

            RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] char[] value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if ((value != null) &&
                (value.Length > 0))
                RootChunk.AppendChunk(new FormatChunk(new string(value)));

            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="charCount">The character count.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] char[] value, int startIndex, int charCount)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if ((value != null) &&
                (value.Length > 0) &&
                (startIndex >= 0) &&
                (charCount >= 0))
            {
                if (startIndex + charCount > value.Length)
                    charCount = value.Length - startIndex;
                RootChunk.AppendChunk(new FormatChunk(new string(value, startIndex, charCount)));
            }

            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="repeatCount">The repeat count.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine(char value, int repeatCount)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (repeatCount > 0)
                RootChunk.AppendChunk(new FormatChunk(new string(value, repeatCount)));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the string, without additional formatting.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] string value)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(value))
                RootChunk.AppendChunk(new FormatChunk(value));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the chunks.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] FormatChunk chunk)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (chunk != null)
                RootChunk.AppendChunk(chunk);
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the chunks.
        /// </summary>
        /// <param name="chunks">The chunks.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] [InstantHandle] IEnumerable<FormatChunk> chunks)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (chunks != null)
            {
                FormatChunk[] cArr = chunks.ToArray();
                if (cArr.Length > 0)
                    if (RootChunk.ChildrenInternal == null)
                        RootChunk.ChildrenInternal = new List<FormatChunk>(cArr);
                    else
                        RootChunk.ChildrenInternal.AddRange(cArr);
            }
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendLine([CanBeNull] FormatBuilder builder)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (builder != null &&
                !builder.IsEmpty)
            {
                Debug.Assert(builder.RootChunk.ChildrenInternal != null);
                Debug.Assert(builder.RootChunk.ChildrenInternal.Count > 0);

                // ReSharper disable AssignNullToNotNullAttribute
                if (RootChunk.ChildrenInternal == null)
                    RootChunk.ChildrenInternal = new List<FormatChunk>(builder.RootChunk.ChildrenInternal);
                else
                    RootChunk.ChildrenInternal.AddRange(builder.RootChunk.ChildrenInternal);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }
        #endregion

        #region AppendFormat overloads
        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormat([CanBeNull] string format, [CanBeNull] params object[] args)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(format, args == null || args.Length < 1 ? null : new ListResolvable(args, false));
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any tags with the matching properties or fields from the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormatInstance<T>([CanBeNull] string format, [CanBeNull] T instance)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            if (string.IsNullOrEmpty(format)) return this;

            IResolvable resolvable;
            if (!ReferenceEquals(instance, null))
            {
                // ReSharper disable once PossibleNullReferenceException
                IReadOnlyDictionary<string, object> dictionary = ((Accessor<T>)instance).Snapshot();
                resolvable = dictionary.Count > 0
                    ? new DictionaryResolvable(dictionary)
                    : null;
            }
            else
                resolvable = null;
            RootChunk.Append(format, resolvable);
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> the keys are case sensitive.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormat(
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(
                    format,
                    values == null || values.Count < 1 ? null : new DictionaryResolvable(values, isCaseSensitive, false));
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments (overriding pre-specified replacements).
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormat(
            [CanBeNull] string format,
            [CanBeNull] ResolveDelegate resolver,
            bool isCaseSensitive = false)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(format, resolver == null ? null : new FuncResolvable(resolver, isCaseSensitive));
            return this;
        }
        #endregion

        #region AppendFormatLine overloads
        /// <summary>
        /// Appends a new line.
        /// </summary>
        /// <returns>This instance.</returns>
        [NotNull]
        public FormatBuilder AppendFormatLine()
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments (overriding pre-specified replacements).
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>This instance.</returns>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormatLine([CanBeNull] string format, [CanBeNull] params object[] args)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(format, args == null || args.Length < 1 ? null : new ListResolvable(args, false));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any tags with the matching properties or fields from the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="format">The format.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormatLineInstance<T>([CanBeNull] string format, [CanBeNull] T instance)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            if (!string.IsNullOrEmpty(format))
            {
                IResolvable resolvable;
                if (!ReferenceEquals(instance, null))
                {
                    // ReSharper disable once PossibleNullReferenceException
                    IReadOnlyDictionary<string, object> dictionary = ((Accessor<T>)instance).Snapshot();
                    resolvable = dictionary.Count > 0
                        ? new DictionaryResolvable(dictionary)
                        : null;
                }
                else
                    resolvable = null;
                RootChunk.Append(format, resolvable);
            }
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments (overriding pre-specified replacements).
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> the keys are case sensitive.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormatLine(
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(
                    format,
                    values == null || values.Count < 1
                        ? null
                        : new DictionaryResolvable(values, isCaseSensitive, false));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }

        /// <summary>
        /// Appends the specified format string, replacing any integer tags with the matching arguments (overriding pre-specified replacements).
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <returns>
        /// This instance.
        /// </returns>
        [NotNull]
        [StringFormatMethod("format")]
        public FormatBuilder AppendFormatLine(
            [CanBeNull] string format,
            [CanBeNull] ResolveDelegate resolver,
            bool isCaseSensitive = false)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (!string.IsNullOrEmpty(format))
                RootChunk.Append(format, resolver == null ? null : new FuncResolvable(resolver, isCaseSensitive));
            RootChunk.AppendChunk(NewLineChunk);
            return this;
        }
        #endregion

        #region AppendControl
        /// <summary>
        /// Appends the control object for controlling formatting.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="format">The format.</param>
        /// <param name="value">The value.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        [StringFormatMethod("format")]
        // ReSharper disable once CodeAnnotationAnalyzer
        public FormatBuilder AppendControl(
            [NotNull] string tag,
            int alignment = 0,
            [CanBeNull] string format = null,
            Optional<object> value = default(Optional<object>))
        {
            if (string.IsNullOrEmpty(tag)) throw new ArgumentException(Resources.FormatBuilder_TagNullOrEmpty, "tag");
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            // Ensure the tag starts with the control character
            if (tag[0] != ControlChar)
                tag = ControlChar + tag;

            RootChunk.AppendChunk(new FormatChunk(null, tag, alignment, format, value));
            return this;
        }
        #endregion

        #region Resolve Overloads
        /// <summary>
        /// The initial resolutions
        /// </summary>
        [CanBeNull]
        private Resolutions _initialResolutions;

        /// <summary>
        /// Resolves any tags.
        /// </summary>
        /// <typeparam name="T">The instance type</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        public FormatBuilder ResolveInstance<T>([CanBeNull] T instance)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if ((IsEmpty) ||
                ReferenceEquals(instance, null))
                return this;

            // ReSharper disable once PossibleNullReferenceException
            IReadOnlyDictionary<string, object> dictionary = ((Accessor<T>)instance).Snapshot();
            if (dictionary.Count < 1)
                return this;
            _initialResolutions = new Resolutions(
                _initialResolutions,
                new DictionaryResolvable(dictionary, false, false));
            return this;
        }

        /// <summary>
        /// Resolves any tags.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> tag resolution is case sensitive.</param>
        /// <returns>This instance.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        [NotNull]
        public FormatBuilder Resolve(
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (IsEmpty ||
                (values == null) ||
                (values.Count < 1))
                return this;

            _initialResolutions = new Resolutions(
                _initialResolutions,
                new DictionaryResolvable(values, isCaseSensitive, false));
            return this;
        }

        /// <summary>
        /// Resolves any tags.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <returns>This instance.</returns>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        [NotNull]
        public FormatBuilder Resolve(
            [CanBeNull] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (IsEmpty ||
                (resolver == null))
                return this;

            _initialResolutions = new Resolutions(_initialResolutions, resolver, isCaseSensitive, true, resolveControls);
            return this;
        }
        #endregion

        #region ToString Overloads
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToStringInstance<T>([CanBeNull] T instance)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteToInstance(writer, null, instance);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The position.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString([CanBeNull] Layout layout, ref int position)
        {
            using (StringWriter writer = new StringWriter())
            {
                position = WriteTo(writer, layout, position);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="instance">The istance.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToStringInstance<T>([CanBeNull] Layout layout, ref int position, [CanBeNull] T instance)
        {
            using (StringWriter writer = new StringWriter())
            {
                position = WriteToInstance(writer, layout, position, null, instance);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteTo(writer, null, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter())
            {
                position = WriteTo(writer, layout, position, null, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="resolver">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteTo(writer, null, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="resolver">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString([CanBeNull] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter())
            {
                WriteTo(writer, null, resolver);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="resolver">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter())
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    null,
                    resolver,
                    isCaseSensitive,
                    resolveOuterTags,
                    resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="resolver">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter())
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    null,
                    resolver);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="instance">The istance.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToStringInstance<T>([CanBeNull] IFormatProvider formatProvider, [CanBeNull] T instance)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteToInstance(writer, null, instance);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="instance">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToStringInstance<T>(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] T instance)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteToInstance(writer, layout, position, null, instance);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, null, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(writer, layout, position, null, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString([CanBeNull] IFormatProvider formatProvider)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, null, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, null, resolver);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    null,
                    resolver,
                    isCaseSensitive,
                    resolveOuterTags,
                    resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    null,
                    resolver);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format. 
        /// <list type="table">
        ///     <listheader> <term>Format string</term> <description>Description</description> </listheader>
        ///     <item> <term>G/g/null</term> <description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description> </item>
        ///     <item> <term>F/f</term> <description>All control and fill point chunks will have their tags output.</description> </item>
        ///     <item> <term>S/s</term> <description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description> </item>
        /// </list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance. </returns>
        public string ToString([CanBeNull] string format, [CanBeNull] IFormatProvider formatProvider = null)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, format);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(writer, layout, position, format);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="values">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToStringInstance<T>(
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] T values)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteToInstance(writer, format, values);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="instance">The values.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToStringInstance<T>(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] T instance)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteToInstance(writer, layout, position, format, instance);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format. 
        /// <list type="table">
        ///     <listheader> <term>Format string</term> <description>Description</description> </listheader>
        ///     <item> <term>G/g/null</term> <description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description> </item>
        ///     <item> <term>F/f</term> <description>All control and fill point chunks will have their tags output.</description> </item>
        ///     <item> <term>S/s</term> <description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description> </item>
        /// </list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, format, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(writer, layout, position, format, values, isCaseSensitive, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.
        /// <list type="table">
        /// <listheader> <term>Format string</term> <description>Description</description> </listheader>
        /// <item> <term>G/g/null</term> <description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description> </item>
        /// <item> <term>F/f</term> <description>All control and fill point chunks will have their tags output.</description> </item>
        /// <item> <term>S/s</term> <description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description> </item>
        /// </list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        [NotNull]
        public string ToString(
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, format, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, format, resolver);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The position.</param>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    format,
                    resolver,
                    isCaseSensitive,
                    resolveOuterTags,
                    resolveControls);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The position.</param>
        /// <param name="format">The format.
        /// <list type="table"><listheader><term>Format string</term><description>Description</description></listheader><item><term>G/g/null</term><description>Any unresolved fill points will have their tags output. Control chunks are ignored.</description></item><item><term>F/f</term><description>All control and fill point chunks will have their tags output.</description></item><item><term>S/s</term><description>Any unresolved fill points will be treated as an empty string. Control chunks are ignored.</description></item></list></param>
        /// <param name="formatProvider">The format provider.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString(
            [CanBeNull] Layout layout,
            ref int position,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider,
            [CanBeNull] IResolvable resolver)
        {
            using (StringWriter writer = new StringWriter(formatProvider))
            {
                position = WriteTo(
                    writer,
                    layout,
                    position,
                    format,
                    resolver);
                return writer.ToString();
            }
        }
        #endregion

        #region WriteToConsole Overloads
        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        public void WriteToConsole()
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteToConsoleInstance<T>(
            [CanBeNull] string format,
            [CanBeNull] T values)
        {
            if (IsEmpty) return;
            WriteToInstance(ConsoleTextWriter.Default, format, values);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="values">The values.</param>
        public void WriteToConsoleInstance<T>(
            [CanBeNull] T values)
        {
            if (IsEmpty) return;
            WriteToInstance(ConsoleTextWriter.Default, null, values);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> [is case sensitive].</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToConsole(
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, format, values, isCaseSensitive, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToConsole(
            [CanBeNull] string format,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, format, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        public void WriteToConsole(
            [CanBeNull] string format,
            [CanBeNull] [InstantHandle] IResolvable resolver)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, format, resolver);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> [is case sensitive].</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToConsole(
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, null, values, isCaseSensitive, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToConsole(
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, null, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the console.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public void WriteToConsole(
            [CanBeNull] [InstantHandle] IResolvable resolver)
        {
            if (IsEmpty) return;
            WriteTo(ConsoleTextWriter.Default, null, resolver);
        }
        #endregion

        #region WriteToTrace Overloads
        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        public void WriteToTrace()
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        public void WriteToTraceInstance<T>(
            [CanBeNull] string format,
            [CanBeNull] T values)
        {
            if (IsEmpty) return;
            WriteToInstance(TraceTextWriter.Default, format, values);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="values">The values.</param>
        public void WriteToTraceInstance<T>(
            [CanBeNull] T values)
        {
            if (IsEmpty) return;
            WriteToInstance(TraceTextWriter.Default, null, values);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> [is case sensitive].</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToTrace(
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, format, values, isCaseSensitive, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToTrace(
            [CanBeNull] string format,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, format, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        public void WriteToTrace(
            [CanBeNull] string format,
            [CanBeNull] IResolvable resolver)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, format, resolver);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> [is case sensitive].</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToTrace(
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, null, values, isCaseSensitive, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        public void WriteToTrace(
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, null, resolver, isCaseSensitive, resolveOuterTags, resolveControls);
        }

        /// <summary>
        /// Writes the builder to the trace.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        public void WriteToTrace(
            [CanBeNull] IResolvable resolver)
        {
            if (IsEmpty) return;
            WriteTo(TraceTextWriter.Default, null, resolver);
        }
        #endregion

        #region WriteTo Overloads
        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="position">The start position.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <returns>The end position.</returns>
        public int WriteTo([CanBeNull] TextWriter writer, [CanBeNull] Layout layout = null, int position = 0)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                null,
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                "G",
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <returns>The end position.</returns>
        public void WriteTo([CanBeNull] TextWriter writer, string format)
        {
            if (writer == null || IsEmpty) return;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                null,
                InitialLayout,
                format,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>The end position.</returns>
        public int WriteToInstance<T>(
            [CanBeNull] TextWriter writer,
            [CanBeNull] string format,
            [CanBeNull] T instance)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            IResolvable resolvable;
            if (!ReferenceEquals(instance, null))
            {
                // ReSharper disable once PossibleNullReferenceException
                IReadOnlyDictionary<string, object> dictionary = ((Accessor<T>)instance).Snapshot();
                resolvable = dictionary.Count > 0
                    ? new DictionaryResolvable(dictionary)
                    : null;
            }
            else
                resolvable = null;

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolvable,
                InitialLayout,
                format,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="format">The format.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] string format)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                null,
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                format,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout"/>.</param>
        /// <param name="position">The start position.</param>
        /// <param name="format">The format.</param>
        /// <param name="instance">The values.</param>
        /// <returns>The end position.</returns>
        public int WriteToInstance<T>(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] string format,
            [CanBeNull] T instance)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            IResolvable resolvable;
            if (!ReferenceEquals(instance, null))
            {
                // ReSharper disable once PossibleNullReferenceException
                IReadOnlyDictionary<string, object> dictionary = ((Accessor<T>)instance).Snapshot();
                resolvable = dictionary.Count > 0
                    ? new DictionaryResolvable(dictionary)
                    : null;
            }
            else
                resolvable = null;

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolvable,
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                format,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> tag resolution is case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                values == null || values.Count < 1
                    ? null
                    : new DictionaryResolvable(values, isCaseSensitive, false, resolveControls),
                InitialLayout,
                format,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> tag resolution is case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] string format,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                values == null || values.Count < 1
                    ? null
                    : new DictionaryResolvable(values, isCaseSensitive, false, resolveControls),
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                format,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> tag resolution is case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                values == null || values.Count < 1
                    ? null
                    : new DictionaryResolvable(values, isCaseSensitive, false, resolveControls),
                InitialLayout,
                null,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="values">The values.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> tag resolution is case sensitive.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] IReadOnlyDictionary<string, object> values,
            bool isCaseSensitive = false,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                values == null || values.Count < 1
                    ? null
                    : new DictionaryResolvable(values, isCaseSensitive, false, resolveControls),
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                null,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] string format,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver == null
                    ? null
                    : new FuncResolvable(resolver, isCaseSensitive, resolveOuterTags, resolveControls),
                InitialLayout,
                format,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] string format,
            [CanBeNull] IResolvable resolver)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver,
                InitialLayout,
                format,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] string format,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver == null
                    ? null
                    : new FuncResolvable(resolver, isCaseSensitive, resolveOuterTags, resolveControls),
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                format,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="format">The format.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] string format,
            [CanBeNull] IResolvable resolver)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver,
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                format,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver == null
                    ? null
                    : new FuncResolvable(resolver, isCaseSensitive, resolveOuterTags, resolveControls),
                InitialLayout,
                null,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] IResolvable resolver)
        {
            if (writer == null || IsEmpty) return 0;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver,
                InitialLayout,
                null,
                0);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        /// <param name="resolveControls">if set to <see langword="true" /> then controls will passed to the resolvable.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] [InstantHandle] ResolveDelegate resolver,
            bool isCaseSensitive = false,
            bool resolveOuterTags = true,
            bool resolveControls = false)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver == null
                    ? null
                    : new FuncResolvable(resolver, isCaseSensitive, resolveOuterTags, resolveControls),
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                null,
                position);
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="layout">The layout is applied to the original <see cref="InitialLayout" />.</param>
        /// <param name="position">The start position.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>The end position.</returns>
        public int WriteTo(
            [CanBeNull] TextWriter writer,
            [CanBeNull] Layout layout,
            int position,
            [CanBeNull] IResolvable resolver)
        {
            if (writer == null || IsEmpty) return position;
            Debug.Assert(RootChunk.ChildrenInternal != null);

            return WriteTo(
                RootChunk,
                writer,
                _initialResolutions,
                resolver,
                layout == null ? InitialLayout : InitialLayout.Apply(layout),
                null,
                position);
        }
        #endregion

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="rootChunk"></param>
        /// <param name="writer">The writer.</param>
        /// <param name="initialResolutions"></param>
        /// <param name="resolver"></param>
        /// <param name="initialLayout">The initial layout.</param>
        /// <param name="format">The format.</param>
        /// <param name="position">The position.</param>
        /// <returns>The final position.</returns>
        private static int WriteTo(
            [NotNull] FormatChunk rootChunk,
            [NotNull] TextWriter writer,
            [CanBeNull] Resolutions initialResolutions,
            [CanBeNull] IResolvable resolver,
            [NotNull] Layout initialLayout,
            [CanBeNull] string format,
            int position)
        {
            if (rootChunk == null) throw new ArgumentNullException("rootChunk");
            if (writer == null) throw new ArgumentNullException("writer");
            if (initialLayout == null) throw new ArgumentNullException("initialLayout");

            ISerialTextWriter serialTextWriter = writer as ISerialTextWriter;
            return serialTextWriter == null
                ? DoWrite(
                    rootChunk,
                    writer,
                    null,
                    initialResolutions,
                    resolver,
                    initialLayout,
                    format,
                    position)
                : serialTextWriter.Context.Invoke(
                    () =>
                        DoWrite(
                            rootChunk,
                            writer,
                            serialTextWriter.Writer,
                            initialResolutions,
                            resolver,
                            initialLayout,
                            format,
                            position));
        }

        /// <summary>
        /// The items tag.
        /// </summary>
        [NotNull]
        public const string ItemsTag = "<items>";

        /// <summary>
        /// The item tag.
        /// </summary>
        [NotNull]
        public const string ItemTag = "<item>";

        /// <summary>
        /// The index tag.
        /// </summary>
        [NotNull]
        public const string IndexTag = "<index>";

        /// <summary>
        /// The join tag.
        /// </summary>
        [NotNull]
        public const string JoinTag = "<join>";

        /// <summary>
        /// The new line characters.
        /// </summary>
        [NotNull]
        private static readonly char[] _newLineChars = { '\r', '\n' };

        /// <summary>
        /// Gets the chunk as a string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>System.String.</returns>
        [NotNull]
        private static string GetChunkString(
            [NotNull] object value,
            int alignment,
            [CanBeNull] string format,
            [CanBeNull] IFormatProvider formatProvider)
        {
            if (value == null) throw new ArgumentNullException("value");

            string vStr;
            // We are not aligning so we can output the value directly.
            if (!string.IsNullOrEmpty(format))
            {
                IWriteable writeable = value as IWriteable;
                if (writeable != null)
                    using (StringWriter sw = new StringWriter(formatProvider))
                    {
                        writeable.WriteTo(sw, format);
                        vStr = sw.ToString();
                    }
                else
                {
                    IFormattable formattable = value as IFormattable;
                    if (formattable != null)
                        // When using this interface we have to suppress <see cref="FormatException"/>.
                        try
                        {
                            vStr = formattable.ToString(format, formatProvider);
                        }
                            // ReSharper disable once EmptyGeneralCatchClause
                        catch (FormatException)
                        {
                            vStr = value.ToString();
                        }
                    else
                        vStr = value.ToString();
                }
            }
            else
                vStr = value.ToString();

            // Suppress large alignment values for safety. (Note this handles int.MinValue unlike Math.Abs())
            if (alignment == 0 ||
                alignment > 1024 ||
                alignment < -1024)
                return vStr;

            // Pad the string if necessary.
            int len = vStr.Length;
            if (len < alignment)
                return new string(' ', alignment - len) + vStr;
            if (len >= -alignment) return vStr;
            return vStr + new string(' ', -alignment - len);
        }

        /// <summary>
        /// Handles a layout control chunk.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="layoutStack">The layout stack.</param>
        /// <param name="writerWidth">Width of the writer.</param>
        /// <returns><see langword="true" /> if layout has changed, <see langword="false" /> otherwise.</returns>
        private static bool HandleLayoutChunk(
            [NotNull] FormatChunk chunk,
            [NotNull] Stack<Layout> layoutStack,
            int writerWidth)
        {
            Debug.Assert(chunk.IsControl);
            Debug.Assert(layoutStack.Count > 0);
            Debug.Assert(string.Equals(chunk.Tag, LayoutTag, StringComparison.InvariantCultureIgnoreCase));

            Layout newLayout;
            bool hasFormat = !string.IsNullOrEmpty(chunk.Format);
            if (((newLayout = chunk.Value as Layout) != null) ||
                (hasFormat && Layout.TryParse(chunk.Format, out newLayout)))
            {
                // ReSharper disable once PossibleNullReferenceException
                if (newLayout.IsEmpty) return false;

                // ReSharper disable PossibleNullReferenceException
                newLayout = layoutStack.Peek().Apply(newLayout);
                // ReSharper restore PossibleNullReferenceException

                // Ensure layout doesn't exceed writer width.
                if (newLayout.Width.Value >= writerWidth)
                    newLayout = newLayout.Apply(writerWidth);

                layoutStack.Push(newLayout);
                return true;
            }
            if ((hasFormat) ||
                (layoutStack.Count < 2)) return false;

            layoutStack.Pop();
            return true;
        }

        /// <summary>
        /// Handles the control chunk.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="coloredTextWriter">The colored text writer.</param>
        /// <returns><see langword="true" /> if color control, <see langword="false" /> otherwise.</returns>
        private static bool HandleColor([NotNull] FormatChunk chunk, [NotNull] IColoredTextWriter coloredTextWriter)
        {
            Debug.Assert(chunk.IsControl);

            // Handle colored output.
            // ReSharper disable once PossibleNullReferenceException
            switch (chunk.Tag.ToLowerInvariant())
            {
                case ResetColorsTag:
                    coloredTextWriter.ResetColors();
                    return true;
                case ForegroundColorTag:
                    if (string.IsNullOrWhiteSpace(chunk.Format))
                        coloredTextWriter.ResetForegroundColor();
                    else if (chunk.IsResolved &&
                             chunk.Value is Color)
                        coloredTextWriter.SetForegroundColor((Color)chunk.Value);
                    else
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Optional<Color> color = ColorHelper.GetColor(chunk.Format);
                        if (color.IsAssigned)
                            coloredTextWriter.SetForegroundColor(color.Value);
                    }
                    return true;
                case BackgroundColorTag:
                    if (string.IsNullOrWhiteSpace(chunk.Format))
                        coloredTextWriter.ResetBackgroundColor();
                    else if (chunk.IsResolved &&
                             chunk.Value is Color)
                        coloredTextWriter.SetBackgroundColor((Color)chunk.Value);
                    else
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        Optional<Color> color = ColorHelper.GetColor(chunk.Format);
                        if (color.IsAssigned)
                            coloredTextWriter.SetBackgroundColor(color.Value);
                    }
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Character types.
        /// </summary>
        private enum CharType
        {
            /// <summary>
            /// No character.
            /// </summary>
            None,

            /// <summary>
            /// A white space character
            /// </summary>
            WhiteSpace,

            /// <summary>
            /// Symbol character.
            /// </summary>
            Apostrophe,

            /// <summary>
            /// Symbol character.
            /// </summary>
            Symbol,

            /// <summary>
            /// Alphanumeric character
            /// </summary>
            Alphanumeric
        }

        /// <summary>
        /// Writes the builder to the specified <see cref="TextWriter" />.
        /// </summary>
        /// <param name="rootChunk">The root chunk.</param>
        /// <param name="writer">The writer.</param>
        /// <param name="serialWriter">The serial writer.</param>
        /// <param name="initialResolutions">The initial resolutions.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="initialLayout">The initial layout.</param>
        /// <param name="format">The format.</param>
        /// <param name="position">The position.</param>
        /// <returns>The final position.</returns>
        [StringFormatMethod("format")]
        private static int DoWrite(
            [NotNull] FormatChunk rootChunk,
            [NotNull] TextWriter writer,
            [CanBeNull] TextWriter serialWriter,
            [CanBeNull] Resolutions initialResolutions,
            [CanBeNull] IResolvable resolver,
            [NotNull] Layout initialLayout,
            [CanBeNull] string format,
            int position)
        {
            Debug.Assert(rootChunk.ChildrenInternal != null);
            // ReSharper disable once PossibleNullReferenceException
            Debug.Assert(rootChunk.ChildrenInternal.Count > 0);

            #region Setup
            /*
             * Setup writers
             */
            // ReSharper disable SuspiciousTypeConversion.Global
            IControllableTextWriter controller = serialWriter as IControllableTextWriter ??
                                                 writer as IControllableTextWriter;
            // ReSharper restore SuspiciousTypeConversion.Global
            ILayoutTextWriter layoutWriter = serialWriter as ILayoutTextWriter ?? writer as ILayoutTextWriter;
            IColoredTextWriter coloredTextWriter = serialWriter as IColoredTextWriter ?? writer as IColoredTextWriter;
            if (serialWriter != null) writer = serialWriter;

            int writerWidth;
            bool autoWraps;
            if (layoutWriter != null)
            {
                position = layoutWriter.Position;
                writerWidth = layoutWriter.Width;
                autoWraps = layoutWriter.AutoWraps;

                // Ensure layout doesn't exceed writer width.
                if (initialLayout.Width.Value >= writerWidth)
                    initialLayout = initialLayout.Apply(writerWidth);
            }
            else
            {
                writerWidth = int.MaxValue;
                autoWraps = false;
            }

            /*
             * Setup format
             */
            if (format == null)
                format = "g";

            // Always write tags out - 'F' format.
            bool writeTags;
            // Whether to write out unresolved tags.
            bool skipUnresolvedTags;
            // Whether we require layout.
            bool isLayoutRequired;

            // Check which format we have 'f' will just write out tags, and ignore Layout.
            switch (format.ToLowerInvariant())
            {
                // Always output's the tag if the chunk has one, otherwise output's the value as normal
                case "f":
                    writeTags = true;
                    skipUnresolvedTags = false;
                    isLayoutRequired = false;
                    break;

                // Should output the value as normal, but treats unresolved tags as an empty string value
                case "s":
                    writeTags = false;
                    skipUnresolvedTags = true;
                    isLayoutRequired = initialLayout != Layout.Default;
                    break;

                // Outputs the value if set, otherwise the format tag. Control tags ignored
                default:
                    writeTags = false;
                    skipUnresolvedTags = false;
                    isLayoutRequired = initialLayout != Layout.Default;
                    break;
            }

            // The layout stack is used to hold the current layout
            Stack<Layout> layoutStack = new Stack<Layout>();
            layoutStack.Push(initialLayout);

            // The following are only used if we're laying out.
            List<string> words = null;
            int lineLength = 0;
            bool lineTerminated = false;
            Alignment alignment = Alignment.None;
            Layout layout = initialLayout;
            StringBuilder wordBuilder = null;
            CharType lastCharType = CharType.None;
            int lineStart = position;
            LineType lineType = position < 1 ? LineType.First : LineType.None;

            // Controls are queued until being written, they are trigged by a null string.
            LinkedList<FormatChunk> controls = new LinkedList<FormatChunk>();

            // Set up resolutions
            initialResolutions = resolver != null
                ? new Resolutions(initialResolutions, resolver)
                : initialResolutions;

            // Create out context object (we only need one as we run serially).
            FormatWriteContext context = new FormatWriteContext(
                writerWidth,
                coloredTextWriter != null,
                controller != null,
                autoWraps,
                writer.FormatProvider,
                writer.Encoding,
                writer.NewLine);

            // The stack holds any chunks that we need to process, so start by pushing the root chunks children onto it
            // in reverse, so that they are taken off in order.
            Stack<FormatChunk, Resolutions> stack = new Stack<FormatChunk, Resolutions>();
            for (int rsi = rootChunk.ChildrenInternal.Count - 1; rsi > -1; rsi--)
                stack.Push(rootChunk.ChildrenInternal[rsi], initialResolutions);
            #endregion

            /*
             * Process chunks
             */
            while (stack.Count > 0 ||
                   (wordBuilder != null && wordBuilder.Length > 0) ||
                   (words != null && words.Count > 0))
            {
                string chunkStr;
                if (stack.Count > 0)
                {
                    FormatChunk chunk;
                    Resolutions resolutions;
                    stack.Pop(out chunk, out resolutions);

                    #region Resolution - will flatten chunks and resolve to a series of strings.
                    if (chunk.Tag != null)
                        /*
                         * Process fill point
                         */
                        if (writeTags)
                            chunkStr = chunk.ToString("F");
                        else
                        {
                            if (chunk.Resolver != null)
                                resolutions = new Resolutions(resolutions, chunk.Resolver);

                            bool isResolved;
                            object resolvedValue;
                            // Resolve the tag if it's the first time we've seen it.
                            if (resolutions != null)
                            {
                                // Update context
                                context.LineType = lineType;
                                context.Layout = layout;
                                context.Position = position;
                                // ReSharper disable PossibleNullReferenceException
                                Resolution resolved = (Resolution)resolutions.Resolve(context, chunk);
                                // ReSharper restore PossibleNullReferenceException
                                isResolved = resolved.IsResolved;
                                resolvedValue = resolved.Value;
                            }
                            else
                            {
                                isResolved = false;
                                resolvedValue = null;
                            }

                            if (isResolved || chunk.IsResolved)
                            {
                                // If we haven't resolved the value, get the chunks value.
                                if (!isResolved)
                                {
                                    // Use the current resolution.
                                    isResolved = true;
                                    resolvedValue = chunk.Value;
                                }

                                // Check for resolved to null.
                                if (resolvedValue != null)
                                {
                                    /*
                                     * Check if we have an actual FormatChunk as the value, in which case, unwrap it.
                                     */
                                    bool unwrapped = false;
                                    do
                                    {
                                        FormatChunk fc = resolvedValue as FormatChunk;
                                        if (fc == null)
                                        {
                                            if (!unwrapped &&
                                                !(chunk.IsResolved &&
                                                  Equals(resolvedValue, chunk.Value)))
                                            {
                                                List<FormatChunk> children = chunk.ChildrenInternal;
                                                chunk = new FormatChunk(
                                                    chunk.Resolver,
                                                    chunk.Tag,
                                                    chunk.Alignment,
                                                    chunk.Format,
                                                    true,
                                                    resolvedValue,
                                                    chunk.IsControl)
                                                {
                                                    ChildrenInternal = children
                                                };
                                            }
                                            break;
                                        }

                                        chunk = fc;
                                        isResolved = chunk.IsResolved;
                                        if (!isResolved)
                                            break;

                                        resolvedValue = fc.Value;
                                        unwrapped = true;
                                    } while (true);

                                    if (isResolved)
                                    {
                                        /*
                                         * Unwrap format builders, or enumerations of chunks
                                         */
                                        IEnumerable<FormatChunk> formatChunks =
                                            resolvedValue as IEnumerable<FormatChunk>;
                                        if (formatChunks != null)
                                        {
                                            foreach (FormatChunk fci in formatChunks.Reverse())
                                                stack.Push(fci, resolutions);
                                            continue;
                                        }

                                        /*
                                         * Check if we have any child chunks, and flatten
                                         */
                                        if (chunk.ChildrenInternal != null &&
                                            chunk.ChildrenInternal.Count > 0)
                                        {
                                            // Get the chunks for the fill point.
                                            // BUG Alignment is ignored in the scenario we have child chunks.
                                            Stack<FormatChunk> subFormatChunks = new Stack<FormatChunk>();
                                            bool hasFillPoint = false;
                                            bool hasItemsFillPoint = false;
                                            FormatChunk joinChunk = null;
                                            foreach (FormatChunk subFormatChunk in chunk.ChildrenInternal)
                                            {
                                                // ReSharper disable once PossibleNullReferenceException
                                                if (subFormatChunk.Tag != null)
                                                {
                                                    hasFillPoint = true;
                                                    if (string.Equals(
                                                        subFormatChunk.Tag,
                                                        ItemsTag,
                                                        StringComparison.CurrentCultureIgnoreCase))
                                                        hasItemsFillPoint = true;
                                                    else if (string.Equals(
                                                        subFormatChunk.Tag,
                                                        JoinTag,
                                                        StringComparison.CurrentCultureIgnoreCase))
                                                    {
                                                        // Very special case! 
                                                        // We only allow one join chunk, so we take the last one, and we remove it from the outer format.
                                                        joinChunk = subFormatChunk;
                                                        continue;
                                                    }
                                                }
                                                subFormatChunks.Push(subFormatChunk);
                                            }

                                            // If there are any fill points, then the value might be needed to resolve them
                                            if (hasFillPoint)
                                            {
                                                IResolvable r = resolvedValue as IResolvable;
                                                if (r != null)
                                                    resolutions = new Resolutions(
                                                        resolutions,
                                                        r.Resolve,
                                                        r.IsCaseSensitive,
                                                        r.ResolveOuterTags,
                                                        r.ResolveControls);
                                                else
                                                {
                                                    Accessor acc = resolvedValue as Accessor ??
                                                                   Accessor.Create(resolvedValue);
                                                    resolutions = new Resolutions(
                                                        resolutions,
                                                        new DictionaryResolvable(acc, acc.IsCaseSensitive));
                                                }
                                            }

                                            /*
                                             * Special case enumerable, if we have an '<items>' tag, then we will treat each item in the enumeration
                                             * individually, otherwise we'll treat the enumeration as one value.
                                             */
                                            if (hasItemsFillPoint)
                                            {
                                                // We have an <item> fill point, so check if we have an enumerable.
                                                IEnumerable enumerable = resolvedValue as IEnumerable;
                                                if (enumerable != null)
                                                {
                                                    // Set the value of the joinChunk to FormatOutput.Default, which is an object that writes out it's own format!
                                                    if (joinChunk != null)
                                                        joinChunk = new FormatChunk(joinChunk.Format);

                                                    // Ensure we only enumerate once, and get the enumeration, bound with it's index, in reverse order.
                                                    KeyValuePair<object, int>[] indexedArray = enumerable
                                                        .Cast<object>()
                                                        .Select((r, i) => new KeyValuePair<object, int>(r, i))
                                                        .Reverse()
                                                        .ToArray();

                                                    // We have an enumeration format, so we need to add each item back in individually with new contextual information.
                                                    while (subFormatChunks.Count > 0)
                                                    {
                                                        FormatChunk subFormatChunk = subFormatChunks.Pop();
                                                        if (!string.Equals(
                                                            subFormatChunk.Tag,
                                                            ItemsTag,
                                                            StringComparison.CurrentCultureIgnoreCase))
                                                        {
                                                            stack.Push(subFormatChunk, resolutions);
                                                            continue;
                                                        }

                                                        // We have an <items> chunk, which we now expand for each item.
                                                        foreach (KeyValuePair<object, int> kvp in indexedArray)
                                                        {
                                                            object key = kvp.Key;
                                                            int value = kvp.Value;

                                                            // This will add a fall-through value for the '<item>' and '<index>' tags - a new child Resolutions will be created based on this one
                                                            // when the IResolution object is later resolved below, which means that you can still technically override the value of these tags in
                                                            // the underlying resolver.
                                                            Resolutions inner = new Resolutions(
                                                                resolutions,
                                                                (_, c) =>
                                                                {
                                                                    // ReSharper disable PossibleNullReferenceException
                                                                    switch (c.Tag.ToLowerInvariant())
                                                                        // ReSharper restore PossibleNullReferenceException
                                                                    {
                                                                        case IndexTag:
                                                                            return value;
                                                                        case ItemTag:
                                                                            return key;
                                                                        default:
                                                                            return Resolution.Unknown;
                                                                    }
                                                                },
                                                                false,
                                                                true,
                                                                false);

                                                            // Add a new chunk with, the <Item> tag.
                                                            FormatChunk innerChunk = new FormatChunk(
                                                                null,
                                                                ItemTag,
                                                                subFormatChunk.Alignment,
                                                                subFormatChunk.Format,
                                                                key);

                                                            if (subFormatChunk.ChildrenInternal != null)
                                                                innerChunk.ChildrenInternal =
                                                                    subFormatChunk.ChildrenInternal.ToList();

                                                            stack.Push(innerChunk, inner);

                                                            // If we have join chunk, push it for all but the 'first' element.
                                                            if (value > 0 &&
                                                                joinChunk != null)
                                                                stack.Push(joinChunk, inner);
                                                        }
                                                    }
                                                    continue;
                                                }
                                            }

                                            // If we have a value, and a format, then we may need to recurse.
                                            if (hasFillPoint)
                                            {
                                                while (subFormatChunks.Count > 0)
                                                    stack.Push(subFormatChunks.Pop(), resolutions);
                                                continue;
                                            } // No fill points in format.
                                        } // No children
                                    } // No resolution after unwrapping
                                } // Null resolution
                            } // No resolution.

                            if (chunk.IsControl)
                            {
                                // We need to queue this chunk for when the writer reaches it.
                                chunkStr = null;
                                controls.AddLast(chunk);
                            }
                            else if (!isResolved)
                            {
                                if (skipUnresolvedTags)
                                    continue;
                                chunkStr = chunk.ToString("F");
                            }
                            else if (resolvedValue == null)
                                continue;
                            else
                            {
                                ResolvableWriteable rw = resolvedValue as ResolvableWriteable;
                                if (rw != null)
                                {
                                    // We can use the default format.
                                    resolutions = new Resolutions(resolutions, rw);
                                    foreach (FormatChunk fci in rw.DefaultFormat.Reverse())
                                        stack.Push(fci, resolutions);
                                    continue;
                                }
                                chunkStr = GetChunkString(
                                    resolvedValue,
                                    chunk.Alignment,
                                    chunk.Format,
                                    writer.FormatProvider);
                            }
                        }
                    else if (chunk.Value == null)
                        continue;
                    else
                    {
                        // We have a value chunk.
                        chunkStr = GetChunkString(
                            chunk.Value,
                            chunk.Alignment,
                            chunk.Format,
                            writer.FormatProvider);

                        // If we're writing tags, need to escape open curly braces in the chunk
                        if (writeTags)
                            chunkStr = chunkStr.Replace("{", "{{");
                    }
                    #endregion

                    if (!isLayoutRequired)
                    {
                        // Check for control chunks.
                        if (chunkStr == null)
                        {
                            Debug.Assert(controls.Count > 0, "No controls left in queue, unhandled null.");
                            chunk = controls.First.Value;
                            controls.RemoveFirst();
                            if (string.Equals(chunk.Tag, LayoutTag, StringComparison.InvariantCultureIgnoreCase))
                            {
                                isLayoutRequired = HandleLayoutChunk(chunk, layoutStack, writerWidth);
                                continue;
                            }

                            if (coloredTextWriter != null &&
                                HandleColor(chunk, coloredTextWriter))
                                continue;

                            if (controller != null)
                                controller.OnControlChunk(chunk);

                            continue;
                        }

                        if (chunkStr.Length < 1)
                            continue;

                        // We need to find the distance since the last newline so we can report an accurate position.
                        int index = chunkStr.LastIndexOfAny(_newLineChars);
                        position = index < 0
                            ? chunkStr.Length + position
                            : chunkStr.Length - index - 1;
                        writer.Write(chunkStr);
                        continue;
                    }

                    // Ignore empty chunks
                    if ((chunkStr != null) &&
                        (chunkStr.Length < 1))
                        continue;
                }
                else
                    chunkStr = string.Empty;

                #region Layout
                /*
                 * Layout chunks
                 */

                // Create layout data on first past, otherwise update layout if we're at the start of a line.
                if (words == null)
                {
                    words = new List<string>();
                    wordBuilder = new StringBuilder();
                }

                // Take one character at a time.
                int cPos = 0;
                do
                {
                    string word;

                    #region Process characters into 'words'.
                    if (!string.IsNullOrEmpty(chunkStr) &&
                        cPos < chunkStr.Length)
                    {
                        /*
                         * Process characters into 'words'.
                         */
                        char ch = chunkStr[cPos++];
                        position++;

                        if (ch == '\r')
                        {
                            // Skip next '\n' if any.
                            if ((cPos < chunkStr.Length) &&
                                (chunkStr[cPos] == '\n'))
                                cPos++;
                            word = wordBuilder.ToString();
                            wordBuilder.Clear();
                            // we normalize all newline styles to '\r'.
                            wordBuilder.Append('\r');
                            lastCharType = CharType.WhiteSpace;
                            position = 0;
                        }
                        else if (ch == '\n')
                        {
                            word = wordBuilder.ToString();
                            wordBuilder.Clear();
                            // we normalize all newline styles to '\r'.
                            wordBuilder.Append('\r');
                            lastCharType = CharType.WhiteSpace;
                            position = 0;
                        }
                        else if (char.IsLetterOrDigit(ch))
                        {
                            if ((lastCharType == CharType.Apostrophe) ||
                                (wordBuilder.Length < 2 && lastCharType != CharType.WhiteSpace) ||
                                (lastCharType == CharType.Alphanumeric))
                            {
                                lastCharType = CharType.Alphanumeric;
                                wordBuilder.Append(ch);
                                continue;
                            }
                            // This letter follows a non alpha numeric, so split the word, unless the character was an
                            // apostrophe, i.e. keep "Craig's" and "I'm" as one word, or the symbol started a word,
                            // e.g. "'quoted'".
                            lastCharType = CharType.Alphanumeric;
                            word = wordBuilder.ToString();
                            wordBuilder.Clear();
                            wordBuilder.Append(ch);
                        }
                        else if (!char.IsWhiteSpace(ch))
                        {
                            if (lastCharType == CharType.Alphanumeric)
                            {
                                lastCharType = ch == '\'' ? CharType.Apostrophe : CharType.Symbol;
                                wordBuilder.Append(ch);
                                continue;
                            }
                            lastCharType = ch == '\'' ? CharType.Apostrophe : CharType.Symbol;
                            word = wordBuilder.ToString();
                            wordBuilder.Clear();
                            wordBuilder.Append(ch);
                        }
                        else
                        {
                            lastCharType = CharType.WhiteSpace;
                            word = wordBuilder.ToString();
                            wordBuilder.Clear();
                            wordBuilder.Append(ch);
                        }
                        if (string.IsNullOrEmpty(word))
                            continue;
                    }
                    else if (wordBuilder.Length > 0)
                    {
                        word = wordBuilder.ToString();
                        wordBuilder.Clear();
                    }
                    else if (chunkStr == null)
                        word = null;
                    else
                        word = string.Empty;
                    #endregion

                    if ((word == null) &&
                        (string.Equals(controls.Last.Value.Tag, LayoutTag, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        HandleLayoutChunk(controls.Last.Value, layoutStack, writerWidth);
                        controls.RemoveLast();
                        continue;
                    }

                    if (words.All(w => w == null))
                    {
                        // The layout has changed before we've added any non-control chunks to the line,
                        // so we can update the layout.
                        layout = layoutStack.Peek();
                        switch (lineType)
                        {
                            case LineType.None:
                                Debug.Assert(lineStart > 0);
                                alignment = Alignment.None;
                                break;
                            case LineType.First:
                                alignment = layout.Alignment.Value;
                                lineStart = layout.FirstLineIndentSize.Value;
                                break;
                            default:
                                alignment = layout.Alignment.Value;
                                lineStart = layout.IndentSize.Value;
                                break;
                        }
                    }

                    do
                    {
                        #region Process words into lines
                        if (word == null)
                        {
                            // Add null to words list - list is a control chunk and will be dequeued when writing the line out.
                            words.Add(word);
                            if (stack.Count > 0)
                                continue;
                        }
                        else if (word.Length > 0)
                        {
                            char c = word[0];
                            // Check if we're at the start of a line.
                            byte split = layout.SplitLength.Value;
                            if (lineLength < 1)
                            {
                                // Skip spaces at the start of a line, if we have an alignment
                                if ((c == ' ') &&
                                    (alignment != Alignment.None))
                                {
                                    word = null;
                                    continue;
                                }

                                // This is the first word, if it's not a partial line we will always split if it's too long,
                                // as we're going from the start of a line.
                                if (lineType != LineType.None)
                                    split = 1;
                            }

                            // Check for newline
                            if (c != '\r')
                            {
                                // Calculate remaining space.
                                int remaining = layout.Width.Value - lineStart - lineLength;

                                if (remaining > 0)
                                {
                                    if (c == '\t')
                                    {
                                        int tabSize;
                                        if (layout.TabStops.IsAssigned &&
                                            layout.TabStops.Value != null)
                                        {
                                            int lp = lineStart + lineLength;
                                            // ReSharper disable once PossibleNullReferenceException
                                            int nextTab = layout.TabStops.Value.FirstOrDefault(t => t > lp);
                                            tabSize = nextTab > lp
                                                ? nextTab - lp
                                                : layout.TabSize.Value;
                                        }
                                        else
                                            tabSize = layout.TabSize.Value;

                                        if (tabSize > remaining)
                                            tabSize = remaining;

                                        // Change word to spacer
                                        word = new string(layout.TabChar.Value, tabSize);
                                    }

                                    // Append word if short enough.
                                    if (word.Length <= remaining)
                                    {
                                        words.Add(word);
                                        lineLength += word.Length;
                                        word = null;
                                        continue;
                                    }

                                    // The word is too long to fit on the current line.
                                    int maxSplit = word.Length - split;
                                    int hyphenate = layout.Hyphenate.Value ? 1 : 0;
                                    if ((split > 0) &&
                                        (remaining >= (hyphenate + layout.SplitLength.Value)) &&
                                        (maxSplit >= split))
                                    {
                                        // Split the current word to fill remaining space
                                        int splitPoint = remaining - hyphenate;

                                        // Can only split if enough characters are left on line.
                                        if (splitPoint > maxSplit)
                                            splitPoint = maxSplit;

                                        string part = word.Substring(0, splitPoint);
                                        if (hyphenate > 0) part += layout.HyphenChar;
                                        words.Add(part);
                                        lineLength += word.Length;
                                        word = word.Substring(splitPoint);
                                    }
                                }
                                // No space left on the line, but not end of paragraph.
                                lineTerminated = true;
                            }
                            else
                            {
                                // Finished an input line.
                                lineTerminated = true;
                                if (lineType != LineType.None)
                                    lineType = LineType.Last;
                                word = null;
                            }
                        }
                        #endregion

                        #region Align lines and write.
                        /*
                         * Alignment
                         */
                        char indentChar = layout.IndentChar.Value;

                        // If we finished mid line then we can only left align/none
                        int p = 0;
                        if (words.Count > 0)
                        {
                            // We cannot fully align an unterminated line, and we don't justify the last line.
                            if ((!lineTerminated && alignment != Alignment.None) ||
                                (lineType == LineType.Last && alignment == Alignment.Justify))
                                alignment = Alignment.Left;

                            Queue<int> spacers = null;
                            // Calculate indentation
                            int indent;

                            // Trim whitespace in the line.
                            int lws = lineLength;
                            if (lineTerminated && alignment != Alignment.None)
                            {
                                bool seenWord = false;
                                int c = words.Count;
                                while (c > 0)
                                {
                                    string chunk = words[--c];
                                    if (chunk == null) continue;
                                    lws -= chunk.Length;
                                    if (!string.IsNullOrWhiteSpace(chunk))
                                    {
                                        if (alignment != Alignment.Justify)
                                            break;
                                        // When justifying use this loop to find the last whitespace before a word.
                                        seenWord = true;
                                        continue;
                                    }
                                    if (seenWord)
                                        break;
                                    lineLength -= chunk.Length;
                                    words.RemoveAt(c);
                                }
                            }

                            switch (alignment)
                            {
                                case Alignment.Centre:
                                    indent = (lineStart + layout.Width.Value - lineLength) / 2;
                                    break;
                                case Alignment.Right:
                                    indent = layout.Width.Value - lineLength;
                                    break;
                                case Alignment.Justify:
                                    indent = lineStart;
                                    int remaining = layout.Width.Value - indent - lineLength;
                                    if (remaining > 0 &&
                                        lws > 0)
                                    {
                                        // We need to calculate spacers.
                                        decimal space = ((decimal)lws) / remaining;
                                        // ReSharper disable once RedundantAssignment
                                        int o = (int)Math.Round(space / 2);
                                        spacers =
                                            new Queue<int>(
                                                Enumerable.Range(0, remaining).Select(r => o + (int)(space * r)));
                                    }
                                    break;
                                default:
                                    indent = lineStart;
                                    break;
                            }

                            if (lineType != LineType.None &&
                                indent > 0)
                                writer.Write(new string(indentChar, indent));

                            foreach (string w in words)
                            {
                                if (string.IsNullOrEmpty(w))
                                {
                                    if (w == null)
                                    {
                                        // Handle control chunks
                                        Debug.Assert(
                                            controls.Count > 0,
                                            "No controls left in queue, unhandled null.");

                                        FormatChunk chunk = controls.First.Value;
                                        controls.RemoveFirst();
                                        if (coloredTextWriter != null &&
                                            HandleColor(chunk, coloredTextWriter))
                                            continue;

                                        if (controller != null)
                                            controller.OnControlChunk(chunk);
                                    }
                                    continue;
                                }

                                p += w.Length;
                                if (spacers == null ||
                                    !string.IsNullOrWhiteSpace(w))
                                {
                                    writer.Write(w);
                                    continue;
                                }

                                // We have a white-space chunk, check if we have to add justification spaces
                                while ((spacers.Count > 0) &&
                                       (spacers.Peek() <= p))
                                {
                                    writer.Write(indentChar);
                                    spacers.Dequeue();
                                    p++;
                                }

                                // Check if justification is finished
                                if (spacers.Count < 1)
                                    spacers = null;

                                writer.Write(w);
                            }

                            // Add any remaining spacers
                            if ((spacers != null) &&
                                (spacers.Count > 0))
                            {
                                writer.Write(new string(indentChar, spacers.Count));
                                p += spacers.Count;
                            }

                            // Calculate our finish position
                            p += indent;
                        }
                        else
                        {
                            Debug.Assert(p == 0);
                            p = lineStart;
                        }

                        if (lineTerminated)
                        {
                            // Wrap the line according to our mode.
                            switch (layout.WrapMode.Value)
                            {
                                case LayoutWrapMode.NewLineOnShort:
                                    if (p < layout.Width.Value)
                                        writer.WriteLine();
                                    break;
                                case LayoutWrapMode.PadToWrap:
                                    writer.Write(
                                        new string(
                                            indentChar,
                                            (writerWidth < int.MaxValue ? writerWidth : layout.Width.Value) - p));
                                    break;
                                case LayoutWrapMode.PadToNewLine:
                                    writer.Write(
                                        new string(
                                            indentChar,
                                            (writerWidth < int.MaxValue ? writerWidth : layout.Width.Value) - p));
                                    if (!autoWraps ||
                                        (p < writerWidth))
                                        writer.WriteLine();
                                    break;
                                default:
                                    if (!autoWraps ||
                                        (p < writerWidth))
                                        writer.WriteLine();
                                    break;
                            }
                            position = wordBuilder.Length;
                        }
                        else
                            position = p + wordBuilder.Length;

                        lineLength = 0;
                        words.Clear();
                        lineTerminated = false;
                        alignment = layout.Alignment.Value;
                        layout = layoutStack.Peek();
                        switch (lineType)
                        {
                            case LineType.None:
                            case LineType.Last:
                                lineType = LineType.First;
                                lineStart = layout.FirstLineIndentSize.Value;
                                break;
                            default:
                                lineType = LineType.Middle;
                                lineStart = layout.IndentSize.Value;
                                break;
                        }
                        #endregion
                    } while (!string.IsNullOrEmpty(word));
                } while ((chunkStr != null && cPos < chunkStr.Length) ||
                         (wordBuilder != null && wordBuilder.Length > 0) ||
                         (stack.Count < 1 && lineLength > 0));
                #endregion
            }
            return position;
        }

        #region Color Control
        /// <summary>
        /// The reset colors control tag.
        /// </summary>
        [NotNull]
        public const string ResetColorsTag = "!resetcolors";

        /// <summary>
        /// The reset colors chunk.
        /// </summary>
        [NotNull]
        public static readonly FormatChunk ResetColorsChunk = new FormatChunk(null, ResetColorsTag, 0, null);

        /// <summary>
        /// The foreground color control tag.
        /// </summary>
        [NotNull]
        public const string ForegroundColorTag = "!fgcolor";

        /// <summary>
        /// The background color control tag.
        /// </summary>
        [NotNull]
        public const string BackgroundColorTag = "!bgcolor";

        /// <summary>
        /// The reset foreground color chunk.
        /// </summary>
        [NotNull]
        public static readonly FormatChunk ResetForegroundColorChunk = new FormatChunk(
            null,
            ForegroundColorTag,
            0,
            null);

        /// <summary>
        /// The reset background color chunk.
        /// </summary>
        [NotNull]
        public static readonly FormatChunk ResetBackgroundColorChunk = new FormatChunk(
            null,
            BackgroundColorTag,
            0,
            null);

        /// <summary>
        /// Adds a control to reset the foreground and background colors
        /// </summary>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendResetColors()
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            RootChunk.AppendChunk(ResetColorsChunk);
            return this;
        }

        /// <summary>
        /// Adds a control to reset the foreground color.
        /// </summary>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendResetForegroundColor()
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            RootChunk.AppendChunk(ResetForegroundColorChunk);
            return this;
        }

        /// <summary>
        /// Adds a control to reset the background color.
        /// </summary>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendResetBackgroundColor()
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            RootChunk.AppendChunk(ResetBackgroundColorChunk);
            return this;
        }

        /// <summary>
        /// Adds a control to set the foreground color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendForegroundColor(ConsoleColor color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            Color c = color.ToColor();
            RootChunk.AppendChunk(new FormatChunk(null, ForegroundColorTag, 0, c.GetName(), c));
            return this;
        }

        /// <summary>
        /// Adds a control to set the foreground color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendForegroundColor(Color color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            RootChunk.AppendChunk(new FormatChunk(null, ForegroundColorTag, 0, color.GetName(), color));
            return this;
        }

        /// <summary>
        /// Adds a control to set the foreground color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendForegroundColor([CanBeNull] string color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (string.IsNullOrWhiteSpace(color)) return this;

            RootChunk.AppendChunk(new FormatChunk(null, ForegroundColorTag, 0, color));
            return this;
        }

        /// <summary>
        /// Adds a control to set the background color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendBackgroundColor(ConsoleColor color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            Color c = color.ToColor();
            RootChunk.AppendChunk(new FormatChunk(null, BackgroundColorTag, 0, c.GetName(), c));
            return this;
        }

        /// <summary>
        /// Adds a control to set the background color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendBackgroundColor(Color color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);

            RootChunk.AppendChunk(new FormatChunk(null, BackgroundColorTag, 0, color.GetName(), color));
            return this;
        }

        /// <summary>
        /// Adds a control to set the console's background color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendBackgroundColor([CanBeNull] string color)
        {
            if (_isReadOnly) throw new InvalidOperationException(Resources.FormatBuilder_ReadOnly);
            if (string.IsNullOrWhiteSpace(color)) return this;

            RootChunk.AppendChunk(new FormatChunk(null, BackgroundColorTag, 0, color));
            return this;
        }
        #endregion

        #region Layout Control
        /// <summary>
        /// The layout control tag.
        /// </summary>
        [NotNull]
        public const string LayoutTag = "!layout";

        /// <summary>
        /// The pop layout chunk.
        /// </summary>
        [NotNull]
        public static readonly FormatChunk PopLayoutChunk = new FormatChunk(null, LayoutTag, 0, null);

        /// <summary>
        /// The new line chunk
        /// </summary>
        [NotNull]
        public static readonly FormatChunk NewLineChunk = new FormatChunk(Environment.NewLine);

        /// <summary>
        /// Pops the layout off the stack.
        /// </summary>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendPopLayout()
        {
            RootChunk.AppendChunk(PopLayoutChunk);
            return this;
        }

        /// <summary>
        /// Sets the layout (if outputting to a layout writer).
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendLayout([CanBeNull] Layout layout)
        {
            if (layout == null) return this;
            RootChunk.AppendChunk(new FormatChunk(null, LayoutTag, 0, layout.ToString("f"), layout));
            return this;
        }

        /// <summary>
        /// Sets the layout (if outputting to a layout writer).
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="indentSize">Size of the indent.</param>
        /// <param name="rightMarginSize">Size of the right margin.</param>
        /// <param name="indentChar">The indent character.</param>
        /// <param name="firstLineIndentSize">First size of the line indent.</param>
        /// <param name="tabStops">The tab stops.</param>
        /// <param name="tabSize">Size of the tab.</param>
        /// <param name="tabChar">The tab character.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="splitLength">if set to <see langword="true" /> then words will split across lines.</param>
        /// <param name="hyphenate">if set to <see langword="true" /> [hyphenate].</param>
        /// <param name="hyphenChar">The hyphenation character.</param>
        /// <param name="wrapMode">The line wrap mode.</param>
        /// <returns>FormatBuilder.</returns>
        [NotNull]
        public FormatBuilder AppendLayout(
            Optional<int> width = default(Optional<int>),
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
            Optional<LayoutWrapMode> wrapMode = default(Optional<LayoutWrapMode>))
        {
            Layout layout = new Layout(
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
            return Append(new FormatChunk(null, LayoutTag, 0, layout.ToString("f"), layout));
        }
        #endregion

        #region Conversions
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="FormatBuilder"/>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The result of the conversion.</returns>
        [StringFormatMethod("format")]
        public static implicit operator FormatBuilder(string format)
        {
            return format != null
                ? new FormatBuilder(format)
                : null;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="FormatBuilder"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(FormatBuilder format)
        {
            return format != null
                ? format.ToString()
                : null;
        }
        #endregion

        #region Equality
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword="true" /> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
        public override bool Equals([CanBeNull] object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is FormatBuilder &&
                   Equals((FormatBuilder)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals([CanBeNull] FormatBuilder other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _isReadOnly.Equals(other._isReadOnly) &&
                   InitialLayout.Equals(other.InitialLayout) &&
                   string.Equals(ToString("F"), other.ToString("F"));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return ToString("F").GetHashCode();
        }

        /// <summary>
        /// Implements the ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(FormatBuilder left, FormatBuilder right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Implements the !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(FormatBuilder left, FormatBuilder right)
        {
            return !Equals(left, right);
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerator<FormatChunk> GetEnumerator()
        {
            return (RootChunk.ChildrenInternal ?? Enumerable.Empty<FormatChunk>())
                .GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
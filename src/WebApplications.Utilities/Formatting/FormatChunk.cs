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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Formatting
{
    /// <summary>
    /// A format chunk, holds information about a chunk of formatted Value.
    /// </summary>
    [PublicAPI]
    public class FormatChunk : IFormattable, IWriteable
    {
        /// <summary>
        /// Used during parsing of chunks, to indicate the current parser state.
        /// </summary>
        private enum ParserState
        {
            Tag,
            Alignment,
            Format,
            Value
        }

        /// <summary>
        /// Any resolver that should be used to resolve this chunk (and any nested chunks).
        /// </summary>
        [CanBeNull]
        public readonly IResolvable Resolver;

        /// <summary>
        /// Control chunks are never written out when you call <see cref="ToString()"/>, but can be used by consumers of a <see cref="FormatBuilder"/> to
        /// extend functionality.
        /// </summary>
        public readonly bool IsControl;

        /// <summary>
        /// The tag, if this is a formatting chunk, if any; otherwise <see langword="null"/>. (e.g. '0' for '{0,-3:G}')
        /// </summary>
        [CanBeNull]
        public readonly string Tag;

        /// <summary>
        /// The alignment, if any; otherwise 0. (e.g. -3 for '{0,-3:G}'
        /// </summary>
        public readonly int Alignment;

        /// <summary>
        /// The format, if this is a formatting chunk and a format was specified, if any; otherwise <see langword="null"/>. (e.g. 'G' for '{0,-3:G}')
        /// </summary>
        [CanBeNull]
        public readonly string Format;

        /// <summary>
        /// The chunk Value, if any (will always be <see langword="null"/> if <see cref="IsResolved"/> is <see langword="true"/>).
        /// </summary>
        [CanBeNull]
        public readonly object Value;

        /// <summary>
        /// Gets a value indicating whether this instance is resolved.
        /// </summary>
        /// <value><see langword="true" /> if this instance is resolved; otherwise, <see langword="false" />.</value>
        public readonly bool IsResolved;

        /// <summary>
        /// The child chunks.
        /// </summary>
        [CanBeNull]
        internal List<FormatChunk> ChildrenInternal;

        /// <summary>
        /// Gets the children (if any).
        /// </summary>
        /// <value>The children.</value>
        [NotNull]
        public IEnumerable<FormatChunk> Children
        {
            get { return ChildrenInternal ?? Enumerable.Empty<FormatChunk>(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatChunk" /> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="format">The format.</param>
        internal FormatChunk(
            [CanBeNull] IResolvable resolver,
            [NotNull] string tag,
            [CanBeNull] string alignment,
            [CanBeNull] string format)
        {
            if (tag == null) throw new ArgumentNullException("tag");

            Resolver = resolver;
            Tag = tag;
            IsControl = tag.Length > 0 && tag[0] == FormatBuilder.ControlChar;
            if (alignment != null)
                int.TryParse(alignment, out Alignment);
            Format = format;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatChunk" /> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="format">The format.</param>
        /// <param name="value">The value.</param>
        internal FormatChunk(
            [CanBeNull] IResolvable resolver,
            [CanBeNull] string tag,
            int alignment,
            [CanBeNull] string format,
            Optional<object> value = default(Optional<object>))
        {
            Resolver = resolver;
            Tag = tag;
            IsControl = !string.IsNullOrEmpty(tag) && tag[0] == FormatBuilder.ControlChar;
            Alignment = alignment;
            Format = format;

            if (!value.IsAssigned) return;

            IsResolved = true;
            Value = value.Value;
        }

        /// <summary>
        /// Creates a non-fill point chunk.
        /// </summary>
        /// <param name="value">The value.</param>
        public FormatChunk(Optional<object> value = default(Optional<object>))
        {
            if (!value.IsAssigned) return;

            IsResolved = true;
            Value = value.Value;
        }

        /// <summary>
        /// Creates a clone of the <see cref="FormatChunk" />, changing the resolved value.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        /// <param name="value">The value.</param>
        public FormatChunk([NotNull] FormatChunk chunk, Optional<object> value)
        {
            if (chunk == null) throw new ArgumentNullException("chunk");

            Resolver = chunk.Resolver;
            Tag = chunk.Tag;
            IsControl = chunk.IsControl;
            Alignment = chunk.Alignment;
            Format = chunk.Format;

            DeepCopyChunks(chunk, this);

            if (!value.IsAssigned) return;

            IsResolved = true;
            Value = value.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatChunk"/> class.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="alignment">The alignment.</param>
        /// <param name="format">The format.</param>
        /// <param name="isResolved">if set to <see langword="true" /> [is resolved].</param>
        /// <param name="value">The value.</param>
        /// <param name="isControl">if set to <see langword="true" /> [is control].</param>
        internal FormatChunk(
            [CanBeNull] IResolvable resolver,
            [CanBeNull] string tag,
            int alignment,
            [CanBeNull] string format,
            bool isResolved,
            [CanBeNull] object value,
            bool isControl)
        {
            Resolver = resolver;
            Tag = tag;
            Alignment = alignment;
            Format = format;
            IsResolved = isResolved;
            Value = value;
            IsControl = isControl;
        }

        /// <summary>
        /// Performs a deep copy of the child chunks from <paramref name="source"/> to <paramref name="destination"/>.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        internal static void DeepCopyChunks([NotNull] FormatChunk source, [NotNull] FormatChunk destination)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");

            if (source.ChildrenInternal != null &&
                source.ChildrenInternal.Count > 0)
            {
                Stack<FormatChunk, IEnumerable<FormatChunk>> stack = new Stack<FormatChunk, IEnumerable<FormatChunk>>();
                stack.Push(destination, source.ChildrenInternal.ToArray());

                while (stack.Count > 0)
                {
                    FormatChunk currParent;
                    IEnumerable<FormatChunk> chunks;
                    stack.Pop(out currParent, out chunks);

                    Debug.Assert(currParent != null);
                    Debug.Assert(chunks != null);

                    // ReSharper disable once PossibleNullReferenceException
                    currParent.ChildrenInternal = new List<FormatChunk>();

                    // Adds each chunk to the current parent
                    // ReSharper disable once PossibleNullReferenceException
                    foreach (FormatChunk child in chunks)
                    {
                        Debug.Assert(child != null);
                        FormatChunk newChunk = new FormatChunk(
                            child.Resolver,
                            child.Tag,
                            child.Alignment,
                            child.Format,
                            child.IsResolved,
                            child.Value,
                            child.IsControl);

                        currParent.ChildrenInternal.Add(newChunk);

                        // If the chunk has any children they need to be added to the new chunk
                        if (child.ChildrenInternal != null &&
                            child.ChildrenInternal.Count >= 1)
                            stack.Push(newChunk, child.ChildrenInternal);
                    }
                }
            }
        }

        /// <summary>
        /// Parses a string into an enumeartion of <see cref="FormatChunk"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>An enumeration of <see cref="FormatChunk"/>.</returns>
        [NotNull]
        public static IEnumerable<FormatChunk> Parse([NotNull] string value, [CanBeNull] IResolvable resolver = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            FormatChunk fc = new FormatChunk();
            fc.Append(
                value,
                resolver);
            return fc.Children;
        }

        /// <summary>
        /// Appends the specified value, using the supplied resolver (if any).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns>FormatChunk.</returns>
        [NotNull]
        internal FormatChunk Append([NotNull] string value, [CanBeNull] IResolvable resolver = null)
        {
            if (value == null) throw new ArgumentNullException("value");

            string tag = null;
            string alignment = null;
            string format = null;

            Stack<FormatChunk> chunks = new Stack<FormatChunk>();
            FormatChunk chunk = this;
            StringBuilder builder = new StringBuilder(value.Length);

            ParserState state = ParserState.Value;

            int i = 0;
            while (i < value.Length)
            {
                char c = value[i++];

                // Un-escape
                if (c == '\\' &&
                    (i < value.Length))
                {
                    builder.Append(value[i++]);
                    continue;
                }

                bool gotFillPoint = false;
                switch (state)
                {
                    case ParserState.Tag:
                        switch (c)
                        {
                            case FormatBuilder.AlignmentChar:
                                state = ParserState.Alignment;
                                break;
                            case FormatBuilder.FormatChar:
                                state = ParserState.Format;
                                break;
                            case FormatBuilder.CloseChar:
                                state = ParserState.Value;
                                gotFillPoint = true;
                                break;
                            default:
                                builder.Append(c);
                                break;
                        }
                        if (state != ParserState.Tag)
                        {
                            // We've got a tag
                            tag = builder.ToString();
                            builder.Clear();
                        }
                        break;

                    case ParserState.Alignment:
                        switch (c)
                        {
                            case FormatBuilder.FormatChar:
                                state = ParserState.Format;
                                break;
                            case FormatBuilder.CloseChar:
                                state = ParserState.Value;
                                gotFillPoint = true;
                                break;
                            default:
                                builder.Append(c);
                                break;
                        }
                        if (state != ParserState.Alignment)
                        {
                            // We've got an alignment
                            alignment = builder.ToString();
                            builder.Clear();
                        }
                        break;

                    case ParserState.Format:
                        switch (c)
                        {
                            case FormatBuilder.OpenChar:
                                if (i >= value.Length || value[i] == FormatBuilder.OpenChar)
                                {
                                    builder.Append(FormatBuilder.OpenChar);
                                    i++;
                                    break;
                                }

                                // We have a nested format!
                                Debug.Assert(tag != null);
                                FormatChunk newChunk = new FormatChunk(
                                    chunks.Count < 1 ? resolver : null,
                                    tag,
                                    alignment,
                                    null);
                                tag = null;
                                alignment = null;
                                if (builder.Length > 0)
                                {
                                    Debug.Assert(newChunk.ChildrenInternal == null);
                                    newChunk.AppendChunk(new FormatChunk(builder.ToString()));
                                    builder.Clear();
                                }
                                chunk.AppendChunk(newChunk);
                                chunks.Push(chunk);
                                chunk = newChunk;
                                state = ParserState.Tag;
                                break;
                            case FormatBuilder.CloseChar:
                                state = ParserState.Value;
                                gotFillPoint = true;
                                format = builder.ToString();
                                builder.Clear();
                                break;
                            default:
                                builder.Append(c);
                                break;
                        }
                        break;

                    default:
                        switch (c)
                        {
                            case FormatBuilder.OpenChar:
                                if (i >= value.Length || value[i] == FormatBuilder.OpenChar)
                                {
                                    builder.Append(FormatBuilder.OpenChar);
                                    i++;
                                    break;
                                }

                                state = ParserState.Tag;
                                // We've got a value
                                if (builder.Length > 0)
                                {
                                    chunk.AppendChunk(new FormatChunk(builder.ToString()));
                                    builder.Clear();
                                }
                                break;
                            case FormatBuilder.CloseChar:
                                state = ParserState.Value;
                                if (chunks.Count > 0)
                                {
                                    // Closing a nest fill point.
                                    if (builder.Length > 0)
                                    {
                                        chunk.AppendChunk(new FormatChunk(builder.ToString()));
                                        builder.Clear();
                                    }
                                    chunk = chunks.Pop();
                                    Debug.Assert(chunk != null);
                                }
                                else
                                    builder.Append(c);
                                break;
                            default:
                                builder.Append(c);
                                break;
                        }
                        break;
                }

                if (gotFillPoint)
                {
                    Debug.Assert(tag != null);
                    FormatChunk newChunk = new FormatChunk(
                        chunks.Count < 1 ? resolver : null,
                        tag,
                        alignment,
                        format);
                    chunk.AppendChunk(newChunk);
                    tag = null;
                    alignment = null;
                    format = null;
                }
            }

            if (builder.Length <= 0) return this;

            // We have some left overs
            string v = builder.ToString();
            builder.Clear();

            if (tag != null)
                builder.Append(FormatBuilder.OpenChar).Append(tag);
            if (alignment != null)
                builder.Append(FormatBuilder.AlignmentChar).Append(alignment);
            if (builder.Length < 1)
                chunk.AppendChunk(new FormatChunk(v));
            else
            {
                builder.Append(FormatBuilder.FormatChar).Append(v);
                chunk.AppendChunk(new FormatChunk(builder.ToString()));
            }
            return this;
        }

        /// <summary>
        /// Appends the specified chunk.
        /// </summary>
        /// <param name="chunk">The chunk.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // ReSharper disable once CodeAnnotationAnalyzer
        internal void AppendChunk([NotNull] FormatChunk chunk)
        {
            if (ChildrenInternal == null)
                ChildrenInternal = new List<FormatChunk>();
            ChildrenInternal.Add(chunk);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            using (TextWriter writer = new StringWriter())
            {
                WriteTo(writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        [StringFormatMethod("format")]
        public string ToString([CanBeNull] string format, [CanBeNull] IFormatProvider formatProvider = null)
        {
            using (TextWriter writer = new StringWriter(formatProvider))
            {
                WriteTo(writer, format);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Writes this instance to the
        /// <paramref name="writer" />, using the optional
        /// <paramref name="format" />.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="format">The format.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        // ReSharper disable once CodeAnnotationAnalyzer
        public void WriteTo(TextWriter writer, string format = null)
        {
            if (format == null)
                format = "g";

            bool writeTag;

            switch (format.ToLowerInvariant())
            {
                // Always output's the tag if the chunk has one, otherwise output's the value as normal
                case "f":
                    // We don't pad format strings
                    writeTag = Tag != null;
                    break;

                // Should output the value as normal, but treats unresolved tags as an empty string value
                case "s":
                    if (IsControl) return;

                    writeTag = false;
                    break;

                // Outputs the value if set, otherwise the format tag. Control tags ignored
                default:
                    if (IsControl) return;

                    writeTag = !IsResolved && Tag != null;
                    break;
            }

            if (writeTag)
            {
                writer.Write(FormatBuilder.OpenChar);
                writer.Write(Tag);
                if (Alignment != 0)
                {
                    writer.Write(FormatBuilder.AlignmentChar);
                    writer.Write(Alignment.ToString("D"));
                }
                if (Format != null)
                {
                    writer.Write(FormatBuilder.FormatChar);
                    writer.Write(Format);
                }
                if (ChildrenInternal != null &&
                    ChildrenInternal.Count > 0)
                {
                    Debug.Assert(Format == null);

                    writer.Write(FormatBuilder.FormatChar);
                    foreach (FormatChunk chunk in ChildrenInternal)
                    {
                        Debug.Assert(chunk != null);
                        chunk.WriteTo(writer, format);
                    }
                }
                writer.Write(FormatBuilder.CloseChar);
                return;
            }

            if (!IsResolved ||
                Value == null)
                return;

            if (Alignment == 0)
            {
                // We are not aligning so we can output the value directly.
                if (!string.IsNullOrEmpty(Format))
                {
                    IWriteable writeable = Value as IWriteable;
                    if (writeable != null)
                    {
                        writeable.WriteTo(writer, Format);
                        return;
                    }

                    IFormattable formattable = Value as IFormattable;
                    if (formattable != null)
                        // When using this interface we have to suppress <see cref="FormatException"/>.
                        try
                        {
                            writer.Write(formattable.ToString(Format, writer.FormatProvider));
                            return;
                        }
                            // ReSharper disable once EmptyGeneralCatchClause
                        catch (FormatException)
                        {
                        }
                }
                writer.Write(Value.ToString());
                return;
            }

            // We have to align the value, so we need it's length.
            string value;
            // We are not aligning so we can output the value directly.
            if (!string.IsNullOrEmpty(Format))
            {
                IWriteable writeable = Value as IWriteable;
                if (writeable != null)
                    using (StringWriter sw = new StringWriter(writer.FormatProvider))
                    {
                        writeable.WriteTo(sw, Format);
                        value = sw.ToString();
                    }
                else
                {
                    IFormattable formattable = Value as IFormattable;
                    if (formattable != null)
                        // When using this interface we have to suppress <see cref="FormatException"/>.
                        try
                        {
                            value = formattable.ToString(Format, writer.FormatProvider);
                        }
                            // ReSharper disable once EmptyGeneralCatchClause
                        catch (FormatException)
                        {
                            value = Value.ToString();
                        }
                    else
                        value = Value.ToString();
                }
            }
            else
                value = Value.ToString();

            int len = value.Length;
            // Add any left padding
            if (len < Alignment)
                writer.Write(new string(' ', Alignment - len));

            writer.Write(value);
            if (len >= -Alignment) return;

            // Add right padding
            writer.Write(new string(' ', -Alignment - len));
        }


        /// <summary>
        /// Returns all children of this <see cref="FormatChunk"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        [NotNull]
        public IEnumerable<FormatChunk> AllChildren
        {
            get
            {
                Stack<FormatChunk> stack = new Stack<FormatChunk>();
                stack.Push(this);
                while (stack.Count > 0)
                {
                    FormatChunk chunk = stack.Pop();
                    if (!ReferenceEquals(chunk, this))
                        // ReSharper disable once PossibleNullReferenceException
                        yield return chunk;

                    // ReSharper disable once PossibleNullReferenceException
                    if (chunk.ChildrenInternal == null) continue;
                    foreach (FormatChunk child in chunk.Children.Reverse())
                        stack.Push(child);
                }
            }
        }
    }
}
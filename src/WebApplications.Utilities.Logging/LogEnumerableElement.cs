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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Formatting;

namespace WebApplications.Utilities.Logging
{
    public sealed partial class Log
    {
        /// <summary>
        /// The verbose element format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder EnumerableElementVerboseFormat = new FormatBuilder()
            .AppendLine()
            .AppendForegroundColor(Color.DarkCyan)
            .AppendFormat("{Key}")
            .AppendResetForegroundColor()
            .AppendFormat("\t: {Value:{<items>:{<item>}}{<join>:, }}")
            .MakeReadOnly();

        /// <summary>
        /// The single line element format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder EnumerableElementNoLineFormat = new FormatBuilder()
            .AppendForegroundColor(Color.DarkCyan)
            .AppendFormat("{Key}")
            .AppendResetForegroundColor()
            .AppendFormat("\t: {Value:{<items>:{<item>}}{<join>:, }}")
            .MakeReadOnly();

        /// <summary>
        /// The XML element format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder EnumerableElementXMLFormat = new FormatBuilder()
            .AppendFormatLine("<{KeyXmlTag}>")
            .AppendLayout(firstLineIndentSize: 8)
            .AppendFormatLine("{valuexml:{<items>:<item>{<item>}</item>}{<join>:\r\n}}")
            .AppendPopLayout()
            .AppendFormatLine("</{KeyXmlTag}>")
            .MakeReadOnly();

        /// <summary>
        /// The JSON element format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder EnumerableElementJSONFormat = new FormatBuilder()
            .Append(',')
            .AppendLine()
            .AppendFormat("\"{Key}\"={Value:{<items>:\"{<item>}\"}{<join>:, }}")
            .MakeReadOnly();

        /// <summary>
        /// The HTML element format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder EnumerableElementHTMLFormat = new FormatBuilder()
            .AppendFormatLine(
/* language=HTML */ @"
<tr>
    <td>{KeyXml}</td>
    <td><ul>{valuehtml:{<items>:<li>{<item>}</li>}{<join>:\r\n}}</ul></td>
</tr>")
            .MakeReadOnly();

        private class LogEnumerableElement : ResolvableWriteable
        {
            /// <summary>
            /// The resource.
            /// </summary>
            [NotNull]
            public readonly Expression<Func<string>> Resource;

            /// <summary>
            /// The value.
            /// </summary>
            [NotNull]
            public readonly IEnumerable<object> Values;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogElement" /> class.
            /// </summary>
            /// <param name="resource">The resource.</param>
            /// <param name="values">The values.</param>
            public LogEnumerableElement(
                [NotNull] Expression<Func<string>> resource,
                [NotNull] IEnumerable<object> values)
            {
                Resource = resource;
                Values = values;
            }

            /// <summary>
            /// Resolves the specified tag.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="chunk">The chunk.</param>
            /// <returns>An object that will be cached unless it is a <see cref="T:WebApplications.Utilities.Formatting.Resolution" />.</returns>
            public override object Resolve(FormatWriteContext context, FormatChunk chunk)
            {
                string key;
                // ReSharper disable once PossibleNullReferenceException
                switch (chunk.Tag.ToLowerInvariant())
                {
                    case "default":
                    case "verbose":
                        return EnumerableElementVerboseFormat;
                    case "xml":
                        return EnumerableElementXMLFormat;
                    case "json":
                        return EnumerableElementJSONFormat;
                    case "html":
                        return EnumerableElementHTMLFormat;
                    case "noline":
                        return EnumerableElementNoLineFormat;
                    case "key":
                        key = Translation.GetResource(
                            Resource,
                            context.FormatProvider as CultureInfo ?? Translation.DefaultCulture);
                        return string.IsNullOrEmpty(key)
                            ? Resolution.Null
                            : key;
                    case "keyxml":
                        key = Translation.GetResource(
                            Resource,
                            context.FormatProvider as CultureInfo ?? Translation.DefaultCulture);
                        return string.IsNullOrEmpty(key)
                            ? Resolution.Null
                            : key.XmlEscape();
                    case "keyxmltag":
                        key = Translation.GetResource(
                            Resource,
                            context.FormatProvider as CultureInfo ?? Translation.DefaultCulture);
                        return string.IsNullOrEmpty(key)
                            ? Resolution.Null
                            : key.Replace(' ', '_');
                    case "value":
                        return Values;
                    case "valuexml":
                        return Values.Select(v => v?.XmlEscape());
                    case "valuehtml":
                        return Values.Select(v => v != null ? _newLineRegex.Replace(v.XmlEscape(), m => "<br/>" + m.Value) : null);
                    default:
                        return Resolution.Unknown;
                }
            }

            /// <summary>
            /// Gets the default format.
            /// </summary>
            /// <value>The default format.</value>
            /// <exception cref="System.NotImplementedException"></exception>
            public override FormatBuilder DefaultFormat
            {
                get { return EnumerableElementVerboseFormat; }
            }
        }
    }
}
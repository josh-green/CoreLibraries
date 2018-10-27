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

using System.IO;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Formatting
{
    /// <summary>
    /// Indicates the object supports writing to a <see cref="TextWriter"/> using an optional <see cref="FormatBuilder"/> to specify the format.
    /// </summary>
    [PublicAPI]
    public interface IResolvable
    {
        /// <summary>
        /// Gets a value indicating whether this instance uses case sensitive tag resolution caching.
        /// </summary>
        /// <value><see langword="true" /> if this instance is case sensitive; otherwise, <see langword="false" />.</value>
        bool IsCaseSensitive { get; }

        /// <summary>
        /// Gets a value indicating whether outer tags should be resolved automatically in formats.
        /// </summary>
        /// <value><see langword="true" /> if the <see cref="FormatBuilder"/> should allow outer tags when resolving formats for this instance; otherwise, <see langword="false" />.</value>
        bool ResolveOuterTags { get; }

        /// <summary>
        /// Gets a value indicating whether control tags should be based to the resolver.
        /// </summary>
        /// <value><see langword="true" /> if the <see cref="FormatBuilder"/> should allow outer tags when resolving formats for this instance; otherwise, <see langword="false" />.</value>
        bool ResolveControls { get; }

        /// <summary>
        /// Resolves the specified tag.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="chunk">The chunk.</param>
        /// <returns>An object that will be cached unless it is a <see cref="Resolution" />.</returns>
        [CanBeNull]
        object Resolve([NotNull] FormatWriteContext context, [NotNull] FormatChunk chunk);
    }
}
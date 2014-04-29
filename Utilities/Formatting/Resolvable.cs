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

namespace WebApplications.Utilities.Formatting
{
    /// <summary>
    /// Base implementation for a class that implements <see cref="IResolvable"/>.
    /// </summary>
    public abstract class Resolvable : IResolvable
    {
        private readonly bool _resolveOuterTags;
        private readonly bool _isCaseSensitive;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resolvable"/> class.
        /// </summary>
        /// <param name="isCaseSensitive">if set to <see langword="true" /> then tags are case sensitive.</param>
        /// <param name="resolveOuterTags">if set to <see langword="true" />  outer tags should be resolved automatically in formats.</param>
        protected Resolvable(bool isCaseSensitive = false, bool resolveOuterTags = false)
        {
            _isCaseSensitive = isCaseSensitive;
            _resolveOuterTags = resolveOuterTags;
        }

        /// <summary>
        /// Gets a value indicating whether this instance uses case sensitive tag resolution.
        /// </summary>
        /// <value><see langword="true" /> if this instance is case sensitive; otherwise, <see langword="false" />.</value>
        public bool IsCaseSensitive
        {
            get { return _isCaseSensitive; }
        }

        /// <summary>
        /// Gets a value indicating whether outer tags should be resolved automatically in formats.
        /// </summary>
        /// <value><see langword="true" /> if the <see cref="FormatBuilder" /> should allow outer tags when resolving formats for this instance; otherwise, <see langword="false" />.</value>
        public bool ResolveOuterTags
        {
            get { return _resolveOuterTags; }
        }

        /// <summary>
        /// Resolves the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Optional&lt;System.Object&gt;.</returns>
        public abstract Optional<object> Resolve(string tag);
    }
}
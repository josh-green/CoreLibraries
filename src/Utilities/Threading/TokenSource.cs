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

using System.Threading;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Threading
{
    internal class TokenSource : ITokenSource
    {
        [NotNull]
        public static readonly ITokenSource Cancelled = new TokenSource(TaskResult.CancelledToken);

        [NotNull]
        public static readonly ITokenSource None = new TokenSource(CancellationToken.None);

        /// <summary>
        /// The token
        /// </summary>
        private readonly CancellationToken _token;

        /// <summary>
        /// Gets the <see cref="CancellationToken" /> associated with this <see cref="ITokenSource" />.
        /// </summary>
        /// <value>
        /// The <see cref="CancellationToken" /> associated with this <see cref="ITokenSource" />.
        /// </value>
        public CancellationToken Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Gets whether cancellation has been requested for this token source.
        /// </summary>
        /// <value>
        /// Whether cancellation has been requested for this token source.
        /// </value>
        public bool IsCancellationRequested
        {
            get { return _token.IsCancellationRequested; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenSource"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public TokenSource(CancellationToken token)
        {
            _token = token;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
    }
}
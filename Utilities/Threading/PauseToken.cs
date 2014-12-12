#region � Copyright Web Applications (UK) Ltd, 2014.  All rights reserved.
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

using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Threading
{
    /// <summary>
    /// Used to wait when the associated <see cref="PauseTokenSource"/> is paused.
    /// </summary>
    /// <remarks>See http://blogs.msdn.com/b/pfxteam/archive/2013/01/13/cooperatively-pausing-async-methods.aspx.
    /// </remarks>
    public struct PauseToken
    {
        /// <summary>
        /// The default PauseToken never pauses.
        /// </summary>
        [PublicAPI]
        public static readonly PauseToken None = default(PauseToken);

        /// <summary>
        /// A pause token that is always paused.
        /// </summary>
        [PublicAPI]
        public static readonly PauseToken Paused;

        static PauseToken()
        {
            PauseTokenSource pausedSource= new PauseTokenSource();
            pausedSource.IsPaused = true;
            Paused = pausedSource.Token;
        }

        /// <summary>
        /// The source <see cref="PauseTokenSource"/>, if any.
        /// </summary>
        [PublicAPI]
        [CanBeNull]
        private readonly PauseTokenSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="PauseToken"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        internal PauseToken([NotNull] PauseTokenSource source)
        {
            Contract.Requires(source != null);
            _source = source;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can pause.
        /// </summary>
        /// <value><see langword="true" /> if this instance can pause; otherwise, <see langword="false" />.</value>
        [PublicAPI]
        public bool CanPause
        {
            get { return _source != null; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is paused.
        /// </summary>
        /// <value><see langword="true" /> if this instance is paused; otherwise, <see langword="false" />.</value>
        [PublicAPI]
        public bool IsPaused
        {
            get { return _source != null && _source.IsPaused; }
        }

        /// <summary>
        /// Waits the while paused asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        [PublicAPI]
        [NotNull]
        [WebApplications.Utilities.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public Task WaitWhilePausedAsync(CancellationToken token = default(CancellationToken))
        {
            return _source != null && _source.IsPaused
                ? _source.WaitWhilePausedAsync(token)
                : TaskResult.Completed;
        }
    }
}
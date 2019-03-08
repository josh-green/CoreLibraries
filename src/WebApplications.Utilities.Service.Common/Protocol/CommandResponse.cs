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
using ProtoBuf;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Service.Common.Protocol
{
    /// <summary>
    /// Command response message, sent by the server in response to a <see cref="CommandRequest"/>.
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    public class CommandResponse : Response
    {
        /// <summary>
        /// The sequence starts at 0 and continues until the final chunk which is set at -1 for completed, or -2 for error.
        /// </summary>
        [ProtoMember(1)]
        public readonly int Sequence;

        /// <summary>
        /// The response chunk.
        /// </summary>
        [ProtoMember(2)]
        [NotNull]
        public readonly string Chunk;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResponse" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sequence">The sequence.</param>
        /// <param name="chunk">The chunk.</param>
        public CommandResponse(Guid id, int sequence, [NotNull] string chunk)
            : base(id)
        {
            if (chunk == null) throw new ArgumentNullException("chunk");

            Sequence = sequence;
            Chunk = chunk;
        }
    }
}
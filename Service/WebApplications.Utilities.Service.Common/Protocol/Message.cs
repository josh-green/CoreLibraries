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

using System.Diagnostics.Contracts;
using System.IO;
using JetBrains.Annotations;
using ProtoBuf;

namespace WebApplications.Utilities.Service.Common.Protocol
{
    /// <summary>
    /// Base message class, used for communication between named pipe client and server.
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    [ProtoInclude(100, typeof (Request))]
    [ProtoInclude(200, typeof (Response))]
    [ProtoInclude(300, typeof (LogResponse))]
    public abstract class Message
    {
        /// <summary>
        /// Gets the serialized form of a message.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        [NotNull]
        public byte[] Serialize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the specified data to a message.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>Message.</returns>
        [NotNull]
        public static Message Deserialize([NotNull] byte[] data)
        {
            Contract.Requires(data != null);
            using (MemoryStream ms = new MemoryStream(data))
                // ReSharper disable once AssignNullToNotNullAttribute
                return Serializer.Deserialize<Message>(ms);
        }
    }
}
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
using ProtoBuf;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Logging;

namespace WebApplications.Utilities.Service.Common.Protocol
{
    /// <summary>
    /// Log response message, sent by the server when <see cref="Log">logs</see> are added.
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    public class LogResponse : Message
    {
        /// <summary>
        /// The logs.
        /// </summary>
        [ProtoMember(1, OverwriteList = true)]
        [ItemNotNull]
        [NotNull]
        public readonly IEnumerable<Log> Logs;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogResponse"/> class.
        /// </summary>
        /// <param name="logs">The logs.</param>
        public LogResponse([NotNull] IEnumerable<Log> logs)
        {
            if (logs == null) throw new ArgumentNullException("logs");
            Logs = logs;
        }
    }
}
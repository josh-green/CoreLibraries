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
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Logging;

namespace WebApplications.Utilities.Database.Exceptions
{
    /// <summary>
    ///   Exceptions thrown during the execution of a <see cref="WebApplications.Utilities.Database.SqlProgram"/>.
    /// </summary>
    [PublicAPI]
    public class SqlProgramExecutionException : LoggingException
    {
        /// <summary>
        /// The prefix reservation.
        /// </summary>
        private static readonly Guid _prefixReservation = System.Guid.NewGuid();

        /// <summary>
        /// The program name context key
        /// </summary>
        [NotNull]
        public static readonly string ProgramNameContextKey = LogContext.ReserveKey(
            "SqlProgram Name",
            _prefixReservation);

        /// <summary>
        /// The gateway name of the module.
        /// </summary>
        [CanBeNull]
        public string ProgramName
        {
            get { return this[ProgramNameContextKey]; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SqlProgramExecutionException"/> class.
        /// </summary>
        /// <param name="sqlProgram">The executing SQL program.</param>
        /// <param name="innerException">The inner exception.</param>
        internal SqlProgramExecutionException([NotNull] SqlProgram sqlProgram, [NotNull] Exception innerException)
            : base(
                new LogContext()
                    .Set(_prefixReservation, ProgramNameContextKey, sqlProgram.Name),
                innerException,
                () => Resources.SqlProgramExecutionException_ErrorOccurredDuringExecution,
                sqlProgram.Name,
                innerException.Message)
        {
        }
    }
}
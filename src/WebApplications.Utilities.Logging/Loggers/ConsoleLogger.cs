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
using System.Threading;
using System.Threading.Tasks;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Formatting;

namespace WebApplications.Utilities.Logging.Loggers
{
    /// <summary>
    ///   Allows coloured logging to the console window, using an extended <see cref="TextWriterLogger.Format"/> syntax.
    /// </summary>
    [PublicAPI]
    public sealed class ConsoleLogger : TextWriterLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="format">The format.</param>
        /// <param name="validLevels">The valid levels.</param>
        /// <remarks>
        /// <para>Along with the <see cref="T:WebApplications.Utilities.Logging.Log.ToString(string, IFormatProvider)">standard formats supported by the logger</see>, also supports colouration using colour formats.</para>
        /// <para>To change colour use a '+' or '-' followed by a <see cref="ConsoleColor"/> e.g. <code>{+White}</code>. '+' indicates a foreground colour change, whilst
        /// '-' will change the background colour.</para>
        /// <para>The '?' colour will pick a colour based on the current log level, e.g. <code>{+?}</code> for an <see cref="LoggingLevel.Error">error</see> will
        /// change the foreground colour to red.</para>
        /// <para>The '_' colour will use the current consoles default foreground or background colour (depending on whether it is preceeded with '+' or '-').</para>
        /// </remarks>
        public ConsoleLogger(
            [NotNull] string name,
            [CanBeNull] FormatBuilder format = null,
            LoggingLevels validLevels = LoggingLevels.All)
            : base(name, ConsoleTextWriter.Default, format, false, validLevels)
        {
            if (!ConsoleHelper.IsConsole) throw new InvalidOperationException(Resources.ConsoleLogger_NotConsole);
            Format = format ?? Log.VerboseFormat;
        }

        /// <summary>
        /// Adds the specified logs to storage in batches.
        /// </summary>
        /// <param name="logs">The logs to add to storage.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public override Task Add(IEnumerable<Log> logs, CancellationToken token = new CancellationToken())
        {
            if (Console.CursorLeft > 0)
                ConsoleTextWriter.Default.WriteLine();
            return base.Add(logs, token);
        }
    }
}
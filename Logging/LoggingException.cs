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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using JetBrains.Annotations;

namespace WebApplications.Utilities.Logging
{
    /// <summary>
    ///   Implements a specialised Exception handler used throughout Babel, automatically logs errors
    ///   and the context in which they occurred. Also makes use of Babel's late-binding translation.
    ///   BabelException should always be used where exceptions are thrown.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("[{ExceptionTypeFullName}] {Message} @ {TimeStamp}")]
    public class LoggingException : ApplicationException, IEnumerable<KeyValuePair<string, string>>, IFormattable
    {
        /// <summary>
        /// The associated log item.
        /// </summary>
        [NotNull]
        [PublicAPI]
        protected readonly Log Log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        [UsedImplicitly]
        public LoggingException([NotNull] string message, [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters))
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(null, this, LoggingLevel.Error, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="context">The log context.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        [UsedImplicitly]
        public LoggingException([CanBeNull] LogContext context, [NotNull] string message,
            [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters))
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(context, this, LoggingLevel.Error, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        public LoggingException(LoggingLevel level, [NotNull] string message, [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters))
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(null, this, level, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="context">The log context.</param>
        /// <param name="level">The log level.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="context" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        public LoggingException([CanBeNull] LogContext context, LoggingLevel level, [NotNull] string message,
            [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters))
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(context, this, level, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="level">The log level.</param>
        /// <remarks>There is a 
        /// <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        /// <paramref name="exception" /> to not be a non null value.</remarks>
        public LoggingException([NotNull] Exception exception, LoggingLevel level = LoggingLevel.Error)
            : base(exception.Message)
        {
            Contract.Requires(exception != null, Resources.LoggingException_InnerExceptionCannotBeNull);

            // Log the exception
            Log = new Log(null, this, level, exception.Message);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="exception">The inner exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        public LoggingException(
            [CanBeNull] Exception exception,
            [NotNull] string message,
            [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters), exception)
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(null, this, LoggingLevel.Error, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="exception">The inner exception.</param>
        /// <param name="level">The log level.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        public LoggingException(
            [CanBeNull] Exception exception,
            LoggingLevel level,
            [NotNull] string message,
            [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters), exception)
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(null, this, level, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException" /> class.
        /// </summary>
        /// <param name="context">The log context.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="level">The log level.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks><para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="message" /> to not be a null value.</para>
        ///   <para>There is a <see cref="System.Diagnostics.Contracts.Contract">contract</see> on this method requiring the
        ///   <paramref name="parameters" /> to not be a null value.</para></remarks>
        [StringFormatMethod("message")]
        public LoggingException(
            [CanBeNull] LogContext context,
            [CanBeNull] Exception innerException,
            LoggingLevel level,
            [NotNull] string message,
            [NotNull] params object[] parameters)
            : base(message.SafeFormat(Thread.CurrentThread.CurrentUICulture, parameters), innerException)
        {
            Contract.Requires(message != null, Resources.LoggingException_MessageCannotBeNull);
            Contract.Requires(parameters != null, Resources.LoggingException_ParametersCannotBeNull);

            // Log the exception
            Log = new Log(context, this, level, message, parameters);
            Contract.Assert(Log != null);

            // Finally increment performance counter.
            Log.PerfCounterException.Increment();
        }

        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        [PublicAPI]
        public CombGuid Guid
        {
            get { return Log.Guid; }
        }

        /// <summary>
        /// Gets the log group.
        /// </summary>
        /// <value>The log group.</value>
        [PublicAPI]
        public LoggingLevel Level
        {
            get { return Log.Level; }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        [NotNull]
        [PublicAPI]
        public IEnumerable<string> Parameters
        {
            get { return Log.Parameters; }
        }

        /// <summary>
        /// Gets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        [PublicAPI]
        public DateTime TimeStamp
        {
            get { return Log.TimeStamp; }
        }

        /// <summary>
        /// Gets the message format.
        /// </summary>
        /// <value>The message format.</value>
        [CanBeNull]
        [PublicAPI]
        public string MessageFormat
        {
            get { return Log.MessageFormat; }
        }

        /// <summary>
        /// Gets the thread ID.
        /// </summary>
        /// <value>The thread ID.</value>
        [PublicAPI]
        public int ThreadID
        {
            get { return Log.ThreadID; }
        }

        /// <summary>
        /// Gets the name of the thread.
        /// </summary>
        /// <value>The name of the thread.</value>
        [CanBeNull]
        [PublicAPI]
        public string ThreadName
        {
            get { return Log.ThreadName; }
        }

        /// <summary>
        /// Gets the full name of the type of the exception.
        /// </summary>
        /// <value>The full name of the type of the exception.</value>
        [NotNull]
        [PublicAPI]
        public string ExceptionTypeFullName
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            get { return Log.Get(Log.ExceptionTypeFullNameKey) ?? this.GetType().FullName; }
        }

        /// <summary>
        /// Gets the stored procedure name (if a SQL exception - otherwise null).
        /// </summary>
        /// <value>The stored procedure.</value>
        [PublicAPI]
        [CanBeNull]
        public string StoredProcedure
        {
            get { return Log.Get(Log.StoredProcedureKey); }
        }

        /// <summary>
        /// Gets the stored procedure line number (if a SQL exception - otherwise -1).
        /// </summary>
        /// <value>The stored procedure line.</value>
        [PublicAPI]
        public int StoredProcedureLine
        {
            get
            {
                string line = Log.Get(Log.StoredProcedureLineKey);
                if (string.IsNullOrWhiteSpace(line))
                    return -1;
                int l;
                return int.TryParse(line, out l) ? l : -1;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the specified key. If the specified key is not found, throws a <see cref="T:System.Collections.Generic.KeyNotFoundException" />.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and <paramref name="key" /> does not exist in the collection.</exception>
        [CanBeNull]
        public string this[[NotNull] string key]
        {
            get { return Log[key]; }
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value>The message.</value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get { return Log.Message ?? string.Empty; }
        }

        /// <summary>
        ///   Gets the stack trace as a <see cref="string"/>.
        ///   Occasionally the stack trace becomes unavailable, so we capture to string on construction.
        /// </summary>
        /// <value>The safe stack trace.</value>
        [NotNull]
        [UsedImplicitly]
        public new string StackTrace
        {
            get { return Log.StackTrace ?? string.Empty; }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator{KeyValuePair{System.StringSystem.String}}.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Log.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Gets the value of the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        [CanBeNull]
        [PublicAPI]
        public string Get([NotNull] string key)
        {
            Contract.Requires(key != null);
            return Log.Get(key);
        }

        /// <summary>
        /// Gets all keys and their values that match the prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringSystem.String}}.</returns>
        [NotNull]
        [PublicAPI]
        public IEnumerable<KeyValuePair<string, string>> GetPrefixed([NotNull] string prefix)
        {
            Contract.Requires(prefix != null);
            return Log.GetPrefixed(prefix);
        }

        /// <summary>
        ///   Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> representation this instance.</returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        /// </PermissionSet>
        [UsedImplicitly]
        public override string ToString()
        {
            return Log.ToString(LogFormat.General);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        [NotNull]
        public string ToString([CanBeNull] IFormatProvider formatProvider)
        {
            return Log.ToString(null, formatProvider);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <exception cref="System.FormatException"></exception>
        [NotNull]
        public string ToString(LogFormat format)
        {
            return Log.ToString(format);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString([CanBeNull]string format, [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (format == null) format = LogFormat.General.ToString();

            if (formatProvider != null)
            {
                ICustomFormatter formatter = formatProvider.GetFormat(GetType()) as ICustomFormatter;

                if (formatter != null)
                    return formatter.Format(format, this, formatProvider) ?? string.Empty;
            }
            return Log.ToString(format, formatProvider);
        }
    }
}
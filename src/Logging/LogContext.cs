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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using ProtoBuf;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Formatting;

namespace WebApplications.Utilities.Logging
{
    /// <summary>
    /// Allows for additional contextual information to be stored against a <see cref="Log">log item</see>.
    /// </summary>
    /// <remarks>As well as constructing a <see cref="LogContext" /> directly, it is equally valid to use one of the
    /// implicit casts, or the static <see cref="Empty">new LogContext()</see>.</remarks>
    [ProtoContract(UseProtoMembersOnly = true, IgnoreListHandling = true)]
    [Serializable]
    [PublicAPI]
    public partial class LogContext : ResolvableWriteable, IEnumerable<KeyValuePair<string, string>>
    {
        /// <summary>
        /// The verbose format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder VerboseFormat = new FormatBuilder()
            .AppendLine()
            .AppendForegroundColor(Color.Teal)
            .AppendFormat("{" + Log.FormatTagHeader + ":-}")
            .AppendLayout(alignment: Alignment.Centre)
            .AppendFormat("{Key}")
            .AppendPopLayout()
            .AppendLayout(firstLineIndentSize: 3)
            .AppendFormat("{value:{<items>:{<item>:\r\n{key}\t: {value}}{<join>:\r\n}}}")
            .AppendPopLayout();

        /// <summary>
        /// The single line format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder NoLineFormat = new FormatBuilder()
            .AppendForegroundColor(Color.Teal)
            .AppendFormat("{Key}")
            .AppendResetForegroundColor()
            .AppendFormat("\t: [{Value:{<items>:{<item>:{noline}}}{<join>:;\t}}]")
            .MakeReadOnly();

        /// <summary>
        /// The XML format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder XMLFormat = new FormatBuilder()
            .AppendFormatLine("<{KeyXmlTag}>")
            .AppendLayout(firstLineIndentSize: 8)
            .AppendFormat("{Value:{<items>:{<item>:{xml}}}}")
            .AppendPopLayout()
            .AppendFormatLine("</{KeyXmlTag}>")
            .MakeReadOnly();

        /// <summary>
        /// The JSON format
        /// </summary>
        [NotNull]
        public static readonly FormatBuilder JSONFormat = new FormatBuilder()
            .AppendFormatLine("\"{Key}\"=")
            .AppendLine("{")
            .AppendLayout(firstLineIndentSize: 8)
            .AppendFormatLine("{Value:{<items>:{<item>:{json}}}{<join>:,\r\n}}")
            .AppendPopLayout()
            .Append('}')
            .MakeReadOnly();

        /// <summary>
        /// The minimum key length (note also minimum prefix length).
        /// </summary>
        [NonSerialized]
        public const int MinimumKeyLength = 3;

        /// <summary>
        /// The maximum key length (note maximum prefix length is this minus <see cref="MinimumKeyLength"/>.
        /// </summary>
        [NonSerialized]
        public const int MaximumKeyLength = 200;

        /// <summary>
        /// An empty, locked, <see cref="LogContext"/>.
        /// </summary>
        [NotNull]
        [NonSerialized]
        public static readonly LogContext Empty = new LogContext().Lock();

        /// <summary>
        /// The Key reservations.
        /// </summary>
        [NotNull]
        [NonSerialized]
        private static readonly Dictionary<string, Guid> _keyReservations = new Dictionary<string, Guid>();

        /// <summary>
        /// The prefix reservations.
        /// </summary>
        [NotNull]
        [NonSerialized]
        private static readonly Dictionary<string, Guid> _prefixReservations = new Dictionary<string, Guid>();

        /// <summary>
        /// The context dictionary.
        /// </summary>
        [NotNull]
        [ProtoMember(1)]
        private readonly ConcurrentDictionary<string, string> _context;

        /// <summary>
        /// Whether the context is locked.
        /// </summary>
        [NonSerialized]
        private bool _locked;

        /// <summary>
        /// Called when deserialized.
        /// </summary>
        /// <param name="context">The context.</param>
        [OnDeserialized]
        private void OnDeserialize(StreamingContext context)
        {
            _locked = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogContext"/> class.
        /// </summary>
        public LogContext()
            : base(false, true, false)
        {
            _context = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogContext"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        internal LogContext([NotNull] IEnumerable<KeyValuePair<string, string>> dictionary)
            : base(false, true, false)
        {
            if (dictionary == null) throw new ArgumentNullException("dictionary");
            _context = new ConcurrentDictionary<string, string>(dictionary);
        }

        /// <summary>
        /// Reserves a key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="reservation">The reservation (must not be <see cref="Guid.Empty" />.</param>
        /// <returns>The reserved key.</returns>
        /// <exception cref="WebApplications.Utilities.Logging.LoggingException">The key reservation fails.</exception>
        /// <remarks><para>The context key can only be modified with the specified reservation GUID after being reserved.</para>
        ///   <para>The recommended practice is to create a random static GUID in the class and use this for reservations, preventing
        /// anyone else modifying a reserved key for a context.</para>
        ///   <para>Trying to reserve a key when it has already been reserved with a different GUID will throw an exception.</para></remarks>
        [NotNull]
        public static string ReserveKey([NotNull] string key, Guid reservation)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            if (reservation == Guid.Empty)
                throw new LoggingException(() => Resources.LogContext_Invalid_Reservation);
            if (key == null)
                throw new LoggingException(() => Resources.LogContext_Null_Key);
            if (reservation == Guid.Empty)
                throw new LoggingException(() => Resources.LogContext_Empty_Reservation);
            if (key.Length < MinimumKeyLength)
                throw new LoggingException(() => Resources.LogContext_Key_Too_Short, key, MinimumKeyLength);
            if (key.Length > MaximumKeyLength)
                throw new LoggingException(() => Resources.LogContext_Key_Too_Long, key, MaximumKeyLength);

            lock (_keyReservations)
            {
                Guid r;
                // Check for existing reservation.
                if (_keyReservations.TryGetValue(key, out r))
                {
                    if (r != reservation)
                        throw new LoggingException(() => Resources.LogContext_Key_Already_Reserved, key);
                    return key;
                }

                // Check for prefix clashes.
                KeyValuePair<string, Guid> rKvp = _prefixReservations.FirstOrDefault(kvp => key.StartsWith(kvp.Key));
                if (rKvp.Key != null)
                {
                    if (rKvp.Value != reservation)
                        throw new LoggingException(
                            () => Resources.LogContext_Key_Reservation_Failed_Prefix_Match,
                            key,
                            rKvp.Key);

                    return key;
                }

                // Add the reservation
                _keyReservations.Add(key, reservation);
            }
            // ReSharper restore AssignNullToNotNullAttribute
            return key;
        }

        /// <summary>
        /// Reserves a prefix.
        /// </summary>
        /// <param name="prefix">The prefix (minimum of 3 characters).</param>
        /// <param name="reservation">The reservation (must not be <see cref="Guid.Empty"/>.</param>
        /// <returns>The reserved key.</returns>
        /// <remarks>
        /// <para>Any context key beginning with the prefix can only be modified with the specified reservation GUID after being reserved.</para>
        /// <para>The recommended practice is to create a random static GUID in the class and use this for reservations, preventing
        /// anyone else modifying a reserved key prefix for a context.</para>
        /// <para>Trying to reserve a key prefix when it has already been reserved with a different GUID, or when a reservation
        /// for a key that matches the prefix has already been reserved will throw an exception.</para>
        /// </remarks>
        [NotNull]
        public static string ReservePrefix([NotNull] string prefix, Guid reservation)
        {
            // ReSharper disable AssignNullToNotNullAttribute
            if (reservation == Guid.Empty)
                throw new LoggingException(() => Resources.LogContext_Invalid_Reservation);
            if (prefix == null)
                throw new LoggingException(() => Resources.LogContext_Null_Prefix);
            if (prefix.Length < MinimumKeyLength)
                throw new LoggingException(() => Resources.LogContext_Prefix_Too_Short, prefix, MinimumKeyLength);
            if (prefix.Length > (MaximumKeyLength - MinimumKeyLength))
                throw new LoggingException(
                    () => Resources.LogContext_Prefix_Too_Long,
                    prefix,
                    (MaximumKeyLength - MinimumKeyLength));
            if (reservation == Guid.Empty)
                throw new LoggingException(() => Resources.LogContext_Empty_Reservation);

            lock (_keyReservations)
            {
                // Check for prefix clashes.
                KeyValuePair<string, Guid> rKvp = _prefixReservations.FirstOrDefault(kvp => prefix.StartsWith(kvp.Key));
                if (rKvp.Key != null)
                {
                    if (rKvp.Value != reservation)
                        throw new LoggingException(
                            () => Resources.LogContext_Prefix_Reservation_Failed_Prefix_Match,
                            prefix,
                            rKvp.Key);

                    return prefix;
                }

                // Check existing key reservations.
                List<string> existingKeys = new List<string>(_keyReservations.Count);
                // ReSharper disable once PossibleNullReferenceException
                foreach (KeyValuePair<string, Guid> kvp in _keyReservations.Where(kvp => kvp.Key.StartsWith(prefix)))
                {
                    if (kvp.Value != reservation)
                        throw new LoggingException(
                            () => Resources.LogContext_Prefix_Reservation_Failed_Key_Match,
                            prefix,
                            kvp.Key);

                    existingKeys.Add(kvp.Key);
                }

                // Remove existing keys, we now have a single prefix reservation.
                foreach (string existingKey in existingKeys)
                    _keyReservations.Remove(existingKey);

                _prefixReservations.Add(prefix, reservation);
            }
            // ReSharper restore AssignNullToNotNullAttribute

            return prefix;
        }

        /// <summary>
        /// Determines whether the specified key is a reserved key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><see langword="true" /> if the specified key is a reserved key; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReservedKey([NotNull] string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            // Check for reservation.
            return Reservation(key) != Guid.Empty;
        }

        /// <summary>
        /// Gets the reservation (if any) for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Guid reservation; otherwise <see cref="Guid.Empty"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Guid Reservation([NotNull] string key)
        {
            Guid reservation;
            lock (_keyReservations)
                return _keyReservations.TryGetValue(key, out reservation)
                    ? reservation
                    // ReSharper disable once AssignNullToNotNullAttribute
                    : _prefixReservations.FirstOrDefault(kvp => key.StartsWith(kvp.Key)).Value;
        }

        /// <summary>
        /// Validates the specified key, throwing an exception if it is reserved.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="LoggingException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [NotNull]
        private static string Validate(Guid reservation, [NotNull] string key)
        {
            Guid r = Reservation(key);
            // ReSharper disable AssignNullToNotNullAttribute
            // Check for reservation
            if (r != Guid.Empty)
            {
                // We have a reservation so must match.
                if (r != reservation)
                    throw new LoggingException(() => Resources.LogContext_Reserved_Key, key);
            }
            // Remaining checks are for un-reserved keys.
            else if (key.Length < MinimumKeyLength)
                throw new LoggingException(() => Resources.LogContext_Key_Too_Short, key, MinimumKeyLength);
            else if (key.Length > MaximumKeyLength)
                throw new LoggingException(() => Resources.LogContext_Key_Too_Long, key, MaximumKeyLength);
            else if (!Char.IsLetter(key[0]))
                throw new LoggingException(() => Resources.LogContext_Key_Invalid_First_Char, key);
            else if (key.Any(c => !Char.IsLetterOrDigit(c) && (c != ' ')))
                throw new LoggingException(() => Resources.LogContext_Key_Invalid_Char, key);
            // ReSharper restore AssignNullToNotNullAttribute
            return key;
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        [NotNull]
        internal LogContext Lock()
        {
            _locked = true;
            return this;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is locked.
        /// </summary>
        /// <value><see langword="true" /> if this instance is locked; otherwise, <see langword="false" />.</value>
        public bool IsLocked
        {
            get { return _locked; }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _context.Count; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext" /> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext Set([NotNull] string key, [CanBeNull] object value)
        {
            return Set(Guid.Empty, key, value);
        }

        /// <summary>
        /// Sets the prefixed values.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext" /> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext SetPrefixed([NotNull] string prefix, [CanBeNull] params object[] values)
        {
            return SetPrefixed(Guid.Empty, prefix, (IEnumerable<object>)values);
        }

        /// <summary>
        /// Sets the prefixed values.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext" /> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext SetPrefixed([NotNull] string prefix, [CanBeNull] [InstantHandle] IEnumerable<object> values)
        {
            return SetPrefixed(Guid.Empty, prefix, values);
        }

        /// <summary>
        /// Sets the prefixed values.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext"/> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext SetPrefixed(Guid reservation, [NotNull] string prefix, [CanBeNull] params object[] values)
        {
            return SetPrefixed(reservation, prefix, (IEnumerable<object>)values);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext"/> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext Set(Guid reservation, [NotNull] string key, [CanBeNull] object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            // ReSharper disable once AssignNullToNotNullAttribute
            if (IsLocked) throw new LoggingException(() => Resources.LogContext_Locked);
            _context.GetOrAdd(Validate(reservation, key), k => value != null ? value.ToString() : null);
            return this;
        }

        /// <summary>
        /// Sets the prefixed values.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="values">The values.</param>
        /// <returns>This <see cref="LogContext"/> to allow fluent syntax.</returns>
        /// <exception cref="LoggingException">The <see cref="LogContext"/> is <see cref="IsLocked">locked</see>.</exception>
        [NotNull]
        public LogContext SetPrefixed(
            Guid reservation,
            [NotNull] string prefix,
            [CanBeNull] [InstantHandle] IEnumerable<object> values)
        {
            if (prefix == null) throw new ArgumentNullException("prefix");

            // ReSharper disable once AssignNullToNotNullAttribute
            if (IsLocked) throw new LoggingException(() => Resources.LogContext_Locked);
            if (values == null) return this;
            int suffix = 1;
            foreach (object value in values)
                _context.GetOrAdd(
                    Validate(reservation, prefix + suffix++),
                    k => value != null ? value.ToString() : null);
            return this;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value associated with the specified key.</returns>
        [CanBeNull]
        public string this[[NotNull] string key]
        {
            get { return Get(key); }
        }

        /// <summary>
        /// Gets the value of the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        [CanBeNull]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string Get([NotNull] string key)
        {
            if (key == null) throw new ArgumentNullException("key");
            string value;
            return _context.TryGetValue(key, out value) ? value : null;
        }

        /// <summary>
        /// Gets all keys and their values that match the prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringSystem.String}}.</returns>
        [NotNull]
        public IEnumerable<KeyValuePair<string, string>> GetPrefixed([NotNull] string prefix)
        {
            if (prefix == null) throw new ArgumentNullException("prefix");

            // ReSharper disable once PossibleNullReferenceException
            return _context.Where(kvp => kvp.Key.StartsWith(prefix));
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>IEnumerator{KeyValuePair{System.StringSystem.String}}.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _context.GetEnumerator();
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
        /// Gets the default format.
        /// </summary>
        /// <value>The default format.</value>
        public override FormatBuilder DefaultFormat
        {
            get { return VerboseFormat; }
        }

        /// <summary>
        /// Resolves the specified tag.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="chunk">The chunk.</param>
        /// <returns>An object that will be cached unless it is a <see cref="T:WebApplications.Utilities.Formatting.Resolution" />.</returns>
        // ReSharper disable once CodeAnnotationAnalyzer
        public override object Resolve(FormatWriteContext context, FormatChunk chunk)
        {
            CultureInfo culture = context.FormatProvider as CultureInfo ?? Translation.DefaultCulture;
            string key;
            // ReSharper disable once PossibleNullReferenceException
            switch (chunk.Tag.ToLowerInvariant())
            {
                case "default":
                case "verbose":
                    return VerboseFormat;
                case "xml":
                    return XMLFormat;
                case "json":
                    return JSONFormat;
                case "noline":
                    return NoLineFormat;
                case "key":
                    key = Translation.GetResource(() => Resources.LogKeys_Context, culture);
                    return string.IsNullOrEmpty(key)
                        ? Resolution.Null
                        : key;
                case "keyxml":
                    key = Translation.GetResource(() => Resources.LogKeys_Context, culture);
                    return string.IsNullOrEmpty(key)
                        ? Resolution.Null
                        : key.XmlEscape();
                case "keyxmltag":
                    key = Translation.GetResource(() => Resources.LogKeys_Context, culture);
                    return string.IsNullOrEmpty(key)
                        ? Resolution.Null
                        : key.Replace(' ', '_');
                case "value":
                    // ReSharper disable once AssignNullToNotNullAttribute
                    return _context.Select(kvp => new ContextElement(kvp.Key, kvp.Value)).ToArray();
                default:
                    return Resolution.Unknown;
            }
        }
    }
}
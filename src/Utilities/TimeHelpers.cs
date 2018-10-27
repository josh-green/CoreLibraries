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

using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using NodaTime.Text;
using WebApplications.Utilities.Annotations;
using WebApplications.Utilities.Globalization;
#if !BUILD_TASKS
using WebApplications.Utilities.Configuration;
#endif

namespace WebApplications.Utilities
{
    /// <summary>
    /// Helper methods for working with NodaTime objects.
    /// </summary>
    [PublicAPI]
    public static class TimeHelpers
    {
        [NotNull]
        private static IClock _clock;

        /// <summary>
        /// Gets or sets the clock.
        /// </summary>
        /// <value>The clock.</value>
        [NotNull]
        public static IClock Clock
        {
            get
            {
                Debug.Assert(_clock != null);
                return _clock;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _clock = value;
            }
        }

        /// <summary>
        /// Initializes the <see cref="TimeHelpers"/> class.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized - _dateTimeZoneProvider is set by SetDateTimeZoneProvider
        static TimeHelpers()
        {
#if !BUILD_TASKS
            SetDateTimeZoneProvider();
#endif

            // ReSharper disable once AssignNullToNotNullAttribute
            _clock = SystemClock.Instance;
        }

        #region Parsing
        /// <summary>
        /// Attempts to parse the given <paramref name="text"/> according to the rules of the <paramref name="pattern"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to parse.</typeparam>
        /// <param name="pattern">The pattern to attempt parse with.</param>
        /// <param name="text">The text to parse.</param>
        /// <param name="value">The parsed value if successful, otherwise <see langword="default{T}"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="pattern"/> was <see langword="null"/>.</exception>
        public static bool TryParse<T>([NotNull] this IPattern<T> pattern, string text, out T value)
        {
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            ParseResult<T> result = pattern.Parse(text);
            Debug.Assert(result != null);
            if (result.Success)
            {
                value = result.Value;
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>
        /// Attempts to parse the given <paramref name="text"/> according to the rules of any of the <paramref name="patterns"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value to parse.</typeparam>
        /// <param name="patterns">The pattern to attempt to parse with.</param>
        /// <param name="text">The text to parse.</param>
        /// <param name="value">The parsed value if successful, otherwise <see langword="default{T}"/>.</param>
        /// <returns><c>true</c> if parsed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="patterns"/> or one of its elements was <see langword="null"/>.</exception>
        public static bool TryParseAny<T>(
            [NotNull] this IEnumerable<IPattern<T>> patterns,
            string text,
            out T value) => TryParseAny(patterns, text, out value, out _);

        /// <summary>
        /// Attempts to parse the given <paramref name="text" /> according to the rules of any of the <paramref name="patterns" />.
        /// </summary>
        /// <typeparam name="T">The type of the value to parse.</typeparam>
        /// <param name="patterns">The pattern to attempt to parse with.</param>
        /// <param name="text">The text to parse.</param>
        /// <param name="value">The parsed value if successful, otherwise <see langword="default{T}" />.</param>
        /// <param name="matched">The matched pattern.</param>
        /// <returns><c>true</c> if parsed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="patterns" /> or one of its elements was <see langword="null" />.</exception>
        public static bool TryParseAny<T>([NotNull] this IEnumerable<IPattern<T>> patterns, string text, out T value, out IPattern<T> matched)
        {
            if (patterns == null) throw new ArgumentNullException(nameof(patterns));

            foreach (IPattern<T> pattern in patterns)
            {
                if (pattern == null) throw new ArgumentNullException(nameof(pattern));

                ParseResult<T> result = pattern.Parse(text);
                Debug.Assert(result != null);
                if (result.Success)
                {
                    value = result.Value;
                    matched = pattern;
                    return true;
                }
            }

            value = default(T);
            matched = null;
            return false;
        }
        #endregion

#if !BUILD_TASKS
        /// <summary>
        /// The current date time zone provider.
        /// </summary>
        [NotNull]
        private static IDateTimeZoneProvider _dateTimeZoneProvider;

        /// <summary>
        /// Whether <see cref="_dateTimeZoneProvider"/> was loaded from the config.
        /// </summary>
        private static bool _isFromConfig;

        /// <summary>
        /// A constant used to specify an infinite waiting period, for methods that accept a <see cref="Duration"/> parameter.
        /// </summary>
        public static readonly Duration InfiniteDuration = Duration.FromMilliseconds(Timeout.Infinite);

        /// <summary>
        /// A constant used to specify the maximum duration, for methods that accept a <see cref="Duration"/> parameter.
        /// </summary>
        public static readonly Duration MaxDuration = Duration.FromTicks(long.MaxValue);

        /// <summary>
        /// A constant used to specify the minimum duration, for methods that accept a <see cref="Duration"/> parameter.
        /// </summary>
        public static readonly Duration MinDuration = Duration.FromTicks(long.MinValue);

        /// <summary>
        /// A constant used to specify the maximum local date time, for methods that accept a <see cref="LocalDateTime"/> parameter.
        /// </summary>
        public static readonly LocalDateTime MaxLocalDateTime = new LocalDateTime(31195, 12, 31, 23, 59, 59, 999, 9999);

        /// <summary>
        /// A constant used to specify the minimum local date time, for methods that accept a <see cref="LocalDateTime"/> parameter.
        /// </summary>
        public static readonly LocalDateTime MinLocalDateTime = new LocalDateTime(-27255, 1, 1, 0, 0);

        /// <summary>
        /// A constant used to specify the maximum local date, for methods that accept a <see cref="LocalDate"/> parameter.
        /// </summary>
        public static readonly LocalDate MaxLocalDate = MaxLocalDateTime.Date;

        /// <summary>
        /// A constant used to specify the minimum local date, for methods that accept a <see cref="LocalDate"/> parameter.
        /// </summary>
        public static readonly LocalDate MinLocalDate = MinLocalDateTime.Date;

        /// <summary>
        /// A constant used to specify the maximum local time, for methods that accept a <see cref="LocalTime"/> parameter.
        /// </summary>
        public static readonly LocalTime MaxLocalTime = new LocalTime(23, 59, 59, 999, 9999);

        /// <summary>
        /// A constant used to specify the minimum local time, for methods that accept a <see cref="LocalTime"/> parameter.
        /// </summary>
        public static readonly LocalTime MinLocalTime = new LocalTime(0, 0);

        /// <summary>
        /// The one tick <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneTick = Duration.FromTicks(1);

        /// <summary>
        /// The one millisecond <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneMillisecond = Duration.FromMilliseconds(1);

        /// <summary>
        /// The one second <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneSecond = Duration.FromSeconds(1);

        /// <summary>
        /// The one minute <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneMinute = Duration.FromMinutes(1);

        /// <summary>
        /// The one hour <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneHour = Duration.FromHours(1);

        /// <summary>
        /// The one standard day <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneStandardDay = Duration.FromStandardDays(1);

        /// <summary>
        /// The one standard week <see cref="Duration"/>.
        /// </summary>
        public static readonly Duration OneStandardWeek = Duration.FromStandardWeeks(1);

        /// <summary>
        /// The one tick <see cref="Period"/>.
        /// </summary>
        public static readonly Period OneTickPeriod = Period.FromTicks(1);

        /// <summary>
        /// The one millisecond <see cref="Duration"/>.
        /// </summary>
        public static readonly Period OneMillisecondPeriod = Period.FromMilliseconds(1);
        
        // ReSharper disable AssignNullToNotNullAttribute
        /// <summary>
        /// The one second <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneSecondPeriod = Period.FromSeconds(1);

        /// <summary>
        /// The one minute <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneMinutePeriod = Period.FromMinutes(1);

        /// <summary>
        /// The one hour <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneHourPeriod = Period.FromHours(1);

        /// <summary>
        /// The one day <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneDayPeriod = Period.FromDays(1);

        /// <summary>
        /// The one week <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneWeekPeriod = Period.FromWeeks(1);

        /// <summary>
        /// The one month <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneMonthPeriod = Period.FromMonths(1);

        /// <summary>
        /// The one year <see cref="Period"/>.
        /// </summary>
        [NotNull]
        public static readonly Period OneYearPeriod = Period.FromYears(1);
        // ReSharper restore AssignNullToNotNullAttribute

        /// <summary>
        /// The file time epoch, 12:00 A.M. January 1, 1601.
        /// </summary>
        public static readonly Instant FileTimeEpoch = Instant.FromUtc(1601, 1, 1, 0, 0);

       /// <summary>
        /// Called when the utility configuration changes. If the <see cref="UtilityConfiguration.TimeZoneDB"/> property changes, the database will be reloaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ConfigurationSection{T}.ConfigurationChangedEventArgs"/> instance containing the event data.</param>
        internal static void OnActiveUtilityConfigurationChanged(
            [NotNull] UtilityConfiguration sender,
            [NotNull] ConfigurationSection<UtilityConfiguration>.ConfigurationChangedEventArgs e)
        {
            // TODO REVIEW!
            if (_isFromConfig)
                SetDateTimeZoneProvider();
        }

        /// <summary>
        /// Gets an <see cref="Instant"/> from file time ticks.
        /// </summary>
        /// <param name="fileTimeTicks">The number of 100-nanosecond intervals that have elapsed since <see cref="FileTimeEpoch"/>.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Instant InstantFromFileTimeUtc(long fileTimeTicks)
        {
            return FileTimeEpoch.PlusTicks(fileTimeTicks);
        }

        /// <summary>
        /// Gets or sets the date time zone provider.
        /// </summary>
        /// <value>The date time zone provider.</value>
        [NotNull]
        public static IDateTimeZoneProvider DateTimeZoneProvider
        {
            get
            {
                Debug.Assert(_dateTimeZoneProvider != null);
                return _dateTimeZoneProvider;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _dateTimeZoneProvider = value;
                _isFromConfig = false;
            }
        }

        /// <summary>
        /// Sets the <see cref="DateTimeZoneProvider"/> to the time zone database given.
        /// </summary>
        /// <param name="path">The path of the database file to load, or <see langword="null"/> to use the path in the configuration.
        /// If no path is given in the config, the default NodaTime <see cref="DateTimeZoneProviders.Tzdb"/> will be used.</param>
        /// <returns></returns>
        public static void SetDateTimeZoneProvider(string path = null)
        {
            _dateTimeZoneProvider = LoadTzdb(path);
            _isFromConfig = path == null;
        }

        /// <summary>
        /// Loads a time zone database.
        /// </summary>
        /// <param name="path">The path of the database file to load, or <see langword="null"/> to use the path in the configuration.
        /// If no path is given in the config, the default NodaTime <see cref="DateTimeZoneProviders.Tzdb"/> will be used.</param>
        /// <returns></returns>
        [NotNull]
        public static IDateTimeZoneProvider LoadTzdb(string path = null)
        {
            IDateTimeZoneProvider provider;

            // Load the time zone database from a file, if specified.
            path = path ?? UtilityConfiguration.Active.TimeZoneDB;
            if (!string.IsNullOrWhiteSpace(path))
            {
                Uri uri = new Uri(path.TrimStart('\\', '/'), UriKind.RelativeOrAbsolute);
                if (!uri.IsAbsoluteUri)
                    uri = new Uri(UtilityExtensions.AppDomainBaseUri, uri);

                // If the URI is a file, load it from the file system, otherwise download it
                if (uri.IsFile)
                {
                    path = uri.LocalPath;
                    if (!File.Exists(path))
                        throw new FileNotFoundException(
                            // ReSharper disable once AssignNullToNotNullAttribute
                            string.Format(Resources.TimeHelper_TimeHelper_TimeZoneDB_Not_Found, path));

                    try
                    {
                        using (FileStream stream = File.OpenRead(path))
                            // ReSharper disable once AssignNullToNotNullAttribute
                            provider = new DateTimeZoneCache(TzdbDateTimeZoneSource.FromStream(stream));
                    }
                    catch (Exception e)
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        throw new FileLoadException(
                            string.Format(Resources.TimeHelper_TimeHelper_TimeZoneDB_Failed, path),
                            e);
                    }
                }
                else
                {
                    path = uri.AbsoluteUri;
                    try
                    {
                        // ReSharper disable AssignNullToNotNullAttribute
                        WebRequest request = WebRequest.Create(uri);
                        using (WebResponse response = request.GetResponse())
                        using (Stream stream = response.GetResponseStream())
                            provider = new DateTimeZoneCache(TzdbDateTimeZoneSource.FromStream(stream));
                        // ReSharper restore AssignNullToNotNullAttribute
                    }
                    catch (Exception e)
                    {
                        // ReSharper disable once AssignNullToNotNullAttribute
                        throw new FileLoadException(
                            string.Format(Resources.TimeHelper_TimeHelper_TimeZoneDB_Failed, path),
                            e);
                    }
                }
            }
            // ReSharper disable once AssignNullToNotNullAttribute
            else
            // ReSharper disable once AssignNullToNotNullAttribute
                provider = DateTimeZoneProviders.Tzdb;

            Debug.Assert(provider != null);

            return provider;
        }

        /// <summary>
        /// The instant patterns.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly InstantPattern[] _instantPatterns =
        {
            InstantPattern.ExtendedIsoPattern,
            InstantPattern.GeneralPattern
        };

        /// <summary>
        /// Gets the instant patterns.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>An enumeration of instant patterns.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<InstantPattern> GetInstantPatterns(CultureInfo cultureInfo = null)
        {
            // Default, culture invariant parsers
            foreach (InstantPattern pattern in _instantPatterns)
                yield return pattern;

            // Get specific culture
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            if (cultureInfo.IsInvariant()) yield break;

            // Culture specific patterns
            foreach (InstantPattern pattern in _instantPatterns)
                yield return pattern.WithCulture(cultureInfo);
        }

        /// <summary>
        /// The duration patterns.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly DurationPattern[] _durationPatterns =
        {
            DurationPattern.RoundtripPattern,
            DurationPattern.CreateWithInvariantCulture("-H:mm:ss.FFFFFFF"),
            DurationPattern.CreateWithInvariantCulture("-D.hh:mm:ss.FFFFFFF")
        };

        /// <summary>
        /// Gets the duration patterns.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>An enumeration of duration patterns.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<DurationPattern> GetDurationPatterns(CultureInfo cultureInfo = null)
        {
            // Default, culture invariant parsers
            foreach (DurationPattern pattern in _durationPatterns)
                yield return pattern;

            // Get specific culture
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            if (cultureInfo.IsInvariant()) yield break;

            // Culture specific patterns (if any)
            foreach (DurationPattern pattern in _durationPatterns)
                yield return pattern.WithCulture(cultureInfo);
        }

        /// <summary>
        /// The local date time patterns.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly LocalDateTimePattern[] _localDateTimePatterns =
        {
            LocalDateTimePattern.FullRoundtripPattern,
            LocalDateTimePattern.BclRoundtripPattern,
            LocalDateTimePattern.ExtendedIsoPattern,
            LocalDateTimePattern.GeneralIsoPattern,
        };

        /// <summary>
        /// Gets the local date time patterns.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>An enumeration of local date time patterns.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<LocalDateTimePattern> GetLocalDateTimePatterns(CultureInfo cultureInfo = null)
        {
            // Default, culture invariant parsers
            foreach (LocalDateTimePattern pattern in _localDateTimePatterns)
                yield return pattern;

            // Get specific culture
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            if (cultureInfo.IsInvariant()) yield break;

            // Culture specific patterns
            foreach (LocalDateTimePattern pattern in _localDateTimePatterns)
                yield return pattern.WithCulture(cultureInfo);
        }

        /// <summary>
        /// The period patterns.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly PeriodPattern[] _periodPatterns =
        {
            PeriodPattern.RoundtripPattern,
            PeriodPattern.NormalizingIsoPattern,
        };

        /// <summary>
        /// Gets the local date time patterns.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>An enumeration of local date time patterns.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<PeriodPattern> GetPeriodPatterns(CultureInfo cultureInfo = null) 
            // Currently there's nothing culture-specific about period patterns.
            => _periodPatterns;

        /// <summary>
        /// The zoned date time patterns.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private static readonly ZonedDateTimePattern[] _zonedDateTimePatterns =
        {
            ZonedDateTimePattern.ExtendedFormatOnlyIsoPattern,
            ZonedDateTimePattern.GeneralFormatOnlyIsoPattern
        };

        /// <summary>
        /// Gets the zoned date time patterns.
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="dateTimeZoneProvider">The date time zone provider.</param>
        /// <returns>An enumeration of zoned date time patterns.</returns>
        [NotNull]
        [ItemNotNull]
        public static IEnumerable<ZonedDateTimePattern> GetZonedDateTimePatterns(CultureInfo cultureInfo = null, IDateTimeZoneProvider dateTimeZoneProvider = null)
        {
            if (dateTimeZoneProvider == null) dateTimeZoneProvider = DateTimeZoneProvider;

            int l = _zonedDateTimePatterns.Length;
            ZonedDateTimePattern[] patterns = new ZonedDateTimePattern[l];
            // Default, culture invariant parsers
            for (int i = 0; i < l; i++)
            {
                ZonedDateTimePattern pattern = _zonedDateTimePatterns[i].WithZoneProvider(dateTimeZoneProvider);
                yield return pattern;
                patterns[i] = pattern;
            }

            // Get specific culture
            if (cultureInfo == null) cultureInfo = CultureInfo.CurrentCulture;
            if (cultureInfo.IsInvariant()) yield break;

            // Culture specific patterns
            foreach (ZonedDateTimePattern pattern in patterns)
                yield return pattern.WithCulture(cultureInfo);
        }


        #region Duration
        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional milliseconds.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalMilliseconds(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets the milliseconds component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int Milliseconds(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerMillisecond) % 1000;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional seconds.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalSeconds(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerSecond;
        }

        /// <summary>
        /// Gets the seconds component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int Seconds(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerSecond) % 60;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional minutes.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalMinutes(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerMinute;
        }

        /// <summary>
        /// Gets the minutes component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int Minutes(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerMinute) % 60;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional hours.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalHours(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerHour;
        }

        /// <summary>
        /// Gets the hours component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int Hours(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerHour) % 24;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional standard days.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalStandardDays(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerStandardDay;
        }

        /// <summary>
        /// Gets the standard days component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int StandardDays(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerStandardDay);
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional standard days.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static double TotalStandardWeeks(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerStandardWeek;
        }

        /// <summary>
        /// Gets the standard weeks component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static int StandardWeeks(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerStandardWeek);
        }
        #endregion

        #region Floor/Ceiling
        /// <summary>
        /// Floors the specified instant to the second.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant FloorSecond(this Instant instant)
        {
            return new Instant((instant.Ticks / NodaConstants.TicksPerSecond) * NodaConstants.TicksPerSecond);
        }

        /// <summary>
        /// Ceilings the specified instant to the second.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant CeilingSecond(this Instant instant)
        {
            return new Instant(
                ((instant.Ticks + NodaConstants.TicksPerSecond - 1) / NodaConstants.TicksPerSecond) *
                NodaConstants.TicksPerSecond);
        }

        /// <summary>
        /// Floors the <see cref="Instant"/> to the nearest minute.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant FloorMinute(this Instant instant)
        {
            return new Instant((instant.Ticks / NodaConstants.TicksPerMinute) * NodaConstants.TicksPerMinute);
        }

        /// <summary>
        /// Ceilings the <see cref="Instant"/> to the nearest minute.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant CeilingMinute(this Instant instant)
        {
            return new Instant(
                ((instant.Ticks * NodaConstants.TicksPerMinute - 1) / NodaConstants.TicksPerMinute) *
                NodaConstants.TicksPerMinute);
        }

        /// <summary>
        /// Floors the <see cref="Instant"/> to the nearest hour.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant FloorHour(this Instant instant)
        {
            return new Instant((instant.Ticks / NodaConstants.TicksPerHour) * NodaConstants.TicksPerHour);
        }

        /// <summary>
        /// Ceilings the <see cref="Instant"/> to the nearest hour.
        /// </summary>
        /// <param name="instant">The instant.</param>
        /// <returns>Instant.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Instant CeilingHour(this Instant instant)
        {
            return new Instant(
                ((instant.Ticks * NodaConstants.TicksPerHour - 1) / NodaConstants.TicksPerHour) *
                NodaConstants.TicksPerHour);
        }
        #endregion

        #region Periods
        /// <summary>
        /// Determines whether the specified period is zero.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <returns></returns>
        public static bool IsZero([NotNull] this Period period)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return period.Ticks == 0 &&
                   period.Milliseconds == 0 &&
                   period.Seconds == 0 &&
                   period.Minutes == 0 &&
                   period.Hours == 0 &&
                   period.Days == 0 &&
                   period.Weeks == 0 &&
                   period.Months == 0 &&
                   period.Years == 0;
        }

        /// <summary>
        /// Determines whether the specified period is positive, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static bool IsPositive([NotNull] this Period period, LocalDateTime local)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return (local + period) > local;
        }

        /// <summary>
        /// Determines whether the specified period is negative, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static bool IsNegative([NotNull] this Period period, LocalDateTime local)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return (local + period) < local;
        }

        /// <summary>
        /// Determines whether the specified period is positive, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static bool IsPositive([NotNull] this Period period, LocalDate local)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return (local + period) > local;
        }

        /// <summary>
        /// Determines whether the specified period is negative, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static bool IsNegative([NotNull] this Period period, LocalDate local)
        {
            if (period == null) throw new ArgumentNullException(nameof(period));

            return (local + period) < local;
        }
        #endregion

#endif
    }
}
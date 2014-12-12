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
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using WebApplications.Utilities.Annotations;
using NodaTime;
using NodaTime.TimeZones;
using WebApplications.Utilities.Configuration;

namespace WebApplications.Utilities
{
    // TODO Better name?
    /// <summary>
    /// Helper methods for working with NodaTime objects.
    /// </summary>
    [PublicAPI]
    public static class TimeHelpers
    {
        [NotNull]
        private static IDateTimeZoneProvider _dateTimeZoneProvider;

        /// <summary>
        /// The one tick <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneTick = Duration.FromTicks(1);

        /// <summary>
        /// The one millisecond <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneMillisecond = Duration.FromMilliseconds(1);

        /// <summary>
        /// The one second <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneSecond = Duration.FromSeconds(1);

        /// <summary>
        /// The one minute <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneMinute = Duration.FromMinutes(1);

        /// <summary>
        /// The one hour <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneHour = Duration.FromHours(1);

        /// <summary>
        /// The one standard day <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneStandardDay = Duration.FromStandardDays(1);

        /// <summary>
        /// The one standard week <see cref="Duration"/>.
        /// </summary>
        [PublicAPI]
        public static readonly Duration OneStandardWeek = Duration.FromStandardWeeks(1);

        static TimeHelpers()
        {
            LoadTzdb();
        }

        /// <summary>
        /// Loads the time zone database into the <see cref="DateTimeZoneProvider"/>.
        /// </summary>
        /// <param name="path">The path of the database file to load, or <see langword="null"/> to use the path in the configuration.</param>
        /// <returns></returns>
        [PublicAPI]
        [NotNull]
        public static IDateTimeZoneProvider LoadTzdb(string path = null)
        {
            IDateTimeZoneProvider provider;

            // Load the time zone database from a file, if specified.
            string dbPath = path ?? UtilityConfiguration.Active.TimeZoneDB;
            if (!string.IsNullOrWhiteSpace(dbPath))
            {
                if (!File.Exists(dbPath))
                    throw new FileNotFoundException(
                        // ReSharper disable once AssignNullToNotNullAttribute
                        string.Format(Resources.TimeHelper_TimeHelper_TimeZoneDB_Not_Found, dbPath));

                try
                {
                    using (FileStream stream = File.OpenRead(dbPath))
                        // ReSharper disable once AssignNullToNotNullAttribute
                        provider = new DateTimeZoneCache(TzdbDateTimeZoneSource.FromStream(stream));
                }
                catch (Exception e)
                {
                    // ReSharper disable once AssignNullToNotNullAttribute
                    throw new FileLoadException(
                        string.Format(Resources.TimeHelper_TimeHelper_TimeZoneDB_Failed, dbPath),
                        e);
                }
            }
            // ReSharper disable once AssignNullToNotNullAttribute
            else
                // ReSharper disable once AssignNullToNotNullAttribute
                provider = DateTimeZoneProviders.Tzdb;

            Contract.Assert(provider != null);

            _dateTimeZoneProvider = provider;

            // ReSharper disable once AssignNullToNotNullAttribute
            return provider;
        }

        /// <summary>
        /// Gets or sets the date time zone provider.
        /// </summary>
        /// <value>The date time zone provider.</value>
        [NotNull]
        [PublicAPI]
        public static IDateTimeZoneProvider DateTimeZoneProvider
        {
            get { return _dateTimeZoneProvider; }
            set
            {
                Contract.Requires(value != null);
                _dateTimeZoneProvider = value;
            }
        }

        #region Duration
        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional milliseconds.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalMilliseconds(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerMillisecond;
        }

        /// <summary>
        /// Gets the milliseconds component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static int Milliseconds(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerMillisecond) % 1000;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional seconds.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalSeconds(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerSecond;
        }

        /// <summary>
        /// Gets the seconds component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static int Seconds(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerSecond) % 60;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional minutes.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalMinutes(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerMinute;
        }

        /// <summary>
        /// Gets the minutes component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static int Minutes(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerMinute) % 60;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional hours.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalHours(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerHour;
        }

        /// <summary>
        /// Gets the hours component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static int Hours(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerHour) % 24;
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional standard days.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalStandardDays(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerStandardDay;
        }

        /// <summary>
        /// Gets the standard days component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static int StandardDays(this Duration duration)
        {
            return (int)(duration.Ticks / NodaConstants.TicksPerStandardDay);
        }

        /// <summary>
        /// Gets the value of the <see cref="Duration"/> expressed in whole and fractional standard days.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static double TotalStandardWeeks(this Duration duration)
        {
            return (double)duration.Ticks / NodaConstants.TicksPerStandardWeek;
        }

        /// <summary>
        /// Gets the standard weeks component of the <see cref="Duration"/>.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>System.Double.</returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
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
        [PublicAPI]
        public static bool IsZero([NotNull] this Period period)
        {
            Contract.Requires(period != null);

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
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static bool IsPositive([NotNull] this Period period, LocalDateTime local)
        {
            Contract.Requires(period != null);

            return (local + period) > local;
        }

        /// <summary>
        /// Determines whether the specified period is negative, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static bool IsNegative([NotNull] this Period period, LocalDateTime local)
        {
            Contract.Requires(period != null);

            return (local + period) < local;
        }

        /// <summary>
        /// Determines whether the specified period is positive, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static bool IsPositive([NotNull] this Period period, LocalDate local)
        {
            Contract.Requires(period != null);

            return (local + period) > local;
        }

        /// <summary>
        /// Determines whether the specified period is negative, relative to a <see cref="LocalDateTime" />.
        /// </summary>
        /// <param name="period">The period.</param>
        /// <param name="local">The local.</param>
        /// <returns></returns>
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        [WebApplications.Utilities.Annotations.Pure]
        public static bool IsNegative([NotNull] this Period period, LocalDate local)
        {
            Contract.Requires(period != null);

            return (local + period) < local;
        }
        #endregion
    }
}
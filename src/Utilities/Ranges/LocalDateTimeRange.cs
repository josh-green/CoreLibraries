#region � Copyright Web Applications (UK) Ltd, 2015.  All rights reserved.
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
using System.Diagnostics;
using NodaTime;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Ranges
{
    /// <summary>
    /// A range of <see cref="LocalDateTime"/>.
    /// </summary>
    [PublicAPI]
    public class LocalDateTimeRange : Range<LocalDateTime, Period>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeRange"/> class using the specified start and end date time.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        public LocalDateTimeRange(LocalDateTime start, LocalDateTime end)
            : base(start, end, AutoStep(start, end))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeRange" /> class using the specified start date time and duration.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="duration">The duration.</param>
        public LocalDateTimeRange(LocalDateTime start, [NotNull] Period duration)
            : base(start, start + duration, AutoStep(start, start + duration))
        {
            if (duration == null) throw new ArgumentNullException(nameof(duration));
            if (!duration.IsPositive(start))
                throw new ArgumentOutOfRangeException(Resources.LocalRange_DurationMustBePositive);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeRange"/> class using the specified start date time, end date time and step.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="step">The step.</param>
        public LocalDateTimeRange(LocalDateTime start, LocalDateTime end, [NotNull] Period step)
            // ReSharper disable once AssignNullToNotNullAttribute
            : base(start, end, step.Normalize())
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (!step.IsPositive(start)) throw new ArgumentOutOfRangeException(Resources.LocalRange_StepMustBePositive);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDateTimeRange" /> class using the specified start date time, duration and step.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="step">The step.</param>
        public LocalDateTimeRange(LocalDateTime start, [NotNull] Period duration, [NotNull] Period step)
            // ReSharper disable once AssignNullToNotNullAttribute
            : base(start, start + duration, step.Normalize())
        {
            if (step == null) throw new ArgumentNullException(nameof(step));
            if (duration == null) throw new ArgumentNullException(nameof(duration));
            if (!step.IsPositive(start)) throw new ArgumentOutOfRangeException(Resources.LocalRange_StepMustBePositive);
            if (!duration.IsPositive(start))
                throw new ArgumentOutOfRangeException(Resources.LocalRange_DurationMustBePositive);
        }

        /// <summary>
        /// Given a start and end automatically returns a sensible step size.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>
        /// Period.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [NotNull]
        public static Period AutoStep(LocalDateTime start, LocalDateTime end)
        {
            CheckStartGreaterThanEnd(start, end);

            // ReSharper disable once PossibleNullReferenceException
            Period delta = Period.Between(start, end).Normalize();
            Debug.Assert(delta != null);

            // ReSharper disable AssignNullToNotNullAttribute
            if (delta.Months > 0 ||
                delta.Years > 0 ||
                delta.Weeks > 0 ||
                delta.Days > 0)
                return TimeHelpers.OneDayPeriod;
            if (delta.Hours > 0)
                return TimeHelpers.OneHourPeriod;
            if (delta.Minutes > 0)
                return TimeHelpers.OneMinutePeriod;
            if (delta.Seconds > 0)
                return TimeHelpers.OneSecondPeriod;
            if (delta.Milliseconds > 0)
                return TimeHelpers.OneMillisecondPeriod;
            return TimeHelpers.OneTickPeriod;
            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// Converts this range to a <see cref="DateTimeRange"/> with a <see cref="DateTime.Kind" /> of <see cref="DateTimeKind.Unspecified"/>.
        /// </summary>
        [NotNull]
        public DateTimeRange ToDateTimeRangeUnspecified()
        {
            return new DateTimeRange(
                Start.ToDateTimeUnspecified(),
                End.ToDateTimeUnspecified(),
                // ReSharper disable once PossibleNullReferenceException
                new TimeSpan(Period.Between(Start, Start + Step, PeriodUnits.Ticks).Ticks));
        }

        /// <summary>
        /// Gets a <see cref="LocalDateRange"/> of the date components of this <see cref="LocalDateTimeRange"/>. 
        /// The step will be rounded to the nearest day, rounded up to 1 day if less.
        /// </summary>
        /// <value>
        /// The date range.
        /// </value>
        [NotNull]
        public LocalDateRange DateRange
        {
            get { return new LocalDateRange(Start.Date, End.Date, DateStep()); }
        }

        /// <summary>
        /// Rounds the step to the nearest day, rounding up to 1 day if less.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        private Period DateStep()
        {
            // ReSharper disable PossibleNullReferenceException
            PeriodBuilder step =
                (Step + Period.FromTicks(NodaConstants.TicksPerStandardDay >> 1)).Normalize().ToBuilder();
            // ReSharper restore PossibleNullReferenceException

            Debug.Assert(step != null);

            step.Ticks = 0;
            step.Milliseconds = 0;
            step.Seconds = 0;
            step.Minutes = 0;
            step.Hours = 0;

            Period rounded = step.Build();
            Debug.Assert(rounded != null);
            Debug.Assert(!rounded.HasTimeComponent);

            // ReSharper disable once AssignNullToNotNullAttribute
            return rounded.IsZero() ? TimeHelpers.OneDayPeriod : rounded;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Start} - {End}";
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            return $"{Start.ToString(format, formatProvider)} - {End.ToString(format, formatProvider)}";
        }
    }
}
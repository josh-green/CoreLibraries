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
using System.Runtime.InteropServices;
using NodaTime;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities
{
    /// <summary>
    /// A clock that retrieves the current system date and time with the highest possible level of precision (&lt;1us).
    /// </summary>
    /// <remarks>Uses the GetSystemTimePreciseAsFileTime function if available, falling back to using a <see cref="Stopwatch"/> if not.</remarks>
    [PublicAPI]
    public class HighPrecisionClock : IClock
    {
        /// <summary>
        /// The instance of the clock.
        /// </summary>
        [NotNull]
        public static readonly HighPrecisionClock Instance = new HighPrecisionClock();

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        private static readonly bool _getSysTimeAvailable;

        private static readonly Instant _startedTime;
        private static readonly long _startedTicks;

        private static readonly decimal _swToTicks = 10000000M / Stopwatch.Frequency;

        /// <summary>
        /// Initializes the <see cref="HighPrecisionClock"/> class.
        /// </summary>
        static HighPrecisionClock()
        {
            try
            {
                long time;
                GetSystemTimePreciseAsFileTime(out time);
                _getSysTimeAvailable = true;
            }
            catch (Exception)
            {
                _getSysTimeAvailable = false;
                if (Stopwatch.IsHighResolution)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    _startedTime = SystemClock.Instance.Now;
                    _startedTicks = Stopwatch.GetTimestamp();
                }
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="HighPrecisionClock"/> class from being created.
        /// </summary>
        private HighPrecisionClock()
        {
        }

        /// <summary>
        /// Gets the current <see cref="T:NodaTime.Instant" /> on the time line according to this clock.
        /// </summary>
        public Instant Now
        {
            get
            {
                if (_getSysTimeAvailable)
                {
                    long time;
                    GetSystemTimePreciseAsFileTime(out time);
                    return TimeHelpers.FileTimeEpoch.PlusTicks(time);
                }

                if (Stopwatch.IsHighResolution)
                    return _startedTime.PlusTicks((long)((Stopwatch.GetTimestamp() - _startedTicks) * _swToTicks));

                // ReSharper disable once PossibleNullReferenceException
                return SystemClock.Instance.Now;
            }
        }

        /// <summary>
        /// Gets the current ticks on the time line according to this clock.
        /// </summary>
        public long NowTicks
        {
            get
            {
                if (_getSysTimeAvailable)
                {
                    long time;
                    GetSystemTimePreciseAsFileTime(out time);
                    return TimeHelpers.FileTimeEpoch.Ticks + time;
                }

                if (Stopwatch.IsHighResolution)
                    return _startedTime.Ticks + (long)((Stopwatch.GetTimestamp() - _startedTicks) * _swToTicks);

                // ReSharper disable once PossibleNullReferenceException
                return SystemClock.Instance.Now.Ticks;
            }
        }
    }
}
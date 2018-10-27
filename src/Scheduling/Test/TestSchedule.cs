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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using WebApplications.Utilities.Scheduling.Schedules;

namespace WebApplications.Utilities.Scheduling.Test
{
    [TestClass]
    public class TestSchedule
    {
        [TestMethod]
        public void TestNextValidSecond()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(
                i,
                (i - Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every,
                    second: Second.Every)
                );
            Assert.AreEqual(
                i,
                i.NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every,
                    second: Second.Every)
                );
            Assert.AreEqual(
                i + TimeHelpers.OneSecond,
                (i + Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every,
                    second: Second.Every)
                );
        }

        [TestMethod]
        public void TestNextValidMinute()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(
                i,
                (i - Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every)
                );
            Assert.AreEqual(
                i,
                i.NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every)
                );
            Assert.AreEqual(
                i + Duration.FromMinutes(1),
                (i + Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every,
                    minute: Minute.Every)
                );
        }

        [TestMethod]
        public void TestNextValidHour()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(
                i,
                (i - Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every)
                );
            Assert.AreEqual(
                i,
                i.NextValid(
                    hour: Hour.Every)
                );
            Assert.AreEqual(
                i + Duration.FromHours(1),
                (i + Duration.FromTicks(1)).NextValid(
                    hour: Hour.Every)
                );
        }

        [TestMethod]
        public void TestNextValidDay()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(i, (i - Duration.FromTicks(1)).NextValid());
            Assert.AreEqual(i, i.NextValid());
            Assert.AreEqual(i + Duration.FromStandardDays(1), (i + Duration.FromTicks(1)).NextValid());
        }

        [TestMethod]
        public void TestNextWeek()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2014, 8, 11, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(i, (i - Duration.FromTicks(1)).NextValid(weekDay: WeekDay.Monday));
            Assert.AreEqual(i, i.NextValid(weekDay: WeekDay.Monday));
            Assert.AreEqual(
                i + Duration.FromStandardWeeks(1),
                (i + Duration.FromTicks(1)).NextValid(weekDay: WeekDay.Monday));
        }

        [TestMethod]
        public void TestNextMonth()
        {
            Instant i = Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            Assert.AreEqual(i, (i - Duration.FromTicks(1)).NextValid(day: Day.First));
            Assert.AreEqual(i, i.NextValid(day: Day.First));
            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 2, 1, 0, 0, 0, DateTimeKind.Utc)),
                (i + Duration.FromTicks(1)).NextValid(day: Day.First));
        }


#if false
        [TestMethod]
        public void TestNextValidMinute()
        {
            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 0, 1, 0, DateTimeKind.Utc)),
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)).NextValid(minute: Minute.Every)
                );

            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)),
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)).NextValid(minute: Minute.Every)
                );
        }

        [TestMethod]
        public void TestNextValidHour()
        {
            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1, 1, 0, 0)),
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)).NextValid(hour: Hour.Every)
                );

            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)),
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)).NextValid(hour: Hour.Every)
                );
        }
        [TestMethod]
        public void TestNextValidInclusiveDay()
        {
            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 2)),
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)).NextValid(inclusive: false)
                );

            Assert.AreEqual(
                Instant.FromDateTimeUtc(new DateTime(2013, 1, 1)),
                (new DateTime(2013, 1, 1)).NextValid(inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidInclusiveMonth()
        {
            Assert.AreEqual(
                new DateTime(2013, 2, 1),
                (new DateTime(2013, 1, 1)).NextValid(inclusive: false, day: Day.First)
                );

            Assert.AreEqual(
                new DateTime(2013, 1, 1),
                (new DateTime(2013, 1, 1)).NextValid(inclusive: true, day: Day.First)
                );
        }

        [TestMethod]
        public void TestNextValidInclusiveYear()
        {
            Assert.AreEqual(
                new DateTime(2014, 1, 1),
                (new DateTime(2013, 1, 1)).NextValid(inclusive: false, day: Day.First, month: Month.January)
                );

            Assert.AreEqual(
                new DateTime(2013, 1, 1),
                (new DateTime(2013, 1, 1)).NextValid(inclusive: true, day: Day.First, month: Month.January)
                );
        }

        [TestMethod]
        public void TestNextValidLeapYear1()
        {
            // Get next leap year
            int currentYear = DateTime.UtcNow.Year;
            int nextLeapYear = currentYear + (4 - (currentYear % 4));

            Assert.AreEqual(
                new DateTime(nextLeapYear, 2, 29),
                DateTime.UtcNow.NextValid(Month.February, day: Day.TwentyNinth),
                DateTime.UtcNow.ToString()
                );
        }

        [TestMethod]
        public void TestNextValidLeapYear2()
        {
            // Get next leap year
            int currentYear = DateTime.UtcNow.Year;
            int nextLeapYear = currentYear + (4 - (currentYear % 4));
            int leapYearAfterNext = nextLeapYear + 4;

            Assert.AreEqual(
                new DateTime(leapYearAfterNext, 2, 29),
                (new DateTime(nextLeapYear, 2, 29, 0, 0, 1)).NextValid(Month.February, day: Day.TwentyNinth)
                );
        }

        [TestMethod]
        public void TestNextValidFebruary1()
        {
            Assert.AreEqual(
                DateTime.MaxValue,
                DateTime.UtcNow.NextValid(Month.February, day: Day.Thirtieth),
                DateTime.UtcNow.ToString()
                );

            Assert.AreEqual(
                DateTime.MaxValue,
                DateTime.UtcNow.NextValid(Month.February, day: Day.ThirtyFirst),
                DateTime.UtcNow.ToString()
                );
        }

        [TestMethod]
        public void TestNextValidFebruary2()
        {
            // Get year that next Feb 28th is in
            int currentYear = DateTime.UtcNow.Year;
            int testYear = currentYear + ((DateTime.UtcNow >= new DateTime(currentYear, 2, 28)) ? 1 : 0);

            Assert.AreEqual(
                new DateTime(testYear, 2, 28),
                DateTime.UtcNow.NextValid(Month.February, day: Day.TwentyEighth),
                DateTime.UtcNow.ToString()
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday1()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 1),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Saturday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday2()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 2),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Sunday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday3()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 3),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Monday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday4()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 4),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Tuesday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday5()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 5),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Wednesday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday6()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 6),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Thursday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySaturday7()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 7),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Friday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday1()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 2),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Sunday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday2()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 3),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Monday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday3()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 4),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Tuesday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday4()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 5),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Wednesday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday5()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 6),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Thursday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday6()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 7),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Friday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDaySunday7()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 8),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.Saturday, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDayWeekDay1()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 3),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.WeekDay, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDayWeekDay2()
        {
            // 02/01/2000 is a Sunday

            Assert.AreEqual(
                new DateTime(2000, 1, 3),
                (new DateTime(2000, 1, 2)).NextValid(weekDay: WeekDay.WeekDay, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDayWeekEnd1()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                new DateTime(2000, 1, 1),
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Weekend, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDayWeekEnd2()
        {
            // 03/01/2000 is a Monday

            Assert.AreEqual(
                new DateTime(2000, 1, 8),
                (new DateTime(2000, 1, 3)).NextValid(weekDay: WeekDay.Weekend, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeekDayNever()
        {
            // 01/01/2000 is a Saturday

            Assert.AreEqual(
                DateTime.MaxValue,
                (new DateTime(2000, 1, 1)).NextValid(weekDay: WeekDay.Never, inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidWeek1()
        {
            Assert.AreEqual(
                new DateTime(2001, 1, 1),
                (new DateTime(2000, 6, 1)).NextValid(week: Week.First)
                );
        }

        [TestMethod]
        public void TestNextValidWeek2()
        {
            Assert.AreEqual(
                new DateTime(2000, 12, 24),
                (new DateTime(2000, 6, 1)).NextValid(week: Week.FiftySecond)
                );
        }

        [TestMethod]
        public void TestNextValidWeek3()
        {
            Assert.AreEqual(
                new DateTime(2000, 1, 9),
                (new DateTime(2000, 1, 1)).NextValid(week: Week.Second)
                );
        }

        [TestMethod]
        public void TestNextValidWeek4()
        {
            Assert.AreEqual(
                new DateTime(2004, 1, 1),
                (new DateTime(2000, 6, 1)).NextValid(week: Week.Zeroth)
                );
        }

        [TestMethod]
        public void TestNextValidRandom()
        {
            const int numberOfIterations = 10000;

            // Use to allow sequential testing for easier debugging or average timing
            const bool parallel = true;

            Random random = new Random();
            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            Action<int> testAction = iteration =>
            {
                // Random initial DateTime
                int year = random.Next(1, 9999);
                int month = random.Next(1, 13);
                int daysInMonth = calendar.GetDaysInMonth(year, month);
                int day = random.Next(1, daysInMonth);
                DateTime startDateTime = new DateTime(
                    year,
                    month,
                    day,
                    random.Next(24),
                    random.Next(60),
                    random.Next(60));

                // Random incrementation
                int incrementationType = random.Next(5);
                string incrementationTypeName;
                int incrementationAmount;
                DateTime expectedDateTime;
                switch (incrementationType)
                {
                    default:
                    case 0:
                        incrementationTypeName = "Month";
                        incrementationAmount = random.Next(12 - startDateTime.Month);
                        expectedDateTime = calendar.AddMonths(
                            startDateTime,
                            incrementationAmount);
                        if (incrementationAmount > 0)
                        {
                            expectedDateTime = new DateTime(
                                expectedDateTime.Year,
                                expectedDateTime.Month,
                                1,
                                0,
                                0,
                                0);
                        }
                        break;

                    case 1:
                        incrementationTypeName = "Day";
                        incrementationAmount = random.Next(daysInMonth - startDateTime.Day);
                        expectedDateTime = calendar.AddDays(
                            startDateTime,
                            incrementationAmount);
                        if (incrementationAmount > 0)
                        {
                            expectedDateTime = new DateTime(
                                expectedDateTime.Year,
                                expectedDateTime.Month,
                                expectedDateTime.Day,
                                0,
                                0,
                                0);
                        }
                        break;

                    case 2:
                        incrementationTypeName = "Hour";
                        incrementationAmount = random.Next(24 - startDateTime.Hour);
                        expectedDateTime = calendar.AddHours(
                            startDateTime,
                            incrementationAmount);
                        if (incrementationAmount > 0)
                        {
                            expectedDateTime = new DateTime(
                                expectedDateTime.Year,
                                expectedDateTime.Month,
                                expectedDateTime.Day,
                                expectedDateTime.Hour,
                                0,
                                0);
                        }
                        break;

                    case 3:
                        incrementationTypeName = "Minute";
                        incrementationAmount = random.Next(60 - startDateTime.Minute);
                        expectedDateTime = calendar.AddMinutes(
                            startDateTime,
                            incrementationAmount);
                        if (incrementationAmount > 0)
                        {
                            expectedDateTime = new DateTime(
                                expectedDateTime.Year,
                                expectedDateTime.Month,
                                expectedDateTime.Day,
                                expectedDateTime.Hour,
                                expectedDateTime.Minute,
                                0);
                        }
                        break;

                    case 4:
                        incrementationTypeName = "Second";
                        incrementationAmount = random.Next(60 - startDateTime.Second);
                        expectedDateTime = calendar.AddSeconds(
                            startDateTime,
                            incrementationAmount);
                        break;
                }

                // Random week start day
                DayOfWeek weekStartDay;
                string weekStartDayName;
                switch (random.Next(7))
                {
                    default:
                    case 0:
                        weekStartDayName = "Monday";
                        weekStartDay = DayOfWeek.Monday;
                        break;

                    case 1:
                        weekStartDayName = "Tuesday";
                        weekStartDay = DayOfWeek.Tuesday;
                        break;

                    case 2:
                        weekStartDayName = "Wednesday";
                        weekStartDay = DayOfWeek.Wednesday;
                        break;

                    case 3:
                        weekStartDayName = "Thursday";
                        weekStartDay = DayOfWeek.Thursday;
                        break;

                    case 4:
                        weekStartDayName = "Friday";
                        weekStartDay = DayOfWeek.Friday;
                        break;

                    case 5:
                        weekStartDayName = "Saturday";
                        weekStartDay = DayOfWeek.Saturday;
                        break;

                    case 6:
                        weekStartDayName = "Sunday";
                        weekStartDay = DayOfWeek.Sunday;
                        break;
                }

                if (incrementationAmount > 0)
                {
                    // Test
                    DateTime resultantDateTime;
                    switch (incrementationType)
                    {
                        default:
                        case 0:
                            resultantDateTime = startDateTime.NextValid(
                                inclusive: true,
                                firstDayOfWeek:
                                    weekStartDay,
                                month:
                                    Schedule
                                    .Months(
                                        startDateTime
                                            .Month +
                                        incrementationAmount),
                                hour: Hour.Every,
                                minute:
                                    Minute.Every,
                                second:
                                    Second.Every);
                            break;

                        case 1:
                            resultantDateTime = startDateTime.NextValid(
                                inclusive: true,
                                firstDayOfWeek:
                                    weekStartDay,
                                day:
                                    Schedule
                                    .Days(
                                        startDateTime
                                            .Day +
                                        incrementationAmount),
                                hour: Hour.Every,
                                minute:
                                    Minute.Every,
                                second:
                                    Second.Every);
                            break;

                        case 2:
                            resultantDateTime = startDateTime.NextValid(
                                inclusive: true,
                                firstDayOfWeek:
                                    weekStartDay,
                                hour:
                                    Schedule
                                    .Hours(
                                        startDateTime
                                            .Hour +
                                        incrementationAmount),
                                minute:
                                    Minute.Every,
                                second:
                                    Second.Every);
                            break;

                        case 3:
                            resultantDateTime = startDateTime.NextValid(
                                inclusive: true,
                                firstDayOfWeek:
                                    weekStartDay,
                                minute:
                                    Schedule
                                    .Minutes(
                                        startDateTime
                                            .Minute +
                                        incrementationAmount),
                                hour: Hour.Every,
                                second:
                                    Second.Every);
                            break;

                        case 4:
                            resultantDateTime = startDateTime.NextValid(
                                inclusive: true,
                                firstDayOfWeek:
                                    weekStartDay,
                                hour: Hour.Every,
                                minute:
                                    Minute.Every,
                                second:
                                    Schedule
                                    .Seconds(
                                        startDateTime
                                            .Second +
                                        incrementationAmount));
                            break;
                    }

                    // Report
                    Assert.AreEqual(
                        expectedDateTime,
                        resultantDateTime,
                        string.Format(
                            "Started:<{0}>. Iteration: #{1}, Search: {2} {3} ahead, Week start day: {4}",
                            startDateTime,
                            iteration,
                            incrementationAmount,
                            incrementationAmount > 1
                                ? incrementationTypeName + "s"
                                : incrementationTypeName,
                            weekStartDayName)
                        );
                }
            };

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (parallel)
                Parallel.For(0, numberOfIterations, testAction);
            else
            {
                for (int iteration = 0; iteration < numberOfIterations; iteration++)
                    testAction(iteration);
            }
            stopwatch.Stop();

            if (parallel)
            {
                Trace.WriteLine(
                    string.Format(
                        "{0} random iterations took {1}ms",
                        numberOfIterations,
                        stopwatch.ElapsedMilliseconds));
            }
            else
            {
                Trace.WriteLine(
                    string.Format(
                        "{0} random iterations took {1}ms (~{2}ms per iteration)",
                        numberOfIterations,
                        stopwatch.ElapsedMilliseconds,
                        stopwatch.ElapsedMilliseconds / numberOfIterations));
            }
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMonday1()
        {
            Assert.AreEqual(
                new DateTime(2012, 1, 2),
                (new DateTime(2012, 1, 1)).NextValid(firstDayOfWeek: DayOfWeek.Monday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMonday2()
        {
            Assert.AreEqual(
                new DateTime(2013, 1, 1),
                (new DateTime(2012, 12, 31)).NextValid(firstDayOfWeek: DayOfWeek.Monday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayLeapYear1()
        {
            Assert.AreEqual(
                new DateTime(2012, 2, 29),
                (new DateTime(2012, 2, 28)).NextValid(firstDayOfWeek: DayOfWeek.Monday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayLeapYear2()
        {
            Assert.AreEqual(
                new DateTime(2012, 3, 1),
                (new DateTime(2012, 2, 29)).NextValid(firstDayOfWeek: DayOfWeek.Monday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayLeapYear3()
        {
            Assert.AreEqual(
                new DateTime(2011, 3, 1),
                (new DateTime(2011, 2, 28)).NextValid(firstDayOfWeek: DayOfWeek.Monday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday1()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2010, 12, 31),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Friday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday2()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 1),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Saturday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday3()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 2),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Sunday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday4()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 3),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Monday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday5()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 4),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Tuesday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday6()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 5),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Wednesday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday7()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 6),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Thursday,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayFriday8()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 7),
                (new DateTime(2010, 12, 31)).NextValid(firstDayOfWeek: DayOfWeek.Monday, weekDay: WeekDay.Friday)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayWeekDay1()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2010, 12, 31),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.WeekDay,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayWeekDay2()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 3),
                (new DateTime(2011, 1, 1)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.WeekDay,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayWeekEnd1()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 1),
                (new DateTime(2010, 12, 31)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Weekend,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayWeekEnd2()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                new DateTime(2011, 1, 1),
                (new DateTime(2011, 1, 1)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Weekend,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeekDayNever()
        {
            // 31/12/2010 is a Friday

            Assert.AreEqual(
                DateTime.MaxValue,
                (new DateTime(2011, 1, 1)).NextValid(
                    firstDayOfWeek: DayOfWeek.Monday,
                    weekDay: WeekDay.Never,
                    inclusive: true)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeek1()
        {
            Assert.AreEqual(
                new DateTime(2011, 1, 3),
                (new DateTime(2010, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Monday, week: Week.First)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeek2()
        {
            Assert.AreEqual(
                new DateTime(2010, 12, 27),
                (new DateTime(2010, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Monday, week: Week.FiftySecond)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeek3()
        {
            Assert.AreEqual(
                new DateTime(2011, 1, 10),
                (new DateTime(2010, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Monday, week: Week.Second)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayMondayWeek4()
        {
            Assert.AreEqual(
                new DateTime(2011, 1, 1),
                (new DateTime(2010, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Monday, week: Week.Zeroth)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayWednesday()
        {
            Assert.AreEqual(
                new DateTime(2012, 1, 4),
                (new DateTime(2012, 1, 3)).NextValid(firstDayOfWeek: DayOfWeek.Wednesday));
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayWednesdayWeek1()
        {
            Assert.AreEqual(
                new DateTime(2021, 1, 1),
                (new DateTime(2020, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Wednesday, week: Week.First)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayWednesdayWeek2()
        {
            Assert.AreEqual(
                new DateTime(2020, 12, 23),
                (new DateTime(2020, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Wednesday, week: Week.FiftySecond)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayWednesdayWeek3()
        {
            Assert.AreEqual(
                new DateTime(2021, 1, 6),
                (new DateTime(2020, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Wednesday, week: Week.Second)
                );
        }

        [TestMethod]
        public void TestNextValidFirstWeekDayWednesdayWeek4()
        {
            Assert.AreEqual(
                new DateTime(2020, 12, 16),
                (new DateTime(2020, 6, 1)).NextValid(firstDayOfWeek: DayOfWeek.Wednesday, week: Week.FiftyFirst)
                );
        }
#endif
    }
}
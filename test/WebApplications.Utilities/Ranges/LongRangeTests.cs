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
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplications.Utilities.Ranges;

namespace WebApplications.Utilities.Test.Ranges
{
    [TestClass]
    public class LongRangeTests : UtilitiesTestBase
    {
        private static long RandomLong(long minimum, long maximum)
        {
            return (long)(minimum + ((ulong)(maximum - minimum) * Random.NextDouble()));
        }

        [TestMethod]
        public void LongRange_ConvertingToString_IsNotBlank()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            long step = RandomLong(1, length / 2);

            LongRange longRange = new LongRange(start, end, step);

            Assert.AreNotEqual("", longRange.ToString(), "String representation of range must not be an empty string");
        }

        [TestMethod]
        public void LongRange_StepSmallerThanLongRange_ParametersMatchThoseGiven()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            long step = RandomLong(1, length / 2);

            LongRange longRange = new LongRange(start, end, step);

            Assert.AreEqual(start, longRange.Start, "Starting point field must match the value supplied");
            Assert.AreEqual(end, longRange.End, "End point field must match the value supplied");
            Assert.AreEqual(step, longRange.Step, "Step amount field must match the value supplied");
        }

        [TestMethod]
        public void LongRange_StepIsLargerThanLongRange_ParametersMatchThoseGiven()
        {
            long length = RandomLong(1, long.MaxValue / 2);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            long step = length + RandomLong(1, long.MaxValue / 3);

            LongRange longRange = new LongRange(start, end, step);

            Assert.AreEqual(start, longRange.Start, "Starting point field must match the value supplied");
            Assert.AreEqual(end, longRange.End, "End point field must match the value supplied");
            Assert.AreEqual(step, longRange.Step, "Step amount field must match the value supplied");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void LongRange_EndBeforeStart_ThrowsArgumentOutOfRangeException()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue + length, long.MaxValue);
            long end = start - length;

            LongRange longRange = new LongRange(start, end);
        }

        [TestMethod]
        public void LongRange_ConstructorWithoutStepParam_StepDefaultsToOne()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;

            LongRange longRange = new LongRange(start, end);

            Assert.AreEqual(start, longRange.Start, "Starting point field must match the value supplied");
            Assert.AreEqual(end, longRange.End, "End point field must match the value supplied");
            Assert.AreEqual(1, longRange.Step, "Step amount must default to one");
        }

        [TestMethod]
        public void LongRange_Iterating_ValuesStayWithinRange()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            // note that the number of steps is limited to 100 or fewer
            long step = length / RandomLong(4, Math.Max(4, Math.Min(length / 2, 100)));

            // nsure the step is at least 1 (as length of less than four causes it to round down to zero)
            if (step < 1) step = 1;

            LongRange longRange = new LongRange(start, end, step);

            foreach (long i in longRange)
            {
                Assert.IsTrue(i >= start, "Value from iterator must by equal or above start parameter");
                Assert.IsTrue(i <= end, "Value from iterator must be equal or below end parameter");
            }
        }

        [TestMethod]
        public void LongRange_LengthDivisibleByStep_IterationCountMatchesCalculated()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            // note that the number of steps is limited to 1000 or fewer
            long step = length / RandomLong(4, Math.Max(4, Math.Min(length / 2, 1000)));

            // In case range length is under 4, ensure the step is at least 1
            if (step < 1) step = 1;

            //ensure that step size is a factor of the length of the range
            start += length % step;

            LongRange longRange = new LongRange(start, end, step);

            // Range endpoint is inclusive, so must take into account this extra iteration
            Assert.AreEqual(
                length / step + 1,
                longRange.Count(),
                "Iteration count should be (end-start)/step + 1 where endpoint is included");
        }

        [TestMethod]
        public void LongRange_LengthNotDivisibleByStep_IterationCountMatchesCalculated()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            // note that the number of steps is limited to 1000 or fewer
            long step = length / RandomLong(4, Math.Max(4, Math.Min(length / 2, 1000)));

            // In case range length is under 4, ensure the step is at least 2
            if (step < 2) step = 2;

            //ensure that step size is not a factor of the length of the range
            if (length % step == 0)
            {
                start += RandomLong(1, step - 1);
                length = end - start;
            }

            LongRange longRange = new LongRange(start, end, step);

            Assert.AreEqual(length / step + 1, longRange.Count(), "Iteration count should be (start-end)/step +1");
        }

        [TestMethod]
        public void LongRange_Iterating_DifferenceBetweenIterationsMatchesStepSize()
        {
            long length = RandomLong(1, long.MaxValue);
            long start = RandomLong(long.MinValue, long.MaxValue - length);
            long end = start + length;
            // note that the number of steps is limited to 100 or fewer
            long step = length / RandomLong(4, Math.Max(4, Math.Min(length / 2, 100)));

            // In case range length is under 4, ensure the step is at least 1
            if (step < 1) step = 1;

            LongRange longRange = new LongRange(start, end, step);

            long? previous = null;
            foreach (long i in longRange)
            {
                if (previous.HasValue)
                    Assert.AreEqual(
                        i - previous,
                        step,
                        "Difference between iteration values should match the step value supplied");
                previous = i;
            }
        }

        [TestMethod]
        public void LongRange_UsingLargestPossibleParameters_IteratesSuccessfully()
        {
            // Step chosen to avoid an unfeasible number of iterations
            LongRange longRange = new LongRange(long.MinValue, long.MaxValue, long.MaxValue / 16);

            bool iterated = false;
            foreach (long i in longRange)
                iterated = true;

            Assert.AreEqual(true, iterated, "When iterating across full range, at least one value should be returned");
        }

        [TestMethod]
        public void LongRange_NumberBelowRange_BindReturnsStart()
        {
            long start = RandomLong(long.MinValue / 2, long.MaxValue / 2);
            long end = RandomLong(start, long.MaxValue / 2);
            long testValue = RandomLong(long.MinValue, start);

            Assert.AreEqual(
                start,
                (new LongRange(start, end)).Bind(testValue),
                "Bind should return the lower bound of the range if the input is below the range");
        }

        [TestMethod]
        public void LongRange_NumberAboveRange_BindReturnsEnd()
        {
            long start = RandomLong(long.MinValue / 2, long.MaxValue / 2);
            long end = RandomLong(start, long.MaxValue / 2);
            long testValue = RandomLong(end, long.MaxValue);

            Assert.AreEqual(
                end,
                (new LongRange(start, end)).Bind(testValue),
                "Bind should return the upper bound of the range if the input is above the range");
        }

        [TestMethod]
        public void LongRange_NumberWithinRange_BindReturnsInput()
        {
            long start = RandomLong(long.MinValue / 2, long.MaxValue / 2);
            long end = RandomLong(start, long.MaxValue / 2);
            long testValue = RandomLong(start, end);

            Assert.AreEqual(
                testValue,
                (new LongRange(start, end)).Bind(testValue),
                "Bind should return the input if it is within the range");
        }
    }
}
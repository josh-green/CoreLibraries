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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WebApplications.Utilities.Annotations;

namespace WebApplications.Utilities.Difference
{
    /// <summary>
    /// All the differences between two collections.
    /// </summary>
    /// <typeparam name="T">The items type.</typeparam>
    [PublicAPI]
    public class Differences<T> : IReadOnlyList<Chunk<T>>
    {
        /// <summary>
        /// The 'A' list.
        /// </summary>
        [NotNull]
        public readonly ReadOnlyWindow<T> A;

        /// <summary>
        /// The 'B' list.
        /// </summary>
        [NotNull]
        public readonly ReadOnlyWindow<T> B;

        /// <summary>
        /// The chunks lazy evaluator.
        /// </summary>
        [NotNull]
        [ItemNotNull]
        private readonly IReadOnlyList<Chunk<T>> _chunks;

        /// <summary>
        /// Initializes a new instance of the <see cref="Differences{T}" /> class.
        /// </summary>
        /// <param name="a">The "A" list.</param>
        /// <param name="offsetA">The offset to the start of a window in the first collection.</param>
        /// <param name="lengthA">The length of the window in the first collection.</param>
        /// <param name="b">The "B" list.</param>
        /// <param name="offsetB">The offset to the start of a window in the second collection.</param>
        /// <param name="lengthB">The length of the window in the second collection.</param>
        /// <param name="comparer">The comparer to compare items in each collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="a" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="b" /> is <see langword="null" />.</exception>
        internal Differences(
            [NotNull] IReadOnlyList<T> a,
            int offsetA,
            int lengthA,
            [NotNull] IReadOnlyList<T> b,
            int offsetB,
            int lengthB,
            IEqualityComparer<T> comparer = null)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
            if (comparer == null) comparer = EqualityComparer<T>.Default;
            A = new ReadOnlyWindow<T>(a, offsetA, lengthA);
            B = new ReadOnlyWindow<T>(b, offsetB, lengthB);

            List<Chunk<T>> chunks = new List<Chunk<T>>();
            bool[] modificationsA = new bool[A.Count];
            bool[] modificationsB = new bool[B.Count];

            int max = A.Count + B.Count + 1;
            int vectorSize = 2 * max + 2;
            // Vector for the (0,0) to (x,y) search
            int[] downVector = new int[vectorSize];
            // Vector for the (u,v) to (N,M) search
            int[] upVector = new int[vectorSize];

            Stack<int, int, int, int> stack = new Stack<int, int, int, int>();
            stack.Push(0, A.Count, 0, B.Count);

            int lowerA;
            int upperA;
            int lowerB;
            int upperB;
            while (stack.TryPop(out lowerA, out upperA, out lowerB, out upperB))
            {
                // Skip equal lines at start of chunk
                while (lowerA < upperA && lowerB < upperB && comparer.Equals(A[lowerA], B[lowerB]))
                {
                    lowerA++;
                    lowerB++;
                }

                // Skip equal lines at end of the chunk.
                while (lowerA < upperA && lowerB < upperB && comparer.Equals(A[upperA - 1], B[upperB - 1]))
                {
                    --upperA;
                    --upperB;
                }

                if (lowerA == upperA)
                {
                    // Everything left in the second collection is not in the first collection.
                    while (lowerB < upperB)
                        modificationsB[lowerB++] = true;
                    continue;
                }
                if (lowerB == upperB)
                {
                    // Everything left in the first collection is not in the second collection.
                    while (lowerA < upperA)
                        modificationsA[lowerA++] = true;
                    continue;
                }

                /*
                 * Find where to cut A & B for shortest middle snake.
                 */

                // The k-line to start the forward search
                int downK = lowerA - lowerB;
                // The k-line to start the reverse search
                int upK = upperA - upperB;

                // Difference in collection sizes at this point.
                int delta = (upperA - lowerA) - (upperB - lowerB);

                // Whether the delta is an odd number.
                bool oddDelta = (delta & 1) != 0;

                // The vectors in the publication accepts negative indexes. the vectors implemented here are 0-based
                // and are access using a specific offset: upOffset for upVector and downOffset for downVector
                int downOffset = max - downK;
                int upOffset = max - upK;

                int maxD = ((upperA - lowerA + upperB - lowerB) / 2) + 1;

                // Initialize vectors
                downVector[downOffset + downK + 1] = lowerA;
                upVector[upOffset + upK - 1] = upperA;

                int d = 0;
                int cutA = -1;
                int cutB = -1;
                do
                {
                    // Extend the forward path.
                    int downStart = downK - d;
                    int downEnd = downK + d;
                    int upStart = upK - d;
                    int upEnd = upK + d;
                    int k = downStart;
                    do
                    {
                        // Find the only or better starting point
                        int x;
                        int offsetK = downOffset + k;
                        if (k == downStart)
                            // Step down
                            x = downVector[offsetK + 1];
                        else
                        { 
                            // Step right
                            x = downVector[offsetK - 1] + 1;
                            if ((k < downEnd) &&
                                (downVector[offsetK + 1] >= x))
                                // Step down
                                x = downVector[offsetK + 1];
                        }
                        int y = x - k;

                        // Find the end of the furthest reaching forward D-path in diagonal k.
                        while ((x < upperA) &&
                               (y < upperB) &&
                               (comparer.Equals(A[x], B[y])))
                        {
                            x++;
                            y++;
                        }
                        downVector[offsetK] = x;

                        // Check for overlap
                        if (oddDelta &&
                            (upStart < k) &&
                            (k < upEnd) &&
                            upVector[upOffset + k] <= downVector[offsetK])
                        {
                            cutA = downVector[offsetK];
                            cutB = downVector[offsetK] - k;
                            break;
                        }
                        k += 2;
                    } while (k <= downEnd);

                    // Once we find the cut points we're done.
                    if (cutA > -1) break;

                    // Extend the reverse path.
                    k = upStart;
                    do
                    {
                        // Find the only or better starting point
                        int x;
                        int offsetK = upOffset + k;
                        if (k == upEnd)
                            // Step up
                            x = upVector[offsetK - 1];
                        else
                        {
                            // Step left
                            x = upVector[offsetK + 1] - 1;
                            if ((k > upStart) &&
                                (upVector[offsetK - 1] < x))
                                // Step up
                                x = upVector[offsetK - 1];
                        }
                        int y = x - k;

                        while ((x > lowerA) &&
                               (y > lowerB) &&
                               (comparer.Equals(A[x - 1], B[y - 1])))
                        {
                            x--;
                            y--;
                        }
                        upVector[offsetK] = x;

                        // Check for overlap
                        if (!oddDelta &&
                            (downStart <= k) &&
                            (k <= downEnd) &&
                            upVector[offsetK] <= downVector[downOffset + k])
                        {
                            cutA = downVector[downOffset + k];
                            cutB = downVector[downOffset + k] - k;
                            break;
                        }
                        k += 2;
                    } while (k <= upEnd);
                    d++;
                } while (d <= maxD && cutA < 0);

                // Now that we have the cut points, add to stack and try again.
                stack.Push(lowerA, cutA, lowerB, cutB);
                stack.Push(cutA, upperA, cutB, upperB);
            }

            /*
             * Build chunks
             */
            int itemA = 0;
            int itemB = 0;
            int startA = 0;
            int startB = 0;
            while (itemA < A.Count || itemB < B.Count)
            {
                // Scan through unchanged items.
                if ((itemA < A.Count) && (!modificationsA[itemA])
                    && (itemB < B.Count) && (!modificationsB[itemB]))
                {
                    itemA++;
                    itemB++;
                    continue;
                }

                // Output any equal data.
                if (itemA > startA)
                {
                    // The length of A & B should be identical!
                    Debug.Assert(itemB - startB == itemA - startA);
                    chunks.Add(
                        new Chunk<T>(
                            A.GetSubset(startA, itemA - startA),
                            B.GetSubset(startB, itemB - startB)));
                }

                startA = itemA;
                startB = itemB;

                while (itemA < A.Count &&
                       (itemB >= B.Count || modificationsA[itemA]))
                    itemA++;

                if (itemA > startA)
                    chunks.Add(new Chunk<T>(A.GetSubset(startA, itemA - startA), null));

                while (itemB < B.Count &&
                       (itemA >= A.Count || modificationsB[itemB]))
                    itemB++;

                if (itemB > startB)
                    chunks.Add(new Chunk<T>(null, B.GetSubset(startB, itemB - startB)));

                startA = itemA;
                startB = itemB;
            }

            // Output any remaining data
            if (itemA > startA)
            {
                // The length of A & B should be identical!
                Debug.Assert(itemB - startB == itemA - startA);
                chunks.Add(
                    new Chunk<T>(
                        A.GetSubset(startA, itemA - startA),
                        B.GetSubset(startB, itemB - startB)));
            }
            
            _chunks = chunks;
        }
        
        /// <inheritdoc />
        [ItemNotNull]
        public IEnumerator<Chunk<T>> GetEnumerator() => _chunks.GetEnumerator();

        /// <inheritdoc />
        [ItemNotNull]
        IEnumerator IEnumerable.GetEnumerator() => _chunks.GetEnumerator();

        /// <inheritdoc />
        [ItemNotNull]
        public int Count => _chunks.Count;

        /// <inheritdoc />
        [NotNull]
        [ItemNotNull]
        // ReSharper disable once AssignNullToNotNullAttribute
        public Chunk<T> this[int index] => _chunks[index];

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the differences.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents the differences.</returns>
        public override string ToString()
        {
            // TODO Cache and make use of FormatBuilder
            StringBuilder builder = new StringBuilder();
            foreach (Chunk<T> chunk in _chunks)
            {
                ReadOnlyWindow<T> window = chunk.B;
                if (!chunk.AreEqual)
                    if (chunk.A == null)
                        builder.Append("+ ");
                    else
                    {
                        builder.Append("- ");
                        window = chunk.A;
                    }
                else
                    builder.Append("  ");
                // ReSharper disable once AssignNullToNotNullAttribute
                builder.Append(string.Join(",", window));
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}
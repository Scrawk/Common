using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace System.Collections.Generic
{
    public static class IListExtension
    {
        /// <summary>
        /// Return the last item in list.
        /// </summary>
        public static T PeekLast<T>(this IList<T> list)
        {
            int count = list.Count;
            return list[count - 1];
        }

        /// <summary>
        /// Shuffle the list into a random order.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            var rnd = new Random();
            Shuffle(list, rnd);
        }

        /// <summary>
        /// Shuffle the list into a random order.
        /// </summary>
        /// <param name="seed">The random generators seed.</param>
        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var rnd = new Random(seed);
            Shuffle(list, rnd);
        }

        /// <summary>
        /// Shuffle the list into a random order.
        /// </summary>
        /// <param name="rnd">The random generator.</param>
        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

        /// <summary>
        /// Get the element at index i and wrap
        /// the index to the lists bounds.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The element at index i.</returns>
        public static T GetCircular<T>(this IList<T> list, int i)
        {
            return list[MathUtil.Wrap(i, list.Count)];
        }

        /// <summary>
        /// Get the element at index i and clamp
        /// the index to the lists bounds.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The element at index i.</returns>
        public static T GetClamped<T>(this IList<T> list, int i)
        {
            if (i < 0) i = 0;
            if (i > list.Count - 1) i = list.Count - 1;
            return list[i];
        }

    }
}

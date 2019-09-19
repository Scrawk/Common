using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace System.Collections.Generic
{
    public static class IListExtension
    {

        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var rnd = new Random(seed);

            int n = list.Count;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

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

        public static T GetCircular<T>(this IList<T> list, int i)
        {
            return list[IMath.Wrap(i, list.Count)];
        }

        public static T GetClamped<T>(this IList<T> list, int i)
        {
            if (i < 0) i = 0;
            if (i > list.Count - 1) i = list.Count - 1;
            return list[i];
        }

    }
}

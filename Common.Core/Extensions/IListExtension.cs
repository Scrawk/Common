using System;
using System.Collections.Generic;
using System.Linq;

using Common.Core.Numerics;

namespace System.Collections.Generic
{
    public static class IListExtension
    {

        public static T PeekLast<T>(this IList<T> list)
        {
            int count = list.Count;
            return list[count - 1];
        }

        public static void Shuffle<T>(this IList<T> list, int seed)
        {
            var rnd = new Random(seed);
            Shuffle(list, rnd);
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

        public static T[] Slice<T>(this IList<T> list, int pos)
        {
            return Slice(list, pos, list.Count);
        }

        public static T[] Slice<T>(this IList<T> list, int pos, int end)
        {
            if (pos < 0)
            {
                pos = list.Count + pos;
                if (pos < 0)
                    pos = 0;
            }

            if (end < 0)
                end = list.Count + end;

            if (end > list.Count)
                end = list.Count;

            int len = end - pos;
            if (len < 0)
                return new T[0];

            var array = new T[len];
            for (int i = 0; i < len; i++)
                array[i] = list[pos + i];

            return array;
        }

    }
}

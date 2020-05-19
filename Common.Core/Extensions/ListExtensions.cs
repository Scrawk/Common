using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static void Add<T>(this List<T> list, T item1, T item2)
        {
            list.Add(item1);
            list.Add(item2);
        }

        public static void Add<T>(this List<T> list, T item1, T item2, T item3)
        {
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
        }

        public static void Add<T>(this List<T> list, T item1, T item2, T item3, T item4)
        {
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
            list.Add(item4);
        }

        public static void AddRange<T>(this List<T> list, int count, T item)
        {
            for (int i = 0; i < count; i++)
                list.Add(item);
        }

        public static T PopLast<T>(this List<T> list)
        {
            int count = list.Count;
            var v = list[count - 1];
            list.RemoveAt(count - 1);
            return v;
        }
    }
}

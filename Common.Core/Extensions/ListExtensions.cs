using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        /// <summary>
        /// Add two items to the list.
        /// </summary>
        public static void Add<T>(this List<T> list, T item1, T item2)
        {
            list.Add(item1);
            list.Add(item2);
        }

        /// <summary>
        /// Add three items to the list.
        /// </summary>
        public static void Add<T>(this List<T> list, T item1, T item2, T item3)
        {
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
        }

        /// <summary>
        /// Add four items to the list.
        /// </summary>
        public static void Add<T>(this List<T> list, T item1, T item2, T item3, T item4)
        {
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
            list.Add(item4);
        }

        /// <summary>
        /// Add the same item to the list a number of times.
        /// </summary>
        public static void AddRange<T>(this List<T> list, int count, T item)
        {
            for (int i = 0; i < count; i++)
                list.Add(item);
        }

        /// <summary>
        /// Remove and return the first element in the list.
        /// </summary>
        public static T PopFirst<T>(this List<T> list)
        {
            var v = list[0];
            list.RemoveAt(0);
            return v;
        }

        /// <summary>
        /// Remove and return the last element in the list.
        /// </summary>
        public static T PopLast<T>(this List<T> list)
        {
            int count = list.Count;
            var v = list[count - 1];
            list.RemoveAt(count - 1);
            return v;
        }

        /// <summary>
        /// Remove the item at index i by moving the
        /// last element to index i and then removing
        /// the last element.
        /// </summary>
        public static void RemoveBySwap<T>(this List<T> list, int i)
        {
            int count = list.Count;

            if (count == 1)
                list.RemoveAt(i);
            else
            {
                list[i] = list[count - 1];
                list.RemoveAt(count - 1);
            }

        }
    }
}

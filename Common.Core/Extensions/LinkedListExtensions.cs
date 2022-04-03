using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Extensions
{
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Add the item to the end of the list.
        /// </summary>
        public static void Add<T>(this LinkedList<T> list, T item)
        {
            list.AddLast(item);
        }

        /// <summary>
        /// Add a range of items to end of list.
        /// </summary>
        public static void AddRange<T>(this LinkedList<T> list, IEnumerable<T> items)
        {
            foreach(var item in items)
                list.AddLast(item);
        }

        /// <summary>
        /// Find the index of the item in the list.
        /// </summary>
        public static int IndexOf<T>(this LinkedList<T> list, T item)
        {
            int index = 0;

            foreach(var value in list)
            {
                if (EqualityComparer<T>.Default.Equals(item, value))
                    return index;

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Remove the item at index i.
        /// </summary>
        public static void RemoveAt<T>(this LinkedList<T> list, int index)
        {
            if (list.Count == 0) return;

            int count = 0;
            for (var node = list.First; node != list.Last.Next; node = node.Next)
            {
                if (count == index)
                {
                    list.Remove(node);
                    return;
                }

                count++;
            }
        }

        /// <summary>
        /// Sort the linked list.
        /// </summary>
        public static void Sort<T>(this LinkedList<T> linkedList)
            where T : IComparable<T>
        {
            if (linkedList == null || linkedList.Count <= 1) return;
            Sort(linkedList.First, linkedList.Last);
        }

        /// <summary>
        /// Sort the linked list.
        /// </summary>
        public static void Sort<T>(this LinkedList<T> linkedList, IComparer<T> comparer)
        {
            if (linkedList == null || linkedList.Count <= 1) return;
            Sort(linkedList.First, linkedList.Last, comparer);
        }

        /// <summary>
        /// Sort the linked list.
        /// </summary>
        private static void Sort<T>(LinkedListNode<T> head, LinkedListNode<T> tail)
            where T : IComparable<T>
        {
            if (head == tail) return;

            void Swap(LinkedListNode<T> a, LinkedListNode<T> b)
            {
                var tmp = a.Value;
                a.Value = b.Value;
                b.Value = tmp;
            }

            var pivot = tail;
            var node = head;
            while (node.Next != pivot)
            {
                if (node.Value.CompareTo(pivot.Value) > 0)
                {
                    Swap(pivot, pivot.Previous);
                    Swap(node, pivot);
                    pivot = pivot.Previous; // move pivot backward
                }
                else node = node.Next; // move node forward
            }
            if (node.Value.CompareTo(pivot.Value) > 0)
            {
                Swap(node, pivot);
                pivot = node;
            }

            // pivot location is found, now sort nodes below and above pivot.
            // if head or tail is equal to pivot we reached boundaries and we should stop recursion.
            if (head != pivot) Sort(head, pivot.Previous);
            if (tail != pivot) Sort(pivot.Next, tail);
        }

        /// <summary>
        /// Sort the linked list.
        /// </summary>
        private static void Sort<T>(LinkedListNode<T> head, LinkedListNode<T> tail, IComparer<T> comparer)
        {
            if (head == tail) return;

            void Swap(LinkedListNode<T> a, LinkedListNode<T> b)
            {
                var tmp = a.Value;
                a.Value = b.Value;
                b.Value = tmp;
            }

            var pivot = tail;
            var node = head;
            while (node.Next != pivot)
            {
                if (comparer.Compare(node.Value, pivot.Value) > 0)
                {
                    Swap(pivot, pivot.Previous);
                    Swap(node, pivot);
                    pivot = pivot.Previous; // move pivot backward
                }
                else node = node.Next; // move node forward
            }
            if (comparer.Compare(node.Value, pivot.Value) > 0)
            {
                Swap(node, pivot);
                pivot = node;
            }

            // pivot location is found, now sort nodes below and above pivot.
            // if head or tail is equal to pivot we reached boundaries and we should stop recursion.
            if (head != pivot) Sort(head, pivot.Previous, comparer);
            if (tail != pivot) Sort(pivot.Next, tail, comparer);
        }
    }
}

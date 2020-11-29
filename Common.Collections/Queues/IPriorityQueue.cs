using System;
using System.Collections.Generic;

namespace Common.Collections.Queues
{

    /// <summary>
    /// Interface for a list of items sorted by their comparable.
    /// See PriorityList, BinaryHeap or BinaryTree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPriorityQueue<T> : IEnumerable<T>
    {

        /// <summary>
        /// Optional comparer to use.
        /// </summary>
        IComparer<T> Comparer { get; set; }

        /// <summary>
        /// The number of items in queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clear the queue.
        /// </summary>
        void Clear();

        /// <summary>
        /// Return the first item in the queue.
        /// </summary>
        /// <returns></returns>
        T Peek();

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns></returns>
        T Pop();

        /// <summary>
        /// Add a list of items to the queue.
        /// </summary>
        /// <param name="items"></param>
        void Add(IEnumerable<T> items);

        /// <summary>
        /// Add a single item to the queue.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Add(T item);

        /// <summary>
        /// Return queue as list.
        /// </summary>
        /// <returns></returns>
        List<T> ToList();

    }
}

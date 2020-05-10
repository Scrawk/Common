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
        where T : IComparable<T>
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
        /// The capacity of the queue.
        /// </summary>
        int Capacity { get; set; }

        /// <summary>
        /// Return the first item in the queue.
        /// </summary>
        /// <returns></returns>
        T Peek();

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
        /// Remove a item from the queue.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Remove(T item);

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns></returns>
        T RemoveFirst();

        /// <summary>
        /// Find the first item after this item in queue.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="succesor"></param>
        /// <returns></returns>
        bool FindSuccesor(T item, out T succesor);

        /// <summary>
        /// Find the first item before this item in queue.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="predecessor"></param>
        /// <returns></returns>
        bool FindPredecessor(T item, out T predecessor);

        /// <summary>
        /// Return queue as list.
        /// </summary>
        /// <returns></returns>
        List<T> ToList();

        /// <summary>
        /// Clear the queue.
        /// </summary>
        void Clear();

    }
}

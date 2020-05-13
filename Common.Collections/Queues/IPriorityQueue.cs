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
        /// 
        /// </summary>
        /// <returns></returns>
        string ToString();

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
        /// Find if the value is in the list.
        /// This utilizes the type T's Comparer 
        /// and will consider items  the same 
        /// order the same object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ContainsValue(T value);

        /// <summary>
        /// Remove a value from the queue.
        /// This utilizes the type T's Comparer 
        /// and will consider items  the same 
        /// order the same object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool RemoveValue(T value);

        /// <summary>
        /// Remove the first item from the queue.
        /// </summary>
        /// <returns></returns>
        T RemoveFirst();

        /// <summary>
        /// Find the first item after this item in queue.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="succesor"></param>
        /// <returns></returns>
        bool FindSuccesor(T value, out T succesor);

        /// <summary>
        /// Find the first item before this item in queue.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="predecessor"></param>
        /// <returns></returns>
        bool FindPredecessor(T value, out T predecessor);

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

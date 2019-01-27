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

        int Count { get; }

        int Capacity { get; set; }

        T Peek();

        void Add(IEnumerable<T> items);

        bool Add(T item);

        bool Remove(T item);

        T RemoveFirst();

        bool Find(T key, out T item);

        bool FindSuccesor(T item, out T succesor);

        bool FindPredecessor(T item, out T predecessor);

        List<T> ToList();

        void Clear();

    }
}

using System;
using System.Collections.Generic;

namespace Common.Collections.Queues
{
    public interface IPriorityQueue<T>
    {

        int Count { get; }

        int Capacity { get; set; }

        T Peek();

        void Add(IEnumerable<T> items);

        void Add(T item);

        bool Remove(T item);

        bool Contains(T item);

        bool FindSuccesor(T item, out T succesor);

        bool FindPredecessor(T item, out T predecessor);

        List<T> ToList();

        void Clear();

    }
}

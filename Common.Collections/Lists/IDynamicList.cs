using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.Lists
{
    public interface IDynamicList<T> : IEnumerable<T>
    {
        int Count { get; }

        int Capacity { get; set; }

        T this[int i] { get; set; }

        void Clear();

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        bool Contains(T item);

        int IndexOf(T item);

        bool Remove(T item);

    }
}

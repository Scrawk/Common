﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.Lists
{
    public interface IDynamicList<T> : IEnumerable<T>
    {
        int Count { get; }

        T ElementAt(int i);

        void Clear();

        void Add(T item);

        void Add(IEnumerable<T> items);

        bool Contains(T item);

        int IndexOf(T item);

        bool Remove(T item);

        void RemoveAt(int i);

    }
}

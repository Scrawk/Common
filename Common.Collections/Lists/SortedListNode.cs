using System;
using System.Collections.Generic;

namespace Common.Collections.Lists
{

    public class SortedListNode<T>
    {
        public T Item { get; internal set; }

        public SortedListNode<T> Next { get; internal set; }

        internal SortedListNode(T item)
        {
            Item = item;
        }
    }
}

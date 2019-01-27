using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Queues
{
    /// <summary>
    /// A binary heap, useful for sorting data and priority queues.
    /// </summary>
    public class BinaryHeap<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {

        private const int DEFAULT_SIZE = 4;

        private T[] m_data = new T[DEFAULT_SIZE];

        private int m_capacity = DEFAULT_SIZE;

        private bool m_sorted;

        /// <summary>
        /// Gets the number of values in the heap. 
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets or sets the capacity of the heap.
        /// </summary>
        public int Capacity
        {
            get { return m_capacity; }
            set
            {
                int previousCapacity = m_capacity;
                m_capacity = Math.Max(value, Count);
                if (m_capacity != previousCapacity)
                {
                    T[] temp = new T[m_capacity];
                    Array.Copy(m_data, temp, Count);
                    m_data = temp;
                }
            }
        }

        /// <summary>
        /// Creates a new binary heap.
        /// </summary>
        public BinaryHeap()
        {

        }

        /// <summary>
        /// Creates a new binary heap with capacity.
        /// </summary>
        public BinaryHeap(int count)
        {
            Capacity = count;
        }

        /// <summary>
        /// Creates a new binary heap from collection.
        /// </summary>
        public BinaryHeap(ICollection<T> data)
        {
            Capacity = data.Count;
            Add(data);
        }

        /// <summary>
        /// Creates a new binary heap from enumerable.
        /// </summary>
        public BinaryHeap(IEnumerable<T> data)
        {
            Add(data);
        }

        /// <summary>
        /// Gets the first value in the heap without removing it.
        /// </summary>
        /// <returns>The lowest value of type T.</returns>
        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("Cannot peek item, heap is empty.");

            return m_data[0];
        }

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            m_data = new T[m_capacity];
        }

        /// <summary>
        /// Add a enumerable to the heap.
        /// </summary>
        /// <param name="data">a enumerable container</param>
        public void Add(IEnumerable<T> data)
        {
            foreach (var item in data)
                Add(item);
        }

        /// <summary>
        /// Adds a item to the heap.
        /// </summary>
        /// <param name="item">The item to add to the heap.</param>
        public bool Add(T item)
        {
            if (Count == Capacity)
                Capacity *= 2;
            
            m_data[Count] = item;
            UpHeap();
            Count++;
            return true;
        }

        /// <summary>
        /// Removes and returns the first item in the heap.
        /// </summary>
        /// <returns>The next value in the heap.</returns>
        public T RemoveFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException("Cannot remove item, heap is empty.");
            
            T v = m_data[0];
            Count--;
            m_data[0] = m_data[Count];
            m_data[Count] = default(T); //Clears the Last Node
            DownHeap();
            return v;
        }

        /// <summary>
        /// Removes an item from the binary heap. This utilizes the type T's Comparer and will not remove duplicates.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>Boolean true if the item was removed.</returns>
        public bool Remove(T item)
        {
            EnsureSort();
            int i = Array.BinarySearch<T>(m_data, 0, Count, item);
            if (i < 0) return false;
            Array.Copy(m_data, i + 1, m_data, i, Count - i - 1);
            Count--;
            m_data[Count] = default(T);
            return true;
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An IEnumerator of type T.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            EnsureSort();
            for (int i = 0; i < Count; i++)
            {
                yield return m_data[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Checks to see if the binary heap contains the specified item.
        /// </summary>
        /// <param name="item">The item to search the binary heap for.</param>
        /// <returns>A boolean, true if binary heap contains item.</returns>
        public bool Contains(T item)
        {
            EnsureSort();
            return Array.BinarySearch<T>(m_data, 0, Count, item) >= 0;
        }

        /// <summary>
        /// Find a item by its key, which is just another object with same value.
        /// </summary>
        /// <param name="item">The item to search the binary heap for.</param>
        /// <returns>A boolean, true if binary heap contains item.</returns>
        public bool Find(T key, out T item)
        {
            item = default(T);
            EnsureSort();
            int i = Array.BinarySearch<T>(m_data, 0, Count, key);
            if (i < 0) return false;

            item = m_data[i];
            return true;
        }

        /// <summary>
        /// Copy the heap to a list.
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            EnsureSort();
            var list = new List<T>(Count);
            for (int i = 0; i < Count; i++)
                list.Add(m_data[i]);

            return list;
        }

        /// <summary>
        /// helper function that performs up-heap bubbling
        /// </summary>
        private void UpHeap()
        {
            m_sorted = false;
            int p = Count;
            T item = m_data[p];
            int par = Parent(p);
            while (par > -1 && item.CompareTo(m_data[par]) < 0)
            {
                m_data[p] = m_data[par]; //Swap nodes
                p = par;
                par = Parent(p);
            }
            m_data[p] = item;
        }

        /// <summary>
        /// helper function that performs down-heap bubbling
        /// </summary>
        private void DownHeap()
        {
            m_sorted = false;
            int n;
            int p = 0;
            T item = m_data[p];
            while (true)
            {
                int ch1 = Child1(p);
                if (ch1 >= Count) break;
                int ch2 = Child2(p);
                if (ch2 >= Count)
                {
                    n = ch1;
                }
                else
                {
                    n = m_data[ch1].CompareTo(m_data[ch2]) < 0 ? ch1 : ch2;
                }
                if (item.CompareTo(m_data[n]) > 0)
                {
                    m_data[p] = m_data[n]; //Swap nodes
                    p = n;
                }
                else
                {
                    break;
                }
            }
            m_data[p] = item;
        }

        private void EnsureSort()
        {
            if (m_sorted) return;
            Array.Sort(m_data, 0, Count);
            m_sorted = true;
        }

        /// <summary>
        /// helper function that calculates the parent of a node
        /// </summary>
        private static int Parent(int index)
        {
            return (index - 1) >> 1;
        }

        /// <summary>
        /// helper function that calculates the first child of a node
        /// </summary>
        private static int Child1(int index)
        {
            return (index << 1) + 1;
        }

        /// <summary>
        /// helper function that calculates the second child of a node
        /// </summary>
        private static int Child2(int index)
        {
            return (index << 1) + 2;
        }

        public bool FindSuccesor(T item, out T succesor)
        {
            throw new NotImplementedException();
        }

        public bool FindPredecessor(T item, out T predecessor)
        {
            throw new NotImplementedException();
        }
    }
}
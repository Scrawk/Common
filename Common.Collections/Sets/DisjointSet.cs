using System;
using System.Collections.Generic;

namespace Common.Collections.Sets
{

    /// <summary>
    /// a disjoint-set data structure is a data
    /// structure that tracks a set of elements
    /// partitioned into a number of disjoint 
    /// (non-overlapping) subsets.
    /// Elements in the set must be integer 
    /// ranging from 0 to Count-1.
    /// </summary>
    public class DisjointSet
    {
        /// <summary>
        /// The parents of the element at each index.
        /// All elements with the same parent belong 
        /// to the same set. A element may have its parent
        /// set to -1 to indicate this element has 
        /// been deleted. If a elements parent is its self
        /// then this is a root element.
        /// </summary>
        private List<int> m_parent;

        private List<int> m_rank;

        public DisjointSet()
        {
            m_parent = new List<int>();
            m_rank = new List<int>();
        }

        public DisjointSet(int size)
        {
            m_parent = new List<int>(size);
            m_rank = new List<int>(size);
            AddRange(size);
        }

        public int Count => m_rank.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DisjointSet: Count={0}]", Count);
        }

        /// <summary>
        /// Clear the set.
        /// </summary>
        public void Clear()
        {
            m_parent.Clear();
            m_rank.Clear();
        }

        /// <summary>
        /// Added a new elements and set there
        /// parent to its self.
        /// </summary>
        public void AddRange(int count)
        {
            for (int i = 0; i < count; i++)
                AddNext();
        }

        /// <summary>
        /// Added a new element and set its
        /// parent to its self.
        /// </summary>
        public int AddNext()
        {
            int parent = Count;
            m_parent.Add(parent);
            m_rank.Add(1);
            return parent;
        }

        /// <summary>
        /// Find the elements parent. 
        /// If the element has been deleted its parent is -1.
        /// If the elements parent is itself it is the root element.
        /// </summary>
        public int FindParent(int id)
        {
            int p = m_parent[id];
            if (p == -1) return -1;

            if (m_parent[id] != m_parent[m_parent[id]])
                m_parent[id] = FindParent(m_parent[id]);

            return m_parent[id];
        }

        /// <summary>
        /// Merge the two sets.
        /// </summary>
        public bool Union(int from, int to)
        {
            int x = FindParent(from);
            int y = FindParent(to);

            if (x == y) return false;

            // make sure rank[xx] is smaller
            if (m_rank[x] > m_rank[y]) { int t = x; x = y; y = t; }

            // if both are equal, the combined tree becomes 1 deeper
            if (m_rank[x] == m_rank[y]) m_rank[y]++;

            m_parent[x] = y;

            return true;
        }

    }


}
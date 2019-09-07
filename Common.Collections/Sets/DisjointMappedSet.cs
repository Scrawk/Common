using System;
using System.Collections.Generic;

namespace Common.Collections.Sets
{
    /// <summary>
    /// a disjoint-set data structure is a data
    /// structure that tracks a set of elements
    /// partitioned into a number of disjoint 
    /// (non-overlapping) subsets.
    /// Elements in the set can be any integer.
    /// </summary>
    public class DisjointMappedSet
    {
        /// <summary>
        /// The parents of the element at each key.
        /// All elements with the same parent belong 
        /// to the same set. A element may have its parent
        /// set to -1 to indicate this element has 
        /// been deleted. If a elements parent is itsself
        /// then this is a root element.
        /// </summary>
        private Dictionary<int, int> m_parent;

        private Dictionary<int, int> m_rank;

        public DisjointMappedSet()
        {
            m_parent = new Dictionary<int, int>();
            m_rank = new Dictionary<int, int>();
        }

        public int Count => m_rank.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DisjointMappedSet: Count={0}]", Count);
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
        /// Does the set contain 
        /// this element.
        /// </summary>
        public bool Contains(int id)
        {
            return m_parent.ContainsKey(id);
        }

        /// <summary>
        /// Added a new element and set what 
        /// the elements parent is.
        /// </summary>
        public void Add(int id, int parent)
        {
            if(m_parent.ContainsKey(id))
                m_parent[id] = parent;
            else
            {
                m_parent.Add(id, parent);
                m_rank.Add(id, 1);
            }
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

            if (p != m_parent[m_parent[id]])
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
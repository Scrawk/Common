using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Collections.Sets
{

    /// <summary>
    /// a disjoint-set data structure is a data
    /// structure that tracks a set of elements
    /// partitioned into a number of disjoint 
    /// (non-overlapping) subsets.
    /// Elements in the set represent a index
    /// in to a 2D grid.
    /// </summary>
    public class DisjointGridSet2
    {

        /// <summary>
        /// The parents of the element at each index.
        /// All elements with the same parent belong 
        /// to the same set. A element may have its parent
        /// set to -1 to indicate this element has 
        /// been deleted. If a elements parent is itsself
        /// then this is a root element.
        /// </summary>
        private Point2i[,] m_parent;

        private int[,] m_rank;

        public DisjointGridSet2(int width, int height)
        {
            Width = width;
            Height = height;
            m_parent = new Point2i[width, height];
            m_rank = new int[width, height];
        }

        /// <summary>
        /// The width of the grid.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the grid.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DisjointGridSet2: Width={0}, Height={1}]", Width, Height);
        }

        /// <summary>
        /// Clear the set.
        /// </summary>
        public void Clear()
        {
            Array.Clear(m_parent, 0, m_parent.Length);
            Array.Clear(m_rank, 0, m_rank.Length);
        }

        /// <summary>
        /// Added a new element and set what 
        /// the elements parent is.
        /// </summary>
        public void Add(Point2i idx, Point2i p)
        {
            Add(idx.x, idx.y, p.x, p.y);
        }

        /// <summary>
        /// Added a new element and set what 
        /// the elements parent is.
        /// </summary>
        public void Add(int x, int y, int px, int py)
        {
            m_parent[x, y] = new Point2i(px, py);
            m_rank[x, y] = 1;
        }

        /// <summary>
        /// Find the elements parent. 
        /// If the element has been deleted its parent is -1.
        /// If the elements parent is itself it is the root element.
        /// </summary>
        public Point2i FindParent(Point2i idx)
        {
            return FindParent(idx.x, idx.y);
        }

        /// <summary>
        /// Find the elements parent. 
        /// If the element has been deleted its parent is -1.
        /// If the elements parent is itself it is the root element.
        /// </summary>
        public Point2i FindParent(int x, int y)
        {
            Point2i p = m_parent[x, y];

            if (p.x == -1 || p.y == -1)
                return new Point2i(-1);

            if (p != m_parent[p.x, p.y])
                m_parent[x, y] = FindParent(p.x, p.y);

            return m_parent[x, y];
        }

        /// <summary>
        /// Merge the two sets.
        /// </summary>
        public bool Union(Point2i from, Point2i to)
        {
            return Union(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// Merge the two sets.
        /// </summary>
        public bool Union(int fx, int fy, int tx, int ty)
        {

            Point2i a = FindParent(fx, fy);
            Point2i b = FindParent(tx, ty);

            if (a.x == b.x && a.y == b.y) return false;

            // make sure rank[xx] is smaller
            if (m_rank[a.x, a.y] > m_rank[b.x, b.y])
            {
                Point2i t = a; a = b; b = t;
            }

            // if both are equal, the combined tree becomes 1 deeper
            if (m_rank[a.x, a.y] == m_rank[b.x, b.y]) m_rank[b.x, b.y]++;

            m_parent[a.x, a.y] = b;

            return true;
        }

    }


}
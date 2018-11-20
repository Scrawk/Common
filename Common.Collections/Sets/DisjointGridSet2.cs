using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Collections.Sets
{

    public class DisjointGridSet2
    {

        private Vector2i[,] m_parent;

        private int[,] m_rank;

        public DisjointGridSet2(int width, int height)
        {
            m_parent = new Vector2i[width, height];
            m_rank = new int[width, height];
        }

        public void Clear()
        {
            Array.Clear(m_parent, 0, m_parent.Length);
            Array.Clear(m_rank, 0, m_rank.Length);
        }

        public void Add(Vector2i i, Vector2i p)
        {
            Add(i.x, i.y, p.x, p.y);
        }

        public void Add(int x, int y, int px, int py)
        {
            m_parent[x, y] = new Vector2i(px, py);
            m_rank[x, y] = 1;
        }

        public Vector2i FindParent(Vector2i i)
        {
            return FindParent(i.x, i.y);
        }

        public Vector2i FindParent(int x, int y)
        {
            Vector2i p = m_parent[x, y];

            if (p != m_parent[p.x, p.y])
                m_parent[x, y] = FindParent(p.x, p.y);

            return m_parent[x, y];
        }

        public bool Union(Vector2i f, Vector2i t)
        {
            return Union(f.x, f.y, t.x, t.y);
        }

        public bool Union(int fx, int fy, int tx, int ty)
        {

            Vector2i a = FindParent(fx, fy);
            Vector2i b = FindParent(tx, ty);

            if (a.x == b.x && a.y == b.y) return false;

            // make sure rank[xx] is smaller
            if (m_rank[a.x, a.y] > m_rank[b.x, b.y])
            {
                Vector2i t = a; a = b; b = t;
            }

            // if both are equal, the combined tree becomes 1 deeper
            if (m_rank[a.x, a.y] == m_rank[b.x, b.y]) m_rank[b.x, b.y]++;

            m_parent[a.x, a.y] = b;

            return true;
        }

    }


}
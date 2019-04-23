using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.Meshing.GridGraphs
{
    /// <summary>
    /// A data structure to store the results 
    /// from a search algorithm on a grid graph.
    /// </summary>
    public class GridSearch
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Count { get { return Order.Count; } }

        public Vector2i Root { get { return Order[0]; } }

        public IList<Vector2i> Order { get; private set; }

        public Vector2i[,] Parent { get; private set; }

        public bool[,] IsVisited { get; private set; }

        public GridSearch(int width, int height)
        {
            Width = width;
            Height = height;

            Order = new List<Vector2i>();
            IsVisited = new bool[width, height];
            Parent = new Vector2i[width, height];

            Clear();
        }

        public override string ToString()
        {
            return string.Format("[GridGraph: Count={0}, Root={1}, Width={2}, Height={3}]",
                Count, (Count > 0) ? Root : new Vector2i(-1), Width, Height);
        }

        public void Clear()
        {
            Order.Clear();
            Array.Clear(IsVisited, 0, IsVisited.Length);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Parent[x, y].x = -1;
                    Parent[x, y].y = -1;
                }
            }

        }

        public bool IsLeaf(int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                Vector2i p = Parent[xi, yi];

                if (p.x == x && p.y == y) return false;
            }

            return true;
        }

        public bool IsRoot(int x, int y)
        {
            Vector2i p = Parent[x, y];
            return p.x == x && p.y == y;
        }

        public void GetPath(Vector2i dest, List<Vector2i> path)
        {
            int x = dest.x;
            int y = dest.y;

            while (Parent[x, y].x != -1)
            {
                path.Add(new Vector2i(x, y));
                if (IsRoot(x, y)) return;

                Vector2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }
        }

        public void GetPath(Vector2i dest, List<Vector3f> path)
        {
            int x = dest.x;
            int y = dest.y;

            while (Parent[x, y].x != -1)
            {
                path.Add(new Vector3f(x, y, 0));
                if (IsRoot(x, y)) return;

                Vector2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }
        }
    }
}

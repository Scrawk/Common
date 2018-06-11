using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{
    public class GridSearch
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Count { get { return Order.Count; } }

        public Vector2i Root { get { return Order[0]; } }

        public IList<Vector2i> Order { get; private set; }

        public Vector2i[,] Parent { get; private set; }

        public GridSearch(int width, int height, bool needsParents = true)
        {
            Width = width;
            Height = height;

            Order = new List<Vector2i>();

            if (needsParents)
                Parent = new Vector2i[width, height];

            Clear();
        }

        public GridSearch(IList<Vector2i> order, Vector2i[,] parent)
        {
            Order = order;
            Parent = parent;
        }

        public void Clear()
        {
            Order.Clear();

            if (Parent != null)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Parent[x, y].x = -1;
                        Parent[x, y].y = -1;
                    }
                }
            }
        }

        public bool IsLeaf(int x, int y)
        {
            if (Parent == null)
                throw new InvalidOperationException("Parents was not requested when search created.");

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
            if (Parent == null)
                throw new InvalidOperationException("Parents was not requested when search created.");

            Vector2i p = Parent[x, y];
            return p.x == x && p.y == y;
        }

        public List<Vector2i> GetPathEdges(int x, int y)
        {
            if (Parent == null)
                throw new InvalidOperationException("Parents was not requested when search created.");

            List<Vector2i> path = new List<Vector2i>();

            while (x != Parent[x, y].x && y != Parent[x, y].y && Parent[x,y].x != -1)
            {
                path.Add(new Vector2i(x, y));
                Vector2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }

            return path;
        }
    }
}

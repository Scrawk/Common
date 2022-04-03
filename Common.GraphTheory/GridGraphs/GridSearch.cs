using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
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

        public Point2i Root { get { return Order[0]; } }

        private List<Point2i> Order { get; set; }

        private Point2i[,] Parent { get;  set; }

        private bool[,] IsVisited { get; set; }

        public GridSearch(int width, int height)
        {
            Width = width;
            Height = height;

            Order = new List<Point2i>();
            IsVisited = new bool[width, height];
            Parent = new Point2i[width, height];

            Clear();
        }

        public override string ToString()
        {
            return string.Format("[GridSearch: Count={0}, Root={1}, Width={2}, Height={3}]",
                Count, (Count > 0) ? Root : new Point2i(-1), Width, Height);
        }

        public void Print()
        {
            Console.WriteLine(ToString());

            Console.WriteLine("IsVisited");
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int Y = (Height - y - 1);
                    Console.Write(IsVisited[x, Y] + " ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Parent");
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int Y = (Height - y - 1);
                    //Console.Write("(" + x + "," + Y + ") ");
                    Console.Write("("+Parent[x, Y] + ") ");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Order");
            for (int x = 0; x < Order.Count; x++)
            {
                if(x != Order.Count - 1)
                    Console.Write("("+Order[x]+"), ");
                else
                    Console.Write("(" + Order[x] + ")");
            }

            Console.WriteLine();

        }

        public Point2i GetParent(Point2i i)
        {
            return Parent[i.x, i.y];
        }

        public Point2i GetParent(int x, int y)
        {
            return Parent[x, y];
        }

        public void SetParent(Point2i i, Point2i p)
        {
            Parent[i.x, i.y] = p;
        }

        public void SetParent(int x, int y, Point2i p)
        {
            Parent[x, y] = p;
        }

        public bool GetIsVisited(Point2i i)
        {
            return IsVisited[i.x, i.y];
        }

        public bool GetIsVisited(int x, int y)
        {
            return IsVisited[x, y];
        }

        public void SetIsVisited(Point2i i, bool v)
        {
            IsVisited[i.x, i.y] = v;
        }

        public void SetIsVisited(int x, int y, bool v)
        {
            IsVisited[x, y] = v;
        }

        public Point2i GetOrder(int i)
        {
            return Order[i];
        }

        public void AddOrder(Point2i p)
        {
            Order.Add(p);
        }

        public void SetOrder(int i, Point2i p)
        {
            Order[i] = p;
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

                Point2i p = Parent[xi, yi];

                if (p.x == x && p.y == y) return false;
            }

            return true;
        }

        public bool IsRoot(int x, int y)
        {
            Point2i p = Parent[x, y];
            return p.x == x && p.y == y;
        }

        public bool HasPath(Point2i dest)
        {
            int x = dest.x;
            int y = dest.y;

            while (Parent[x, y].x != -1)
            {
                if (IsRoot(x, y)) return true;

                Point2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }

            return false;
        }

        public float GetPathCost(List<Point2i> path, GridGraph graph)
        {
            float cost = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                var p1 = path[i];
                var p2 = path[i + 1];   

                float w = graph.GetWeight(p1.x, p1.y, p2.x, p2.y);
                cost += w;
            }

            return cost;
        }

        public List<Point2i> GetPath(Point2i dest)
        {
            var path = new List<Point2i>();
            GetPath(dest, path);
            return path;
        }

        public void GetPath(Point2i dest, List<Point2i> path)
        {
            int x = dest.x;
            int y = dest.y;

            while (Parent[x, y].x != -1)
            {
                path.Add(new Point2i(x, y));
                if (IsRoot(x, y)) return;

                Point2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }
        }

        public void GetPath(Point2i dest, List<Vector3f> path)
        {
            int x = dest.x;
            int y = dest.y;

            while (Parent[x, y].x != -1)
            {
                path.Add(new Vector3f(x, y, 0));
                if (IsRoot(x, y)) return;

                Point2i p = Parent[x, y];
                x = p.x;
                y = p.y;
            }
        }
    }
}

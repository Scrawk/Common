using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    /// <summary>
    /// A data structure to store the results 
    /// from a algorithm on a grid flow graph.
    /// </summary>
    public class FlowGridSearch
    {

        public float MaxFlow { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int QueueCount => Queue.Count;

        private Point3i[,] Parent { get; set; }

        private int[,] IsVisited { get; set; }

        private Queue<Point2i> Queue { get; set; }

        public FlowGridSearch(int width, int height)
        {
            Width = width;
            Height = height;

            IsVisited = new int[width, height];
            Parent = new Point3i[width, height];
            Queue = new Queue<Point2i>(width * height * 8);

            Clear();
        }

        public override string ToString()
        {
            return string.Format("[GridSearch: MaxFlow={0}, Width={1}, Height={2}]",
                MaxFlow, Width, Height);
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
                    Console.Write("(" + Parent[x, Y] + ") ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();

        }

        public Point3i GetParent(Point2i i)
        {
            return Parent[i.x, i.y];
        }

        public Point3i GetParent(int x, int y)
        {
            return Parent[x, y];
        }

        public void SetParent(Point2i i, Point3i p)
        {
            Parent[i.x, i.y] = p;
        }

        public void SetParent(int x, int y, Point3i p)
        {
            Parent[x, y] = p;
        }

        public int GetIsVisited(Point2i i)
        {
            return IsVisited[i.x, i.y];
        }

        public int GetIsVisited(int x, int y)
        {
            return IsVisited[x, y];
        }

        public void SetIsVisited(Point2i i, int v)
        {
            IsVisited[i.x, i.y] = v;
        }

        public void SetIsVisited(int x, int y, int v)
        {
            IsVisited[x, y] = v;
        }

        public void Enqueue(Point2i p)
        {
            Queue.Enqueue(p);
        }

        public Point2i Dequeue()
        {
            return Queue.Dequeue();
        }

        public void ClearQueue()
        {
            Queue.Clear();
        }

        public void ClearIsVisited()
        {
            Array.Clear(IsVisited, 0, IsVisited.Length);
        }

        public void Clear()
        {
            MaxFlow = 0;
            Queue.Clear();
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

    }
}

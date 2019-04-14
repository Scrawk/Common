using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{
    public class GridGraph
    {

        public int VertexCount { get { return Width * Height; } }

        public int EdgeCount { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public byte[,] Edges { get; private set; }

        public GridGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Edges = new byte[width, height];
        }

        public override string ToString()
        {
            return string.Format("[GridGraph: VertexCount={0}, EdgeCount={1}, Width={2}, Height={3}]", 
                VertexCount, EdgeCount, Width, Height);
        }

        public void Clear()
        {
            Array.Clear(Edges, 0, Edges.Length);
        }

        public bool HasDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            return (Edges[x, y] & 1 << i) != 0;
        }

        public bool HasDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            return (Edges[fx, fy] & 1 << i) != 0;
        }

        public bool HasUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return false;
            if (yi < 0 || yi > Height - 1) return false;

            if ((Edges[x, y] & 1 << i) != 0) return true;
            if ((Edges[xi, yi] & 1 << D8.OPPOSITES[i]) != 0) return true;

            return false;
        }

        public bool HasUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            if ((Edges[fx, fy] & 1 << i) != 0) return true;
            if ((Edges[tx, ty] & 1 << D8.OPPOSITES[i]) != 0) return true;

            return false;
        }

        public int Degree(int x, int y)
        {
            int degree = 0;
            int edge = Edges[x, y];

            for (int i = 0; i < 8; i++)
            {
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                if ((edge & 1 << i) != 0) degree++;
            }

            return degree;
        }

        public void AddDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = (byte)(Edges[x, y] | 1 << i);
            EdgeCount++;
        }

        public bool AddDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            Edges[fx, fy] = (byte)(Edges[fx, fy] | 1 << i);
            EdgeCount++;

            return true;
        }

        public void RemoveDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = (byte)(Edges[x, y] & ~(1 << i));
            EdgeCount--;
        }

        public void AddUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = (byte)(Edges[x, y] | 1 << i);
            Edges[xi, yi] = (byte)(Edges[xi, yi] | 1 << D8.OPPOSITES[i]);
            EdgeCount += 2;
        }

        public bool AddUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            Edges[fx, fy] = (byte)(Edges[fx, fy] | 1 << i);
            Edges[tx, ty] = (byte)(Edges[tx, ty] | 1 << D8.OPPOSITES[i]);
            EdgeCount += 2;

            return true;
        }

        public void RemoveUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = (byte)(Edges[x, y] & ~(1 << i));
            Edges[xi, yi] = (byte)(Edges[xi, yi] & ~(1 << D8.OPPOSITES[i]));
            EdgeCount -= 2;
        }

        public void GetAllVertices(List<GridVertex> vertices)
        {

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                   
                    int edge = Edges[x, y];
                    if (edge == 0)
                    {
                        bool hasEdge = false;

                        for (int i = 0; i < 8; i++)
                        {
                            int xi = x + D8.OFFSETS[i, 0];
                            int yi = y + D8.OFFSETS[i, 1];

                            if (xi < 0 || xi > Width - 1) continue;
                            if (yi < 0 || yi > Height - 1) continue;

                            var e = Edges[xi, yi];
                            if ((e & 1 << D8.OPPOSITES[i]) == 0) continue;

                            hasEdge = true;
                            break;
                        }

                        if (hasEdge)
                            vertices.Add(new GridVertex(new Vector2i(x, y)));
                    }
                    else
                    {
                        vertices.Add(new GridVertex(new Vector2i(x, y)));
                    }

                }
            }

        }

        public void GetAllEdges(List<GridEdge> edges, float[,] weights = null)
        {

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int edge = Edges[x, y];
                    if (edge == 0) continue;

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + D8.OFFSETS[i, 0];
                        int yi = y + D8.OFFSETS[i, 1];

                        if (xi < 0 || xi > Width - 1) continue;
                        if (yi < 0 || yi > Height - 1) continue;

                        if ((edge & 1 << i) == 0) continue;

                        var e = new GridEdge(x, y, xi, yi);

                        if (weights != null)
                            e.Weight = weights[x, y] + weights[xi, yi];

                        edges.Add(e);
                    }
                }
            }

        }

        public void GetEdges(int x, int y, List<GridEdge> edges, float[,] weights = null)
        {

            int edge = Edges[x, y];
            if (edge == 0) return;

            for (int i = 0; i < 8; i++)
            {
                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                if ((edge & 1 << i) == 0) continue;

                var e = new GridEdge(x, y, xi, yi);

                if (weights != null)
                    e.Weight = weights[x, y] + weights[xi, yi];

                edges.Add(e);
                edges.Add(new GridEdge(x, y, xi, yi));
            }

        }

    }
}

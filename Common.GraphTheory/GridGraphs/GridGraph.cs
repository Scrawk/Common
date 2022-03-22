using System;
using System.Collections.Generic;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.Core.Bits;

using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.GridGraphs
{
    /// <summary>
    /// A graph were the vertices make up a grid
    /// like the pixels in a image. Each vertex
    /// has a byte flag where the bits represent 
    /// if a edge is present to a neighbouring
    /// vertex.
    /// </summary>
    public partial class GridGraph
    {
        /// <summary>
        /// 
        /// </summary>
        public int VertexCount { get { return Width * Height; } }

        /// <summary>
        /// 
        /// </summary>
        public int EdgeCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[,] Edges { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasWeights => m_weights != null;

        /// <summary>
        /// 
        /// </summary>
        private float[,,] Weights
        {
            get
            {
                if(m_weights == null)
                    m_weights = new  float[Width, Height, 8];

                return m_weights;
            }
        }

        private float[,,] m_weights;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public GridGraph(int width, int height)
        {
            Width = width;
            Height = height;

            Edges = new byte[width, height];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[GridGraph: VertexCount={0}, EdgeCount={1}, Width={2}, Height={3}]", 
                VertexCount, EdgeCount, Width, Height);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Print()
        {
            Console.WriteLine(ToString());

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int Y = (Height - y - 1);
                    int e = Edges[x, Y];
                    Console.Write(e + " ");
                }

                Console.WriteLine();
            }

            var edges = new List<GridEdge>();
            GetAllEdges(edges);

            foreach(var edge in edges)  
                Console.WriteLine(edge.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GridGraph Copy()
        {
            var copy = new GridGraph(Width, Height);

            if (m_weights != null)
                copy.m_weights = new float[Width, Height, 8];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    copy.Edges[x, y] = Edges[x, y];

                    if(HasWeights)
                    {
                        for (int i = 0; i < 8; i++)
                            copy.m_weights[x, y, i] = m_weights[x, y, i];
                    }

                }
            }

            return copy;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_weights = null;
            Array.Clear(Edges, 0, Edges.Length);
        }

        /// <summary>
        /// Iterate over the graph using a lambda.
        /// </summary>
        /// <param name="func"></param>
        public void Iterate(Action<int, int> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    func(x, y);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <param name="w"></param>
        public void SetWeight(int x, int y, int i, float w)
        {
            Weights[x, y, i] = w;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="w"></param>
        public void SetWeight(int fx, int fy, int tx, int ty, float w)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return;
            if (x < -1 || x > 1) return;
            if (y < -1 || y > 1) return;

            int i = D8.DIRECTION[x + 1, y + 1];

            Weights[fx, fy, i] = w;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        public void SetWeight(Point2i u, Point2i v, float w)
        {
            SetWeight(u.x, u.y, v.x, v.y, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public float GetWeight(int x, int y, int i)
        {
            return Weights[x, y, i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public float GetWeight(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return 0;
            if (x < -1 || x > 1) return 0;
            if (y < -1 || y > 1) return 0;

            int i = D8.DIRECTION[x + 1, y + 1];

            return Weights[fx, fy, i];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public float GetWeight(Point2i u, Point2i v)
        {
            return GetWeight(u.x, u.y, v.x, v.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool HasDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            return Bit.IsSet(Edges[x, y], i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool HasDirectedEdge(Point2i from, int to)
        {
            return HasDirectedEdge(from.x, from.y, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public bool HasDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            return Bit.IsSet(Edges[fx, fy], i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool HasDirectedEdge(Point2i from, Point2i to)
        {
            return HasDirectedEdge(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool HasUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return false;
            if (yi < 0 || yi > Height - 1) return false;

            if (Bit.IsSet(Edges[x, y], i)) return true;
            if (Bit.IsSet(Edges[xi, yi], D8.OPPOSITES[i])) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool HasUndirectedEdge(Point2i from, int to)
        {
            return HasUndirectedEdge(from.x, from.y, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public bool HasUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            if (Bit.IsSet(Edges[fx, fy], i)) return true;
            if (Bit.IsSet(Edges[tx, ty], D8.OPPOSITES[i])) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool HasUndirectedEdge(Point2i from, Point2i to)
        {
            return HasUndirectedEdge(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

                if (Bit.IsSet(edge, i)) degree++;
            }

            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <param name="w"></param>
        public void AddDirectedWeightedEdge(int x, int y, int i, float w)
        {
            AddDirectedEdge(x, y, i);
            SetWeight(x, y, i, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="w"></param>
        public void AddDirectedWeightedEdge(Point2i from, int to, float w)
        {
            AddDirectedWeightedEdge(from.x, from.y, to, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <param name="w"></param>
        public void AddUndirectedWeightedEdge(int x, int y, int i, float w)
        {
            AddUndirectedEdge(x, y, i);
            SetWeight(x, y, i, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="w"></param>
        public void AddUndirectedWeightedEdge(Point2i from, int to, float w)
        {
            AddUndirectedWeightedEdge(from.x, from.y, to, w);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = Bit.Set(Edges[x, y], i);

            //Edges[x, y] = (byte)(Edges[x, y] | 1 << i);
            EdgeCount++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void AddDirectedEdge(Point2i from, int to)
        {
            AddDirectedEdge(from.x, from.y, to);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public bool AddDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            Edges[fx, fy] = Bit.Set(Edges[fx, fy], i);

            //Edges[fx, fy] = (byte)(Edges[fx, fy] | 1 << i);
            EdgeCount++;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool AddDirectedEdge(Point2i from, Point2i to)
        {
            return AddDirectedEdge(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = Bit.Clear(Edges[x, y], i);

            //Edges[x, y] = (byte)(Edges[x, y] & ~(1 << i));
            EdgeCount--;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = Bit.Set(Edges[x, y], i);
            Edges[xi, yi] = Bit.Set(Edges[xi, yi], D8.OPPOSITES[i]);

            //Edges[x, y] = (byte)(Edges[x, y] | 1 << i);
            //Edges[xi, yi] = (byte)(Edges[xi, yi] | 1 << D8.OPPOSITES[i]);
            EdgeCount += 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="i"></param>
        public void AddUndirectedEdge(Point2i u, int i)
        {
            AddUndirectedEdge(u.x, u.y, i);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public bool AddUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return false;
            if (x < -1 || x > 1) return false;
            if (y < -1 || y > 1) return false;

            int i = D8.DIRECTION[x + 1, y + 1];

            Edges[fx, fy] = Bit.Set(Edges[fx, fy], i);
            Edges[tx, ty] = Bit.Set(Edges[tx, ty], D8.OPPOSITES[i]);

            //Edges[fx, fy] = (byte)(Edges[fx, fy] | 1 << i);
            //Edges[tx, ty] = (byte)(Edges[tx, ty] | 1 << D8.OPPOSITES[i]);
            EdgeCount += 2;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public bool AddUndirectedEdge(Point2i from, Point2i to)
        {
            return AddUndirectedEdge(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RemoveUndirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = Bit.Clear(Edges[x, y], i);
            Edges[xi, yi] = Bit.Clear(Edges[xi, yi], D8.OPPOSITES[i]);

            EdgeCount -= 2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vertices"></param>
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
                            if (!Bit.IsSet(e, i)) continue;

                            hasEdge = true;
                            break;
                        }

                        if (hasEdge)
                            vertices.Add(new GridVertex(new Point2i(x, y)));
                    }
                    else
                    {
                        vertices.Add(new GridVertex(new Point2i(x, y)));
                    }

                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fx"></param>
        /// <param name="fy"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <returns></returns>
        public GridEdge GetEdge(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return null;
            if (x < -1 || x > 1) return null;
            if (y < -1 || y > 1) return null;

            int i = D8.DIRECTION[x + 1, y + 1];

            if (!Bit.IsSet(Edges[fx, fy], i)) return null;

            var edge = new GridEdge(fx, fy, tx, ty);

            if(HasWeights)
                edge.Weight = GetWeight(fx, fy, tx, ty);

            return edge;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public GridEdge GetEdge(Point2i from, Point2i to)
        {
            return GetEdge(from.x, from.y, to.x, to.y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        public void GetAllEdges(List<GridEdge> edges)
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

                        if(!Bit.IsSet(edge, i)) continue;

                        var e = new GridEdge(x, y, xi, yi);

                        if (HasWeights)
                            e.Weight = GetWeight(x, y, i);

                        edges.Add(e);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="GetWeightFunc"></param>
        public void GetAllEdges(List<GridEdge> edges, Func<Point2i, Point2i, float> GetWeightFunc = null)
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

                        if(GetWeightFunc != null)
                            e.Weight = GetWeightFunc(e.From, e.To);
                        else if (HasWeights)
                            e.Weight = GetWeight(e.From, e.To);

                        edges.Add(e);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="edges"></param>
        /// <param name="GetWeightFunc"></param>
        public void GetEdges(int x, int y, List<GridEdge> edges, Func<Point2i, Point2i, float> GetWeightFunc  = null)
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

                if (GetWeightFunc != null)
                    e.Weight = GetWeightFunc(e.From, e.To);
                else if (HasWeights)
                    e.Weight = GetWeight(e.From, e.To);

                edges.Add(e);
                edges.Add(new GridEdge(x, y, xi, yi));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DirectedGraph ToDirectedGraph()
        {
            var graph = new DirectedGraph(Width * Height);

            var edges = new List<GridEdge>();
            GetAllEdges(edges);

            foreach (var edge in edges)
            {
                int from = edge.From.x + edge.From.y * Width;
                int to = edge.To.x + edge.To.y * Width;
                float weight = edge.Weight;

                var e = new GraphEdge(from, to, weight, edge);
   
                graph.AddEdge(e);
            }

            return graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UndirectedGraph ToUndirectedGraph()
        {
            var graph = new UndirectedGraph(Width * Height);

            var edges = new List<GridEdge>();
            GetAllEdges(edges);

            foreach (var edge in edges)
            {
                int from = edge.From.x + edge.From.y * Width;
                int to = edge.To.x + edge.To.y * Width;
                float weight = edge.Weight;

                var e = new GraphEdge(from, to, weight, edge);

                graph.AddEdge(e);
            }

            return graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="i"></param>
        /// <exception cref="ArgumentException"></exception>
        private void Check(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("i must represent a bit and have a range of 0-7.");

            if (x < 0 || x >= Width)
                throw new ArgumentException("x must be > 0 and < Width.");

            if (y < 0 || y >= Height)
                throw new ArgumentException("y must be > 0 and < Height.");
        }

    }
}

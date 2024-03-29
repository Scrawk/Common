﻿using System;
using System.Collections.Generic;
using System.Text;

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
    /// 
    /// The edge directions are in the folling order.
    /// See Common.Core.Directions.D8 script.
    /// 
    /// LEFT = 0;
    /// LEFT_TOP = 1;
    /// TOP = 2;
    /// RIGHT_TOP = 3;
    /// RIGHT = 4;
    /// RIGHT_BOTTOM = 5;
    /// BOTTOM = 6;
    /// LEFT_BOTTOM = 7;
    /// 
    /// </summary>
    public partial class GridGraph
    {

        /// <summary>
        /// Create a new graph.
        /// </summary>
        /// <param name="width">The graphs size on the x axis.</param>
        /// <param name="height">The graphs size on the y axis.</param>
        /// <param name="isOrthogonal">Is the graph orthogonal.</param>
        public GridGraph(int width, int height, bool isOrthogonal = false)
        {
            Width = width;
            Height = height;

            Edges = new byte[width, height];

            Directions = new List<int>(8);
            SetIsOrthogonal(isOrthogonal);
        }

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
        /// Does the graph use only orthogonal and not diagonal directions.
        /// </summary>
        public bool IsOrthogonal { get; private set; }

        /// <summary>
        /// The graphs vertices edge connections.
        /// A vertex can only connect to any of its 8 neighbours.
        /// A connection is represented by a bit in the edges byte
        /// being set to 1.
        /// </summary>
        protected byte[,] Edges { get; set; }

        /// <summary>
        /// The neigbour directions each pixel is connect to.
        /// If orthogonal there will be 4 neighbours (left, bottom, right and top).
        /// If not orthogonal there will be 8 neighbours which includes the diagonals.
        /// </summary>
        protected List<int> Directions { get; set; }

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
        /// The neigbour directions each pixel is connect to.
        /// If orthogonal there will be 4 neighbours (left, bottom, right and top).
        /// If not orthogonal there will be 8 neighbours which includes the diagonals.
        /// </summary>
        /// <param name="isOrthogonal">Is the grapgh orthogonal</param>
        public void SetIsOrthogonal(bool isOrthogonal)
        {
            if (Directions == null)
                Directions = new List<int>(8);

            IsOrthogonal = isOrthogonal;
            Directions.Clear();

            if (isOrthogonal)
                Directions.AddRange(D8.ORTHOGONAL);
            else
                Directions.AddRange(D8.ALL);
        }

        /// <summary>
        /// Get the edge direction from the from and to indices.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns>The edge direction of -1 if from and to indices are not valid.</returns>
        public static int GetEdgeDirection(int fx, int fy, int tx, int ty)
        {
            int x = tx - fx;
            int y = ty - fy;

            if (x == 0 && y == 0) return -1;
            if (x < -1 || x > 1) return -1;
            if (y < -1 || y > 1) return -1;

            return D8.DIRECTION[x + 1, y + 1];
        }

        /// <summary>
        /// Are the x and y indices within the bounds of the graph.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool InBounds(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;

            return true;
        }

        /// <summary>
        /// Prints each vertices edge connections.
        /// </summary>
        public virtual void Print()
        {
            var builder = new StringBuilder();
            Print(builder);
            Console.WriteLine(builder.ToString());
        }

        /// <summary>
        /// Prints each vertices edge connections.
        /// </summary>
        public virtual void Print(StringBuilder builder)
        {
            builder.AppendLine(ToString());

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int Y = (Height - y - 1);
                    int e = Edges[x, Y];
                    builder.Append(e + " ");
                }

                builder.AppendLine();
            }

        }

        /// <summary>
        /// Clears the graph.
        /// All vertices connections and weights are removed.
        /// </summary>
        public virtual void Clear()
        {;
            Array.Clear(Edges, 0, Edges.Length);
        }

        /// <summary>
        /// Iterate over the graphs vertices using a lambda.
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
        /// Iterate over the graphs vertices edge connection using a lambda.
        /// </summary>
        /// <param name="func"></param>
        public void Iterate(Action<int, int, int> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int j = 0; j < Directions.Count; j++)
                    {
                        int i = Directions[j];
                        func(x, y, i);
                    }
                }
            }
        }

        /// <summary>
        /// Is there a edge from the vertex at x,y to the 
        /// neighbour vertex in direction i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool HasDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            return Bit.IsSet(Edges[x, y], i);
        }

        /// <summary>
        /// Is the there a edge from the the vertex at fx,fy
        /// to a neighbour vertex at tx,ty.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns></returns>
        public bool HasDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return false;

            return Bit.IsSet(Edges[fx, fy], i);
        }

        /// <summary>
        /// Is there a edge from the vertex at x,y to the 
        /// neighbour vertex in direction i is either direction. 
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
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
        /// Is the there a edge from the the vertex at fx,fy
        /// to a neighbour vertex at tx,ty in either direction.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns></returns>
        public bool HasUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return false;

            if (Bit.IsSet(Edges[fx, fy], i)) return true;
            if (Bit.IsSet(Edges[tx, ty], D8.OPPOSITES[i])) return true;

            return false;
        }

        /// <summary>
        /// Get the number of edges the vertex at x,y has.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <returns></returns>
        public int Degree(int x, int y)
        {
            int degree = 0;
            int edge = Edges[x, y];

            for (int j = 0; j < Directions.Count; j++)
            {
                int i = Directions[j];

                int xi = x + D8.OFFSETS[i, 0];
                int yi = y + D8.OFFSETS[i, 1];

                if (xi < 0 || xi > Width - 1) continue;
                if (yi < 0 || yi > Height - 1) continue;

                if (Bit.IsSet(edge, i)) degree++;
            }

            return degree;
        }

        /// <summary>
        /// Add a edge from the vertex at x,y to the vertices
        /// neighbour i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
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
            EdgeCount++;
        }

        /// <summary>
        /// Add a edge from the vertex at x,y to the vertices
        /// neighbour tx,ty.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns></returns>
        public bool AddDirectedEdge(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return false;

            Edges[fx, fy] = Bit.Set(Edges[fx, fy], i);
            EdgeCount++;

            return true;
        }

        /// <summary>
        /// Remove the edge from the vertex at x,y to the 
        /// neighour vertex at i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <exception cref="ArgumentException">If i less than 0 or greater than 7.</exception>
        public void RemoveDirectedEdge(int x, int y, int i)
        {
            if (i < 0 || i > 7)
                throw new ArgumentException("To must represent a bit and have a range of 0-7.");

            int xi = x + D8.OFFSETS[i, 0];
            int yi = y + D8.OFFSETS[i, 1];

            if (xi < 0 || xi > Width - 1) return;
            if (yi < 0 || yi > Height - 1) return;

            Edges[x, y] = Bit.Clear(Edges[x, y], i);
            EdgeCount--;
        }

        /// <summary>
        /// Add a edge to and from the vertex at x,y to the vertices
        /// neighbour i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <exception cref="ArgumentException">If i less than 0 or greater than 7.</exception>
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
            EdgeCount += 2;
        }

        /// <summary>
        /// Add a edge to and from the vertex at fx,fy to the vertices
        /// neighbour tx,ty.
        /// </summary>
        /// <param name="fx">The x index the edge is from.</param>
        /// <param name="fy">The y index the edge is from.</param>
        /// <param name="tx">The x index the edge goes to.</param>
        /// <param name="ty">The y index the edge goes to.</param>
        /// <returns></returns>
        public bool AddUndirectedEdge(int fx, int fy, int tx, int ty)
        {
            int i = GetEdgeDirection(fx, fy, tx, ty);
            if (i == -1) return false;

            Edges[fx, fy] = Bit.Set(Edges[fx, fy], i);
            Edges[tx, ty] = Bit.Set(Edges[tx, ty], D8.OPPOSITES[i]);
            EdgeCount += 2;

            return true;
        }

        /// <summary>
        /// Remove the edge to and from the vertex at x,y to the vertices
        /// neighbour i.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="i">The bit position in the edge connections byte value.</param>
        /// <exception cref="ArgumentException">If i less than 0 or greater than 7.</exception>
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
        /// For each vertex in the graph create a GridVertex object
        /// and add them to the provided list.
        /// </summary>
        /// <param name="vertices"></param>
        public void GetAllVertices(List<GridVertex> vertices)
        {

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    vertices.Add(new GridVertex(new Point2i(x, y)));
                }
            }

        }

        /// <summary>
        /// Get the vertex at x,y edge connections value.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <returns></returns>
        public byte GetEdges(int x, int y)
        {
            return Edges[x, y];
        }

        /// <summary>
        /// Set the vertex at x,y edge connections value.
        /// </summary>
        /// <param name="x">The vertices x index.</param>
        /// <param name="y">The vertices y index.</param>
        /// <param name="edges"></param>
        public void SetEdges(int x, int y, byte edges)
        {
            Edges[x, y] = edges;
        }

    }
}

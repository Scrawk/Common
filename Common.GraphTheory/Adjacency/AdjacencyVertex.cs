using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{

    public interface IAdjacencyVertex
    {
        int Index { get; set; }

        float Cost { get; set; }
    }

    public class AdjacencyVertex<T> : IAdjacencyVertex
    {

        public int Index { get; set; }

        public float Cost { get; set; }

        public T Data { get; set; }

        public AdjacencyVertex()
        {
            Index = -1;
        }

        public AdjacencyVertex(int index, T data)
        {
            Index = index;
            Data = data;
        }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex: Index={0}, Cost={1}]", Index, Cost);
        }
    }

    public class AdjacencyVertexComparer<VERTEX> : IComparer<VERTEX>
        where VERTEX : class, IAdjacencyVertex, new()
    {

        public static AdjacencyVertexComparer<VERTEX> Instance { get; private set; }

        static AdjacencyVertexComparer()
        {
            Instance = new AdjacencyVertexComparer<VERTEX>();
        }

        public int Compare(VERTEX v0, VERTEX v1)
        {
            return v0.Cost.CompareTo(v1.Cost);
        }
    }

}














using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{

    public interface IAdjacencyVertex : IComparable<IAdjacencyVertex>
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

        public int CompareTo(IAdjacencyVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }

}














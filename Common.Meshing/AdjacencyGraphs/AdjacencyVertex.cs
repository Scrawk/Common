using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{

    /// <summary>
    /// The interface for adjacency vertices.
    /// </summary>
    public interface IAdjacencyVertex : IComparable<IAdjacencyVertex>
    {
        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        int Index { get; set; }

        int Tag { get; set; }

        float Cost { get; set; }
    }

    public abstract class AdjacencyVertex : IAdjacencyVertex
    {

        public AdjacencyVertex()
        {
            Index = -1;
        }

        public AdjacencyVertex(int index)
        {
            Index = index;
        }

        /// <summary>
        /// The index of this vertex in the graph.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The cost of this vertex.
        /// Used in some search algorithms.
        /// ie shortest paths.
        /// </summary>
        public float Cost { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex: Index={0}, Cost={1}]", Index, Cost);
        }

        public abstract void SetPosition(Vector3d pos);

        public abstract Vector3d GetPosition();

        /// <summary>
        /// Used to sort vertices by cost.
        /// </summary>
        public int CompareTo(IAdjacencyVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }
    }

}














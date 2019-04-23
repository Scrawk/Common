using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertex represents a 2D position.
    /// </summary>
    public class AdjacencyGraph2f : AdjacencyGraph<AdjacencyVertex2f, AdjacencyEdge>
    {
        public AdjacencyGraph2f()
        {

        }

        public AdjacencyGraph2f(int size) : base(size)
        {

        }

        public AdjacencyGraph2f(IEnumerable<AdjacencyVertex2f> vertices) : base(vertices)
        {

        }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph2f: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }
    }
}

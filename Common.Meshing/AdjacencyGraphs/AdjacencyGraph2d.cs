using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertex represents a 2D position.
    /// </summary>
    public class AdjacencyGraph2d : AdjacencyGraph<AdjacencyVertex2d, AdjacencyEdge>
    {
        public AdjacencyGraph2d()
        {

        }

        public AdjacencyGraph2d(int size) : base(size)
        {

        }

        public AdjacencyGraph2d(IEnumerable<AdjacencyVertex2d> vertices) : base(vertices)
        {

        }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph2d: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }
    }
}

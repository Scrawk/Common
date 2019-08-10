using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertex represents a 2D position.
    /// </summary>
    public class AdjacencyGraph2i : AdjacencyGraph<AdjacencyVertex2i, AdjacencyEdge>
    {
        public AdjacencyGraph2i()
        {

        }

        public AdjacencyGraph2i(int size) : base(size)
        {

        }

        public AdjacencyGraph2i(IEnumerable<AdjacencyVertex2i> vertices) : base(vertices)
        {

        }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph2i: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }
    }
}

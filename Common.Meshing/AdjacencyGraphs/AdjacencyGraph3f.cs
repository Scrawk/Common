using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertex represents a 3D position.
    /// </summary>
    public class AdjacencyGraph3f : AdjacencyGraph<AdjacencyVertex3f, AdjacencyEdge>
    {
        public AdjacencyGraph3f()
        {

        }

        public AdjacencyGraph3f(int size) : base(size)
        {

        }

        public AdjacencyGraph3f(IEnumerable<AdjacencyVertex3f> vertices) : base(vertices)
        {

        }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph3f: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }
    }
}

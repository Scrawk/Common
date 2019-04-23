using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertex represents a 3D position.
    /// </summary>
    public class AdjacencyGraph3d : AdjacencyGraph<AdjacencyVertex3d, AdjacencyEdge>
    {
        public AdjacencyGraph3d()
        {

        }

        public AdjacencyGraph3d(int size) : base(size)
        {

        }

        public AdjacencyGraph3d(IEnumerable<AdjacencyVertex3d> vertices) : base(vertices)
        {

        }

        public override string ToString()
        {
            return string.Format("[AdjacencyGraph3d: VertexCount={0}, EdgeCount={1}]", VertexCount, EdgeCount);
        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface IEdgeMesh
    {
        int VertexCount { get; }

        int EdgeCount { get; }

        Vector3d GetPosition(int i);

        Vector2i GetEdge(int i);
    }
}

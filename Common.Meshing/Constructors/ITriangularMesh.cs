using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface ITriangularMesh
    {
        int VertexCount { get; }

        int FaceCount { get; }

        Vector3d GetPosition(int i);

        Vector3i GetTriangle(int i);
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface IPolygonalMesh
    {
        int VertexCount { get; }

        int FaceCount { get; }

        Vector3d GetPosition(int i);

        void GetPolygon(int i, List<int> indices);
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.Constructors
{
    public interface IEdgeMeshConstructor<MESH>
    {
        void PushEdgeMesh(int numVertices, int numEdges);

        MESH PopMesh();

        void AddVertex(Vector2d pos);

        void AddVertex(Vector3d pos);

        void AddEdge(int i0, int i1);
    }
}

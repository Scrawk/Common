﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{
    public interface IEdgeMeshConstructor<MESH>
    {
        void PushEdgeMesh(int numVertices, int numEdges);

        MESH PopMesh();

        void AddVertex(Vector2f pos);

        void AddVertex(Vector3f pos);

        void AddEdge(int i0, int i1);
    }
}

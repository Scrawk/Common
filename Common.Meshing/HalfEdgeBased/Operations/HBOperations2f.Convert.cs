﻿using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.FaceBased;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations2f
    {
        /// <summary>
        /// Convert mesh to indexable edge mesh.
        /// </summary>
        public static Mesh2f ToEdgeMesh2f(HBMesh2f mesh)
        {
            var positions = new List<Vector2f>(mesh.Vertices.Count);

            var indices = new List<int>(mesh.Edges.Count);
            mesh.GetEdgeIndices(indices);
            mesh.GetPositions(positions);
            return new Mesh2f(positions, indices);
        }

        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public static Mesh2f ToTriangleMesh2f(HBMesh2f mesh)
        {
            var positions = new List<Vector2f>(mesh.Vertices.Count);

            var indices = new List<int>(mesh.Faces.Count * 3);
            mesh.GetFaceIndices(indices, 3);
            mesh.GetPositions(positions);
            return new Mesh2f(positions, indices);
        }

        /// <summary>
        /// Convert to a triangle face based mesh.
        /// </summary>
        public static FBMesh2f ToFBTriangleMesh2f(HBMesh2f mesh)
        {
            mesh.TagAll();

            var constructor = new FBMeshConstructor2f();
            constructor.PushTriangularMesh(mesh.Vertices.Count, mesh.Faces.Count);

            foreach (var vertex in mesh.Vertices)
                constructor.AddVertex(vertex.Position);

            var vertices = new List<HBVertex2f>(3);
            var neighbours = new List<HBFace>(3);

            foreach (var face in mesh.Faces)
            {
                vertices.Clear();
                face.GetVertices(vertices);
                if (vertices.Count != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(vertices[0].Tag, vertices[1].Tag, vertices[2].Tag);
            }

            foreach (var face in mesh.Faces)
            {
                neighbours.Clear();
                face.GetNeighbours(neighbours, true, true);

                if (neighbours.Count != 3)
                    throw new InvalidOperationException("Face does not have 3 edges.");

                int i0 = neighbours[0] != null ? neighbours[0].Tag : -1;
                int i1 = neighbours[1] != null ? neighbours[1].Tag : -1;
                int i2 = neighbours[2] != null ? neighbours[2].Tag : -1;
                constructor.AddFaceConnection(face.Tag, i0, i1, i2);
            }

            return constructor.PopMesh();
        }
    }
}

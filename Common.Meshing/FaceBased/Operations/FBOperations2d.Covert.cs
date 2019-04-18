using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{
    public static partial class FBOperations2d
    {
        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public static Mesh2d ToTriangleMesh2d(FBMesh2d mesh)
        {
            var positions = new List<Vector2d>(mesh.Vertices.Count);

            var indices = new List<int>(mesh.Faces.Count * 3);
            mesh.GetFaceIndices(indices, 3);
            mesh.GetPositions(positions);
            return new Mesh2d(positions, indices);
        }

        /// <summary>
        /// Convert to a half edge based mesh.
        /// </summary>
        public static HBMesh2d ToHBTriangleMesh2d(FBMesh2d mesh)
        {
            mesh.TagAll();

            var constructor = new HBMeshConstructor2d();
            constructor.PushTriangularMesh(mesh.Vertices.Count, mesh.Faces.Count);

            foreach (var vertex in mesh.Vertices)
                constructor.AddVertex(vertex.Position);

            foreach (var face in mesh.Faces)
            {
                var v = face.Vertices;
                if (v.Length != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(v[0].Tag, v[1].Tag, v[2].Tag);
            }

            foreach (var face in mesh.Faces)
            {
                var n = face.Neighbours;
                if (n.Length != 3)
                    throw new InvalidOperationException("Face does not contain 3 neighbours.");

                int i0 = n[0] != null ? n[0].Tag : -1;
                int i1 = n[1] != null ? n[1].Tag : -1;
                int i2 = n[2] != null ? n[2].Tag : -1;
                constructor.AddFaceConnection(face.Tag, i0, i1, i2);
            }

            return constructor.PopMesh();
        }
    }
}

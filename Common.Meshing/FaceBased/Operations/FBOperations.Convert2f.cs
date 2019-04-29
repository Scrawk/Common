using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{
    public static partial class FBOperations
    {
        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public static Mesh2f ToTriangleMesh2f<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            var positions = new List<Vector3d>(mesh.Vertices.Count);

            var indices = new List<int>(mesh.Faces.Count * 3);
            mesh.GetFaceIndices(indices, 3);
            mesh.GetPositions(positions);

            var m = new Mesh2f();
            m.SetPositions(positions);
            m.SetIndices(indices);

            return m;
        }

        /// <summary>
        /// Convert to a half edge based mesh.
        /// </summary>
        public static HBMesh2f ToHBTriangleMesh2f<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            mesh.TagAll();

            var constructor = new HBMeshConstructor2f();
            constructor.PushTriangularMesh(mesh.Vertices.Count, mesh.Faces.Count);

            foreach (var vertex in mesh.Vertices)
                constructor.AddVertex(vertex.GetPosition());

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

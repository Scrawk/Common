using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector3f as vertices.
    /// </summary>
    public class FBMesh3f : FBMesh<FBVertex3f, FBFace>
    {
        public FBMesh3f() { }

        public FBMesh3f(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh3f: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public Mesh3f ToTriangleMesh3f()
        {
            var positions = new List<Vector3f>(Vertices.Count);

            var indices = new List<int>(Faces.Count * 3);
            GetFaceIndices(indices, 3);
            GetPositions(positions);
            return new Mesh3f(positions, indices);
        }

        /// <summary>
        /// Convert mesh to indexable tetrahedral mesh.
        /// </summary>
        public Mesh3f ToTetrahedralMesh3f()
        {
            var positions = new List<Vector3f>(Vertices.Count);

            var indices = new List<int>(Faces.Count * 4);
            GetFaceIndices(indices, 4);
            GetPositions(positions);
            return new Mesh3f(positions, indices);
        }

        /// <summary>
        /// Convert to a half edge based mesh.
        /// </summary>
        public HBMesh3f ToHBTriangleMesh3f()
        {
            TagAll();

            var constructor = new HBMeshConstructor3f();
            constructor.PushTriangularMesh(Vertices.Count, Faces.Count);

            foreach (var vertex in Vertices)
                constructor.AddVertex(vertex.Position);

            foreach (var face in Faces)
            {
                var v = face.Vertices;
                if (v.Length != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(v[0].Tag, v[1].Tag, v[2].Tag);
            }

            foreach (var face in Faces)
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


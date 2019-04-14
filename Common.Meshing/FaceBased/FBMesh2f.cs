using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector2f as vertices.
    /// </summary>
    public class FBMesh2f : FBMesh<FBVertex2f, FBFace>
    {
        public FBMesh2f() { }

        public FBMesh2f(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh2f: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector2f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public Mesh2f ToTriangleMesh2f()
        {
            var positions = new List<Vector2f>(Vertices.Count);

            var indices = new List<int>(Faces.Count * 3);
            GetFaceIndices(indices, 3);
            GetPositions(positions);
            return new Mesh2f(positions, indices);
        }

        /// <summary>
        /// Convert to a half edge based mesh.
        /// </summary>
        public HBMesh2f ToHBTriangleMesh2f()
        {
            TagAll();

            var constructor = new HBMeshConstructor2f();
            constructor.PushTriangularMesh(Vertices.Count, Faces.Count);

            foreach (var vertex in Vertices)
                constructor.AddVertex(vertex.Position);

            foreach(var face in Faces)
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


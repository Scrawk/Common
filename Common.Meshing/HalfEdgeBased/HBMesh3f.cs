using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.FaceBased;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// HBMesh with Vector3f as vertices.
    /// </summary>
    public class HBMesh3f : HBMesh<HBVertex3f, HBEdge, HBFace>
    {
        public HBMesh3f() { }

        public HBMesh3f(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[HBMesh3f: Vertices={0}, Edges={1}, Faces={2}]",
                Vertices.Count, Edges.Count, Faces.Count);
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
        /// Convert mesh to indexable edge mesh.
        /// </summary>
        public Mesh3f ToEdgeMesh3f()
        {
            var positions = new List<Vector3f>(Vertices.Count);

            var indices = new List<int>(Edges.Count);
            GetEdgeIndices(indices);
            GetPositions(positions);
            return new Mesh3f(positions, indices);
        }

        /// <summary>
        /// Convert mesh to indexable triangle mesh.
        /// </summary>
        public Mesh3f ToMesh3f()
        {
            var positions = new List<Vector3f>(Vertices.Count);

            var indices = new List<int>(Faces.Count * 3);
            GetFaceIndices(indices, 3);
            GetPositions(positions);
            return new Mesh3f(positions, indices);
        }

        /// <summary>
        /// Convert to a triangle face based mesh.
        /// </summary>
        public FBMesh3f ToFBTriangleMesh3f()
        {
            TagAll();

            var constructor = new FBMeshConstructor3f();
            constructor.PushTriangularMesh(Vertices.Count, Faces.Count);

            foreach (var vertex in Vertices)
                constructor.AddVertex(vertex.Position);

            var vertices = new List<HBVertex3f>(3);
            var neighbours = new List<HBFace>(3);

            foreach (var face in Faces)
            {
                vertices.Clear();
                face.GetVertices(vertices);
                if (vertices.Count != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(vertices[0].Tag, vertices[1].Tag, vertices[2].Tag);
            }

            foreach (var face in Faces)
            {
                neighbours.Clear();
                face.GetNeighbours(neighbours, true, true);
                Console.WriteLine(neighbours.Count);

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


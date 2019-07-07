using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {

        public static void ToEdgeMesh<MESH, VERTEX, EDGE, FACE>(IEdgeMeshConstructor<MESH> constructor, HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            mesh.TagAll();
            constructor.PushEdgeMesh(mesh.Vertices.Count, mesh.Edges.Count);

            foreach (var v in mesh.Vertices)
                constructor.AddVertex(v.GetPosition());

            foreach (var edge in mesh.Edges)
                constructor.AddEdge(edge.Tag, edge.Opposite.Tag);
        }

        public static void ToTriangularMesh<MESH, VERTEX, EDGE, FACE>(ITriangleMeshConstructor<MESH> constructor, HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            mesh.TagAll();
            constructor.PushTriangleMesh(mesh.Vertices.Count, mesh.Faces.Count);

            foreach (var vertex in mesh.Vertices)
                constructor.AddVertex(vertex.GetPosition());

            var vertices = new List<HBVertex>(3);
            var neighbours = new List<HBFace>(3);

            foreach (var face in mesh.Faces)
            {
                vertices.Clear();
                face.Edge.GetVertices(vertices);
                if (vertices.Count != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(vertices[0].Tag, vertices[1].Tag, vertices[2].Tag);
            }

            if (constructor.SupportsFaceConnections)
            {
                foreach (var face in mesh.Faces)
                {
                    neighbours.Clear();
                    face.Edge.GetNeighbours(neighbours, true, true);

                    if (neighbours.Count != 3)
                        throw new InvalidOperationException("Face does not have 3 edges.");

                    int i0 = neighbours[0] != null ? neighbours[0].Tag : -1;
                    int i1 = neighbours[1] != null ? neighbours[1].Tag : -1;
                    int i2 = neighbours[2] != null ? neighbours[2].Tag : -1;
                    constructor.AddFaceConnection(face.Tag, i0, i1, i2);
                }
            }
        }
    }
}

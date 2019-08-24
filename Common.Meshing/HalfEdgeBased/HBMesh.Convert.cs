using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public partial class HBMesh<VERTEX>
        where VERTEX : HBVertex, new()
    {

        public void ToEdgeMesh<MESH>(IEdgeMeshConstructor<MESH> constructor)
        {
            TagAll();
            constructor.PushEdgeMesh(Vertices.Count, Edges.Count);

            foreach (var v in Vertices)
                constructor.AddVertex(v.GetPosition());

            foreach (var edge in Edges)
                constructor.AddEdge(edge.Tag, edge.Opposite.Tag);
        }

        public void ToTriangleMesh<MESH>(ITriangleMeshConstructor<MESH> constructor)
        {
            TagAll();
            constructor.PushTriangleMesh(Vertices.Count, Faces.Count);

            foreach (var vertex in Vertices)
                constructor.AddVertex(vertex.GetPosition());

            var vertices = new List<HBVertex>(3);
            var neighbours = new List<HBFace>(3);

            foreach (var face in Faces)
            {
                vertices.Clear();
                face.Edge.GetVertices(vertices);
                if (vertices.Count != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(vertices[0].Tag, vertices[1].Tag, vertices[2].Tag);
            }

            if (constructor.SupportsFaceConnections)
            {
                foreach (var face in Faces)
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

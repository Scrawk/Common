using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static VERTEX PokeFace<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, FACE face)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            int count = face.EdgeCount;
            if (count == 0)
                throw new InvalidOperationException("Face has 0 vertices.");

            var p = FindCentriod(face);
            var v = new VERTEX();
            v.SetPosition(p);
            mesh.Vertices.Add(v as VERTEX);

            var oldEdges = new HBEdge[count];
            var newEdges = new HBEdge[count];

            int index = 0;
            foreach (var edge in face.Edge.EnumerateEdges())
            {
                NewEdge(out EDGE e0, out EDGE e1);
                mesh.Edges.Add(e0 as EDGE);
                mesh.Edges.Add(e1 as EDGE);

                oldEdges[index] = edge;
                newEdges[index] = e0;
                index++;
            }

            for (int i = 0; i < count; i++)
            {
                var edge = oldEdges[i];
                var previous = edge.Previous;
                var e0 = newEdges[i];
                var e1 = e0.Opposite;

                e0.From = v;
                e0.Next = edge;
                edge.Previous = e0;
                e1.From = edge.From;
                e1.Previous = previous;
                previous.Next = e1;

                previous = newEdges[IMath.Wrap(i + 1, count)].Opposite;
                var next = newEdges[IMath.Wrap(i - 1, count)];

                e0.Previous = previous;
                previous.Next = e0;

                e1.Next = next;
                next.Previous = e1;
            }

            newEdges[0].Face = face;
            face.Edge = newEdges[0];

            for (int i = 0; i < count; i++)
            {
                var f = newEdges[i].Face;

                if (f == null)
                {
                    f = new FACE();
                    mesh.Faces.Add(f as FACE);
                }

                foreach (var edge in newEdges[i].EnumerateEdges())
                    edge.Face = f;
            }

            return v;
        }

        private static Vector3d FindCentriod(HBFace face)
        {
            int count = 0;
            Vector3d centroid = Vector3d.Zero;
            foreach(var v in face.Edge.EnumerateVertices())
            {
                centroid += v.GetPosition();
                count++;
            }

            return centroid / count;
        }

    }
}

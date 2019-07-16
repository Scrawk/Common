using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        /// <summary>
        /// Remove a vertex and replace the hole with a new face.
        /// Vertex must be closed ie surrounded by faces.
        /// Removing a vertex on a boundary not supported.
        /// </summary>
        public static FACE RemoveVertex<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, VERTEX vertex, bool remove)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            if (!vertex.IsClosed)
                throw new NotSupportedException("Can only remove closed vertices.");

            var faces = new List<HBFace>();
            var edges = new List<HBEdge>();

            HBEdge sideEdge = null;

            foreach (var edge in vertex.EnumerateEdges())
            {
                var next = edge.Next;
                var previous = edge.Opposite.Previous;

                next.Previous = previous;
                previous.Next = next;

                if (sideEdge == null)
                    sideEdge = next;

                edges.Add(edge);
                edges.Add(edge.Opposite);
                faces.Add(edge.Face);
            }

            var newFace = new FACE();
            newFace.Edge = sideEdge;

            foreach (var edge in sideEdge.EnumerateEdges())
                edge.Face = newFace;

            foreach(var face in faces)
            {
                face.Clear();
                if (remove)
                    mesh.Faces.Remove(face as FACE);
                else
                    face.Tag = -1;
            }

            foreach (var edge in edges)
            {
                edge.Clear();
                if (remove)
                    mesh.Edges.Remove(edge as EDGE);
                else
                    edge.Tag = -1;
            }

            vertex.Clear();

            if (remove)
                mesh.Vertices.Remove(vertex);
            else
                vertex.Tag = -1;

            mesh.Faces.Add(newFace);
            return newFace;
        }
    }
}

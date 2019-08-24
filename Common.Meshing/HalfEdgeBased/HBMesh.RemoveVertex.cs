using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public partial class HBMesh<VERTEX>
        where VERTEX : HBVertex, new()
    {
        /// <summary>
        /// Remove a vertex and replace the hole with a new face.
        /// Vertex must be closed ie surrounded by faces.
        /// Removing a vertex on a boundary not supported.
        /// </summary>
        public HBFace RemoveVertex(VERTEX vertex, bool remove)
        {
            if (!vertex.IsClosed)
                throw new NotSupportedException("Can only remove closed vertices.");

            var faces = new List<HBFace>();
            var edges = new List<HBEdge>();

            HBEdge sideEdge = null;

            foreach (var edge in vertex.EnumerateEdges())
            {
                var vert = edge.To;
                var next = edge.Next;
                var previous = edge.Opposite.Previous;

                next.Previous = previous;
                previous.Next = next;
                next.From = vert;
                vert.Edge = next;

                if (sideEdge == null)
                    sideEdge = next;

                edges.Add(edge);
                edges.Add(edge.Opposite);
                faces.Add(edge.Face);
            }

            var newFace = new HBFace();
            newFace.Edge = sideEdge;

            foreach (var edge in sideEdge.EnumerateEdges())
                edge.Face = newFace;

            foreach(var face in faces)
            {
                face.Clear();
                if (remove)
                    Faces.Remove(face);
                else
                    face.Tag = -1;
            }

            foreach (var edge in edges)
            {
                edge.Clear();
                if (remove)
                    Edges.Remove(edge);
                else
                    edge.Tag = -1;
            }

            vertex.Clear();

            if (remove)
                Vertices.Remove(vertex);
            else
                vertex.Tag = -1;

            Faces.Add(newFace);
            return newFace;
        }
    }
}

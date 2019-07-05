using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// Half edge. Presumes edges are connected in CCW order.
    /// </summary>
    public class HBEdge
    {
        public int Tag;

        /// <summary>
        /// The vertex the edge starts from.
        /// </summary>
        public HBVertex From { get; set; }

        /// <summary>
        /// The vertex edge ends at.
        /// </summary>
        public HBVertex To
        {
            get
            {
                if (Opposite == null) return null;
                return Opposite.From;
            }
        }

        /// <summary>
        /// The face the edge is part of.
        /// </summary>
        public HBFace Face { get; set; }

        /// <summary>
        /// The previous edge.
        /// </summary>
        public HBEdge Previous { get; set; }

        /// <summary>
        /// The next edge.
        /// </summary>
        public HBEdge Next { get; set; }

        /// <summary>
        /// This edges other half.
        /// </summary>
        public HBEdge Opposite { get; set; }

        public HBEdge()
        {

        }

        public HBEdge(HBVertex from, HBFace face, HBEdge previous, HBEdge next, HBEdge opposite)
        {
            Set(from, face, previous, next, opposite);
        }

        public void Set(HBVertex from, HBFace face, HBEdge previous, HBEdge next, HBEdge opposite)
        {
            From = from;
            Face = face;
            Previous = previous;
            Next = next;
            Opposite = opposite;
        }

        /// <summary>
        /// Convert edge to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Edge as string</returns>
        public virtual string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            return string.Format("[HBEdge: Id={0}, From={1}, To={2}, Face={3}, Previous={4}, Next={5}, Opposite={6}]",
                mesh.IndexOf(this), mesh.IndexOf(From), mesh.IndexOf(To), mesh.IndexOf(Face), 
                mesh.IndexOf(Previous), mesh.IndexOf(Next), mesh.IndexOf(Opposite));
        }

        public VERTEX GetFrom<VERTEX>() where VERTEX : HBVertex
        {
            if (From == null) return null;
            VERTEX vert = From as VERTEX;
            if (vert == null)
                throw new InvalidCastException("Vertex is not a " + typeof(VERTEX));

            return vert;
        }

        public VERTEX GetTo<VERTEX>() where VERTEX : HBVertex
        {
            if (To == null) return null;
            VERTEX vert = To as VERTEX;
            if (vert == null)
                throw new InvalidCastException("To is not a " + typeof(VERTEX));

            return vert;
        }

        public FACE GetFace<FACE>() where FACE : HBFace
        {
            if (Face == null) return null;
            FACE face = Face as FACE;
            if (face == null)
                throw new InvalidCastException("Face is not a " + typeof(FACE));

            return face;
        }

        public EDGE GetNext<EDGE>() where EDGE : HBEdge
        {
            if (Next == null) return null;
            EDGE edge = Next as EDGE;
            if (edge == null)
                throw new InvalidCastException("Edge is not a " + typeof(EDGE));

            return edge;
        }

        public EDGE GetPrevious<EDGE>() where EDGE : HBEdge
        {
            if (Previous == null) return null;
            EDGE edge = Previous as EDGE;
            if (edge == null)
                throw new InvalidCastException("Edge is not a " + typeof(EDGE));

            return edge;
        }

        public EDGE GetOpposite<EDGE>() where EDGE : HBEdge
        {
            if (Opposite == null) return null;
            EDGE edge = Opposite as EDGE;
            if (edge == null)
                throw new InvalidCastException("Edge is not a " + typeof(EDGE));

            return edge;
        }

        /// <summary>
        /// The number of edges proceeding this edge, including this edge.
        /// If edge is closed this will be the total number of in face.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                var start = this;
                var e = start;
                int count = 0;

                do
                {
                    count++;
                    if (e.Next == null) return count;
                    e = e.Next;
                }
                while (!ReferenceEquals(start, e));

                return count;
            }
        }

        /// <summary>
        /// Does this edge form a closed face.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                HBEdge current = this;
            
                while (current.Next != null && !ReferenceEquals(this, current.Next))
                    current = current.Next;

                return ReferenceEquals(this, current.Next);
            }
        }

        /// <summary>
        /// Clear edge of all connections.
        /// </summary>
        public virtual void Clear()
        {
            From = null;
            Face = null;
            Next = null;
            Previous = null;
            Opposite = null;
        }

        /// <summary>
        /// Enumerate all edges starting from this edge.
        /// If edge is closed this will enumerate all edges in the face.
        /// </summary>
        /// <param name="forwards">enumerate forward or backwards</param>
        /// <returns></returns>
        public IEnumerable<HBEdge> EnumerateEdges(bool forwards = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) yield break;
                yield return e;
                e = (forwards) ? e.Next : e.Previous;
            }
            while (!ReferenceEquals(start, e));
        }

        /// <summary>
        /// Enumerate all vertices starting from this edges vertex.
        /// If edge is closed this will enumerate all vertices in the face.
        /// </summary>
        /// <param name="forward">enumerate forward or backwards</param>
        /// <returns></returns>
        public IEnumerable<HBVertex> EnumerateVertices(bool forward = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) yield break;
                if (e.From == null) yield break;
                yield return e.From;
                e = (forward) ? e.Next : e.Previous;
            }
            while (!ReferenceEquals(start, e));
        }

        /// <summary>
        /// Add all edges in face to list.
        /// </summary>
        public void GetEdges<EDGE>(List<EDGE> edges, bool forwards = true)
            where EDGE : HBEdge
        {
            foreach (var e in EnumerateEdges(forwards))
                edges.Add(e as EDGE);
        }

        /// <summary>
        /// Add all vertices in face to list.
        /// </summary>
        public void GetVertices<VERTEX>(List<VERTEX> vertices, bool forwards = true)
            where VERTEX : HBVertex
        {
            foreach (var v in EnumerateVertices(forwards))
                vertices.Add(v as VERTEX);
        }

        /// <summary>
        /// Add all neighbours of face to list.
        /// </summary>
        public void GetNeighbours<FACE>(List<FACE> faces, bool forwards = true, bool incudeNull = false)
            where FACE : HBFace
        {
            foreach (var e in EnumerateEdges(forwards))
            {
                if (e.Opposite == null || e.Opposite.Face == null)
                {
                    if (incudeNull) faces.Add(null);
                }
                else
                    faces.Add(e.Opposite.Face as FACE);
            }
        }

        /// <summary>
        /// Calculate the average position of the vertices.
        /// </summary>
        public virtual Vector3d GetCentriod()
        {
            int count = 0;
            Vector3d centroid = Vector3d.Zero;
            foreach (var v in EnumerateVertices())
            {
                centroid += v.GetPosition();
                count++;
            }

            if (count == 0)
                return centroid;
            else
                return centroid / count;
        }

        /// <summary>
        /// Check the edge is valid.
        /// </summary>
        /// <returns>A list of errors</returns>
        public virtual string Check<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, bool quick)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var builder = new StringBuilder();

            if (From == null)
                builder.AppendLine("From is null.");
            else
            {
                if (!quick && mesh.IndexOf(From) == -1)
                    builder.AppendLine("From is not found in mesh.");
            }

            if (Opposite == null)
                builder.AppendLine("Opposite is null.");
            else
            {
                if (Opposite.Opposite != this)
                    builder.AppendLine("Opposite is not opposite to this edge.");

                if (!quick && mesh.IndexOf(Opposite) == -1)
                    builder.AppendLine("Opposite is not found in mesh.");
            }

            if (Next == null)
                builder.AppendLine("Next is null.");
            else
            {
                if (Next.Previous != this)
                    builder.AppendLine("Next is not previous to this edge.");

                if (!quick && mesh.IndexOf(Next) == -1)
                    builder.AppendLine("Next is not found in mesh.");
            }

            if (Previous == null)
                builder.AppendLine("Previous is null.");
            else
            {
                if (Previous.Next != this)
                    builder.AppendLine("Previous is not next to this edge.");

                if (!quick && mesh.IndexOf(Previous) == -1)
                    builder.AppendLine("Previous is not found in mesh.");
            }

            return builder.ToString();
        }

    }
}

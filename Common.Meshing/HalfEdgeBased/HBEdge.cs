using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// Half edge. Presumes edges are connected in CCW order.
    /// </summary>
    public sealed partial class HBEdge
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
        public string ToString<VERTEX>(HBMesh<VERTEX> mesh)
            where VERTEX : HBVertex, new()
        {
            return string.Format("[HBEdge: Id={0}, From={1}, To={2}, Face={3}, Previous={4}, Next={5}, Opposite={6}]",
                mesh.IndexOf(this), mesh.IndexOf(From), mesh.IndexOf(To), mesh.IndexOf(Face), 
                mesh.IndexOf(Previous), mesh.IndexOf(Next), mesh.IndexOf(Opposite));
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
        /// Is this edge on a boundary.
        /// </summary>
        public bool IsBoundary
        {
            get
            {
                if (Face == null) return true;
                if (Opposite == null) return true;
                if (Opposite.Face == null) return true;

                return false;
            }
        }


        /// <summary>
        /// Calculate the average position of the vertices.
        /// </summary>
        public Vector3f FaceCentriod
        {
            get
            {
                int count = 0;
                Vector3f centroid = Vector3f.Zero;
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
        }

        /// <summary>
        /// Compute the face normal. 
        /// </summary>
        public Vector3f FaceNormal
        {
            get
            {
                var p0 = From.GetPosition();
                var p1 = To.GetPosition();
                var p2 = Previous.From.GetPosition();
                return Vector3f.Cross(p1 - p0, p2 - p0).Normalized;
            }
        }

        /// <summary>
        /// Get the length of the edge. 
        /// </summary>
        public float Length
        {
            get
            {
                var p0 = From.GetPosition();
                var p1 = To.GetPosition();
                return Vector3f.Distance(p0, p1);
            }
        }

        /// <summary>
        /// Get the sqr length of the edge. 
        /// </summary>
        public float SqrLength
        {
            get
            {
                var p0 = From.GetPosition();
                var p1 = To.GetPosition();
                return Vector3f.SqrDistance(p0, p1);
            }
        }

        /// <summary>
        /// Clear edge of all connections.
        /// </summary>
        public void Clear()
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
        public void GetEdges(List<HBEdge> edges, bool forwards = true)
        {
            foreach (var e in EnumerateEdges(forwards))
                edges.Add(e);
        }

        /// <summary>
        /// Add all vertices in face to list.
        /// </summary>
        public void GetVertices(List<HBVertex> vertices, bool forwards = true)
        {
            foreach (var v in EnumerateVertices(forwards))
                vertices.Add(v);
        }

        /// <summary>
        /// Add all neighbours of face to list.
        /// </summary>
        public void GetNeighbours(List<HBFace> faces, bool forwards = true, bool incudeNull = false)
        {
            foreach (var e in EnumerateEdges(forwards))
            {
                if (e.Opposite == null || e.Opposite.Face == null)
                {
                    if (incudeNull) faces.Add(null);
                }
                else
                    faces.Add(e.Opposite.Face);
            }
        }

        /// <summary>
        /// Get the position on edge. 
        /// </summary>
        public Vector3f GetPosition(float t)
        {
            var p0 = From.GetPosition();
            var p1 = To.GetPosition();
            return p0 + (p1 - p0) * t;
        }

        /// <summary>
        /// Check the edge is valid.
        /// </summary>
        /// <returns>A list of errors</returns>
        public string Check()
        {
            var builder = new StringBuilder();

            if (From == null)
                builder.AppendLine("From is null.");

            if (Opposite == null)
                builder.AppendLine("Opposite is null.");

            if (Next == null)
                builder.AppendLine("Next is null.");
            else
            {
                if (Next.Previous != this)
                    builder.AppendLine("Next is not previous to this edge.");
            }

            if (Previous == null)
                builder.AppendLine("Previous is null.");
            else
            {
                if (Previous.Next != this)
                    builder.AppendLine("Previous is not next to this edge.");
            }

            return builder.ToString();
        }

    }
}

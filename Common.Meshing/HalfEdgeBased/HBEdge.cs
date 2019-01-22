using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// Half edge. Presumes edges connect CCW.
    /// </summary>
    public class HBEdge
    {
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
        /// The previous edge in CCW order.
        /// </summary>
        public HBEdge Previous { get; set; }

        /// <summary>
        /// The next edge in CCW order.
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
            return string.Format("[HBEdge: Id={0}, From={1}, Face={2}, Previous={3}, Next={4}, Opposite={5}]",
                mesh.IndexOf(this), mesh.IndexOf(From), mesh.IndexOf(Face), 
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
        /// Enumerate all edges starting from this edge.
        /// If edge is closed this will enumerate all edges in the face.
        /// </summary>
        /// <param name="ccw">enumerate counter clockwise or clockwise</param>
        /// <returns></returns>
        public IEnumerable<HBEdge> EnumerateEdges(bool ccw = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) yield break;
                yield return e;
                e = (ccw) ? e.Next : e.Previous;
            }
            while (!ReferenceEquals(start, e));
        }

        /// <summary>
        /// Enumerate all vertices starting from this edges vertex.
        /// If edge is closed this will enumerate all vertices in the face.
        /// </summary>
        /// <param name="ccw">enumerate counter clockwise or clockwise</param>
        /// <returns></returns>
        public IEnumerable<HBVertex> EnumerateVertices(bool ccw = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) yield break;
                if (e.From == null) yield break;
                yield return e.From;
                e = (ccw) ? e.Next : e.Previous;
            }
            while (!ReferenceEquals(start, e));
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

    }
}

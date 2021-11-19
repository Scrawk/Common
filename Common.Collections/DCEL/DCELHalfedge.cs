using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Collections.DCEL
{
    /// <summary>
    /// Half edge. Presumes edges are connected in CCW order.
    /// </summary>
    public partial class DCELHalfedge
    {

        internal DCELHalfedge(int index)
        {
            Index = index;
        }

        /// <summary>
        /// Used for temporary making the edge.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The edges index in the mesh.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// The vertex the edge starts from.
        /// </summary>
        public DCELVertex From { get; internal set; }

        /// <summary>
        /// The vertex edge ends at.
        /// </summary>
        public DCELVertex To
        {
            get
            {
                if (Opposite == null) return null;
                return Opposite.From;
            }
        }

        /// <summary>
        /// The optional edge data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The face the edge is part of.
        /// </summary>
        public DCELFace Face { get; internal set; }

        /// <summary>
        /// The previous edge.
        /// </summary>
        public DCELHalfedge Previous { get; internal set; }

        /// <summary>
        /// The next edge.
        /// </summary>
        public DCELHalfedge Next { get; internal set; }

        /// <summary>
        /// This edges other half.
        /// </summary>
        public DCELHalfedge Opposite { get; internal set; }

        /// <summary>
        /// Set the varibles to make a valid connected edge.
        /// </summary>
        /// <param name="from">The vertex the edge is from.</param>
        /// <param name="face">The edges face.</param>
        /// <param name="previous">The previous edge.</param>
        /// <param name="next">The next edge.</param>
        /// <param name="opposite">The opposite edge.</param>
        internal void Set(DCELVertex from, DCELFace face, DCELHalfedge previous, DCELHalfedge next, DCELHalfedge opposite)
        {
            From = from;
            Face = face;
            Previous = previous;
            Next = next;
            Opposite = opposite;
        }

        /// <summary>
        /// Insert the edge between the previous an next edges.
        /// </summary>
        /// <param name="edge">The edge thats in between.</param>
        /// <param name="previous">The previous edge. Maybe null.</param>
        /// <param name="next">The next edge. Maybe null.</param>
        internal static void InsertBetween(DCELHalfedge edge, DCELHalfedge previous, DCELHalfedge next)
        {
            edge.Previous = previous;
            edge.Next = next;

            if(previous != null)
                previous.Next = edge;

            if(next != null)
                next.Previous = edge;
        }

        internal static void SetFrom(DCELHalfedge edge, DCELVertex vert)
        {
            edge.From = vert;
            if (vert == null) return;
            vert.Edge = edge;
        }

        internal static void SetNext(DCELHalfedge edge, DCELHalfedge next)
        {
            edge.Next = next;
            if (next == null) return;
            next.Previous = edge;
        }

        internal static void SetPrevious(DCELHalfedge edge, DCELHalfedge previous)
        {
            edge.Previous = previous;
            if (previous == null) return;
            previous.Next = edge;
        }

        internal static void SetFace(DCELHalfedge edge, DCELFace face)
        {
            edge.Face = face;
            if (face == null) return;
            face.Edge = edge;
        }

        /// <summary>
        /// Convert edge to string.
        /// </summary>
        /// <returns>Edge as string</returns>
        public override string ToString()
        {
            return string.Format("[DCELEdge: Id={0}, From={1}, To={2}, Previous={3}, Next={4}, Opposite={5}]",
                Index, DCELMesh.IndexOrDefault(From), DCELMesh.IndexOrDefault(To), DCELMesh.IndexOrDefault(Previous),
                DCELMesh.IndexOrDefault(Next), DCELMesh.IndexOrDefault(Opposite));
        }

        /// <summary>
        /// The number of edges proceeding this edge, including this edge.
        /// If edge is closed this will be the total number of edges in face.
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
                DCELHalfedge current = this;
            
                while (current.Next != null && !ReferenceEquals(this, current.Next))
                    current = current.Next;

                return ReferenceEquals(this, current.Next);
            }
        }

        /// <summary>
        /// The first edge in the line if edge is part of a
        /// unconnected cycle or this edge if edge is part of 
        /// a connected cycle.
        /// </summary>
        public DCELHalfedge First
        {
            get
            {
                DCELHalfedge current = this;

                while (current.Previous != null)
                {
                    if (ReferenceEquals(this, current.Previous))
                        return this;
                    else
                        current = current.Previous;
                }

                return current;
            }
        }

        /// <summary>
        /// The last edge in the line if edge is part of a
        /// unconnected cycle or this edge if edge is part of 
        /// a connected cycle.
        /// </summary>
        public DCELHalfedge Last
        {
            get
            {
                DCELHalfedge current = this;

                while (current.Next != null)
                {
                    if (ReferenceEquals(this, current.Next))
                        return this;
                    else
                        current = current.Next;
                }

                return current;
            }
        }

        /// <summary>
        /// The length of the edge.
        /// </summary>
        public double Length => Vector2d.Distance(From.Point, To.Point);

        /// <summary>
        /// The square length of the edge.
        /// </summary>
        public double SqrLength => Vector2d.SqrDistance(From.Point, To.Point);

        /// <summary>
        /// Clear edge of all connections.
        /// </summary>
        internal void Clear()
        {
            Data = null;
            From = null;
            Next = null;
            Previous = null;
            Opposite = null;
            Index = -1;
        }

        /// <summary>
        /// Enumerate all edges in the cycle starting from this edge.
        /// Presumes that the edge forms a closed cycle and will 
        /// throw a exception if the cycle is not closed.
        /// </summary>
        /// <param name="forwards">enumerate forward or backwards</param>
        /// <returns></returns>
        public IEnumerable<DCELHalfedge> EnumerateEdgesInCycle(bool forwards = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) throw new EdgeNotClosedException();
                yield return e;
                e = forwards ? e.Next : e.Previous;
            }
            while (e != start);
        }

        /// <summary>
        /// Enumerate all edges in the line starting from the first or last edge.
        /// Presumes that the edge forms a unclosed cycle but will still work if 
        /// edge forms a closed cycle.
        /// </summary>
        /// <param name="forwards">enumerate forward or backwards</param>
        /// <returns></returns>
        public IEnumerable<DCELHalfedge> EnumerateEdgesInLine(bool forwards = true)
        {
            var start = forwards ? First : Last;
            var e = start;

            do
            {
                if (e == null) yield break;
                yield return e;
                e = forwards ? e.Next : e.Previous;
            }
            while (e != start);
        }

        /// <summary>
        /// Enumerate all edges starting at this edge and stopping at target edge.
        /// </summary>
        /// <param name="target">The edge to stop at.</param>
        /// <returns></returns>
        public IEnumerable<DCELHalfedge> EnumerateEdgesTo(DCELHalfedge target)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) 
                    throw new Exception("Target not found.");

                yield return e;

                if(e == target) 
                    yield break;

                e = e.Next;

                if (e == start)
                    throw new Exception("Target not found.");
            }
            while (true);
        }

        /// <summary>
        /// Enumerate all vertices in the cycle starting from this edge.
        /// Presumes that the edge forms a closed cycle.
        /// </summary>
        /// <param name="forward">enumerate forward or backwards</param>
        /// <returns></returns>
        public IEnumerable<DCELVertex> EnumerateVerticesInCycle(bool forward = true)
        {
            var start = this;
            var e = start;

            do
            {
                if (e == null) throw new EdgeNotClosedException();
                if (e.From == null) throw new InvalidDCELException();
                yield return e.From;
                e = forward ? e.Next : e.Previous;
            }
            while (e != start);
        }

        /// <summary>
        /// Sets the tags of all edges in cycle.
        /// </summary>
        internal void SetFacesInCycle(DCELFace face)
        {
            foreach (var edge in EnumerateEdgesInCycle())
                SetFace(edge, face);
        }

    }
}

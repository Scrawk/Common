using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        /// <summary>
        /// Removes a edges and the two faces either side of it.
        /// </summary>
        /// <param name="mesh">A triangle mesh the edge belongs to.</param>
        /// <param name="edge">The edge to collapse</param>
        public static VERTEX CollapseEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var opp = edge.Opposite;
            if (opp == null)
                throw new NullReferenceException("Edge does not have a opposite edge.");

            //Dont collapse boundary edges
            if (edge.Face == null) return null;
            if (opp.Face == null) return null;

            if (edge.EdgeCount != 3)
                throw new NotSupportedException("Can only collapse triangle edges");

            if (opp.EdgeCount != 3)
                throw new NotSupportedException("Can only collapse triangle edges");

            var v0 = edge.From;
            var v1 = opp.From;
            var v2 = edge.Previous.From;
            var v3 = opp.Previous.From;
            var e0 = edge.Previous;
            var e1 = edge.Next;
            var e2 = opp.Next;
            var e3 = opp.Previous;
            var f0 = edge.Face;
            var f1 = opp.Face;

            //Check for some degenerate cases.
            if (mesh.Vertices.Count <= 3) return null;
            if (mesh.Faces.Count == 1) return null;
            if (v0 == v1) return null;
            if (v2 == v3) return null;
            if (e0.Opposite == e2) return null;
            if (e1.Opposite == e3) return null;

            //Check for non-manifold cases.
            if (e0.Opposite.Previous.From == e1.Opposite.Previous.From) return null;
            if (e2.Opposite.Previous.From == e3.Opposite.Previous.From) return null;

            //move v0 to mid point on edge.
            var newPos = (v0.GetPosition() + v1.GetPosition()) * 0.5;

            //check if any of the triangles flip 
            //if moved to new pos. If so return.
            foreach(var e in v0.EnumerateEdges())
            {
                if (e == edge) continue;

                var p0 = e.From.GetPosition();
                var p1 = e.To.GetPosition();
                var p2 = e.Opposite.Next.To.GetPosition();
                var n0 = Vector3d.Cross(p2 - p0, p1 - p0).Normalized;
                var n1 = Vector3d.Cross(p2 - newPos, p1 - newPos).Normalized;

                if (Vector3d.Dot(n0, n1) < 0) return null;
            }

            foreach (var e in v1.EnumerateEdges())
            {
                if (e == opp) continue;

                var p0 = e.From.GetPosition();
                var p1 = e.To.GetPosition();
                var p2 = e.Opposite.Next.To.GetPosition();
                var n0 = Vector3d.Cross(p2 - p0, p1 - p0).Normalized;
                var n1 = Vector3d.Cross(p2 - newPos, p1 - newPos).Normalized;

                if (Vector3d.Dot(n0, n1) < 0) return null;
            }

            v0.SetPosition(newPos);

            //Make sure all the verts that get kept dont
            //point to a edge that gets removed.
            SetFrom(e0.Opposite, v0);
            SetFrom(e1.Opposite, v2);
            SetFrom(e2.Opposite, v3);

            //v1 is being removed and replaced by v0.
            //All edges that go from v1 now go from v0.
            foreach (var e in v1.EnumerateEdges())
                e.From = v0;

            //For the half edges that are being removed
            //connect their new opposite edges.
            var opp0 = e0.Opposite;
            var opp1 = e1.Opposite;
            var opp2 = e2.Opposite;
            var opp3 = e3.Opposite;

            opp0.Opposite = opp1;
            opp1.Opposite = opp0;
            opp2.Opposite = opp3;
            opp3.Opposite = opp2;

            //Clear and remove unused objects.
            e0.Clear();
            e1.Clear();
            e2.Clear();
            e3.Clear();

            mesh.Vertices.Remove(v1 as VERTEX);
            mesh.Edges.Remove(edge as EDGE);
            mesh.Edges.Remove(opp as EDGE);
            mesh.Edges.Remove(e0 as EDGE);
            mesh.Edges.Remove(e1 as EDGE);
            mesh.Edges.Remove(e2 as EDGE);
            mesh.Edges.Remove(e3 as EDGE);
            mesh.Faces.Remove(f0 as FACE);
            mesh.Faces.Remove(f1 as FACE);

            //return the kept vertex.
            return v0 as VERTEX;
        }
    }
}

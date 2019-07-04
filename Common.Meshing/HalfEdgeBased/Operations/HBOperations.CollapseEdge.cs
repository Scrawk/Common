using System;
using System.Collections.Generic;
using System.Text;

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

            if (edge.EdgeCount != 3)
                throw new NotSupportedException("Can only split triangle edges");

            if (opp.EdgeCount != 3)
                throw new NotSupportedException("Can only split triangle edges");

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
            if (e0.Opposite == e2) return null;
            if (e1.Opposite == e3) return null;

            //move v0 to mid point on edge.
            var mid = (v0.GetPosition() + v1.GetPosition()) * 0.5;
            v0.SetPosition(mid);

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
            e0.Opposite.Opposite = e1.Opposite;
            e1.Opposite.Opposite = e0.Opposite;
            e2.Opposite.Opposite = e3.Opposite;
            e3.Opposite.Opposite = e2.Opposite;

            //Cear and remove unused objects.
            v1.Clear();
            edge.Clear();
            opp.Clear();
            e0.Clear();
            e1.Clear();
            e2.Clear();
            e3.Clear();
            f0.Clear();
            f1.Clear();

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

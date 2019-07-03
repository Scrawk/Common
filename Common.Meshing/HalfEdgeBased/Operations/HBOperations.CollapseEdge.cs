using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static void CollapseEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge)
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
            var e0 = edge.Previous;
            var e1 = edge.Next;
            var e2 = opp.Next;
            var e3 = opp.Previous;
            var f0 = edge.Face;
            var f1 = opp.Face;

            var mid = (v0.GetPosition() + v1.GetPosition()) * 0.5;
            v0.SetPosition(mid);

            SetFrom(e0.Opposite, v0);

            foreach (var e in v1.EnumerateEdges())
                e.From = v0;

            e0.Opposite.Opposite = e1.Opposite;
            e1.Opposite.Opposite = e0.Opposite;
            e2.Opposite.Opposite = e3.Opposite;
            e3.Opposite.Opposite = e2.Opposite;

            mesh.Vertices.Remove(v1 as VERTEX);
            mesh.Edges.Remove(edge as EDGE);
            mesh.Edges.Remove(opp as EDGE);
            mesh.Edges.Remove(e0 as EDGE);
            mesh.Edges.Remove(e1 as EDGE);
            mesh.Edges.Remove(e2 as EDGE);
            mesh.Edges.Remove(e3 as EDGE);
            mesh.Faces.Remove(f0 as FACE);
            mesh.Faces.Remove(f1 as FACE);
        }
    }
}

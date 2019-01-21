using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBMeshOperations
    {

        public static void SplitEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge, float t = 0.5f)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {

            var right0 = edge;
            var left0 = edge.Opposite;

            var from = right0.From;
            var to = left0.From;
            var mid = from.Interpolate(to, t);

            
            var right1 = new EDGE();
            var left1 = new EDGE();
            right1.Opposite = left1;
            left1.Opposite = right1;

            right1.From = mid;
            mid.Edge = right1;
            right1.Previous = right0;
            right1.Next = right0.Next;
            right1.Face = right0.Face;

            left1.From = to;
            to.Edge = left1;
            left1.Previous = left0.Previous;
            left1.Next = left0;
            left1.Face = left0.Face;

            right0.Next = right1;
            left0.From = mid;
            left0.Previous = left1;

            mesh.Edges.Add(right1);
            mesh.Edges.Add(left1);
            mesh.Vertices.Add(mid as VERTEX);
        }

    }
}

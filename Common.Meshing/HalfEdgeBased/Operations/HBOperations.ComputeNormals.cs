using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static void ComputeNormals<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, List<Vector3d> normals)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            foreach (var v in mesh.Vertices)
            {
                var n = Vector3d.Zero;

                foreach (var e in v.EnumerateEdges())
                {
                    var p0 = e.From.GetPosition();
                    var p1 = e.To.GetPosition();
                    var p2 = e.Opposite.Next.To.GetPosition();
                    n = Vector3d.Cross(p2 - p0, p1 - p0);
                }

                normals.Add(n.Normalized);
            }
        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        /// <summary>
        /// Joins two edges where they intersect.
        /// </summary>
        /// <param name="mesh">The mesh containing the edges.</param>
        /// <param name="e0">A edge intersecting e1.</param>
        /// <param name="e1">A edge intersecting e0.</param>
        /// <param name="s">The length along e0 where the intersection occurs.</param>
        /// <param name="t">The length along e1 where the intersection occurs.</param>
        private static void JoinEdges<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE e0, EDGE e1, double s, double t)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            if (e0.Face != null || e1.Face != null)
                throw new NotImplementedException("Edges with faces not implemented.");

            if (e0.From.Dimension != 2 || e1.From.Dimension != 2)
                throw new NotImplementedException("Vertices Dimension != 2 not implemented.");

            //Split both edges at intersection point.
            //Presumes v0 and v1 end up at same position.
            var v0 = HBOperations.SplitEdge(mesh, e0, s);
            var v1 = HBOperations.SplitEdge(mesh, e1, t);

            //Say the horizontal edge is the half edge starting at v0.
            //Say the vertical edge is the half edge starting at v1.
            var horizontal = (e0.From == v0) ? e0 : e0.Opposite;
            var vertical = (e1.From == v1) ? e1 : e1.Opposite;

            //Find where the horizontal and vertical edges
            //are going to in local space.
            var origin = v0.GetPosition();
            var hp = horizontal.To.GetPosition() - origin;
            var vp = vertical.To.GetPosition() - origin;

            //Say horizontal edges is going left to right
            //then we want to check if the vertical edge
            //goes bottom to top. If not take the previous
            //opposite which will be in the opposite direction.
            var dp = Vector2d.Dot(vp.xy.PerpendicularCCW, hp.xy);
            if (dp > 0)
                vertical = vertical.Previous.Opposite;

            //If vertical is going bottom to top then say...
            //left1 is the half edge on left side above horizontal edge.
            //left0 is the half edge on left side below horizontal edge.
            //right1 is the half edge on right side above horizontal edge.
            //right0 is the half edge on right side below horizontal edge.
            var left1 = vertical;
            var left0 = left1.Previous;
            var right1 = left1.Opposite;
            var right0 = right1.Next;

            //If horizontal is going left to right then say...
            //top1 is the half edge on top side right of vertical edge.
            //top0 is the half edge on top side left of vertical edge.
            //bottom1 is the half edge on bottom side right of vertical edge.
            //bottom0 is the half edge on bottom side left of vertical edge.
            var top1 = horizontal;
            var top0 = top1.Previous;
            var bottom1 = top1.Opposite;
            var bottom0 = bottom1.Next;

            //Make the new connections. Use v0 and discard v1.
            left1.Previous = top0;
            top0.Next = left1;

            right1.Next = top1;
            top1.Previous = right1;

            left0.Next = bottom0;
            bottom0.Previous = left0;

            right0.Previous = bottom1;
            bottom1.Next = right0;

            left1.From = v0;
            right0.From = v0;

            //Should not be in use by any edge at this point;
            v1.Edge = null;
            mesh.Vertices.Remove(v1);
        }

    }
}

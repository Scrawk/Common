using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {

        /// <summary>
        /// Splits a edge. Creates a new vertex at split 
        /// position on edge and connects newly created edges.
        /// </summary>
        /// <param name="mesh">parent mesh</param>
        /// <param name="edge">the edge to split</param>
        /// <param name="t">the point to split at</param> 
        /// <returns>The new vertex added at the split position</returns>
        public static VERTEX SplitEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge, double t = 0.5)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            //Say we are looking at the edge going top to bottom.
            //The half edge on right goes from bottom to top.
            //The half edge on the left goes top to bottom.
            var right0 = edge;
            var left0 = edge.Opposite;
            var leftPrevious = left0.Previous;
            var rightNext = right0.Next;

            //Create a new vertex at t dist starting at from.
            var from = right0.From;
            var to = left0.From;
            var pos = Vector3d.Lerp(from.GetPosition(), to.GetPosition(), t);
            var mid = new VERTEX();
            mid.SetPosition(pos);

            //Create a new half edge.
            EDGE right1, left1;
            NewEdge(out right1, out left1);

            //right1 starts at the new vertex above right0.
            //Which means right1 goes between right0 and righ0's next edge.
            SetFrom(right1, mid);
            InsertBetween(right1, right0, rightNext);
            right1.Face = right0.Face;

            //left1 starts at left0 from vertex.
            //Which means left1 goes between left0 and left0's previous edge.
            SetFrom(left1, to);
            InsertBetween(left1, leftPrevious, left0);
            left1.Face = left0.Face;

            //left0 new starts at new vertex.
            left0.From = mid;

            VERTEX v = mid as VERTEX;

            mesh.Edges.Add(right1);
            mesh.Edges.Add(left1);
            mesh.Vertices.Add(v);

            //return new vertex. 
            //The new edge starts from this 
            //vertex and the old edge goes to it.
            return v;
        }

    }
}

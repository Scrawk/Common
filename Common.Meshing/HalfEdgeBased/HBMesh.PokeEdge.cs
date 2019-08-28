using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    public partial class HBMesh<VERTEX>
        where VERTEX : HBVertex, new()
    {

        /// <summary>
        /// Creates a new vertex at t position
        /// on edge and connects newly created edges.
        /// </summary>
        /// <param name="mesh">parent mesh</param>
        /// <param name="edge">the edge to split</param>
        /// <param name="t">the point to split at</param> 
        /// <returns>The new vertex added at the position</returns>
        public VERTEX PokeEdge(HBEdge edge, float t = 0.5f)
        {
            if (edge.Opposite == null)
                throw new NullReferenceException("Edge does not have a opposite edge.");

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
            var pos = Vector3f.Lerp(from.GetPosition(), to.GetPosition(), t);
            var mid = new VERTEX();
            mid.SetPosition(pos);

            //Create a new half edge.
            HBEdge right1, left1;
            HBEdge.NewEdge(out right1, out left1);

            //right1 starts at the new vertex above right0.
            //Which means right1 goes between right0 and righ0's next edge.
            HBEdge.SetFrom(right1, mid);
            HBEdge.InsertBetween(right1, right0, rightNext);
            right1.Face = right0.Face;

            //left1 starts at left0 from vertex.
            //Which means left1 goes between left0 and left0's previous edge.
            HBEdge.SetFrom(left1, to);
            HBEdge.InsertBetween(left1, leftPrevious, left0);
            left1.Face = left0.Face;

            //left0 new starts at new vertex.
            left0.From = mid;

            VERTEX v = mid as VERTEX;

            Edges.Add(right1);
            Edges.Add(left1);
            Vertices.Add(v);

            //return new vertex. 
            //The new edge starts from this 
            //vertex and the old edge goes to it.
            return v;
        }

    }
}

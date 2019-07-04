using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        /// <summary>
        /// Takes a edge that connects 2 triangle faces making a quad
        /// and flips the edges to point to the other 2 vertices.
        /// </summary>
        /// <param name="mesh">Mesh that contains the edge.</param>
        /// <param name="edge">The edge to flip.</param>
        public static void FlipEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var opp = edge.Opposite;
            if (opp == null)
                throw new NullReferenceException("Edge does not have a opposite edge.");

            //Dont flip boundary edges
            if (edge.Face == null) return;
            if (opp.Face == null) return;

            if (edge.EdgeCount != 3)
                throw new NotSupportedException("Can only flip triangle edges");

            if (opp.EdgeCount != 3)
                throw new NotSupportedException("Can only flip triangle edges");

            //If both faces are triangles then the combined faces is a quad.
            //Get a reference to each inner edge in the quad.
            var edges = new HBEdge[4];
            edges[0] = edge.Next;
            edges[1] = edge.Previous;
            edges[2] = opp.Next;
            edges[3] = opp.Previous;

            //Make the connects were the edge is moved from.
            edges[0].Previous = edges[3];
            edges[3].Next = edges[0];
            edges[2].Previous = edges[1];
            edges[1].Next = edges[2];

            edge.Previous = edges[0];
            edges[0].Next = edge;
            edge.Next = edges[3];
            edges[3].Previous = edge;

            //Make the connects were the edge is moved to.
            opp.Previous = edges[2];
            edges[2].Next = opp;
            opp.Next = edges[1];
            edges[1].Previous = opp;

            //Connect the faces. When edge moved 2 of these 
            //edges will be pointing to the wrong face.
            edges[0].Face = edge.Face;
            edges[1].Face = opp.Face;
            edges[2].Face = opp.Face;
            edges[3].Face = edge.Face;

            //Make the faces point to the flipped edge incase they
            //were pointing to a edge that has changed its face.
            edge.Face.Edge = edge;
            opp.Face.Edge = opp;

            //Make vertices point to a edge we know is correct.
            //If they were pointing to the flipped edge they
            //will no longer be correct.
            edges[0].From.Edge = edges[0];
            edges[1].From.Edge = edges[1];
            edges[2].From.Edge = edges[2];
            edges[3].From.Edge = edges[3];

            //Make flipped edge point to the vertices were it was moved to.
            edge.From = edges[1].From;
            opp.From = edges[3].From;
        }
    }
}

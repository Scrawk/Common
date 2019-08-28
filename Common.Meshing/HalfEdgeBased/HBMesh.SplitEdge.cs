using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public partial class HBMesh<VERTEX>
        where VERTEX : HBVertex, new()
    {
        /// <summary>
        /// Splits a edge at its mid point and adds 
        /// the new edges to keep all faces as triangles.
        /// </summary>
        /// <param name="mesh">A triangle mesh the edge belongs to.</param>
        /// <param name="edge">The edge to splits.</param>
        public VERTEX SplitEdge(HBEdge edge)
        {
            //Dont collapse boundary edges
            if (edge.IsBoundary) return null;

            var opp = edge.Opposite;

            if (edge.EdgeCount != 3)
                throw new NotSupportedException("Can only split triangle edges");

            if (opp.EdgeCount != 3)
                throw new NotSupportedException("Can only split triangle edges");

            //Add new vertex at mid point.
            //edge now goes to this vertex and the new edge from it.
            var v = PokeEdge(edge, 0.5f);

            //Get the new edge and its opposite.
            var nedge = v.Edge;
            var nopp = nedge.Opposite;

            //The faces are now quads.
            //Need two new edges (4 half edges) to make four triangles
            HBEdge.NewEdge(out HBEdge e0, out HBEdge e1);
            HBEdge.NewEdge(out HBEdge e2, out HBEdge e3);

            //Connect the new edges previous and next edges.
            HBEdge.InsertBetween(e0, edge, edge.Previous);
            HBEdge.InsertBetween(e1, nedge.Next, nedge);
            HBEdge.InsertBetween(e2, opp.Next, opp);
            HBEdge.InsertBetween(e3, nopp, nopp.Previous);

            //Connect the new edges vertices.
            HBEdge.SetFrom(e0, v);
            HBEdge.SetFrom(e1, edge.Previous.From);
            HBEdge.SetFrom(e2, nopp.Previous.From);
            HBEdge.SetFrom(e3, v);

            //The are two existing faces but need four.
            //Create two new faces.
            var f0 = edge.Face;
            var f1 = opp.Face;
            var f2 = new HBFace();
            var f3 = new HBFace();

            //Iterate all edges in each face and
            //set the correct face for all edges.
            HBEdge.SetFaces(edge, f0);
            HBEdge.SetFaces(opp, f1);
            HBEdge.SetFaces(nedge, f2);
            HBEdge.SetFaces(nopp, f3);

            //Add newly created objects to mesh.
            Edges.Add(e0, e1, e2, e3);
            Faces.Add(f2, f3);

            //return the new vertex.
            return v;
        }
    }
}

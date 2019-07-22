using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBMeshOp
    {
        /// <summary>
        /// Splits a edge at its mid point and adds 
        /// the new edges to keep all faces as triangles.
        /// </summary>
        /// <param name="mesh">A triangle mesh the edge belongs to.</param>
        /// <param name="edge">The edge to splits.</param>
        public static VERTEX SplitEdge<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, EDGE edge)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
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
            var v = PokeEdge(mesh, edge, 0.5);

            //Get the new edge and its opposite.
            var nedge = v.Edge;
            var nopp = nedge.Opposite;

            //The faces are now quads.
            //Need two new edges (4 half edges) to make four triangles
            NewEdge(out EDGE e0, out EDGE e1);
            NewEdge(out EDGE e2, out EDGE e3);

            //Connect the new edges previous and next edges.
            InsertBetween(e0, edge, edge.Previous);
            InsertBetween(e1, nedge.Next, nedge);
            InsertBetween(e2, opp.Next, opp);
            InsertBetween(e3, nopp, nopp.Previous);

            //Connect the new edges vertices.
            SetFrom(e0, v);
            SetFrom(e1, edge.Previous.From);
            SetFrom(e2, nopp.Previous.From);
            SetFrom(e3, v);

            //The are two existing faces but need four.
            //Create two new faces.
            var f0 = edge.Face;
            var f1 = opp.Face;
            var f2 = new FACE();
            var f3 = new FACE();

            //Iterate all edges in each face and
            //set the correct face for all edges.
            SetFaces(edge, f0);
            SetFaces(opp, f1);
            SetFaces(nedge, f2);
            SetFaces(nopp, f3);

            //Add newly created objects to mesh.
            mesh.Edges.Add(e0, e1, e2, e3);
            mesh.Faces.Add(f2, f3);

            //return the new vertex.
            return v;
        }
    }
}

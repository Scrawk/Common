using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Geometry.DCEL
{
    public partial class DCELMesh
    {
        /// <summary>
        /// Add a new vertex to the mesh.
        /// </summary>
        /// <param name="point">The vertices point.</param>
        /// <returns>The new vertex.</returns>
        public DCELVertex InsertVertex(Vector2d point)
        {
            int i = NextVertexIndex();
            var vert = new DCELVertex(i, point);

            Vertices[i] = vert;
            VertexCount++;

            return vert;
        }

        /// <summary>
        /// Add a new edge connecting the two vertices.
        /// </summary>
        /// <returns>The new edge that goes from v0 and to v1.</returns>
        public DCELHalfedge InsertEdge(DCELVertex v0, DCELVertex v1)
        {
            if (v0 == v1)
                throw new Exception("Can not connect the same vertex objects.");

            if (v0.Point == v1.Point)
                throw new Exception("Can not connect the same vertex positions.");

            int count0 = v0.Degree;
            int count1 = v1.Degree;

            int i = NextEdgeIndex();

            var e0 = new DCELHalfedge(i);
            var e1 = new DCELHalfedge(i);
            e0.Opposite = e1;
            e1.Opposite = e0;

            if (count0 == 0 && count1 == 0)
            {

            }
            else if (count0 <= 1 && count1 <= 1)
            {
                DCELHalfedge.SetPrevious(e0, v0.Edge?.Opposite);
                DCELHalfedge.SetNext(e0, v1?.Edge);

                DCELHalfedge.SetPrevious(e1, v1.Edge?.Opposite);
                DCELHalfedge.SetNext(e1, v0?.Edge);
            }
            else if (count0 > 1 && count1 <= 1)
            {
                var edge = v0.FindInBetweenEdges(v1.Point);
                var previous = edge.Previous;

                DCELHalfedge.SetPrevious(e0, previous);
                DCELHalfedge.SetNext(e0, v1?.Edge);

                DCELHalfedge.SetPrevious(e1, v1.Edge?.Opposite);
                DCELHalfedge.SetNext(e1, edge);
            }
            else if (count0 <= 1 && count1 > 1)
            {
                var edge = v1.FindInBetweenEdges(v0.Point);
                var previous = edge.Previous;

                DCELHalfedge.SetPrevious(e0, v0.Edge?.Opposite);
                DCELHalfedge.SetNext(e0, edge);

                DCELHalfedge.SetPrevious(e1, previous);
                DCELHalfedge.SetNext(e1, v0?.Edge);
            }
            else if (count0 > 1 && count1 > 1)
            {
                var edge0 = v0.FindInBetweenEdges(v1.Point);
                var previous0 = edge0.Previous;

                var edge1 = v1.FindInBetweenEdges(v0.Point);
                var previous1 = edge1.Previous;

                DCELHalfedge.SetPrevious(e0, previous0);
                DCELHalfedge.SetNext(e0, edge1);

                DCELHalfedge.SetPrevious(e1, previous1);
                DCELHalfedge.SetNext(e1, edge0);
            }
            else
            {
                throw new Exception("Unhandled case connecting vertices.");
            }

            DCELHalfedge.SetFrom(e0, v0);
            DCELHalfedge.SetFrom(e1, v1);

            Edges[i] = e0;
            EdgeCount++;

            return e0;
        }
    }
}

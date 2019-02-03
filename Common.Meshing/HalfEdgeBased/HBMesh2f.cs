using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.HalfEdgeBased
{

    /// <summary>
    /// HBMesh with Vector2f as vertices.
    /// </summary>
    public class HBMesh2f : HBMesh<HBVertex2f, HBEdge, HBFace>
    {
        public HBMesh2f() { }

        public HBMesh2f(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector2f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

        /// <summary>
        /// Convert mesh to indexable mesh.
        /// </summary>
        public Mesh2f ToMesh2f(int faceVertices = 3)
        {
            var positions = new List<Vector2f>(Vertices.Count);

            if(faceVertices == 2)
            {
                var indices = new List<int>(Edges.Count);
                GetEdgeIndices(indices);
                GetPositions(positions);
                return new Mesh2f(positions, indices);
            }
            else
            {
                var indices = new List<int>(Faces.Count * faceVertices);
                GetFaceIndices(indices, faceVertices);
                GetPositions(positions);
                return new Mesh2f(positions, indices);
            }
        }
    }

}


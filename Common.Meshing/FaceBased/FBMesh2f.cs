using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector2f as vertices.
    /// </summary>
    public class FBMesh2f : FBMesh<FBVertex2f, FBFace>
    {
        public FBMesh2f() { }

        public FBMesh2f(int numVertices, int numFaces)
            : base(numVertices, numFaces)
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

            var indices = new List<int>(Faces.Count * faceVertices);
            GetFaceIndices(indices, faceVertices);
            GetPositions(positions);
            return new Mesh2f(positions, indices);
        }
    }

}


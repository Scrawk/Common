using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector3f as vertices.
    /// </summary>
    public class FBMesh3f : FBMesh<FBVertex3f, FBFace>
    {
        public FBMesh3f() { }

        public FBMesh3f(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

        /// <summary>
        /// Convert mesh to indexable mesh.
        /// </summary>
        public Mesh3f ToMesh3f(int faceVertices = 3)
        {
            var positions = new List<Vector3f>(Vertices.Count);

            var indices = new List<int>(Faces.Count * faceVertices);
            GetFaceIndices(indices, faceVertices);
            GetPositions(positions);
            return new Mesh3f(positions, indices);
        }
    }

}


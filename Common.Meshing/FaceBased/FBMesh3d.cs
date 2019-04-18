using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector3d as vertices.
    /// </summary>
    public class FBMesh3d : FBMesh<FBVertex3d, FBFace>
    {
        public FBMesh3d() { }

        public FBMesh3d(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh3d: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3d> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

    }

}


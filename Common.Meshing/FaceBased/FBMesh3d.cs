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

        public Vector3d Position(int i)
        {
            return Vertices[i].Position;
        }

        public override string ToString()
        {
            return string.Format("[FBMesh3d: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

    }

}


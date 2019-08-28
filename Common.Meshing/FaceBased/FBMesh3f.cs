using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector3f as vertices.
    /// </summary>
    public class FBMesh3f : FBMesh<FBVertex3f>
    {
        public FBMesh3f() { }

        public FBMesh3f(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh3f: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        public Vector3f Position(int i)
        {
            return Vertices[i].Position;
        }

    }

}


using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector2i as vertices.
    /// </summary>
    public class FBMesh3i : FBMesh<FBVertex3i, FBFace>
    {
        public FBMesh3i() { }

        public FBMesh3i(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh3i: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        public Vector3i Position(int i)
        {
            return Vertices[i].Position;
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3i> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

    }

}


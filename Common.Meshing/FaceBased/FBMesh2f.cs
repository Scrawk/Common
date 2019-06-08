using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

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

        public override string ToString()
        {
            return string.Format("[FBMesh2f: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        public Vector2f Position(int i)
        {
            return Vertices[i].Position;
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector2f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

    }

}


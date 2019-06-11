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
    public class FBMesh2i : FBMesh<FBVertex2i, FBFace>
    {
        public FBMesh2i() { }

        public FBMesh2i(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh2i: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        public Vector2i Position(int i)
        {
            return Vertices[i].Position;
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector2i> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

    }

}


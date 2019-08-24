using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.IndexBased;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// FBMesh with Vector2d as vertices.
    /// </summary>
    public class FBMesh2d : FBMesh<FBVertex2d>
    {
        public FBMesh2d() { }

        public FBMesh2d(int numVertices, int numFaces)
            : base(numVertices, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[FBMesh2d: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        public Vector2d Position(int i)
        {
            return Vertices[i].Position;
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector2d> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].Position);
        }

    }

}


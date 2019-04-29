using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// HBMesh with Vector3d as vertices.
    /// </summary>
    public class HBMesh3d : HBMesh<HBVertex3d, HBEdge, HBFace>
    {
        public HBMesh3d() { }

        public HBMesh3d(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[HBMesh3d: Vertices={0}, Edges={1}, Faces={2}]",
                Vertices.Count, Edges.Count, Faces.Count);
        }

    }
}


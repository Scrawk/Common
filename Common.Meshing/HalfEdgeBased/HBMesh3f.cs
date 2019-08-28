using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// HBMesh with Vector3f as vertices.
    /// </summary>
    public class HBMesh3f : HBMesh<HBVertex3f>
    {
        public HBMesh3f() { }

        public HBMesh3f(int numVertices, int numEdges, int numFaces)
            : base(numVertices, numEdges, numFaces)
        {

        }

        public override string ToString()
        {
            return string.Format("[HBMesh3f: Vertices={0}, Edges={1}, Faces={2}]",
                Vertices.Count, Edges.Count, Faces.Count);
        }

        public Vector3f Position(int i)
        {
            return Vertices[i].Position;
        }

    }
}


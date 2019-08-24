using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.FaceBased
{
    public partial class FBMesh<VERTEX>
           where VERTEX : FBVertex, new()
    {
        public void ToTriangleMesh<MESH>(ITriangleMeshConstructor<MESH> constructor)
        {
            TagAll();

            constructor.PushTriangleMesh(Vertices.Count, Faces.Count);

            foreach (var vertex in Vertices)
                constructor.AddVertex(vertex.GetPosition());

            foreach (var face in Faces)
            {
                var v = face.Vertices;
                if (v.Length != 3)
                    throw new InvalidOperationException("Face does not contain 3 vertices.");

                constructor.AddFace(v[0].Tag, v[1].Tag, v[2].Tag);
            }

            if (constructor.SupportsFaceConnections)
            {
                foreach (var face in Faces)
                {
                    var n = face.Neighbours;
                    if (n.Length != 3)
                        throw new InvalidOperationException("Face does not contain 3 neighbours.");

                    int i0 = n[0] != null ? n[0].Tag : -1;
                    int i1 = n[1] != null ? n[1].Tag : -1;
                    int i2 = n[2] != null ? n[2].Tag : -1;
                    constructor.AddFaceConnection(face.Tag, i0, i1, i2);
                }
            }
        }
    }
}

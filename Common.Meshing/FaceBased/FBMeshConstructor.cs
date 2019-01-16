using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;
using Common.Meshing.Descriptors;

namespace Common.Meshing.FaceBased
{

    public class FBMeshConstructor<VERTEX, FACE> : MeshConstructor<FBMesh<VERTEX, FACE>>
           where VERTEX : FBVertex, new()
           where FACE : FBFace, new()
    {

        public override bool SupportsEdges { get { return false; } }

        public override bool SupportsEdgeConnections { get { return false; } }

        public override bool SupportsFaces { get { return true; } }

        public override bool SupportsFaceConnections { get { return true; } }

        private FBMesh<VERTEX, FACE> Mesh { get; set; }

        public override void PushTriangleMesh(int numVertices, int numFaces)
        {
            Mesh = new FBMesh<VERTEX, FACE>(numVertices, numFaces);
        }

        public override void PushEdgeMesh(int numVertices, int numEdges)
        {
            Mesh = new FBMesh<VERTEX, FACE>(numVertices, 0);
        }

        public override FBMesh<VERTEX, FACE> PopMesh()
        {
            var tmp = Mesh;
            Mesh = null;
            return tmp;
        }

        public override void AddVertex(Vector2f pos)
        {
            VERTEX v = new VERTEX();
            v.Initialize(pos);
            Mesh.Vertices.Add(v);
        }

        public override void AddVertex(Vector3f pos)
        {
            VERTEX v = new VERTEX();
            v.Initialize(pos);
            Mesh.Vertices.Add(v);
        }

        public override void AddFace(int i0, int i1, int i2)
        {
            var v0 = Mesh.Vertices[i0];
            var v1 = Mesh.Vertices[i1];
            var v2 = Mesh.Vertices[i2];

            FACE face = new FACE();
            face.SetSize(3);

            v0.Face = face;
            v1.Face = face;
            v2.Face = face;

            face.Vertices[0] = v0;
            face.Vertices[1] = v1;
            face.Vertices[2] = v2;

            Mesh.Faces.Add(face);
        }

        public override void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            var face = Mesh.Faces[faceIndex];
            var f0 = (i0 != -1) ? Mesh.Faces[i0] : null;
            var f1 = (i1 != -1) ? Mesh.Faces[i1] : null;
            var f2 = (i2 != -1) ? Mesh.Faces[i2] : null;

            face.Neighbors[0] = f0;
            face.Neighbors[1] = f1;
            face.Neighbors[2] = f2;
        }

    }
}

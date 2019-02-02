using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.FaceBased
{

    public class FBMeshConstructor<VERTEX, FACE> :
           ITriangleMeshConstructor<FBMesh<VERTEX, FACE>>,
           IGeneralMeshConstructor<FBMesh<VERTEX, FACE>>
           where VERTEX : FBVertex, new()
           where FACE : FBFace, new()
    {

        public bool SupportsEdgeConnections { get { return false; } }

        public bool SupportsFaceConnections { get { return true; } }

        private FBMesh<VERTEX, FACE> Mesh { get; set; }

        public void PushTriangleMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            Mesh = new FBMesh<VERTEX, FACE>(numVertices, numFaces);
        }

        public void PushGeneralMesh(int numVertices, int numFaces)
        {
            if (Mesh != null)
                throw new InvalidOperationException("Mesh under construction. Can not push new mesh.");

            Mesh = new FBMesh<VERTEX, FACE>(numVertices, numFaces);
        }

        public FBMesh<VERTEX, FACE> PopMesh()
        {
            var tmp = Mesh;
            Mesh = null;
            return tmp;
        }

        public void AddVertex(Vector2f pos)
        {
            VERTEX v = new VERTEX();
            v.Initialize(pos);
            Mesh.Vertices.Add(v);
        }

        public void AddVertex(Vector3f pos)
        {
            VERTEX v = new VERTEX();
            v.Initialize(pos);
            Mesh.Vertices.Add(v);
        }

        public void AddFace(int i0, int i1, int i2)
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

        public void AddFace(IList<int> vertList)
        {
            int count = vertList.Count;
            FACE face = new FACE();
            face.SetSize(count);

            for(int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertList[i]];
                v.Face = face;
                face.Vertices[i] = v;
            }

            Mesh.Faces.Add(face);
        }

        public void AddFace(int vertStart, int numVertices)
        {
            int count = numVertices;
            FACE face = new FACE();
            face.SetSize(count);

            for (int i = 0; i < count; i++)
            {
                var v = Mesh.Vertices[vertStart + i];
                v.Face = face;
                face.Vertices[i] = v;
            }

            Mesh.Faces.Add(face);
        }

        public void AddFaceConnection(int faceIndex, int i0, int i1, int i2)
        {
            var face = Mesh.Faces[faceIndex];
            var f0 = (i0 != -1) ? Mesh.Faces[i0] : null;
            var f1 = (i1 != -1) ? Mesh.Faces[i1] : null;
            var f2 = (i2 != -1) ? Mesh.Faces[i2] : null;

            face.Neighbors[0] = f0;
            face.Neighbors[1] = f1;
            face.Neighbors[2] = f2;
        }

        public void AddFaceConnection(int faceIndex, IList<int> neighbours)
        {
            int count = neighbours.Count;
            var face = Mesh.Faces[faceIndex];

            for (int i = 0; i < count; i++)
            {
                int n = neighbours[i];
                if (n == -1) continue;
                face.Neighbors[i] = Mesh.Faces[n];
            }

        }

    }
}

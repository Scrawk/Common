using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.IndexBased
{
    public class Mesh3d : IndexableMesh
    {
        public override int VerticesCount { get { return (Positions != null) ? Positions.Length : 0; } }

        public Vector3d[] Positions { get; private set; }

        public bool HasNormals { get { return Normals != null; } }

        public Vector3d[] Normals { get; private set; }

        public bool HasTexCoords0 { get { return TexCoords0 != null; } }

        public Vector2d[] TexCoords0 { get; private set; }

        public Mesh3d()
        {

        }

        public Mesh3d(int numPositions)
        {
            Positions = new Vector3d[numPositions];
        }

        public Mesh3d(IList<Vector3d> positions)
        {
            SetPositions(positions);
        }

        public Mesh3d(IList<Vector3d> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetIndices(indices);
        }

        public Mesh3d(int numPositions, int numIndices)
        {
            Positions = new Vector3d[numPositions];
            Indices = new int[numIndices];
        }

        public override string ToString()
        {
            return string.Format("[Mesh3d: Vertices={0}, Indices={1}]", VerticesCount, IndicesCount);
        }

        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector3d[size];
        }

        public void SetPositions(IList<Vector3d> positions)
        {
            SetPositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        public void SetNormals(int size)
        {
            if (Normals == null || Normals.Length != size)
                Normals = new Vector3d[size];
        }

        public void SetNormals(IList<Vector3d> normals)
        {
            SetNormals(normals.Count);
            normals.CopyTo(Normals, 0);
        }

        public void SetTexCoords0(int size)
        {
            if (TexCoords0 == null || TexCoords0.Length != size)
                TexCoords0 = new Vector2d[size];
        }

        public void SetTexCoords0(IList<Vector2d> texCoords)
        {
            SetTexCoords0(texCoords.Count);
            texCoords.CopyTo(TexCoords0, 0);
        }

        public void Translate(Vector3d translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        public void Scale(Vector3d scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        public void Transform(Matrix4x4d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (m * Positions[i].xyz1).xyz;
        }

        public void Transform(Matrix3x3d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

    }
}

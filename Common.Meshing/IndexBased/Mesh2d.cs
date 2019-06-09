using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.IndexBased
{
    public class Mesh2d : IndexableMesh
    {
        public override int VerticesCount { get { return (Positions != null) ? Positions.Length : 0; } }

        public Vector2d[] Positions { get; private set; }

        public bool HasNormals { get { return Normals != null; } }

        public Vector2d[] Normals { get; private set; }

        public bool HasTexCoords0 { get { return TexCoords0 != null; } }

        public Vector2d[] TexCoords0 { get; private set; }

        public Mesh2d()
        {

        }

        public Mesh2d(int numPositions)
        {
            Positions = new Vector2d[numPositions];
        }

        public Mesh2d(IList<Vector2d> positions)
        {
            SetPositions(positions);
        }

        public Mesh2d(IList<Vector2d> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetIndices(indices);
        }

        public Mesh2d(int numPositions, int numIndices)
        {
            Positions = new Vector2d[numPositions];
            Indices = new int[numIndices];
        }

        public override string ToString()
        {
            return string.Format("[Mesh2d: Vertices={0}, Indices={1}]", VerticesCount, IndicesCount);
        }

        public void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new Vector2d[size];
        }

        public void SetPositions(IList<Vector2d> positions)
        {
            SetPositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        public void SetNormals(int size)
        {
            if (Normals == null || Normals.Length != size)
                Normals = new Vector2d[size];
        }

        public void SetPositions(IList<Vector3d> positions)
        {
            SetPositions(positions.Count);
            for (int i = 0; i < positions.Count; i++)
                Positions[i] = positions[i].xy;
        }

        public void SetNormals(IList<Vector2d> normals)
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

        public void Translate(Vector2d translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        public void Scale(Vector2d scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        public void Transform(Matrix4x4d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (m * Positions[i].xy01).xy;
        }

        public void Transform(Matrix2x2d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }



    }
}

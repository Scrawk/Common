using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using VECTOR3 = Common.Core.Numerics.Vector3f;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using MATRIX = Common.Core.Numerics.Matrix3x3f;

namespace Common.Meshing.IndexBased
{
    public class Mesh3f : IndexableMesh
    {

        public Mesh3f()
        {

        }

        public Mesh3f(int numPositions)
        {
            Positions = new VECTOR3[numPositions];
        }

        public Mesh3f(IList<VECTOR3> positions)
        {
            SetPositions(positions);
        }

        public Mesh3f(IList<VECTOR3> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetPositionIndices(indices);
        }

        public Mesh3f(int numPositions, int numIndices)
        {
            Positions = new VECTOR3[numPositions];
            SetIndices(numIndices);
        }

        /// <summary>
        /// The number of positions in mesh.
        /// </summary>
        public override int PositionCount { get { return (Positions != null) ? Positions.Length : 0; } }

        /// <summary>
        /// The vertex positions.
        /// </summary>
        public VECTOR3[] Positions { get; private set; }

        /// <summary>
        /// The vertex normals.
        /// </summary>
        public VECTOR3[] Normals { get; private set; }

        /// <summary>
        /// The number of normals in mesh.
        /// </summary>
        public override int NormalCount { get { return (Normals != null) ? Normals.Length : 0; } }

        /// <summary>
        /// The vertex uvs.
        /// </summary>
        public VECTOR2[] TexCoords { get; private set; }

        /// <summary>
        /// The number of texCoords in mesh.
        /// </summary>
        public override int TexCoordCount { get { return (TexCoords != null) ? TexCoords.Length : 0; } }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Mesh2d: Vertices={0}, Indices={1}]", PositionCount, IndexCount);
        }

        /// <summary>
        /// Get the vertex position at index i.
        /// </summary>
        public override Vector3d GetPosition(int i)
        {
            return Positions[i];
        }

        /// <summary>
        /// Set the vertex position at index i.
        /// </summary>
        public override void SetPosition(int i, Vector3d pos)
        {
            Positions[i] = (VECTOR3)pos;
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public override void SetPositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new VECTOR3[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetPositions(IList<VECTOR3> positions)
        {
            SetPositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetPositions(IList<Vector3d> positions)
        {
            SetPositions(positions.Count);
            for (int i = 0; i < positions.Count; i++)
                Positions[i] = (VECTOR3)positions[i];
        }

        /// <summary>
        /// Creates the normals array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetNormals(int size)
        {
            if (Normals == null || Normals.Length != size)
                Normals = new VECTOR3[size];
        }

        /// <summary>
        /// Create the normal array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetNormals(IList<VECTOR3> normals)
        {
            SetNormals(normals.Count);
            normals.CopyTo(Normals, 0);
        }

        /// <summary>
        /// Creates the uv array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetTexCoords(int size)
        {
            if (TexCoords == null || TexCoords.Length != size)
                TexCoords = new VECTOR2[size];
        }

        /// <summary>
        /// Create the uv array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetTexCoords(IList<VECTOR2> texCoords)
        {
            SetTexCoords(texCoords.Count);
            texCoords.CopyTo(TexCoords, 0);
        }

        /// <summary>
        /// Translate the positions.
        /// </summary>
        public void Translate(VECTOR3 translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        /// <summary>
        /// Rotate allpositions.
        /// </summary>
        public void Rotate(VECTOR3 rotate)
        {
            var q = Quaternion3f.FromEuler(rotate);
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= q;
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(VECTOR3 scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(Matrix4x4d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (VECTOR3)(m * Positions[i].xyz1).xyz;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

        /// <summary>
        /// Create the area weighted normals
        /// presuming mesh has triangle faces.
        /// </summary>
        public void CreateTriangleNormals()
        {
            if (IndexCount == 0) return;

            SetNormals(PositionCount);
            Array.Clear(Normals, 0, NormalCount);

            for (int i = 0; i < IndexCount; i++)
                Indices[i].normal = Indices[i].position;

            for (int i = 0; i < IndexCount / 3; i++)
            {
                var p0 = Positions[Indices[i * 3 + 0].position];
                var p1 = Positions[Indices[i * 3 + 1].position];
                var p2 = Positions[Indices[i * 3 + 2].position];

                var n = VECTOR3.Cross(p1 - p0, p2 - p0);

                Normals[Indices[i * 3 + 0].normal] += n;
                Normals[Indices[i * 3 + 1].normal] += n;
                Normals[Indices[i * 3 + 2].normal] += n;
            }

            for (int i = 0; i < NormalCount; i++)
                Normals[i] = Normals[i].Normalized;
        }

    }
}

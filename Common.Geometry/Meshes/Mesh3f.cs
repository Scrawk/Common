using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using VECTOR3 = Common.Core.Numerics.Vector3f;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using POINT3 = Common.Core.Numerics.Point3f;
using MATRIX3 = Common.Core.Numerics.Matrix3x3f;
using MATRIX4 = Common.Core.Numerics.Matrix4x4f;

namespace Common.Geometry.Meshes
{
    public class Mesh3f : IndexableMesh
    {
        /// <summary>
        /// 
        /// </summary>
        public Mesh3f()
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numPositions"></param>
        public Mesh3f(int numPositions)
        {
            Positions = new POINT3[numPositions];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positions"></param>
        public Mesh3f(IList<POINT3> positions)
        {
            SetPositions(positions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="indices"></param>
        public Mesh3f(IList<POINT3> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetIndices(indices);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numPositions"></param>
        /// <param name="numIndices"></param>
        public Mesh3f(int numPositions, int numIndices)
        {
            Positions = new POINT3[numPositions];
            CreateIndices(numIndices);
        }

        /// <summary>
        /// The number of positions in mesh.
        /// </summary>
        public override int PositionCount { get { return (Positions != null) ? Positions.Length : 0; } }

        /// <summary>
        /// The vertex positions.
        /// </summary>
        public POINT3[] Positions { get; private set; }

        /// <summary>
        /// Does the mesh have normals.
        /// </summary>
        public bool HasNormals => Normals != null;

        /// <summary>
        /// The vertex normals.
        /// </summary>
        public VECTOR3[] Normals { get; private set; }

        /// <summary>
        /// Does the mesh have tangents.
        /// </summary>
        public bool HasTangents => Tangents != null;

        /// <summary>
        /// The vertex normals.
        /// </summary>
        public VECTOR3[] Tangents { get; private set; }

        /// <summary>
        /// Does the mesh have tex coords.
        /// </summary>
        public bool HasTexCoords => TexCoords != null;

        /// <summary>
        /// The vertex uvs.
        /// </summary>
        public VECTOR2[] TexCoords { get; private set; }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Mesh3f: Vertices={0}, Indices={1}]", PositionCount, IndexCount);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreatePositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new POINT3[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        public void SetPositions(IList<POINT3> positions)
        {
            CreatePositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Creates the normals array.
        /// </summary>
        public void CreateNormals()
        {
            int size = PositionCount;
            if (Normals == null || Normals.Length != size)
                Normals = new VECTOR3[size];
        }

        /// <summary>
        /// Create the normal array.
        /// </summary>
        public void SetNormals(IList<VECTOR3> normals)
        {
            if (normals.Count != PositionCount)
                throw new Exception("Normal array must match positions count");

            CreateNormals();
            normals.CopyTo(Normals, 0);
        }

        /// <summary>
        /// Creates the tangents array.
        /// </summary>
        public void CreateTangents()
        {
            int size = PositionCount;
            if (Tangents == null || Tangents.Length != size)
                Tangents = new VECTOR3[size];
        }

        /// <summary>
        /// Create the tangent array.
        /// </summary>
        public void SetTangents(IList<VECTOR3> tangents)
        {
            if (tangents.Count != PositionCount)
                throw new Exception("Tangents array must match positions count");

            CreateTangents();
            tangents.CopyTo(Tangents, 0);
        }

        /// <summary>
        /// Creates the uv array.
        /// </summary>
        public void CreateTexCoords()
        {
            int size = PositionCount;
            if (TexCoords == null || TexCoords.Length != size)
                TexCoords = new VECTOR2[size];
        }

        /// <summary>
        /// Create the uv array.
        /// </summary>
        public void SetTexCoords(IList<VECTOR2> texCoords)
        {
            if (texCoords.Count != PositionCount)
                throw new Exception("TexCoord array must match positions count");

            CreateTexCoords();
            texCoords.CopyTo(TexCoords, 0);
        }

        /// <summary>
        /// Translate the positions.
        /// </summary>
        public void Translate(POINT3 translate)
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
            {
                Positions[i] = q * Positions[i];

                if (HasNormals)
                    Normals[i] = q * Normals[i];

                if (HasTangents)
                    Tangents[i] = q * Tangents[i];
            }
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(POINT3 scale)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] *= scale;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX4 m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
            {
                Positions[i] = m * Positions[i];

                if (HasNormals)
                    Normals[i] = m * Normals[i];

                if (HasTangents)
                    Tangents[i] = m * Tangents[i];
            }
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX3 m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
            {
                Positions[i] = m * Positions[i];

                if(HasNormals)
                    Normals[i] = m * Normals[i];

                if (HasTangents)
                    Tangents[i] = m * Tangents[i];
            }
                
        }

        /// <summary>
        /// Create the area weighted normals
        /// presuming mesh has triangle faces.
        /// </summary>
        public void CreateTriangleNormals()
        {
            if (IndexCount == 0) return;

            CreateNormals();
            Array.Clear(Normals, 0, Normals.Length);

            for (int i = 0; i < IndexCount / 3; i++)
            {
                var p0 = Positions[Indices[i * 3 + 0]];
                var p1 = Positions[Indices[i * 3 + 1]];
                var p2 = Positions[Indices[i * 3 + 2]];

                var n = VECTOR3.Cross(p1 - p0, p2 - p0);

                Normals[Indices[i * 3 + 0]] += n;
                Normals[Indices[i * 3 + 1]] += n;
                Normals[Indices[i * 3 + 2]] += n;
            }

            for (int i = 0; i < Normals.Length; i++)
                Normals[i] = Normals[i].Normalized;
        }

        /// <summary>
        /// Flip triangle orientation.
        /// </summary>
        public void FlipTriangles()
        {
            if (IndexCount == 0) return;

            for (int i = 0; i < IndexCount / 3; i++)
            {
                var i0 = Indices[i * 3 + 0];
                var i1 = Indices[i * 3 + 1];
                var i2 = Indices[i * 3 + 2];

                Indices[i * 3 + 0] = i2;
                Indices[i * 3 + 1] = i1;
                Indices[i * 3 + 2] = i0;
            }

            if(HasNormals)
            {
                for (int i = 0; i < Normals.Length; i++)
                    Normals[i] = Normals[i] *= -1;
            }

            if (HasTangents)
            {
                for (int i = 0; i < Tangents.Length; i++)
                    Tangents[i] = Tangents[i] *= -1;
            }

        }

    }
}

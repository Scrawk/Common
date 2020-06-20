﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using VECTOR2 = Common.Core.Numerics.Vector2f;
using MATRIX2 = Common.Core.Numerics.Matrix2x2f;
using MATRIX4 = Common.Core.Numerics.Matrix4x4f;

namespace Common.Meshes
{
    public class Mesh2f : IndexableMesh
    {

        public Mesh2f()
        {

        }

        public Mesh2f(int numPositions)
        {
            Positions = new VECTOR2[numPositions];
        }

        public Mesh2f(IList<VECTOR2> positions)
        {
            SetPositions(positions);
        }

        public Mesh2f(IList<VECTOR2> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetIndices(indices);
        }

        public Mesh2f(int numPositions, int numIndices)
        {
            Positions = new VECTOR2[numPositions];
            CreateIndices(numIndices);
        }

        /// <summary>
        /// The number of positions in mesh.
        /// </summary>
        public override int PositionCount { get { return (Positions != null) ? Positions.Length : 0; } }

        /// <summary>
        /// The vertex positions.
        /// </summary>
        public VECTOR2[] Positions { get; private set; }

        /// <summary>
        /// Does the mesh have normals.
        /// </summary>
        public bool HasNormals => Normals != null;

        /// <summary>
        /// The vertex normals.
        /// </summary>
        public VECTOR2[] Normals { get; private set; }

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
            return string.Format("[Mesh2f: Vertices={0}, Indices={1}]", PositionCount, IndexCount);
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreatePositions(int size)
        {
            if (Positions == null || Positions.Length != size)
                Positions = new VECTOR2[size];
        }

        /// <summary>
        /// Create the position array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetPositions(IList<VECTOR2> positions)
        {
            CreatePositions(positions.Count);
            positions.CopyTo(Positions, 0);
        }

        /// <summary>
        /// Creates the normals array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreateNormals()
        {
            int size = PositionCount;
            if (Normals == null || Normals.Length != size)
                Normals = new VECTOR2[size];
        }

        /// <summary>
        /// Create the normal array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetNormals(IList<VECTOR2> normals)
        {
            if (normals.Count != PositionCount)
                throw new Exception("Normal array must match positions count");

            CreateNormals();
            normals.CopyTo(Normals, 0);
        }

        /// <summary>
        /// Creates the uv array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreateTexCoords()
        {
            int size = PositionCount;
            if (TexCoords == null || TexCoords.Length != size)
                TexCoords = new VECTOR2[size];
        }

        /// <summary>
        /// Create the uv array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
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
        public void Translate(VECTOR2 translate)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] += translate;
        }

        /// <summary>
        /// Scale the positions.
        /// </summary>
        public void Scale(VECTOR2 scale)
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
                Positions[i] = (m * Positions[i].xy01).xy;
        }

        /// <summary>
        /// Transform the positions.
        /// </summary>
        public void Transform(MATRIX2 m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = m * Positions[i];
        }

    }
}
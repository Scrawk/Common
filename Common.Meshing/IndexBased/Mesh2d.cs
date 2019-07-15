﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

using VECTOR2 = Common.Core.Numerics.Vector2d;
using MATRIX = Common.Core.Numerics.Matrix2x2d;

namespace Common.Meshing.IndexBased
{
    public class Mesh2d : IndexableMesh
    {

        public Mesh2d()
        {

        }

        public Mesh2d(int numPositions)
        {
            Positions = new VECTOR2[numPositions];
        }

        public Mesh2d(IList<VECTOR2> positions)
        {
            SetPositions(positions);
        }

        public Mesh2d(IList<VECTOR2> positions, IList<int> indices)
        {
            SetPositions(positions);
            SetIndices(indices);
        }

        public Mesh2d(int numPositions, int numIndices)
        {
            Positions = new VECTOR2[numPositions];
            SetIndices(numIndices);
        }

        /// <summary>
        /// The number of vertices in mesh.
        /// </summary>
        public override int VertexCount { get { return (Positions != null) ? Positions.Length : 0; } }

        /// <summary>
        /// The vertex positions.
        /// </summary>
        public VECTOR2[] Positions { get; private set; }

        /// <summary>
        /// Does the mesh have normals.
        /// </summary>
        public bool HasNormals { get { return Normals != null; } }

        /// <summary>
        /// The vertex normals.
        /// </summary>
        public VECTOR2[] Normals { get; private set; }

        /// <summary>
        /// Does the mesh have uvs.
        /// </summary>
        public bool HasTexCoords { get { return TexCoords != null; } }

        /// <summary>
        /// The vertex uvs.
        /// </summary>
        public VECTOR2[] TexCoords { get; private set; }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Mesh2d: Vertices={0}, Indices={1}]", VertexCount, IndexCount);
        }

        /// <summary>
        /// Get the vertex position at index i.
        /// </summary>
        public override Vector3d GetPosition(int i)
        {
            return Positions[i].xy0;
        }

        /// <summary>
        /// Set the vertex position at index i.
        /// </summary>
        public override void SetPosition(int i, Vector3d pos)
        {
            Positions[i] = pos.xy;
        }

        /// <summary>
        /// Creates the position array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public override void SetPositions(int size)
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
                Positions[i] = positions[i].xy;
        }

        /// <summary>
        /// Creates the normals array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetNormals(int size)
        {
            if (Normals == null || Normals.Length != size)
                Normals = new VECTOR2[size];
        }

        /// <summary>
        /// Create the normal array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetNormals(IList<VECTOR2> normals)
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
        public void Transform(Matrix4x4d m)
        {
            int numVerts = Positions.Length;
            for (int i = 0; i < numVerts; i++)
                Positions[i] = (m * Positions[i].xy01).xy;
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

    }
}

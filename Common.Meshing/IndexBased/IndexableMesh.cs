﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Colors;
using Common.Meshing.Constructors;

namespace Common.Meshing.IndexBased
{
    public struct MeshVertex
    {
        public int position, normal, uv, color;

        public MeshVertex(int position)
        {
            this.position = position;
            normal = 0;
            uv = 0;
            color = 0;
        }

        public MeshVertex(int position, int normal)
        {
            this.position = position;
            this.normal = normal;
            uv = 0;
            color = 0;
        }

        public MeshVertex(int position, int normal, int uv)
        {
            this.position = position;
            this.normal = normal;
            this.uv = uv;
            color = 0;
        }

        public override string ToString()
        {
            return string.Format("[MeshVertex: Position={0}, Normal={1}, UV={2}, Color={3}]", 
                position, normal, uv, color);
        }
    }

    public struct MeshTriangle
    {
        public MeshVertex a, b, c;

        public override string ToString()
        {
            return string.Format("[MeshTriangle: A={0}, B={1}, C={2}]",a, b, c);
        }
    }

    public struct MeshEdge
    {
        public MeshVertex a, b;

        public override string ToString()
        {
            return string.Format("[MeshEdge: A={0}, B={1}]", a, b);
        }
    }

    public abstract class IndexableMesh
    {

        /// <summary>
        /// The number of positions in mesh.
        /// </summary>
        public abstract int PositionCount { get; }

        /// <summary>
        /// The number of normals in mesh.
        /// </summary>
        public abstract int NormalCount { get; }

        /// <summary>
        /// The number of texCoords in mesh.
        /// </summary>
        public abstract int TexCoordCount { get; }

        /// <summary>
        /// Does the mesh have indices.
        /// </summary>
        public bool HasIndice { get { return Indices != null; } }

        /// <summary>
        /// The number of indices in mesh.
        /// </summary>
        public int IndexCount { get { return (Indices != null) ? Indices.Length : 0; } }

        /// <summary>
        /// The mesh indices.
        /// </summary>
        public MeshVertex[] Indices { get; protected set; }

        /// <summary>
        /// The number of colors in mesh.
        /// </summary>
        public int ColorCount { get { return Colors != null ? Colors.Length : 0; } }

        /// <summary>
        /// The mesh colors.
        /// </summary>
        public ColorRGBA[] Colors { get; protected set; }

        public abstract Vector3d GetPosition(int i);

        /// <summary>
        /// Get the vertex position at index i.
        /// </summary>
        public abstract void SetPosition(int i, Vector3d pos);

        /// <summary>
        /// Create the vertex array. 
        /// </summary>
        /// <param name="size">size of the array</param>
        public abstract void SetPositions(int size);

        /// <summary>
        /// Get the triangle at index i.
        /// Presumes indices represents triangles.
        /// </summary>
        public MeshTriangle GetTriangle(int i)
        {
            var triangle = new MeshTriangle();
            triangle.a = Indices[i / 3 + 0];
            triangle.b = Indices[i / 3 + 1];
            triangle.c = Indices[i / 3 + 2];
            return triangle;
        }

        /// <summary>
        /// Get the edge at index i.
        /// Presumes indices represents edges.
        /// </summary>
        public MeshEdge GetEdge(int i)
        {
            var edge = new MeshEdge();
            edge.a = Indices[i / 3 + 0];
            edge.b = Indices[i / 3 + 1];
            return edge;
        }

        /// <summary>
        /// Creates the color array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetColors(int size)
        {
            if (Colors == null || Colors.Length != size)
                Colors = new ColorRGBA[size];
        }

        /// <summary>
        /// Create the color array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetColors(IList<ColorRGBA> colors)
        {
            SetColors(colors.Count);
            colors.CopyTo(Colors, 0);
        }


        /// <summary>
        /// Creates the index array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetIndices(int size)
        {
            if (Indices == null || Indices.Length != size)
                Indices = new MeshVertex[size];
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetIndices(IList<MeshVertex> indices)
        {
            SetIndices(indices.Count);
            indices.CopyTo(Indices, 0);
        }

        /// <summary>
        /// Create the index array for the positions.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetPositionIndices(IList<int> indices)
        {
            SetIndices(indices.Count);
            for (int i = 0; i < indices.Count; i++)
                Indices[i].position = indices[i];
        }

        /// <summary>
        /// Return a copy of the positions indices.
        /// </summary>
        public List<int> GetPositionIndices()
        {
            var indices = new List<int>(IndexCount);
            for (int i = 0; i < IndexCount; i++)
                indices.Add(Indices[i].position);

            return indices;
        }

    }
}

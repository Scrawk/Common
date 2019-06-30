using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Colors;
using Common.Meshing.Constructors;

namespace Common.Meshing.IndexBased
{
    public abstract class IndexableMesh
    {

        /// <summary>
        /// The number of vertices in mesh.
        /// </summary>
        public abstract int VertexCount { get; }

        /// <summary>
        /// Does the mesh have indices.
        /// </summary>
        public bool HasIndice { get { return Indices != null; } }

        /// <summary>
        /// The number of indices in mesh.
        /// </summary>
        public int IndicesCount { get { return (Indices != null) ? Indices.Length : 0; } }

        /// <summary>
        /// The mesh indices.
        /// </summary>
        public int[] Indices { get; protected set; }

        /// <summary>
        /// Does the mesh have colors.
        /// </summary>
        public bool HasColors { get { return Colors != null; } }

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
        public Vector3i GetTriangle(int i)
        {
            int a = Indices[i / 3 + 0];
            int b = Indices[i / 3 + 1];
            int c = Indices[i / 3 + 2];
            return new Vector3i(a, b, c);
        }

        /// <summary>
        /// Get the edge at index i.
        /// Presumes indices represents edges.
        /// </summary>
        public Vector2i GetEdge(int i)
        {
            int a = Indices[i / 3 + 0];
            int b = Indices[i / 3 + 1];
            return new Vector2i(a, b);
        }

        /// <summary>
        /// Creates the index array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void SetIndices(int size)
        {
            if (Indices == null || Indices.Length != size)
                Indices = new int[size];
        }

        /// <summary>
        /// Create the index array.
        /// </summary>
        /// <param name="indices">Array to copy from.</param>
        public void SetIndices(IList<int> indices)
        {
            SetIndices(indices.Count);
            indices.CopyTo(Indices, 0);
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
        /// Flip winding order of the triangles.
        /// Presumes indices represent triangles.
        /// </summary>
        public void FlipTriangles()
        {
            if (Indices == null) return;
            int count = IndicesCount;
            for (int i = 0; i < count / 3; i++)
            {
                int tmp = Indices[i * 3 + 0];
                Indices[i * 3 + 0] = Indices[i * 3 + 2];
                Indices[i * 3 + 2] = tmp;
            }
        }

        /// <summary>
        /// Create the indices presumig the mesh
        /// represents a polygon.
        /// </summary>
        public void BuildPolygonIndices()
        {
            int numPoints = VertexCount;
            if (numPoints == 0) return;

            int size = numPoints * 2;
            SetIndices(size);

            for (int i = 0; i < numPoints; i++)
            {
                Indices[i * 2 + 0] = i;
                Indices[i * 2 + 1] = (i + 1) % numPoints;
            }
        }

    }
}

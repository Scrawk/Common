using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Colors;

namespace Common.Geometry.Meshes
{

    public abstract class IndexableMesh
    {

        /// <summary>
        /// The number of positions in mesh.
        /// </summary>
        public abstract int PositionCount { get; }

        /// <summary>
        /// Does the mesh have indices.
        /// </summary>
        public bool HasIndices { get { return Indices != null; } }

        /// <summary>
        /// The number of indices in mesh.
        /// </summary>
        public int IndexCount { get { return (Indices != null) ? Indices.Length : 0; } }

        /// <summary>
        /// The mesh indices.
        /// </summary>
        public int[] Indices { get; protected set; }

        /// <summary>
        /// The does the mesh have colors..
        /// </summary>
        public bool HasColors => Colors != null;

        /// <summary>
        /// The mesh colors.
        /// </summary>
        public ColorRGBA[] Colors { get; protected set; }

        /// <summary>
        /// Creates the color array.
        /// </summary>
        public void CreateColors()
        {
            int size = PositionCount;
            if (Colors == null || Colors.Length != size)
                Colors = new ColorRGBA[size];
        }

        /// <summary>
        /// Create the color array.
        /// </summary>
        /// <param name="colors">Array to copy from.</param>
        public void SetColors(IList<ColorRGBA> colors)
        {
            if (colors.Count != PositionCount)
                throw new Exception("Color array must match positions count");

            CreateColors();
            colors.CopyTo(Colors, 0);
        }

        /// <summary>
        /// Creates the index array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public void CreateIndices(int size)
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
            CreateIndices(indices.Count);
            indices.CopyTo(Indices, 0);
        }

    }
}

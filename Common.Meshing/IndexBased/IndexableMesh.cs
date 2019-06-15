using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;
using Common.Meshing.Constructors;

namespace Common.Meshing.IndexBased
{
    public abstract class IndexableMesh : ITriangularMesh, IEdgeMesh
    {

        public abstract int VertexCount { get; }

        public int FaceCount => IndicesCount / 3;

        public int EdgeCount => IndicesCount / 2;

        public bool HasIndice { get { return Indices != null; } }

        public int IndicesCount { get { return (Indices != null) ? Indices.Length : 0; } }

        public int[] Indices { get; protected set; }

        public bool HasColors { get { return Colors != null; } }

        public ColorRGBA[] Colors { get; protected set; }

        public abstract Vector3d GetPosition(int i);

        public abstract void SetPosition(int i, Vector3d pos);

        public abstract void SetPositions(int size);

        public Vector3i GetTriangle(int i)
        {
            int a = Indices[i / 3 + 0];
            int b = Indices[i / 3 + 1];
            int c = Indices[i / 3 + 2];
            return new Vector3i(a, b, c);
        }

        public Vector2i GetEdge(int i)
        {
            int a = Indices[i / 3 + 0];
            int b = Indices[i / 3 + 1];
            return new Vector2i(a, b);
        }

        public void SetIndices(int size)
        {
            if (Indices == null || Indices.Length != size)
                Indices = new int[size];
        }

        public void SetIndices(IList<int> indices)
        {
            SetIndices(indices.Count);
            indices.CopyTo(Indices, 0);
        }

        public void SetColors(int size)
        {
            if (Colors == null || Colors.Length != size)
                Colors = new ColorRGBA[size];
        }

        public void SetColors(IList<ColorRGBA> colors)
        {
            SetColors(colors.Count);
            colors.CopyTo(Colors, 0);
        }

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

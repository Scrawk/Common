using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{

    public class GridVertex
    {

        public Vector2i Index { get; set; }

        public float Cost { get; set; }

        public GridVertex()
        {

        }

        public GridVertex(Vector2i index)
        {
            Index = index;
        }

        public GridVertex(Vector2i index, float cost)
        {
            Index = index;
            Cost = cost;
        }

    }

    public class GridVertexComparer : IComparer<GridVertex>
    {
        public static GridVertexComparer Instance { get; private set; }

        static GridVertexComparer()
        {
            Instance = new GridVertexComparer();
        }

        public int Compare(GridVertex v0, GridVertex v1)
        {
            return v0.Cost.CompareTo(v1.Cost);
        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{ 

    public class GridEdge
    {

        public Vector2i From { get; set; }

        public Vector2i To { get; set; }

        public float Weight { get; set; }

        public GridEdge()
        {

        }

        public GridEdge(Vector2i from, Vector2i to)
        {
            From = from;
            To = to;
        }

        public GridEdge(int fx, int fy, int tx, int ty)
        {
            From = new Vector2i(fx, fy);
            To = new Vector2i(tx, ty);
        }

    }

    public class GridEdgeComparer : IComparer<GridEdge>
    {
        public static GridEdgeComparer Instance { get; private set; }

        static GridEdgeComparer()
        {
            Instance = new GridEdgeComparer();
        }

        public int Compare(GridEdge e0, GridEdge e1)
        {
            return e0.Weight.CompareTo(e1.Weight);
        }
    }
}

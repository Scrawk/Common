using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{ 

    public class GridEdge : IComparable<GridEdge>
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

        public override string ToString()
        {
            return string.Format("[GridEdge: From={0}, To={1}, Weight={2}]", From, To, Weight);
        }

        public int CompareTo(GridEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }

    }
}

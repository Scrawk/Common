using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.GraphTheory.Grids
{

    public class GridVertex : IComparable<GridVertex>
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

        public override string ToString()
        {
            return string.Format("[GridVertex: Index={0}, Cost={1}]", Index, Cost);
        }

        public int CompareTo(GridVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }

    }

}

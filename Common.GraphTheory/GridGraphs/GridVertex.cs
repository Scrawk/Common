using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{
    /// <summary>
    /// A vertex for a grid graph. A grid graph normally
    /// use a grid of indices to represent vertices
    /// but if they need to be instantied into objects
    /// this class is used.
    /// </summary>
    public class GridVertex : IComparable<GridVertex>
    {
        /// <summary>
        /// The index in the grid this vertex
        /// belongs to.
        /// </summary>
        public Point2i Index { get; set; }

        /// <summary>
        /// The cost of the vertex. used in
        /// algorithms such as shortest paths.
        /// </summary>
        public float Cost { get; set; }

        public GridVertex()
        {

        }

        public GridVertex(Point2i index)
        {
            Index = index;
        }

        public GridVertex(Point2i index, float cost)
        {
            Index = index;
            Cost = cost;
        }

        public GridVertex(int x, int y, float cost)
        {
            Index = new Point2i(x, y);
            Cost = cost;
        }

        public override string ToString()
        {
            return string.Format("[GridVertex: Index={0}, Cost={1}]", Index, Cost);
        }

        /// <summary>
        /// Used to sort vertices by their cost.
        /// </summary>
        public int CompareTo(GridVertex other)
        {
            return Cost.CompareTo(other.Cost);
        }

    }

}

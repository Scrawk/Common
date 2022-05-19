using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.GraphTheory.GridGraphs
{ 
    /// <summary>
    /// A edge for a grid graph. A grid graph normally
    /// use a grid of byte flags to represent edges
    /// but if they need to be instantied into objects
    /// this class is used.
    /// </summary>
    public class WeighedGridEdge : IComparable<WeighedGridEdge>
    {
        /// <summary>
        /// The index of the vertex this edge starts at.
        /// </summary>
        public Point2i From { get; set; }

        /// <summary>
        /// The index of the vertex this edge ends at.
        /// </summary>
        public Point2i To { get; set; }

        /// <summary>
        /// The weight of this edge.
        /// used in algorithms like spanning trees.
        /// </summary>
        public float Weight { get; set; }

        public WeighedGridEdge()
        {

        }

        public WeighedGridEdge(Point2i from, Point2i to)
        {
            From = from;
            To = to;
        }

        public WeighedGridEdge(int fx, int fy, int tx, int ty)
        {
            From = new Point2i(fx, fy);
            To = new Point2i(tx, ty);
        }

        public override string ToString()
        {
            return string.Format("[GridEdge: From={0}, To={1}, Weight={2}]", From, To, Weight);
        }

        /// <summary>
        /// Used to sort edges by their weight.
        /// </summary>
        public int CompareTo(WeighedGridEdge other)
        {
            return Weight.CompareTo(other.Weight);
        }

    }
}

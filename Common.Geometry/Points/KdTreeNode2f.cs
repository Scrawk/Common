using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Points
{
    public class KdTreeNode2f : IEnumerable<KdTreeNode2f>
    {

        public KdTreeNode2f(Point2f point, int depth)
        {
            Point = point;
            Depth = depth;
        }

        public Point2f Point { get; private set; }

        public KdTreeNode2f Left { get; internal set; }

        public KdTreeNode2f Right { get; internal set; }

        /// <summary>
        /// The depth of this node in the tree.
        /// </summary>
        public int Depth { get; private set; }

        public bool IsLeaf { get { return Left == null && Right == null; } }

        public bool IsVertical {  get { return Depth % 2 == 0; } }

        /// <summary>
        /// Enumerate all points from this node.
        /// </summary>
        public IEnumerator<KdTreeNode2f> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var n in Left)
                    yield return n;
            }

            yield return this;

            if (Right != null)
            {
                foreach (var n in Right)
                    yield return n;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

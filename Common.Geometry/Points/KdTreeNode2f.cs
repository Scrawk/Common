using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Geometry.Points
{
    public class KdTreeNode2f<T> : IEnumerable<KdTreeNode2f<T>>
        where T : IPoint2f
    {

        public KdTreeNode2f(T point, int depth)
        {
            Point = point;
            Depth = depth;
        }

        public T Point { get; private set; }

        public KdTreeNode2f<T> Left { get; internal set; }

        public KdTreeNode2f<T> Right { get; internal set; }

        /// <summary>
        /// The depth of this node in the tree.
        /// </summary>
        public int Depth { get; private set; }

        public bool IsLeaf { get { return Left == null && Right == null; } }

        public bool IsVertical {  get { return Depth % 2 == 0; } }

        /// <summary>
        /// Enumerate all points from this node.
        /// </summary>
        public IEnumerator<KdTreeNode2f<T>> GetEnumerator()
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

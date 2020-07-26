using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public class KdTreeNode3f<T> : IEnumerable<KdTreeNode3f<T>>
	where T : IPoint3f
    {
        public KdTreeNode3f(T point, int depth)
        {
            Point = point;
            Depth = depth;
        }

        public T Point { get; private set; }

        public KdTreeNode3f<T> Left { get; internal set; }

        public KdTreeNode3f<T> Right { get; internal set; }

        /// <summary>
        /// The depth of this node in the tree.
        /// </summary>
        public int Depth { get; private set; }

        public bool IsLeaf { get { return Left == null && Right == null; } }

        /// <summary>
        /// Enumerate all points from this node.
        /// </summary>
        public IEnumerator<KdTreeNode3f<T>> GetEnumerator()
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

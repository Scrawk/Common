using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{

    public class BVHTreeNode2f : IEnumerable<BVHTreeNode2f>
    {

        public BVHTreeNode2f(IShape2f shape)
        {
            Shape = shape;
            Bounds = shape.Bounds;
        }

        public BVHTreeNode2f(Box2f bounds)
        {
            Shape = null;
            Bounds = bounds;
        }

        public BVHTreeNode2f Left { get; internal set; }

        public BVHTreeNode2f Right { get; internal set; }

        public BVHTreeNode2f Parent { get; internal set; }

        public Box2f Bounds { get; internal set; }

        public IShape2f Shape { get; internal set; }

        public bool IsLeaf => Shape != null;

        internal BVHTreeNode2f Sibling
        {
            get
            {
                return (Parent.Left == this) ? Parent.Right : Parent.Left;
            }
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        public IEnumerator<BVHTreeNode2f> GetEnumerator()
        {
            if (Left != null)
            {
                foreach (var n in Left)
                    yield return n;
            }

            if (IsLeaf)
                yield return this;

            if (Right != null)
            {
                foreach (var n in Right)
                    yield return n;
            }
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}

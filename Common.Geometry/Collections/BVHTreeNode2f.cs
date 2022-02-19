using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;

using BOX2 = Common.Core.Shapes.Box2f;

namespace Common.Geometry.Collections
{
    /// <summary>
    /// A node in a BVH tree.
    /// </summary>
    public class BVHTreeNode2f<T> : IEnumerable<BVHTreeNode2f<T>>
        where T : class, IShape2f
    {

        public BVHTreeNode2f(T shape)
        {
            Shape = shape;
            Bounds = shape.Bounds;
        }

        public BVHTreeNode2f(BOX2 bounds)
        {
            Shape = null;
            Bounds = bounds;
        }

        /// <summary>
        /// The nodes left child.
        /// </summary>
        public BVHTreeNode2f<T> Left { get; internal set; }

        /// <summary>
        /// The nodes right child.
        /// </summary>
        public BVHTreeNode2f<T> Right { get; internal set; }

        /// <summary>
        /// The nodes parent.
        /// </summary>
        public BVHTreeNode2f<T> Parent { get; internal set; }

        /// <summary>
        /// The bounds of the nodes shape if its a leaf 
        /// or the bounds of its child if not a leaf.
        /// </summary>
        public BOX2 Bounds;

        /// <summary>
        /// The nodes shape.
        /// Will be null if this node is not a leaf.
        /// </summary>
        public T Shape { get; internal set; }

        /// <summary>
        /// If this node in a leaf in the tree.
        /// Leaves have no childern.
        /// </summary>
        public bool IsLeaf => Shape != null;

        /// <summary>
        /// The nodes sibling is its parents other child.
        /// </summary>
        internal BVHTreeNode2f<T> Sibling => Parent.Left == this ? Parent.Right : Parent.Left;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsLeaf)
                return string.Format("[BVHNode2f: IsLeaf={0}, Shape={1}]", IsLeaf, Shape);
            else
                return string.Format("[BVHNode2f: IsLeaf={0}, Bounds={1}]", IsLeaf, Bounds);
        }

        /// <summary>
        /// Enumerate all leaf nodes.
        /// </summary>
        public IEnumerator<BVHTreeNode2f<T>> GetEnumerator()
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

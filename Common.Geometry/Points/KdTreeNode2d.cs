﻿using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public class KdTreeNode2f : IEnumerable<KdTreeNode2f>
    {

        public KdTreeNode2f(Vector2f point, int depth, int index)
        {
            Point = point;
            Depth = depth;
            Index = index;
        }

        public Vector2f Point { get; private set; }

        public KdTreeNode2f Left { get; internal set; }

        public KdTreeNode2f Right { get; internal set; }

        /// <summary>
        /// The depth of this node in the tree.
        /// </summary>
        public int Depth { get; private set; }

        /// <summary>
        /// The index of the point in the tree.
        /// Represents the order node 
        /// was added to the tree.
        /// </summary>
        public int Index { get; private set; }

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

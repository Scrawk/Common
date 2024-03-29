﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Trees
{

    /// <summary>
    /// Node for a binary tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T> : IEnumerable<BinaryTreeNode<T>>
    {

        /// <summary>
        /// The nodes item.
        /// </summary>
        public T Item { get; internal set; }

        /// <summary>
        /// The left node. Maybe null.
        /// </summary>
        public BinaryTreeNode<T> Left { get; internal set; }

        /// <summary>
        /// The right node. Maybe null.
        /// </summary>
        public BinaryTreeNode<T> Right { get; internal set; }

        /// <summary>
        /// Is this node a leaf.
        /// </summary>
        public bool IsLeaf
        {
            get { return Left == null && Right == null; }
        }

        /// <summary>
        /// Used internally to balance AVLTrees.
        /// </summary>
        internal int Balance { get; set; }

        /// <summary>
        /// The nodes parent. Null if node is the root.
        /// </summary>
        public BinaryTreeNode<T> Parent { get; internal set; }

        internal BinaryTreeNode(BinaryTreeNode<T> parent, T item)
        {
            Parent = parent;
            Item = item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[BinaryTreeNode: IsLeaf={0}, Item={1}]", IsLeaf, Item);
        }

        /// <summary>
        /// Enumerate all items from this node.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<BinaryTreeNode<T>> GetEnumerator()
        {
            if(Left != null)
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

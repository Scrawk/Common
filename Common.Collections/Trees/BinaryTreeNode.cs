using System;
using System.Collections.Generic;

namespace Common.Collections.Trees
{

    /// <summary>
    /// Node for a binary tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T>
    {

        /// <summary>
        /// The nodes item.
        /// </summary>
        public T Item { get; internal set; }

        /// <summary>
        /// Th left node. Maybe null.
        /// </summary>
        public BinaryTreeNode<T> Left { get; internal set; }

        /// <summary>
        /// Th right node. Maybe null.
        /// </summary>
        public BinaryTreeNode<T> Right { get; internal set; }

        /// <summary>
        /// Used internally to balance AVLTrees.
        /// </summary>
        internal int Height { get; set; }

        internal BinaryTreeNode()
        {

        }

        internal BinaryTreeNode(T item)
        {
            Item = item;
        }

    }
}

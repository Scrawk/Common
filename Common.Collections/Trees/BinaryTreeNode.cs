using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Trees
{

    /// <summary>
    /// Node for a binary tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T> : IEnumerable<T>
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
        /// Used internally to balance AVLTrees.
        /// </summary>
        internal int Balance { get; set; }

        public BinaryTreeNode<T> Parent { get; internal set; }

        internal BinaryTreeNode(BinaryTreeNode<T> parent, T item)
        {
            Parent = parent;
            Item = item;
        }

        /// <summary>
        /// Enumerate all items from this node.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            if(Left != null)
            {
                foreach (var item in Left)
                    yield return item;
            }

            yield return Item;

            if (Right != null)
            {
                foreach (var item in Right)
                    yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}

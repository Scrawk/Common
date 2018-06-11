using System;
using System.Collections.Generic;

namespace Common.Collections.Trees
{
    public class BinaryTreeNode<T>
    {

        public T Item { get; internal set; }

        public BinaryTreeNode<T> Left { get; internal set; }

        public BinaryTreeNode<T> Right { get; internal set; }

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

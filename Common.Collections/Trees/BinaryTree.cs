using System;
using System.Collections;
using System.Collections.Generic;


namespace Common.Collections.Trees
{

    /// <summary>
    /// A special type of tree which has the property that for every node 
    /// in the tree, the value of any node in its left subtree is less than
    /// the value of the node, and any node in its right subtree is greater
    /// than the value of the node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTree<T> where T : IComparable<T>
    {

        /// <summary>
        /// Create new empty tree.
        /// </summary>
        public BinaryTree()
        {

        }

        /// <summary>
        /// Create new tree from a enumerable.
        /// </summary>
        public BinaryTree(IEnumerable<T> items)
        {
            Add(items);
        }

        /// <summary>
        /// The number of elements in the tree,
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// The root element of the tree
        /// </summary>
        public BinaryTreeNode<T> Root { get; protected set; }

        /// <summary>
        /// Clears the tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        /// <summary>
        /// Returns the path from the root to the item.
        /// </summary>
        /// <param name="item">The item the path terminates at</param>
        /// <param name="path">The list of items. The list is not cleared</param>
        public void GetPath(T item, List<T> path)
        {
            BinaryTreeNode<T> current = Root;

            while (current != null)
            {
                path.Add(current.Item);

                if (item.CompareTo(current.Item) < 0)
                    current = current.Left;
                else if (item.CompareTo(current.Item) > 0)
                    current = current.Right;
                else
                    break;
            }
        }

        /// <summary>
        /// Returns the path from the root to the item.
        /// </summary>
        /// <param name="item">The item the path terminates at</param>
        /// <param name="path">The list of nodes. The list is not cleared</param>
        protected void GetPathNodes(T item, List<BinaryTreeNode<T>> path)
        {
            BinaryTreeNode<T> current = Root;

            while (current != null)
            {
                path.Add(current);

                if (item.CompareTo(current.Item) < 0)
                    current = current.Left;
                else if (item.CompareTo(current.Item) > 0)
                    current = current.Right;
                else
                    break;
            }
        }

        /// <summary>
        /// Does the tree contain the item.
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns>If the item is in the tree</returns>
        public bool Contains(T item)
        {
            BinaryTreeNode<T> current = Root;

            while(current != null)
            {
                if (item.CompareTo(current.Item) < 0)
                    current = current.Left;
                else if (item.CompareTo(current.Item) > 0)
                    current = current.Right;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a list of items to the tree.
        /// </summary>
        /// <param name="list"></param>
        public void Add(IEnumerable<T> list)
        {
            foreach (T item in list)
                Add(item);
        }

        /// <summary>
        /// Add a item to the tree.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Add(T item)
        {
            if (Root == null)
                Root = new BinaryTreeNode<T>(item);
            else
            {
                BinaryTreeNode<T> parent = null;
                BinaryTreeNode<T> current = Root;

                while (current != null)
                {
                    if (item.CompareTo(current.Item) < 0)
                    {
                        parent = current;
                        current = current.Left;
                    }
                    else if (item.CompareTo(current.Item) > 0)
                    {
                        parent = current;
                        current = current.Right;
                    }
                    else
                        return false;
                }

                if (item.CompareTo(parent.Item) < 0)
                    parent.Left = new BinaryTreeNode<T>(item);
                else if (item.CompareTo(parent.Item) > 0)
                    parent.Right = new BinaryTreeNode<T>(item);
            }

            Count++;
            return true;

        }

        /// <summary>
        /// Remove a item from the tree.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if the item was removed</returns>
        public virtual bool Remove(T item)
        {
            BinaryTreeNode<T> parent = null;
            BinaryTreeNode<T> current = Root;

            while (current != null)
            {
                if (item.CompareTo(current.Item) < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (item.CompareTo(current.Item) > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else
                    break;
            }

            if (current == null) return false;

            if(current.Left == null)
            {
                if (parent == null)
                    Root = current.Right;
                else
                {
                    if (item.CompareTo(parent.Item) < 0)
                        parent.Left = current.Right;
                    else
                        parent.Right = current.Right;
                }
            }
            else
            {
                BinaryTreeNode<T> parentOfRightMost = current;
                BinaryTreeNode<T> rightMost = current.Left;

                while(rightMost.Right != null)
                {
                    parentOfRightMost = rightMost;
                    rightMost = rightMost.Right;
                }

                current.Item = rightMost.Item;

                if (parentOfRightMost.Right == rightMost)
                    parentOfRightMost.Right = rightMost.Left;
                else
                    parentOfRightMost.Left = rightMost.Left;
                    
            }

            Count--;
            return true;
        }

        /// <summary>
        /// Copy the tree into a list in order.
        /// </summary>
        /// <returns>ordered list</returns>
        public List<T> ToList()
        {
            var list = new List<T>(Count);
            Inorder(list, Root);
            return list;
        }

        private void Inorder(List<T> list, BinaryTreeNode<T> node)
        {
            if (node == null) return;
            Inorder(list, node.Left);
            list.Add(node.Item);
            Inorder(list, node.Right);
        }

    }

}

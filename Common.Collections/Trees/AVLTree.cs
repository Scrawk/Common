using System;
using System.Collections.Generic;

namespace Common.Collections.Trees
{
    /// <summary>
    /// AVL is a balanced binary tree.
    /// This optimizes searches on the tree.
    /// Does not support duplicates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AVLTree<T> : BinaryTree<T> where T : IComparable<T>
    {

        List<BinaryTreeNode<T>> m_path;

        /// <summary>
        /// Create a new tree.
        /// </summary>
        public AVLTree()
        {
            
        }

        public AVLTree(IEnumerable<T> items) : base(items)
        {

        }

        /// <summary>
        /// Add a item to the tree.
        /// Tree must be rebalanced.
        /// </summary>
        /// <param name="item"></param>
        public override bool Add(T item)
        {
            if (base.Add(item))
            {
                BalancePath(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Remove a item to the tree.
        /// Tree must be rebalanced.
        /// </summary>
        /// <param name="item"></param>
        public override bool Remove(T item)
        {
            if (Root == null) return false;

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

            if (current.Left == null)
            {
                if (parent == null)
                    Root = current.Right;
                else
                {
                    if (item.CompareTo(parent.Item) < 0)
                        parent.Left = current.Right;
                    else
                        parent.Right = current.Right;

                    BalancePath(parent.Item);
                }
            }
            else
            {
                BinaryTreeNode<T> parentOfRightMost = current;
                BinaryTreeNode<T> rightMost = current.Left;

                while (rightMost.Right != null)
                {
                    parentOfRightMost = rightMost;
                    rightMost = rightMost.Right;
                }

                current.Item = rightMost.Item;

                if (parentOfRightMost.Right == rightMost)
                    parentOfRightMost.Right = rightMost.Left;
                else
                    parentOfRightMost.Left = rightMost.Left;

                BalancePath(parentOfRightMost.Item);
            }

            Count--;
            return true;
        }
       
        private void BalancePath(T item)
        {

            if (m_path == null)
                m_path = new List<BinaryTreeNode<T>>();

            m_path.Clear();
            GetPathNodes(item, m_path);
            int count = m_path.Count;

            for(int i = count-1; i >= 0; i--)
            {
                BinaryTreeNode<T> node = m_path[i];
                UpdateHeight(node);

                BinaryTreeNode<T> parent = (node == Root) ? null : m_path[i - 1];

                switch(BalanceFactor(node))
                {
                    case -2:
                        if (BalanceFactor(node.Left) <= 0)
                            BalanceLL(node, parent);
                        else
                            BalanceLR(node, parent);
                        break;

                    case 2:
                        if (BalanceFactor(node.Right) >= 0)
                            BalanceRR(node, parent);
                        else
                            BalanceRL(node, parent);
                        break;
                }

            }

            m_path.Clear();
        }

        private void UpdateHeight(BinaryTreeNode<T> node)
        {
            if (node.Left == null && node.Right == null)
                node.Height = 0;
            else if (node.Left == null)
                node.Height = 1 + node.Right.Height;
            else if (node.Right == null)
                node.Height = 1 + node.Left.Height;
            else
                node.Height = 1 + Math.Max(node.Right.Height, node.Left.Height);
        }

        private int BalanceFactor(BinaryTreeNode<T> node)
        {
            if (node.Right == null)
                return -node.Height;
            else if (node.Left == null)
                return node.Height;
            else
                return node.Right.Height - node.Left.Height;
        }

        private void BalanceLL(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Left;

            if (A == Root)
            {
                Root = B;
                SetParent(null, Root);
            }
            else
            {
                if (parent.Left == A)
                {
                    parent.Left = B;
                    SetParent(parent, parent.Left);
                }
                else
                {
                    parent.Right = B;
                    SetParent(parent, parent.Right);
                }
            }

            A.Left = B.Right;
            B.Right = A;

            SetParent(A, A.Left);
            SetParent(B, B.Right);

            UpdateHeight(A);
            UpdateHeight(B);
        }

        private void BalanceLR(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Left;
            BinaryTreeNode<T> C = B.Right;

            if (A == Root)
            {
                Root = C;
                SetParent(null, Root);
            }
            else
            {
                if (parent.Left == A)
                {
                    parent.Left = C;
                    SetParent(parent, parent.Left);
                }
                else
                {
                    parent.Right = C;
                    SetParent(parent, parent.Right);
                }
            }

            A.Left = C.Right;
            B.Right = C.Left;
            C.Left = B;
            C.Right = A;

            SetParent(A, A.Left);
            SetParent(B, B.Right);
            SetParent(C, C.Left);
            SetParent(C, C.Right);

            UpdateHeight(A);
            UpdateHeight(B);
            UpdateHeight(C);
        }

        private void BalanceRR(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Right;

            if (A == Root)
            {
                Root = B;
                SetParent(null, Root);
            }
            else
            {
                if (parent.Left == A)
                {
                    parent.Left = B;
                    SetParent(parent, parent.Left);
                }
                else
                {
                    parent.Right = B;
                    SetParent(parent, parent.Right);
                }
            }

            A.Right = B.Left;
            B.Left = A;

            SetParent(A, A.Right);
            SetParent(B, B.Left);

            UpdateHeight(A);
            UpdateHeight(B);

        }

        private void BalanceRL(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Right;
            BinaryTreeNode<T> C = B.Left;

            if (A == Root)
            {
                Root = C;
                SetParent(null, Root);
            }
            else
            {
                if (parent.Left == A)
                {
                    parent.Left = C;
                    SetParent(parent, parent.Left);
                }
                else
                {
                    parent.Right = C;
                    SetParent(parent, parent.Right);
                }
            }

            A.Right = C.Left;
            B.Left = C.Right;
            C.Left = A;
            C.Right = B;

            SetParent(A, A.Right);
            SetParent(B, B.Left);
            SetParent(C, C.Left);
            SetParent(C, C.Right);

            UpdateHeight(A);
            UpdateHeight(B);
            UpdateHeight(C);
        }

    }
}

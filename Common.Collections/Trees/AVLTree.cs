using System;
using System.Collections.Generic;

namespace Common.Collections.Trees
{
    public class AVLTree<T> : BinaryTree<T> where T : IComparable<T>
    {

        public AVLTree()
        {
            
        }

        public override bool Add(T item)
        {
            if (base.Add(item))
            {
                BalancePath(item);
                return true;
            }
            else
                return false;
        }

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

            List<BinaryTreeNode<T>> path = GetPathNodes(item);
            int count = path.Count;

            for(int i = count-1; i >= 0; i--)
            {
                BinaryTreeNode<T> node = path[i];
                UpdateHeight(node);

                BinaryTreeNode<T> parent = (node == Root) ? null : path[i - 1];

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
                Root = B;
            else
            {
                if (parent.Left == A)
                    parent.Left = B;
                else
                    parent.Right = B;
            }

            A.Left = B.Right;
            B.Right = A;
            UpdateHeight(A);
            UpdateHeight(B);
        }

        private void BalanceLR(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Left;
            BinaryTreeNode<T> C = B.Right;

            if (A == Root)
                Root = C;
            else
            {
                if (parent.Left == A)
                    parent.Left = C;
                else
                    parent.Right = C;
            }

            A.Left = C.Right;
            B.Right = C.Left;
            C.Left = B;
            C.Right = A;

            UpdateHeight(A);
            UpdateHeight(B);
            UpdateHeight(C);
        }

        private void BalanceRR(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Right;

            if (A == Root)
                Root = B;
            else
            {
                if (parent.Left == A)
                    parent.Left = B;
                else
                    parent.Right = B;
            }

            A.Right = B.Left;
            B.Left = A;
            UpdateHeight(A);
            UpdateHeight(B);

        }

        private void BalanceRL(BinaryTreeNode<T> A, BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> B = A.Right;
            BinaryTreeNode<T> C = B.Left;

            if (A == Root)
                Root = C;
            else
            {
                if (parent.Left == A)
                    parent.Left = C;
                else
                    parent.Right = C;
            }

            A.Right = C.Left;
            B.Left = C.Right;
            C.Left = A;
            C.Right = B;

            UpdateHeight(A);
            UpdateHeight(B);
            UpdateHeight(C);
        }

    }
}

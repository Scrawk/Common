using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Trees
{
    /// <summary>
    /// AVL is a balanced binary tree.
    /// This optimizes searches on the tree.
    /// Does not support duplicates.
    /// Shamelessly stolen from https://github.com/bitlush/avl-tree-c-sharp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AVLTree<T> : BinaryTree<T> where T : IComparable<T>
    {

        public AVLTree()
        {

        }

        public AVLTree(IEnumerable<T> items) : base(items)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[AVLTree: Count={0}, Depth={1}]", Count, Depth);
        }

        /// <summary>
        /// Add a item to the tree.
        /// Tree must be rebalanced.
        /// </summary>
        /// <param name="item"></param>
        public override bool Add(T item)
        {
            var node = Root;
            while (node != null)
            {
                int compare = item.CompareTo( node.Item);
                if (compare < 0)
                {
                    var left = node.Left;
                    if (left == null)
                    {
                        node.Left = new BinaryTreeNode<T>(node, item);
                        InsertBalance(node, 1);
                        Count++;
                        return true;
                    }
                    else
                        node = left;
                }
                else if (compare > 0)
                {
                   var right = node.Right;
                    if (right == null)
                    {
                        node.Right = new BinaryTreeNode<T>(node, item);
                        InsertBalance(node, -1);
                        Count++;
                        return true;
                    }
                    else
                        node = right;
                }
                else
                {
                    node.Item = item;
                    return false;
                }
            }

            Root = new BinaryTreeNode<T>(null, item);
            Count++;
            return true;
        }

        /// <summary>
        /// Remove a item to the tree.
        /// Tree must be rebalanced.
        /// </summary>
        /// <param name="item"></param>
        public override bool Remove(T key)
        {
            var node = Root;
            while (node != null)
            {
                int c = key.CompareTo(node.Item);
                if (c < 0)
                    node = node.Left;
                else if (c > 0)
                    node = node.Right;
                else
                {
                    var left = node.Left;
                    var right = node.Right;

                    if (left == null)
                    {
                        if (right == null)
                        {
                            if (node == Root)
                                Root = null;
                            else
                            {
                                var parent = node.Parent;
                                if (parent.Left == node)
                                {
                                    parent.Left = null;
                                    DeleteBalance(parent, -1);
                                }
                                else
                                {
                                    parent.Right = null;
                                    DeleteBalance(parent, 1);
                                }
                            }
                        }
                        else
                        {
                            Replace(node, right);
                            DeleteBalance(node, 0);
                        }
                    }
                    else if (right == null)
                    {
                        Replace(node, left);
                        DeleteBalance(node, 0);
                    }
                    else
                    {
                        var successor = right;
                        if (successor.Left == null)
                        {
                            var parent = node.Parent;
                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            left.Parent = successor;

                            if (node == Root)
                                Root = successor;
                            else
                            {
                                if (parent.Left == node)
                                    parent.Left = successor;
                                else
                                    parent.Right = successor;
                            }

                            DeleteBalance(successor, 1);
                        }
                        else
                        {
                            while (successor.Left != null)
                                successor = successor.Left;

                            var parent = node.Parent;
                            var successorParent = successor.Parent;
                            var successorRight = successor.Right;

                            if (successorParent.Left == successor)
                                successorParent.Left = successorRight;
                            else
                                successorParent.Right = successorRight;

                            if (successorRight != null)
                                successorRight.Parent = successorParent;

                            successor.Parent = parent;
                            successor.Left = left;
                            successor.Balance = node.Balance;
                            successor.Right = right;
                            right.Parent = successor;
                            left.Parent = successor;

                            if (node == Root)
                                Root = successor;
                            else
                            {
                                if (parent.Left == node)
                                    parent.Left = successor;
                                else
                                    parent.Right = successor;
                            }

                            DeleteBalance(successorParent, -1);
                        }
                    }

                    Count--;
                    return true;
                }
            }

            return false;
        }

        private void InsertBalance(BinaryTreeNode<T> node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);

                if (balance == 0)
                    return;
                else if (balance == 2)
                {
                    if (node.Left.Balance == 1)
                        RotateRight(node);
                    else
                        RotateLeftRight(node);

                    return;
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance == -1)
                        RotateLeft(node);
                    else
                        RotateRightLeft(node);

                    return;
                }

                var parent = node.Parent;
                if (parent != null)
                    balance = parent.Left == node ? 1 : -1;

                node = parent;
            }
        }

        private BinaryTreeNode<T> RotateLeft(BinaryTreeNode<T> node)
        {
            var right = node.Right;
            var rightLeft = right.Left;
            var parent = node.Parent;

            right.Parent = parent;
            right.Left = node;
            node.Right = rightLeft;
            node.Parent = right;

            if (rightLeft != null)
                rightLeft.Parent = node;

            if (node == Root)
                Root = right;
            else if (parent.Right == node)
                parent.Right = right;
            else
                parent.Left = right;

            right.Balance++;
            node.Balance = -right.Balance;

            return right;
        }

        private BinaryTreeNode<T> RotateRight(BinaryTreeNode<T> node)
        {
            var left = node.Left;
            var leftRight = left.Right;
            var parent = node.Parent;

            left.Parent = parent;
            left.Right = node;
            node.Left = leftRight;
            node.Parent = left;

            if (leftRight != null)
                leftRight.Parent = node;

            if (node == Root)
                Root = left;
            else if (parent.Left == node)
                parent.Left = left;
            else
                parent.Right = left;

            left.Balance--;
            node.Balance = -left.Balance;

            return left;
        }

        private BinaryTreeNode<T> RotateLeftRight(BinaryTreeNode<T> node)
        {
            var left = node.Left;
            var leftRight = left.Right;
            var parent = node.Parent;
            var leftRightRight = leftRight.Right;
            var leftRightLeft = leftRight.Left;

            leftRight.Parent = parent;
            node.Left = leftRightRight;
            left.Right = leftRightLeft;
            leftRight.Left = left;
            leftRight.Right = node;
            left.Parent = leftRight;
            node.Parent = leftRight;

            if (leftRightRight != null)
                leftRightRight.Parent = node;

            if (leftRightLeft != null)
                leftRightLeft.Parent = left;

            if (node == Root)
                Root = leftRight;
            else if (parent.Left == node)
                parent.Left = leftRight;
            else
                parent.Right = leftRight;

            if (leftRight.Balance == -1)
            {
                node.Balance = 0;
                left.Balance = 1;
            }
            else if (leftRight.Balance == 0)
            {
                node.Balance = 0;
                left.Balance = 0;
            }
            else
            {
                node.Balance = -1;
                left.Balance = 0;
            }

            leftRight.Balance = 0;

            return leftRight;
        }

        private BinaryTreeNode<T> RotateRightLeft(BinaryTreeNode<T> node)
        {
            var right = node.Right;
            var rightLeft = right.Left;
            var parent = node.Parent;
            var rightLeftLeft = rightLeft.Left;
            var rightLeftRight = rightLeft.Right;

            rightLeft.Parent = parent;
            node.Right = rightLeftLeft;
            right.Left = rightLeftRight;
            rightLeft.Right = right;
            rightLeft.Left = node;
            right.Parent = rightLeft;
            node.Parent = rightLeft;

            if (rightLeftLeft != null)
                rightLeftLeft.Parent = node;

            if (rightLeftRight != null)
                rightLeftRight.Parent = right;

            if (node == Root)
                Root = rightLeft;
            else if (parent.Right == node)
                parent.Right = rightLeft;
            else
                parent.Left = rightLeft;

            if (rightLeft.Balance == 1)
            {
                node.Balance = 0;
                right.Balance = -1;
            }
            else if (rightLeft.Balance == 0)
            {
                node.Balance = 0;
                right.Balance = 0;
            }
            else
            {
                node.Balance = 1;
                right.Balance = 0;
            }

            rightLeft.Balance = 0;

            return rightLeft;
        }

        private void DeleteBalance(BinaryTreeNode<T> node, int balance)
        {
            while (node != null)
            {
                balance = (node.Balance += balance);
                if (balance == 2)
                {
                    if (node.Left.Balance >= 0)
                    {
                        node = RotateRight(node);
                        if (node.Balance == -1)
                            return;
                    }
                    else
                        node = RotateLeftRight(node);
                }
                else if (balance == -2)
                {
                    if (node.Right.Balance <= 0)
                    {
                        node = RotateLeft(node);
                        if (node.Balance == 1)
                            return;
                    }
                    else
                        node = RotateRightLeft(node);
                }
                else if (balance != 0)
                    return;

                var parent = node.Parent;
                if (parent != null)
                    balance = parent.Left == node ? -1 : 1;

                node = parent;
            }
        }

        private static void Replace(BinaryTreeNode<T> target, BinaryTreeNode<T> source)
        {
            var left = source.Left;
            var right = source.Right;

            target.Balance = source.Balance;
            target.Item = source.Item;
            target.Left = left;
            target.Right = right;

            if (left != null)
                left.Parent = target;

            if (right != null)
                right.Parent = target;
        }
    }
}
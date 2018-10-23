using System;
using System.Collections.Generic;


namespace Common.Collections.Trees
{
    public class BinaryTree<T> where T : IComparable<T>
    {

        public int Count { get; protected set; }

        public BinaryTreeNode<T> Root { get; protected set; }

        public BinaryTree()
        {

        }

        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        public List<T> GetPath(T item)
        {

            List<T> path = new List<T>();

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

            return path;
        }

        protected List<BinaryTreeNode<T>> GetPathNodes(T item)
        {

            List<BinaryTreeNode<T>> path = new List<BinaryTreeNode<T>>();

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

            return path;
        }

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

        public void Add(IEnumerable<T> list)
        {
            foreach (T item in list)
                Add(item);
        }

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

    }

}

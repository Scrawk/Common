using System;
using System.Collections;
using System.Collections.Generic;

using Common.Collections.Queues;

namespace Common.Collections.Trees
{

    /// <summary>
    /// A special type of tree which has the property that for every node 
    /// in the tree, the value of any node in its left subtree is less than
    /// the value of the node, and any node in its right subtree is greater
    /// than the value of the node.
    /// Does not support duplicates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTree<T> : IPriorityQueue<T>
        where T : IComparable<T>
    {

        /// <summary>
        /// Create new empty tree.
        /// </summary>
        public BinaryTree()
        {

        }

        /// <summary>
        /// Create new empty tree.
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
        /// The max depth of the tree.
        /// </summary>
        public int Depth
        {
            get { return MaxDepth(Root, 0); }
        }

        /// <summary>
        /// Do nothing. Tree has no set capacity.
        /// Needed for IPriorityQueue<T>.
        /// </summary>
        public int Capacity
        {
            get { return 0; }
            set { ; }
        }

        /// <summary>
        /// The root element of the tree
        /// </summary>
        public BinaryTreeNode<T> Root { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[BinaryTree: Count={0}, Depth={1}]", Count, Depth);
        }

        /// <summary>
        /// Clears the tree.
        /// </summary>
        public void Clear()
        {
            Root = null;
            Count = 0;
        }

        /// <summary>
        /// Return first item in tree.
        /// </summary>
        /// <returns>First item</returns>
        public T Peek()
        {
            return FindMinimum();
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
                int c = item.CompareTo(current.Item);
                if (c < 0)
                    current = current.Left;
                else if (c > 0)
                    current = current.Right;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a enumerable to the heap.
        /// </summary>
        /// <param name="data">a enumerable container</param>
        /// <returns>If any of the items not added</returns>
        public void Add(IEnumerable<T> data)
        {
            foreach (var item in data)
                Add(item);
        }

        /// <summary>
        /// Add a item to the tree.
        /// </summary>
        public virtual bool Add(T item)
        {
            if (Root == null)
                Root = new BinaryTreeNode<T>(null, item);
            else
            {
                int c;
                BinaryTreeNode<T> parent = null;
                BinaryTreeNode<T> current = Root;

                while (current != null)
                {
                    c = item.CompareTo(current.Item);
                    if (c < 0)
                    {
                        parent = current;
                        current = current.Left;
                    }
                    else if (c > 0)
                    {
                        parent = current;
                        current = current.Right;
                    }
                }

                c = item.CompareTo(parent.Item);
                if (c < 0)
                    parent.Left = new BinaryTreeNode<T>(parent, item);
                else if (c > 0)
                    parent.Right = new BinaryTreeNode<T>(parent, item);
                else
                    return false;
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
                int c = item.CompareTo(current.Item);
                if (c < 0)
                {
                    parent = current;
                    current = current.Left;
                }
                else if (c > 0)
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
                {
                    Root = current.Right;
                    SetParent(null, Root);
                }
                else
                {
                    if (item.CompareTo(parent.Item) < 0)
                    {
                        parent.Left = current.Right;
                        SetParent(parent, parent.Left);
                    }
                    else
                    {
                        parent.Right = current.Right;
                        SetParent(parent, parent.Right);
                    }
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
        /// Remove first item in tree.
        /// </summary>
        /// <returns>First item</returns>
        public T RemoveFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException("Queue is empty.");

            T item = FindMinimum();
            Remove(item);
            return item;
        }

        /// <summary>
        /// FInds the item by a key which is just another item
        /// that compares to the same value.
        /// </summary>
        /// <returns>True if item found</returns>
        public bool Find(T key, out T item)
        {
            BinaryTreeNode<T> current = Root;

            while (current != null)
            {
                int c = key.CompareTo(current.Item);
                if (c < 0)
                    current = current.Left;
                else if (c > 0)
                    current = current.Right;
                else
                {
                    item = current.Item;
                    return true;
                }
            }

            item = default(T);
            return false;
        }

        /// <summary>
        /// Finds the node which item belongs to.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The found node or null if no match found</returns>
        public BinaryTreeNode<T> FindNode(T item)
        {
            BinaryTreeNode<T> current = Root;
            while (current != null)
            {
                int c = item.CompareTo(current.Item);
                if (c < 0)
                    current = current.Left;
                else if (c > 0)
                    current = current.Right;
                else
                    return current;
            }

            return null;
        }

        /// <summary>
        /// Finds the minimum value in the tree.
        /// </summary>
        public T FindMinimum()
        {
            if (Root == null)
                throw new InvalidOperationException("Can not find minimum if tree is empty.");
            return FindMinimumNode(Root).Item;
        }

        /// <summary>
        /// Finds the maximum value in the tree.
        /// </summary>
        public T FindMaximum()
        {
            if (Root == null)
                throw new InvalidOperationException("Can not find maximum if tree is empty.");
            return FindMaximumNode(Root).Item;
        }

        /// <summary>
        /// Finds the nodes succesor. ie the next highest item.
        /// </summary>
        /// <param name="node">the node</param>
        /// <param name="succesor">the nodes succesors item</param>
        /// <returns>if the node has a succesor</returns>
        public bool FindSuccesor(T item, out T succesor)
        {
            return FindSuccesor(FindNode(item), out succesor);
        }

        public bool FindSuccesor(BinaryTreeNode<T> node, out T succesor)
        {
            succesor = default(T);
            if (node == null) return false;

            if (node.Right != null)
            {
                succesor = FindMinimumNode(node.Right).Item;
                return true;
            }

            var y = node.Parent;
            var x = node;
            while (y != null && x == y.Right)
            {
                x = y;
                y = y.Parent;
            }

            if (y == null) return false;
            succesor = y.Item;
            return true;
        }

        /// <summary>
        /// Finds the nodes succesor. ie the next highest item.
        /// </summary>
        /// <param name="node">the node</param>
        /// <param name="predecessor">the nodes predecessors item</param>
        /// <returns>if the node has a succesor</returns>
        public bool FindPredecessor(T item, out T predecessor)
        {
            return FindPredecessor(FindNode(item), out predecessor);
        }

        public bool FindPredecessor(BinaryTreeNode<T> node, out T predecessor)
        {
            predecessor = default(T);
            if (node == null) return false;

            if (node.Left != null)
            {
                predecessor = FindMaximumNode(node.Left).Item;
                return true;
            }

            var y = node.Parent;
            var x = node;
            while (y != null && x == y.Left)
            {
                x = y;
                y = y.Parent;
            }

            if (y == null) return false;
            predecessor = y.Item;
            return true;
        }

        /// <summary>
        /// Copy the tree into a list in order.
        /// </summary>
        /// <returns>ordered list</returns>
        public List<T> ToList()
        {
            var list = new List<T>(Count);
            Inorder(Root, list);
            return list;
        }

        /// <summary>
        /// Gets an enumerator for the tree.
        /// </summary>
        /// <returns>An IEnumerator of type T.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            if (Root != null)
            {
                foreach (var n in Root)
                    yield return n.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Copy the tree into a list in order.
        /// </summary>
        /// <returns>ordered list</returns>
        public void Inorder(BinaryTreeNode<T> node, List<T> list)
        {
            if (node == null) return;
            Inorder(node.Left, list);
            list.Add(node.Item);
            Inorder(node.Right, list);
        }

        /// <summary>
        /// Copy the tree into a list in depth first order.
        /// </summary>
        /// <returns>ordered list</returns>
        public void DepthFirst(BinaryTreeNode<T> node, List<T> list)
        {
            if (node == null) return;
            list.Add(node.Item);
            DepthFirst(node.Left, list);
            DepthFirst(node.Right, list);
        }

        /// <summary>
        /// Copy the tree into a list in breadth first order.
        /// </summary>
        /// <returns>ordered list</returns>
        public void BreadthFirst(BinaryTreeNode<T> node, List<T> list)
        {
            if (node == null) return;

            var queue = new Queue<BinaryTreeNode<T>>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                var n = queue.Dequeue();
                list.Add(n.Item);

                if (n.Left != null)
                    queue.Enqueue(n.Left);

                if (n.Right != null)
                    queue.Enqueue(n.Right);
            }
        }

        protected void SetParent(BinaryTreeNode<T> parent, BinaryTreeNode<T> node)
        {
            if (node == null) return;
            node.Parent = parent;
        }

        private BinaryTreeNode<T> FindMinimumNode(BinaryTreeNode<T> node)
        {
            if (node == null) return null;
            if (node.Left != null)
                return FindMinimumNode(node.Left);
            else
                return node;
        }

        private BinaryTreeNode<T> FindMaximumNode(BinaryTreeNode<T> node)
        {
            if (node == null) return null;
            if (node.Right != null)
                return FindMaximumNode(node.Right);
            else
                return node;
        }

        private int MaxDepth(BinaryTreeNode<T> node, int depth)
        {
            if (node == null || node.IsLeaf) return depth;
            return Math.Max(MaxDepth(node.Left, depth + 1), MaxDepth(node.Right, depth + 1));
        }

    }

}

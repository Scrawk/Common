using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Common.Collections.Trees;
using Common.Collections.Queues;
using Common.Core.Time;

namespace Common.Collections.Test.Trees
{
    [TestClass]
    public class Collections_Trees_AVLTreeTest
    {
        [TestMethod]
        public void Add()
        {

            AVLTree<int> tree = new AVLTree<int>();

            tree.Add(25);
            tree.Add(20);

            Assert.AreEqual(25, tree.Root.Item);
            Assert.AreEqual(20, tree.Root.Left.Item);
            Assert.AreEqual(2, tree.Count);

            tree.Add(5);

            Assert.AreEqual(20, tree.Root.Item);
            Assert.AreEqual(5, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(3, tree.Count);

            tree.Add(34);

            Assert.AreEqual(20, tree.Root.Item);
            Assert.AreEqual(5, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(34, tree.Root.Right.Right.Item);
            Assert.AreEqual(4, tree.Count);

            tree.Add(50);

            Assert.AreEqual(20, tree.Root.Item);
            Assert.AreEqual(5, tree.Root.Left.Item);
            Assert.AreEqual(34, tree.Root.Right.Item);
            Assert.AreEqual(25, tree.Root.Right.Left.Item);
            Assert.AreEqual(50, tree.Root.Right.Right.Item);
            Assert.AreEqual(5, tree.Count);

            tree.Add(30);

            Assert.AreEqual(25, tree.Root.Item);
            Assert.AreEqual(20, tree.Root.Left.Item);
            Assert.AreEqual(5, tree.Root.Left.Left.Item);
            Assert.AreEqual(34, tree.Root.Right.Item);
            Assert.AreEqual(30, tree.Root.Right.Left.Item);
            Assert.AreEqual(50, tree.Root.Right.Right.Item);
            Assert.AreEqual(6, tree.Count);

            tree.Add(10);

            Assert.AreEqual(25, tree.Root.Item);
            Assert.AreEqual(10, tree.Root.Left.Item);
            Assert.AreEqual(5, tree.Root.Left.Left.Item);
            Assert.AreEqual(20, tree.Root.Left.Right.Item);
            Assert.AreEqual(34, tree.Root.Right.Item);
            Assert.AreEqual(30, tree.Root.Right.Left.Item);
            Assert.AreEqual(50, tree.Root.Right.Right.Item);
            Assert.AreEqual(7, tree.Count);

            CheckParent(null, tree.Root);

        }

        [TestMethod]
        public void Remove()
        {

            AVLTree<int> tree = new AVLTree<int>();

            tree.Add(25);
            tree.Add(20);
            tree.Add(5);
            tree.Add(34);
            tree.Add(50);
            tree.Add(30);
            tree.Add(10);

            tree.Remove(34);
            tree.Remove(30);
            tree.Remove(50);

            Assert.AreEqual(10, tree.Root.Item);
            Assert.AreEqual(5, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(20, tree.Root.Right.Left.Item);
            Assert.AreEqual(4, tree.Count);

            tree.Remove(5);

            Assert.AreEqual(20, tree.Root.Item);
            Assert.AreEqual(10, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(3, tree.Count);

            CheckParent(null, tree.Root);
        }

        /*
        [TestMethod]
        public void Performance()
        {
            var set = new HashSet<string>();
            for (int i = 0; i < 100000; i++)
            {
                set.Add(RandomString(10));
            }

            Console.WriteLine("Count = " + set.Count);

            var avl = new AVLTree<string>();
            avl.Add(set);

            var timer = new Timer();
            timer.Start();

            foreach(var str in set)
            {
                avl.Contains(str);
            }

            Console.WriteLine("AVLTree Time = " + timer.Stop());

            var tree = new BinaryTree<string>();
            tree.Add(set);

            timer = new Timer();
            timer.Start();

            foreach (var str in set)
            {
                tree.Contains(str);
            }

            Console.WriteLine("BinaryTree Time = " + timer.Stop());

            var queue = new BinaryHeap<string>();
            queue.Add(set);

            timer = new Timer();
            timer.Start();

            foreach (var str in set)
            {
                queue.Contains(str);
            }

            Console.WriteLine("BinaryHeap Time = " + timer.Stop());

        }

        private static Random random = new Random();
        private const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        */

        private void CheckParent<T>(BinaryTreeNode<T> parent, BinaryTreeNode<T> node)
        {
            if (node == null) return;

            Assert.AreEqual(parent, node.Parent);
            CheckParent(node, node.Left);
            CheckParent(node, node.Right);
        }
    }

}

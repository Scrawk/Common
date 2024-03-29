﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using Common.Collections.Trees;

namespace Common.Collections.Test.Trees
{
    [TestClass]
    public class AVLTreeTest
    {
        [TestMethod]
        public void Add()
        {

            var tree = new AVLTree<int>();

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
        public void AddRandom()
        {
            var rnd = new Random(0);
            var set = new HashSet<int>();

            for (int i = 0; i < 1000; i++)
                set.Add(rnd.Next());

            var tree = new AVLTree<int>(set);
            var list = new List<int>(set);
            list.Sort();

            CollectionAssert.AreEqual(list, tree.ToList());
        }

        [TestMethod]
        public void RemoveValue()
        {

            var tree = new AVLTree<int>();

            tree.Add(25);
            tree.Add(20);
            tree.Add(5);
            tree.Add(34);
            tree.Add(50);
            tree.Add(30);
            tree.Add(10);

            tree.RemoveValue(34);
            tree.RemoveValue(30);
            tree.RemoveValue(50);

            Assert.AreEqual(10, tree.Root.Item);
            Assert.AreEqual(5, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(20, tree.Root.Right.Left.Item);
            Assert.AreEqual(4, tree.Count);

            tree.RemoveValue(5);

            Assert.AreEqual(20, tree.Root.Item);
            Assert.AreEqual(10, tree.Root.Left.Item);
            Assert.AreEqual(25, tree.Root.Right.Item);
            Assert.AreEqual(3, tree.Count);

            CheckParent(null, tree.Root);
        }

        [TestMethod]
        public void RemoveRandom()
        {
            var rnd = new Random(0);
            var set = new HashSet<int>();

            for (int i = 0; i < 1000; i++)
                set.Add(rnd.Next());

            var tree = new AVLTree<int>(set);
            var list = new List<int>(set);

            for (int i = 0; i < 500; i++)
            {
                int j = list[i];
                list.Remove(j);
                tree.RemoveValue(j);
            }

            list.Sort();
            CollectionAssert.AreEqual(list, tree.ToList());
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

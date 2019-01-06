using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Trees;

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

        }
    }

}

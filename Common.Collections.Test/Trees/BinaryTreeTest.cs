using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Trees;

namespace Common.Collections.Test.Trees
{
    [TestClass]
    public class Collections_Trees_BinaryTreeTest
    {
        [TestMethod]
        public void Contains()
        {
            BinaryTree<string> tree = new BinaryTree<string>();

            IList<string> list = GetTestList();

            tree.Add(list);

            foreach(string name in list)
            {
                Assert.IsTrue(tree.Contains(name));
            }
        }

        [TestMethod]
        public void Add()
        {
            BinaryTree<string> tree = new BinaryTree<string>();

            IList<string> list = GetTestList();

            foreach (string name in list)
            {
                Assert.IsTrue(tree.Add(name));
            }

            Assert.AreEqual(tree.Count, list.Count);
        }

        [TestMethod]
        public void Order()
        {
            var tree = new BinaryTree<string>(GetTestList());

            Assert.AreEqual("George", tree.Root.Item);
            Assert.AreEqual("Adam", tree.Root.Left.Item);
            Assert.AreEqual("Daniel", tree.Root.Left.Right.Item);
            Assert.AreEqual("Michael", tree.Root.Right.Item);
            Assert.AreEqual("Jones", tree.Root.Right.Left.Item);
            Assert.AreEqual("Tom", tree.Root.Right.Right.Item);
            Assert.AreEqual("Peter", tree.Root.Right.Right.Left.Item);
        }

        [TestMethod]
        public void Path()
        {
            var tree = new BinaryTree<string>(GetTestList());
            var path = new List<string>();

            string[] expected;

            expected = new string[]
            {
                "George", "Michael", "Tom", "Peter"
            };

            path.Clear();
            tree.GetPath("Peter", path);
            CollectionAssert.AreEqual(expected, path);

            expected = new string[]
            {
                "George", "Michael", "Tom"
            };

            path.Clear();
            tree.GetPath("Tom", path);
            CollectionAssert.AreEqual(expected, path);

            expected = new string[]
            {
                "George", "Michael", "Jones"
            };

            path.Clear();
            tree.GetPath("Jones", path);
            CollectionAssert.AreEqual(expected, path);

            expected = new string[]
            {
                "George", "Adam"
            };

            path.Clear();
            tree.GetPath("Adam", path);
            CollectionAssert.AreEqual(expected, path);

            expected = new string[]
            {
                "George"
            };

            path.Clear();
            tree.GetPath("George", path);
            CollectionAssert.AreEqual(expected, path);
        }

        [TestMethod]
        public void Remove()
        {
            var tree = new BinaryTree<string>(GetTestList());

            tree.Remove("George");

            Assert.AreEqual("Daniel", tree.Root.Item);
            Assert.AreEqual("Adam", tree.Root.Left.Item);
            Assert.AreEqual("Michael", tree.Root.Right.Item);
            Assert.AreEqual("Jones", tree.Root.Right.Left.Item);
            Assert.AreEqual("Tom", tree.Root.Right.Right.Item);
            Assert.AreEqual("Peter", tree.Root.Right.Right.Left.Item);
            Assert.AreEqual(6, tree.Count);

            tree.Remove("Adam");

            Assert.AreEqual("Daniel", tree.Root.Item);
            Assert.AreEqual("Michael", tree.Root.Right.Item);
            Assert.AreEqual("Jones", tree.Root.Right.Left.Item);
            Assert.AreEqual("Tom", tree.Root.Right.Right.Item);
            Assert.AreEqual("Peter", tree.Root.Right.Right.Left.Item);
            Assert.AreEqual(5, tree.Count);

            tree.Remove("Michael");

            Assert.AreEqual("Daniel", tree.Root.Item);
            Assert.AreEqual("Jones", tree.Root.Right.Item);
            Assert.AreEqual("Tom", tree.Root.Right.Right.Item);
            Assert.AreEqual("Peter", tree.Root.Right.Right.Left.Item);
            Assert.AreEqual(4, tree.Count);
        }

        [TestMethod]
        public void ToList()
        {
            var list = GetTestList();
            var tree = new BinaryTree<string>(list);

            var sorted = new List<string>(list);
            sorted.Sort();

            CollectionAssert.AreEqual(tree.ToList(), sorted);
        }

        private string[] GetTestList()
        {

            string[] list = new string[]
            {
                "George", "Michael", "Tom", "Adam", "Jones", "Peter", "Daniel"
            };

            return list;
        }

    }
}

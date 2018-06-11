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

            BinaryTree<string> tree = new BinaryTree<string>(new StringComparer());

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

            BinaryTree<string> tree = new BinaryTree<string>(new StringComparer());

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

            BinaryTree<string> tree = new BinaryTree<string>(new StringComparer());

            IList<string> list = GetTestList();

            tree.Add(list);

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
            BinaryTree<string> tree = new BinaryTree<string>(new StringComparer());

            IList<string> list = GetTestList();
            tree.Add(list);

            string[] expected;

            expected = new string[]
            {
                "George", "Michael", "Tom", "Peter"
            };

            CollectionAssert.AreEqual(expected, tree.GetPath("Peter"));

            expected = new string[]
            {
                "George", "Michael", "Tom"
            };

            CollectionAssert.AreEqual(expected, tree.GetPath("Tom"));

            expected = new string[]
            {
                "George", "Michael", "Jones"
            };

            CollectionAssert.AreEqual(expected, tree.GetPath("Jones"));

            expected = new string[]
            {
                "George", "Adam"
            };

            CollectionAssert.AreEqual(expected, tree.GetPath("Adam"));

            expected = new string[]
            {
                "George"
            };

            CollectionAssert.AreEqual(expected, tree.GetPath("George"));

        }

        [TestMethod]
        public void Remove()
        {

            BinaryTree<string> tree = new BinaryTree<string>(new StringComparer());

            IList<string> list = GetTestList();

            tree.Add(list);

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

        private IList<string> GetTestList()
        {

            string[] list = new string[]
            {
                "George", "Michael", "Tom", "Adam", "Jones", "Peter", "Daniel"
            };

            return list;
        }

        private class StringComparer : IComparer<string>
        {
            public int Compare(string s0, string s1)
            {
                return s0.CompareTo(s1);
            }
        }

    }
}

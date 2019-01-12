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

            tree.Add("George");
            Assert.AreEqual("George", tree.Root.Item);
            Assert.AreEqual(1, tree.Count);

            tree.Add("Micheal");
            Assert.AreEqual("Micheal", tree.Root.Right.Item);
            Assert.AreEqual(2, tree.Count);

            tree.Add("Tom");
            Assert.AreEqual("Tom", tree.Root.Right.Right.Item);
            Assert.AreEqual(3, tree.Count);

            tree.Add("Adam");
            Assert.AreEqual("Adam", tree.Root.Left.Item);
            Assert.AreEqual(4, tree.Count);

            tree.Add("Jones");
            Assert.AreEqual("Jones", tree.Root.Right.Left.Item);
            Assert.AreEqual(5, tree.Count);

            tree.Add("Peter");
            Assert.AreEqual("Peter", tree.Root.Right.Right.Left.Item);
            Assert.AreEqual(6, tree.Count);

            tree.Add("Daniel");
            Assert.AreEqual("Daniel", tree.Root.Left.Right.Item);
            Assert.AreEqual(7, tree.Count);

        }

        [TestMethod]
        public void Order()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

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
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());
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
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

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
        public void ParentSet()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());
            CheckParent(null, tree.Root);

            tree.Remove("Adam");
            CheckParent(null, tree.Root);

            tree.Remove("Jones");
            CheckParent(null, tree.Root);

            tree.Remove("Michael");
            CheckParent(null, tree.Root);

            tree.Remove("Tom");
            CheckParent(null, tree.Root);

            tree.Remove("Peter");
            CheckParent(null, tree.Root);
        }

        [TestMethod]
        public void ToList()
        {
            var list = GetTestList();
            var tree = new BinaryTree<string>();
            tree.Add(list);

            var sorted = new List<string>(list);
            sorted.Sort();

            CollectionAssert.AreEqual(tree.ToList(), sorted);
        }

        [TestMethod]
        public void FindMinimum()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            string min = "";
            tree.FindMinimum(ref min);

            Assert.AreEqual("Adam", min);
        }

        [TestMethod]
        public void FindMaximum()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            string max = "";
            tree.FindMaximum(ref max);

            Assert.AreEqual("Tom", max);
        }

        [TestMethod]
        public void FindNode()
        {
            var list = GetTestList();
            var tree = new BinaryTree<string>();
            tree.Add(list);

            foreach(var name in list)
            {
                var node = tree.FindNode(name);
                Assert.AreEqual(name, node.Item);
            }
        }

        [TestMethod]
        public void FindSuccesor()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            string succesor = "";
            tree.FindSuccesor("George", ref succesor);
            Assert.AreEqual("Jones", succesor);

            succesor = "";
            tree.FindSuccesor("Peter", ref succesor);
            Assert.AreEqual("Tom", succesor);

            succesor = "";
            tree.FindSuccesor("Daniel", ref succesor);
            Assert.AreEqual("George", succesor);

            Assert.IsFalse(tree.FindSuccesor("Tom", ref succesor));
        }

        [TestMethod]
        public void FindPredeccesor()
        {
            var tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            string predeccesor = "";
            tree.FindPredecessor("George", ref predeccesor);
            Assert.AreEqual("Daniel", predeccesor);

            predeccesor = "";
            tree.FindPredecessor("Peter", ref predeccesor);
            Assert.AreEqual("Michael", predeccesor);

            predeccesor = "";
            tree.FindPredecessor("Daniel", ref predeccesor);
            Assert.AreEqual("Adam", predeccesor);

            Assert.IsFalse(tree.FindPredecessor("Adam", ref predeccesor));
        }

        [TestMethod]
        public void DepthFirst()
        {
            BinaryTree<string> tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            var list = new List<string>();
            tree.DepthFirst(list, tree.Root);

            CollectionAssert.AreEqual(new string[] { "George", "Adam", "Daniel", "Michael", "Jones", "Tom", "Peter" }, list);
        }

        [TestMethod]
        public void BreadthFirst()
        {
            BinaryTree<string> tree = new BinaryTree<string>();
            tree.Add(GetTestList());

            var list = new List<string>();
            tree.BreadthFirst(list, tree.Root);

            CollectionAssert.AreEqual(new string[] { "George", "Adam", "Michael", "Daniel", "Jones", "Tom", "Peter" }, list);
        }

        private string[] GetTestList()
        {
            string[] list = new string[]
            {
                "George", "Michael", "Tom", "Adam", "Jones", "Peter", "Daniel"
            };

            return list;
        }

        private void CheckParent<T>(BinaryTreeNode<T> parent, BinaryTreeNode<T> node)
        {
            if (node == null) return;

            Assert.AreEqual(parent, node.Parent);
            CheckParent(node, node.Left);
            CheckParent(node, node.Right);
        }

    }
}

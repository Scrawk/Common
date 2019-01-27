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
        public void Count()
        {
            BinaryTree<string> tree = new BinaryTree<string>();

            tree.Add("Joe");
            Assert.AreEqual(1, tree.Count);

            tree.Remove("Joe");
            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void Contains()
        {
            var tree = TestTree();
            var list = TestList();

            foreach (string name in list)
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
        public void Peek()
        {
            var tree = TestTree();

            Assert.AreEqual("Adam", tree.Peek());
        }

        [TestMethod]
        public void Order()
        {
            var tree = TestTree();

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
            var tree = TestTree();
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
            var tree = TestTree();

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
        public void RemoveFirst()
        {
            var tree = TestTree();

            Assert.AreEqual("Adam", tree.RemoveFirst());
            Assert.AreEqual("Daniel", tree.RemoveFirst());
            Assert.AreEqual("George", tree.RemoveFirst());
        }

        [TestMethod]
        public void Find()
        {
            var tree = TestTree();

            string key = "Daniel";
            string item;
            Assert.IsTrue(tree.Find(key, out item));
            Assert.AreEqual("Daniel", item);

            key = "Dave";
            Assert.IsFalse(tree.Find(key, out item));
        }

        [TestMethod]
        public void ParentSet()
        {
            var tree = TestTree();
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
            var tree = TestTree();

            var sorted = new List<string>(TestList());
            sorted.Sort();

            CollectionAssert.AreEqual(tree.ToList(), sorted);
        }

        [TestMethod]
        public void Enumerate()
        {
            var tree = TestTree();
            var list = new List<string>();
            foreach (var str in tree)
                list.Add(str);

            var sorted = new List<string>(TestList());
            sorted.Sort();

            CollectionAssert.AreEqual(list, sorted);
        }

        [TestMethod]
        public void FindMinimum()
        {
            var tree = TestTree();

            string min = "";
            tree.FindMinimum(out min);

            Assert.AreEqual("Adam", min);
        }

        [TestMethod]
        public void FindMaximum()
        {
            var tree = TestTree();

            string max = "";
            tree.FindMaximum(out max);

            Assert.AreEqual("Tom", max);
        }

        [TestMethod]
        public void FindNode()
        {
            var list = TestTree();
            var tree = TestTree();

            foreach (var name in list)
            {
                var node = tree.FindNode(name);
                Assert.AreEqual(name, node.Item);
            }
        }

        [TestMethod]
        public void FindSuccesor()
        {
            var tree = TestTree();

            string succesor = "";
            tree.FindSuccesor("George", out succesor);
            Assert.AreEqual("Jones", succesor);

            succesor = "";
            tree.FindSuccesor("Peter", out succesor);
            Assert.AreEqual("Tom", succesor);

            succesor = "";
            tree.FindSuccesor("Daniel", out succesor);
            Assert.AreEqual("George", succesor);

            Assert.IsFalse(tree.FindSuccesor("Tom", out succesor));
        }

        [TestMethod]
        public void FindPredeccesor()
        {
            var tree = TestTree();

            string predeccesor = "";
            tree.FindPredecessor("George", out predeccesor);
            Assert.AreEqual("Daniel", predeccesor);

            predeccesor = "";
            tree.FindPredecessor("Peter", out predeccesor);
            Assert.AreEqual("Michael", predeccesor);

            predeccesor = "";
            tree.FindPredecessor("Daniel", out predeccesor);
            Assert.AreEqual("Adam", predeccesor);

            Assert.IsFalse(tree.FindPredecessor("Adam", out predeccesor));
        }

        [TestMethod]
        public void DepthFirst()
        {
            var tree = TestTree();

            var list = new List<string>();
            tree.DepthFirst(list, tree.Root);

            CollectionAssert.AreEqual(new string[] { "George", "Adam", "Daniel", "Michael", "Jones", "Tom", "Peter" }, list);
        }

        [TestMethod]
        public void BreadthFirst()
        {
            var tree = TestTree();

            var list = new List<string>();
            tree.BreadthFirst(list, tree.Root);

            CollectionAssert.AreEqual(new string[] { "George", "Adam", "Michael", "Daniel", "Jones", "Tom", "Peter" }, list);
        }

        private BinaryTree<string> TestTree()
        {
            var tree = new BinaryTree<string>();
            tree.Add(TestList());

            return tree;
        }

        private string[] TestList()
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

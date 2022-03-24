using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.Test.AdjacencyGraphs
{
    [TestClass]
    public class GraphTreeTest
    {
        [TestMethod]
        public void GetPathToRoot()
        {
            var graph = AdjacencyGraphTest.CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);

            CollectionAssert.AreEquivalent(new int[] { 0, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Seattle")));
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("San Francisco")));
            CollectionAssert.AreEquivalent(new int[] { 2, 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Los Angeles")));
            CollectionAssert.AreEquivalent(new int[] { 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Denver")));
            CollectionAssert.AreEquivalent(new int[] { 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Kansas City")));
            CollectionAssert.AreEquivalent(new int[] { }, tree.GetPathToRoot(graph.IndexOfVertexData("Chicago")));
            CollectionAssert.AreEquivalent(new int[] { 6, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Boston")));
            CollectionAssert.AreEquivalent(new int[] { 7, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("New York")));
            CollectionAssert.AreEquivalent(new int[] { 8, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Atlanta")));
            CollectionAssert.AreEquivalent(new int[] { 9, 8, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Miami")));
            CollectionAssert.AreEquivalent(new int[] { 10, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Dallas")));
            CollectionAssert.AreEquivalent(new int[] { 11, 10, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Houston")));
        }

        [TestMethod]
        public void SetParent()
        {
            var graph = AdjacencyGraphTest.CreateCitiesDirectedGraph();

            //create tree manually
            var tree = new GraphTree(graph, 5);

            tree.SetParent(0, 5);
            tree.SetParent(1, 3);
            tree.SetParent(2, 3);
            tree.SetParent(3, 5);
            tree.SetParent(4, 5);
            tree.SetParent(5, 5);
            tree.SetParent(6, 5);
            tree.SetParent(7, 5);
            tree.SetParent(8, 4);
            tree.SetParent(9, 8);
            tree.SetParent(10, 4);
            tree.SetParent(11, 10);

            CollectionAssert.AreEquivalent(new int[] { 0, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Seattle")));
            CollectionAssert.AreEquivalent(new int[] { 1, 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("San Francisco")));
            CollectionAssert.AreEquivalent(new int[] { 2, 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Los Angeles")));
            CollectionAssert.AreEquivalent(new int[] { 3, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Denver")));
            CollectionAssert.AreEquivalent(new int[] { 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Kansas City")));
            CollectionAssert.AreEquivalent(new int[] { }, tree.GetPathToRoot(graph.IndexOfVertexData("Chicago")));
            CollectionAssert.AreEquivalent(new int[] { 6, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Boston")));
            CollectionAssert.AreEquivalent(new int[] { 7, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("New York")));
            CollectionAssert.AreEquivalent(new int[] { 8, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Atlanta")));
            CollectionAssert.AreEquivalent(new int[] { 9, 8, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Miami")));
            CollectionAssert.AreEquivalent(new int[] { 10, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Dallas")));
            CollectionAssert.AreEquivalent(new int[] { 11, 10, 4, 5 }, tree.GetPathToRoot(graph.IndexOfVertexData("Houston")));
        }

        [TestMethod]
        public void GetDegree()
        {
            var graph = AdjacencyGraphTest.CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);
            tree.CreateChildren();

            Assert.AreEqual(0, tree.GetDegree(0));
            Assert.AreEqual(0, tree.GetDegree(1));
            Assert.AreEqual(0, tree.GetDegree(2));
            Assert.AreEqual(2, tree.GetDegree(3));
            Assert.AreEqual(2, tree.GetDegree(4));
            Assert.AreEqual(5, tree.GetDegree(5));
            Assert.AreEqual(0, tree.GetDegree(6));
            Assert.AreEqual(0, tree.GetDegree(7));
            Assert.AreEqual(1, tree.GetDegree(8));
            Assert.AreEqual(0, tree.GetDegree(9));
            Assert.AreEqual(1, tree.GetDegree(10));
            Assert.AreEqual(0, tree.GetDegree(11));
        }

        [TestMethod]
        public void IsLeaf()
        {
            var graph = AdjacencyGraphTest.CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);
            tree.CreateChildren();

            Assert.AreEqual(true, tree.IsLeaf(0));
            Assert.AreEqual(true, tree.IsLeaf(1));
            Assert.AreEqual(true, tree.IsLeaf(2));
            Assert.AreEqual(false, tree.IsLeaf(3));
            Assert.AreEqual(false, tree.IsLeaf(4));
            Assert.AreEqual(false, tree.IsLeaf(5));
            Assert.AreEqual(true, tree.IsLeaf(6));
            Assert.AreEqual(true, tree.IsLeaf(7));
            Assert.AreEqual(false, tree.IsLeaf(8));
            Assert.AreEqual(true, tree.IsLeaf(9));
            Assert.AreEqual(false, tree.IsLeaf(10));
            Assert.AreEqual(true, tree.IsLeaf(11));
        }

        [TestMethod]
        public void RemoveBranch()
        {
            var graph = AdjacencyGraphTest.CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);
            tree.CreateChildren();

            tree.RemoveBranch(4);
            var ordering = tree.DepthFirstOrder();

            int[] order = new int[] { 5, 7, 6, 3, 2, 1, 0 };
            CollectionAssert.AreEquivalent(order, ordering);
        }

    }
}

﻿
using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;
using Common.GraphTheory.AdjacencyGraphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.GraphTheory.Test.AdjacencyGraphs
{
    [TestClass]
    public class AdjacencyGraphTest
    {

        [TestMethod]
        public void DepthFirstOrder()
        {
            var graph = CreateCitiesDirectedGraph();
            var ordering = graph.DepthFirstOrder(5);

            int[] order = new int[] { 5, 7, 8, 11, 10, 2, 1, 9, 6, 4, 3, 0 };

            CollectionAssert.AreEquivalent(order, ordering);
        }

        [TestMethod]
        public void BreadthFirstOrder()
        {
            var graph = CreateCitiesDirectedGraph();
            var ordering = graph.BreadthFirstOrder(5);

            int[] order = new int[] { 5, 0, 3, 4, 6, 7, 1, 2, 8, 10, 9, 11 };

            CollectionAssert.AreEquivalent(order, ordering);
        }


        [TestMethod]
        public void DijkstrasShortestPath()
        {
            var graph = CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);
            tree.CreateChildren();
            var ordering = tree.DepthFirstOrder();

            int[] order = new int[] { 5, 7, 6, 4, 10, 11, 8, 9, 3, 2, 1, 0 };

            CollectionAssert.AreEquivalent(order, ordering);

            Assert.AreEqual(2097, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Seattle"))));
            Assert.AreEqual(2270, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("San Francisco"))));
            Assert.AreEqual(2018, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Los Angeles"))));
            Assert.AreEqual(1003, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Denver"))));
            Assert.AreEqual(533, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Kansas City"))));
            Assert.AreEqual(0, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Chicago"))));
            Assert.AreEqual(983, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Boston"))));
            Assert.AreEqual(787, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("New York"))));
            Assert.AreEqual(1397, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Atlanta"))));
            Assert.AreEqual(2058, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Miami"))));
            Assert.AreEqual(1029, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Dallas"))));
            Assert.AreEqual(1268, tree.FindWeightSum(tree.GetPathToRoot(graph.IndexOfVertexData("Houston"))));
        }
       

        [TestMethod]
        public void PrimsMinimumSpanningTree()
        {
            var graph = CreateCitiesUndirectedGraph();
            var tree = graph.PrimsMinimumSpanningTree(0);
            tree.CreateChildren();
            var ordering = tree.DepthFirstOrder();

            int[] order = new int[] { 0, 1, 2, 3, 4, 10, 11, 8, 9, 5, 7, 6 };

            CollectionAssert.AreEquivalent(order, ordering);

            var sum = tree.FindWeightSum();
            Assert.AreEqual(6513, sum);
        }

        [TestMethod]
        public void KruskalsMinimumSpanningForest()
        {
            var graph = CreateCitiesUndirectedGraph();
            var forest = graph.KruskalsMinimumSpanningForest();

            Assert.AreEqual(1, forest.Count);

            double sum = 0;
            foreach(var tree in forest.Trees)
            {
                int[] order = new int[] { 9, 8, 10, 11, 4, 5, 7, 6, 3, 2, 1, 0 };

                tree.CreateChildren();
                sum += tree.FindWeightSum();

                var ordering = tree.DepthFirstOrder();

                CollectionAssert.AreEquivalent(order, ordering);
            }

            Assert.AreEqual(6513, sum);
        }

       
        [TestMethod]
        public void FordFulkersonMaxFlow()
        {
            int[,] graph = new int[,] 
            { 
             {0, 16, 13, 0, 0, 0},
             {0, 0, 10, 12, 0, 0},
             {0, 4, 0, 0, 14, 0},
             {0, 0, 9, 0, 0, 20},
             {0, 0, 0, 7, 0, 4},
             {0, 0, 0, 0, 0, 0}
            };

            var g = DirectedGraph.FromMatrix(graph);

            float max_flow = g.FordFulkersonMaxFlow(0, 5);
            Assert.AreEqual(23, max_flow);

            var cut = g.FordFulkersonMinCut(0, 5);

            max_flow = 0;
            foreach (var e in cut)
                max_flow += e.Weight;

            Assert.AreEqual(23, max_flow);

        }

        [TestMethod]
        public void KhansTopologicalSort()
        {
            var graph = new DirectedGraph(8);

            graph.GetVertex(0).Data = 7;
            graph.GetVertex(1).Data = 5;
            graph.GetVertex(2).Data = 3;
            graph.GetVertex(3).Data = 11;
            graph.GetVertex(4).Data = 8;
            graph.GetVertex(5).Data = 2;
            graph.GetVertex(6).Data = 9;
            graph.GetVertex(7).Data = 10;

            graph.AddDirectedEdge(1, 3);
            graph.AddDirectedEdge(0, 3);
            graph.AddDirectedEdge(0, 4);
            graph.AddDirectedEdge(2, 4);
            graph.AddDirectedEdge(2, 7);
            graph.AddDirectedEdge(4, 6);
            graph.AddDirectedEdge(3, 5);
            graph.AddDirectedEdge(3, 6);
            graph.AddDirectedEdge(3, 7);

            var sorted = graph.KhansTopologicalSort();

            var dependacies = new Dictionary<int, IList<int>>();

            dependacies.Add(7, new int[] { });
            dependacies.Add(5, new int[] { });
            dependacies.Add(3, new int[] { });
            dependacies.Add(11, new int[] { 7, 5 });
            dependacies.Add(8, new int[] { 7, 3 });
            dependacies.Add(2, new int[] { 11, 7, 5 });
            dependacies.Add(9, new int[] { 11, 7, 5, 8, 3 });
            dependacies.Add(10, new int[] { 11, 7, 5, 8, 3 });

            List<int> previous = new List<int>();

            int count = sorted.Count;
            for (int i = 0; i < count; i++)
            {
                int Id = (int)sorted[i].Data;

                IList<int> depList = dependacies[Id];
                foreach (int d in depList)
                {
                    if (!previous.Contains(d))
                        throw new Exception(Id + " Missing dependacy " + d);
                }

                previous.Add(Id);
            }
        }

        public static UndirectedGraph CreateCitiesUndirectedGraph()
        {
            var directed = CreateCitiesDirectedGraph();
            return directed.ToUndirectedGraph();
        }

        public static DirectedGraph CreateCitiesDirectedGraph()
        {
            var graph = new DirectedGraph(12);

            graph.GetVertex(0).Data = "Seattle";
            graph.GetVertex(1).Data = "San Francisco";
            graph.GetVertex(2).Data = "Los Angeles";
            graph.GetVertex(3).Data = "Denver";
            graph.GetVertex(4).Data = "Kansas City";
            graph.GetVertex(5).Data = "Chicago";
            graph.GetVertex(6).Data = "Boston";
            graph.GetVertex(7).Data = "New York";
            graph.GetVertex(8).Data = "Atlanta";
            graph.GetVertex(9).Data = "Miami";
            graph.GetVertex(10).Data = "Dallas";
            graph.GetVertex(11).Data = "Houston";

            graph.AddDirectedEdge(new GraphEdge(0, 1, 807));
            graph.AddDirectedEdge(new GraphEdge(0, 3, 1331));
            graph.AddDirectedEdge(new GraphEdge(0, 5, 2097));

            graph.AddDirectedEdge(new GraphEdge(1, 0, 807));
            graph.AddDirectedEdge(new GraphEdge(1, 2, 381));
            graph.AddDirectedEdge(new GraphEdge(1, 3, 1267));

            graph.AddDirectedEdge(new GraphEdge(2, 1, 381));
            graph.AddDirectedEdge(new GraphEdge(2, 3, 1015));
            graph.AddDirectedEdge(new GraphEdge(2, 4, 1663));
            graph.AddDirectedEdge(new GraphEdge(2, 10, 1435));

            graph.AddDirectedEdge(new GraphEdge(3, 0, 1331));
            graph.AddDirectedEdge(new GraphEdge(3, 1, 1267));
            graph.AddDirectedEdge(new GraphEdge(3, 2, 1015));
            graph.AddDirectedEdge(new GraphEdge(3, 4, 599));
            graph.AddDirectedEdge(new GraphEdge(3, 5, 1003));

            graph.AddDirectedEdge(new GraphEdge(4, 2, 1663));
            graph.AddDirectedEdge(new GraphEdge(4, 3, 599));
            graph.AddDirectedEdge(new GraphEdge(4, 5, 533));
            graph.AddDirectedEdge(new GraphEdge(4, 7, 1260));
            graph.AddDirectedEdge(new GraphEdge(4, 8, 864));
            graph.AddDirectedEdge(new GraphEdge(4, 10, 496));

            graph.AddDirectedEdge(new GraphEdge(5, 0, 2097));
            graph.AddDirectedEdge(new GraphEdge(5, 3, 1003));
            graph.AddDirectedEdge(new GraphEdge(5, 4, 533));
            graph.AddDirectedEdge(new GraphEdge(5, 6, 983));
            graph.AddDirectedEdge(new GraphEdge(5, 7, 787));

            graph.AddDirectedEdge(new GraphEdge(6, 5, 983));
            graph.AddDirectedEdge(new GraphEdge(6, 7, 214));

            graph.AddDirectedEdge(new GraphEdge(7, 4, 1260));
            graph.AddDirectedEdge(new GraphEdge(7, 5, 787));
            graph.AddDirectedEdge(new GraphEdge(7, 6, 214));
            graph.AddDirectedEdge(new GraphEdge(7, 8, 888));

            graph.AddDirectedEdge(new GraphEdge(8, 4, 864));
            graph.AddDirectedEdge(new GraphEdge(8, 7, 888));
            graph.AddDirectedEdge(new GraphEdge(8, 9, 661));
            graph.AddDirectedEdge(new GraphEdge(8, 10, 781));
            graph.AddDirectedEdge(new GraphEdge(8, 11, 810));

            graph.AddDirectedEdge(new GraphEdge(9, 8, 661));
            graph.AddDirectedEdge(new GraphEdge(9, 11, 1187));

            graph.AddDirectedEdge(new GraphEdge(10, 2, 1435));
            graph.AddDirectedEdge(new GraphEdge(10, 4, 496));
            graph.AddDirectedEdge(new GraphEdge(10, 8, 781));
            graph.AddDirectedEdge(new GraphEdge(10, 11, 239));

            graph.AddDirectedEdge(new GraphEdge(11, 8, 810));
            graph.AddDirectedEdge(new GraphEdge(11, 9, 1187));
            graph.AddDirectedEdge(new GraphEdge(11, 10, 239));

            return graph;
        }

        public static DirectedGraph CreateVectorDirectedGraph()
        {
            var graph = new DirectedGraph(13);

            graph.GetVertex(0).Data = new Vector2f(0,0);
            graph.GetVertex(1).Data = new Vector2f(0,2);
            graph.GetVertex(2).Data = new Vector2f(0,4);
            graph.GetVertex(3).Data = new Vector2f(0,6);
            graph.GetVertex(4).Data = new Vector2f(4,4);
            graph.GetVertex(5).Data = new Vector2f(3,0);
            graph.GetVertex(6).Data = new Vector2f(6,0);
            graph.GetVertex(7).Data = new Vector2f(3,-3);
            graph.GetVertex(8).Data = new Vector2f(-4,0);
            graph.GetVertex(9).Data = new Vector2f(-4,2);
            graph.GetVertex(10).Data = new Vector2f(-7,0);
            graph.GetVertex(11).Data = new Vector2f(-7,-3);
            graph.GetVertex(12).Data = new Vector2f(-4, -3);

            graph.AddUndirectedEdge(0, 1);
            graph.AddUndirectedEdge(1, 2);
            graph.AddUndirectedEdge(2, 3);
            graph.AddUndirectedEdge(2, 4);
            graph.AddDirectedEdge(5, 0);
            graph.AddUndirectedEdge(5, 6);
            graph.AddUndirectedEdge(5, 7);
            graph.AddUndirectedEdge(0, 8);
            graph.AddUndirectedEdge(8, 9);
            graph.AddUndirectedEdge(8, 10);
            graph.AddUndirectedEdge(8, 12);
            graph.AddUndirectedEdge(11, 12);
            graph.AddUndirectedEdge(10, 11);

            return graph;
        }

    }
}

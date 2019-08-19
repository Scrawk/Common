using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.GraphTheory.AdjacencyGraphs;

namespace Common.GraphTheory.Test.AdjacencyGraphs
{
    [TestClass]
    public class AdjacencyGraphTest
    {
        [TestMethod]
        public void DepthFirstSearch()
        {
            var graph = CreateCitiesDirectedGraph();
            var ordering = graph.DepthFirstOrder(5);

            int[] order = new int[] { 5, 7, 8, 11, 10, 2, 1, 9, 6, 4, 3, 0 };

            Assert.AreEqual(order.Length, ordering.Count);

            for (int i = 0; i < order.Length; i++)
                Assert.AreEqual(order[i], ordering.Vertices[i]);
        }

        [TestMethod]
        public void BreadthFirstSearch()
        {
            var graph = CreateCitiesDirectedGraph();
            var ordering = graph.BreadthFirstOrder(5);

            int[] order = new int[] { 5, 0, 3, 4, 6, 7, 1, 2, 8, 10, 9, 11 };

            Assert.AreEqual(order.Length, ordering.Count);

            for (int i = 0; i < order.Length; i++)
                Assert.AreEqual(order[i], ordering.Vertices[i]);
        }

        [TestMethod]
        public void DijkstrasShortestPath()
        {
            var graph = CreateCitiesDirectedGraph();
            var tree = graph.DijkstrasShortestPathTree(5);

        }
       

        [TestMethod]
        public void PrimsMinimumSpanningTree()
        {
            var graph = CreateCitiesUndirectedGraph();
            var tree = graph.PrimsMinimumSpanningTree(0);
            var ordering = tree.DepthFirstOrder();

            int[] order = new int[] { 0, 1, 2, 3, 4, 10, 11, 8, 9, 5, 7, 6 };

            Assert.AreEqual(order.Length, ordering.Count);

            var sum = graph.FindWeightSum(tree);
            Assert.AreEqual(6513, sum);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], ordering.Vertices[i]);
            }

        }

        [TestMethod]
        public void KruskalsMinimumSpanningForest()
        {
            var graph = CreateCitiesUndirectedGraph();
            var forest = graph.KruskalsMinimumSpanningForest();

            Assert.AreEqual(1, forest.Count);

            float sum = 0;
            foreach(var tree in forest.Trees)
            {
                int[] order = new int[] { 9, 8, 10, 11, 4, 5, 7, 6, 3, 2, 1, 0 };

                sum += graph.FindWeightSum(tree);

                var ordering = tree.DepthFirstOrder();

                for (int i = 0; i < order.Length; i++)
                {
                    Assert.AreEqual(order[i], ordering.Vertices[i]);
                }
            }

            Assert.AreEqual(6513, sum);
        }

        [TestMethod]
        public void KhansTopologicalSort()
        {
            var graph = new DirectedGraph<int>(8);

            graph.Vertices[0].Data = 7;
            graph.Vertices[1].Data = 5;
            graph.Vertices[2].Data = 3;
            graph.Vertices[3].Data = 11;
            graph.Vertices[4].Data = 8;
            graph.Vertices[5].Data = 2;
            graph.Vertices[6].Data = 9;
            graph.Vertices[7].Data = 10;

            graph.AddEdge(1, 3);
            graph.AddEdge(0, 3);
            graph.AddEdge(0, 4);
            graph.AddEdge(2, 4);
            graph.AddEdge(2, 7);
            graph.AddEdge(4, 6);
            graph.AddEdge(3, 5);
            graph.AddEdge(3, 6);
            graph.AddEdge(3, 7);

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
                int Id = sorted[i].Data;

                IList<int> depList = dependacies[Id];
                foreach (int d in depList)
                {
                    if (!previous.Contains(d))
                        throw new Exception(Id + " Missing dependacy " + d);
                }

                previous.Add(Id);
            }
        }

        private UndirectedGraph<string> CreateCitiesUndirectedGraph()
        {
            var directed = CreateCitiesDirectedGraph();
            return directed.ToUndirectedGraph();
        }

        private DirectedGraph<string> CreateCitiesDirectedGraph()
        {
            var graph = new DirectedGraph<string>(12);

            graph.Vertices[0].Data = "Seattle";
            graph.Vertices[1].Data = "San Fran";
            graph.Vertices[2].Data = "Los Angeles";
            graph.Vertices[3].Data = "Denver";
            graph.Vertices[4].Data = "Kansas";
            graph.Vertices[5].Data = "Chicago";
            graph.Vertices[6].Data = "Boston";
            graph.Vertices[7].Data = "New York";
            graph.Vertices[8].Data = "Atlanta";
            graph.Vertices[9].Data = "Miami";
            graph.Vertices[10].Data = "Dallas";
            graph.Vertices[11].Data = "Houston";

            graph.AddEdge(new GraphEdge(0, 1, 807));
            graph.AddEdge(new GraphEdge(0, 3, 1331));
            graph.AddEdge(new GraphEdge(0, 5, 2097));

            graph.AddEdge(new GraphEdge(1, 0, 807));
            graph.AddEdge(new GraphEdge(1, 2, 381));
            graph.AddEdge(new GraphEdge(1, 3, 1267));

            graph.AddEdge(new GraphEdge(2, 1, 381));
            graph.AddEdge(new GraphEdge(2, 3, 1015));
            graph.AddEdge(new GraphEdge(2, 4, 1663));
            graph.AddEdge(new GraphEdge(2, 10, 1435));

            graph.AddEdge(new GraphEdge(3, 0, 1331));
            graph.AddEdge(new GraphEdge(3, 1, 1267));
            graph.AddEdge(new GraphEdge(3, 2, 1015));
            graph.AddEdge(new GraphEdge(3, 4, 599));
            graph.AddEdge(new GraphEdge(3, 5, 1003));

            graph.AddEdge(new GraphEdge(4, 2, 1663));
            graph.AddEdge(new GraphEdge(4, 3, 599));
            graph.AddEdge(new GraphEdge(4, 5, 533));
            graph.AddEdge(new GraphEdge(4, 7, 1260));
            graph.AddEdge(new GraphEdge(4, 8, 864));
            graph.AddEdge(new GraphEdge(4, 10, 496));

            graph.AddEdge(new GraphEdge(5, 0, 2097));
            graph.AddEdge(new GraphEdge(5, 3, 1003));
            graph.AddEdge(new GraphEdge(5, 4, 533));
            graph.AddEdge(new GraphEdge(5, 6, 983));
            graph.AddEdge(new GraphEdge(5, 7, 787));

            graph.AddEdge(new GraphEdge(6, 5, 983));
            graph.AddEdge(new GraphEdge(6, 7, 214));

            graph.AddEdge(new GraphEdge(7, 4, 1260));
            graph.AddEdge(new GraphEdge(7, 5, 787));
            graph.AddEdge(new GraphEdge(7, 6, 214));
            graph.AddEdge(new GraphEdge(7, 8, 888));

            graph.AddEdge(new GraphEdge(8, 4, 864));
            graph.AddEdge(new GraphEdge(8, 7, 888));
            graph.AddEdge(new GraphEdge(8, 9, 661));
            graph.AddEdge(new GraphEdge(8, 10, 781));
            graph.AddEdge(new GraphEdge(8, 11, 810));

            graph.AddEdge(new GraphEdge(9, 8, 661));
            graph.AddEdge(new GraphEdge(9, 11, 1187));

            graph.AddEdge(new GraphEdge(10, 2, 1435));
            graph.AddEdge(new GraphEdge(10, 4, 496));
            graph.AddEdge(new GraphEdge(10, 8, 781));
            graph.AddEdge(new GraphEdge(10, 11, 239));

            graph.AddEdge(new GraphEdge(11, 8, 810));
            graph.AddEdge(new GraphEdge(11, 9, 1187));
            graph.AddEdge(new GraphEdge(11, 10, 239));

            return graph;
        }

    }
}

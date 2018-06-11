using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.GraphTheory.Adjacency;

namespace Common.GraphTheory.Test.Adjacency
{
    [TestClass]
    public class GraphTheory_Adjacency_AdjacencyGraphTest
    {
        [TestMethod]
        public void DepthFirstSearch()
        {

            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            graph.DepthFirstSearch(search, 5);

            int[] order = new int[] { 5, 7, 8, 11, 10, 2, 1, 9, 6, 4, 3, 0 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }
        }

        [TestMethod]
        public void BreadthFirstSearch()
        {

            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            graph.BreadthFirstSearch(search, 5);

            int[] order = new int[] { 5, 0, 3, 4, 6, 7, 1, 2, 8, 10, 9, 11 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }

        /*
        [TestMethod]
        public void DijkstrasShortestPath()
        {

            var graph = CreateGraph();
            var search = graph.DijkstrasShortestPathTree(5);

            int[] order = new int[] { 5, 4, 7, 6, 3, 10, 11, 8, 2, 9, 0, 1 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }
        */

        [TestMethod]
        public void PrimsMinimumSpanningTree()
        {
            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            graph.PrimsMinimumSpanningTree(search, 0, new AdjacencyEdgeComparer());

            int[] order = new int[] { 0, 1, 2, 3, 4, 10, 11, 5, 8, 9, 7, 6 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }

        [TestMethod]
        public void KhansTopologicalSort()
        {

            AdjacencyGraph<int> graph = new AdjacencyGraph<int>(8);

            graph.Vertices[0] = 7;
            graph.Vertices[1] = 5;
            graph.Vertices[2] = 3;
            graph.Vertices[3] = 11;
            graph.Vertices[4] = 8;
            graph.Vertices[5] = 2;
            graph.Vertices[6] = 9;
            graph.Vertices[7] = 10;

            graph.AddEdge(5, 11);
            graph.AddEdge(7, 11);
            graph.AddEdge(7, 8);
            graph.AddEdge(3, 8);
            graph.AddEdge(3, 10);
            graph.AddEdge(8, 9);
            graph.AddEdge(11, 2);
            graph.AddEdge(11, 9);
            graph.AddEdge(11, 10);

            List<int> sorted = graph.KhansTopologicalSort();

            Dictionary<int, IList<int>> dependacies = new Dictionary<int, IList<int>>();

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
                int Id = sorted[i];

                IList<int> depList = dependacies[Id];
                foreach (int d in depList)
                {
                    if (!previous.Contains(d))
                        throw new Exception(Id + " Missing dependacy " + d);
                }

                previous.Add(Id);

            }

        }

        [TestMethod]
        public void FoldFulkersonMaxFlow()
        {
         
            AdjacencyGraph<int> graph = new AdjacencyGraph<int>(6);

            graph.Vertices[0] = 0;
            graph.Vertices[1] = 1;
            graph.Vertices[2] = 2;
            graph.Vertices[3] = 3;
            graph.Vertices[4] = 4;
            graph.Vertices[5] = 5;

            graph.AddEdge(0, 1, 16);
            graph.AddEdge(0, 2, 13);
            graph.AddEdge(1, 3, 12);
            graph.AddEdge(1, 2, 10);
            graph.AddEdge(2, 1, 4);
            graph.AddEdge(2, 4, 14);
            graph.AddEdge(3, 2, 9);
            graph.AddEdge(3, 5, 20);
            graph.AddEdge(4, 3, 7);
            graph.AddEdge(4, 5, 4);

            int source = 0;
            int sink = 5;

            var flow = graph.FoldFulkersonMaxFlow(source, sink);

            Assert.AreEqual(23, flow.MaxFlow);

            Assert.IsTrue(flow.InSourceCut(0));
            Assert.IsTrue(flow.InSourceCut(1));
            Assert.IsTrue(flow.InSourceCut(2));
            Assert.IsTrue(flow.InSourceCut(4));

            Assert.IsTrue(flow.InSinkCut(3));
            Assert.IsTrue(flow.InSinkCut(5));

            float minCut = 0;
            foreach(var edges in flow.Edges)
            {
                if (edges == null) continue;

                foreach(var edge in edges)
                {
                    if (flow.InSourceCut(edge.From) && flow.InSinkCut(edge.To))
                        minCut += edge.Flow;
                }
            }

            Assert.AreEqual(23, minCut);

        }

        private AdjacencyGraph<string> CreateCitiesGraph()
        {
            AdjacencyGraph<string> graph = new AdjacencyGraph<string>(12);

            graph.Vertices[0] = "Seattle";
            graph.Vertices[1] = "San Fran";
            graph.Vertices[2] = "Los Angeles";
            graph.Vertices[3] = "Denver";
            graph.Vertices[4] = "Kansas";
            graph.Vertices[5] = "Chicago";
            graph.Vertices[6] = "Boston";
            graph.Vertices[7] = "New York";
            graph.Vertices[8] = "Atlanta";
            graph.Vertices[9] = "Miami";
            graph.Vertices[10] = "Dallas";
            graph.Vertices[11] = "Houston";

            graph.AddEdge(new AdjacencyEdge(0, 1, 807));
            graph.AddEdge(new AdjacencyEdge(0, 3, 1331));
            graph.AddEdge(new AdjacencyEdge(0, 5, 2097));

            graph.AddEdge(new AdjacencyEdge(1, 0, 807));
            graph.AddEdge(new AdjacencyEdge(1, 2, 381));
            graph.AddEdge(new AdjacencyEdge(1, 3, 1267));

            graph.AddEdge(new AdjacencyEdge(2, 1, 381));
            graph.AddEdge(new AdjacencyEdge(2, 3, 1015));
            graph.AddEdge(new AdjacencyEdge(2, 4, 1663));
            graph.AddEdge(new AdjacencyEdge(2, 10, 1435));

            graph.AddEdge(new AdjacencyEdge(3, 0, 1331));
            graph.AddEdge(new AdjacencyEdge(3, 1, 1267));
            graph.AddEdge(new AdjacencyEdge(3, 2, 1015));
            graph.AddEdge(new AdjacencyEdge(3, 4, 599));
            graph.AddEdge(new AdjacencyEdge(3, 5, 1003));

            graph.AddEdge(new AdjacencyEdge(4, 2, 1663));
            graph.AddEdge(new AdjacencyEdge(4, 3, 599));
            graph.AddEdge(new AdjacencyEdge(4, 5, 533));
            graph.AddEdge(new AdjacencyEdge(4, 7, 1260));
            graph.AddEdge(new AdjacencyEdge(4, 8, 864));
            graph.AddEdge(new AdjacencyEdge(4, 10, 496));

            graph.AddEdge(new AdjacencyEdge(5, 0, 2097));
            graph.AddEdge(new AdjacencyEdge(5, 3, 1003));
            graph.AddEdge(new AdjacencyEdge(5, 4, 533));
            graph.AddEdge(new AdjacencyEdge(5, 6, 983));
            graph.AddEdge(new AdjacencyEdge(5, 7, 787));

            graph.AddEdge(new AdjacencyEdge(6, 5, 983));
            graph.AddEdge(new AdjacencyEdge(6, 7, 214));

            graph.AddEdge(new AdjacencyEdge(7, 4, 1260));
            graph.AddEdge(new AdjacencyEdge(7, 5, 787));
            graph.AddEdge(new AdjacencyEdge(7, 6, 214));
            graph.AddEdge(new AdjacencyEdge(7, 8, 888));

            graph.AddEdge(new AdjacencyEdge(8, 4, 864));
            graph.AddEdge(new AdjacencyEdge(8, 7, 888));
            graph.AddEdge(new AdjacencyEdge(8, 9, 661));
            graph.AddEdge(new AdjacencyEdge(8, 10, 781));
            graph.AddEdge(new AdjacencyEdge(8, 11, 810));

            graph.AddEdge(new AdjacencyEdge(9, 8, 661));
            graph.AddEdge(new AdjacencyEdge(9, 11, 1187));

            graph.AddEdge(new AdjacencyEdge(10, 2, 1435));
            graph.AddEdge(new AdjacencyEdge(10, 4, 496));
            graph.AddEdge(new AdjacencyEdge(10, 8, 781));
            graph.AddEdge(new AdjacencyEdge(10, 11, 239));

            graph.AddEdge(new AdjacencyEdge(11, 8, 810));
            graph.AddEdge(new AdjacencyEdge(11, 9, 1187));
            graph.AddEdge(new AdjacencyEdge(11, 10, 239));

            return graph;
        }

    }
}

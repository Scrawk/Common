using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Meshing.AdjacencyGraphs;

namespace Common.Meshing.Test.AdjacencyGraphs
{
    [TestClass]
    public class AdjacencyGraphTest
    {
        [TestMethod]
        public void DepthFirstSearch()
        {

            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            AdjacencyGraphSearch.DepthFirst(graph, search, 5);

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
            AdjacencyGraphSearch.BreadthFirst(graph, search, 5);

            int[] order = new int[] { 5, 0, 3, 4, 6, 7, 1, 2, 8, 10, 9, 11 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }

        [TestMethod]
        public void DijkstrasShortestPath()
        {

            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            AdjacencyGraphSearch.DijkstrasShortestPathTree(graph, search, 5);

            int[] order = new int[] { 5, 4, 7, 6, 3, 10, 11, 8, 2, 9, 0, 1 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < search.Order.Count; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }
       

        [TestMethod]
        public void PrimsMinimumSpanningTree()
        {
            var graph = CreateCitiesGraph();
            var search = new AdjacencySearch(graph.VertexCount);
            AdjacencyGraphSearch.PrimsMinimumSpanningTree(graph, search, 0);

            int[] order = new int[] { 0, 1, 2, 3, 4, 10, 11, 5, 8, 9, 7, 6 };

            Assert.AreEqual(order.Length, search.Order.Count);

            for (int i = 0; i < order.Length; i++)
            {
                Assert.AreEqual(order[i], search.Order[i]);
            }

        }

        [TestMethod]
        public void KruskalsMinimumSpanningForest()
        {
            var graph = CreateCitiesGraph();
            
            var forest = AdjacencyGraphSearch.KruskalsMinimumSpanningForest(graph);

            foreach (var kvp in forest)
            {
                float sum = 0;
                foreach (var edge in kvp.Value)
                    sum += edge.Weight;

                Assert.AreEqual(6513, sum);
            }
        }


        [TestMethod]
        public void KhansTopologicalSort()
        {

            var graph = new AdjacencyDataGraph<int>(8);

            graph.Vertices[0].Data = 7;
            graph.Vertices[1].Data = 5;
            graph.Vertices[2].Data = 3;
            graph.Vertices[3].Data = 11;
            graph.Vertices[4].Data = 8;
            graph.Vertices[5].Data = 2;
            graph.Vertices[6].Data = 9;
            graph.Vertices[7].Data = 10;

            var v = graph.Vertices;

            graph.AddEdge(v[1], v[3]);
            graph.AddEdge(v[0], v[3]);
            graph.AddEdge(v[0], v[4]);
            graph.AddEdge(v[2], v[4]);
            graph.AddEdge(v[2], v[7]);
            graph.AddEdge(v[4], v[6]);
            graph.AddEdge(v[3], v[5]);
            graph.AddEdge(v[3], v[6]);
            graph.AddEdge(v[3], v[7]);

            var sorted = AdjacencyGraphSearch.KhansTopologicalSort(graph);

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

        [TestMethod]
        public void FoldFulkersonMaxFlow()
        {
         
            var graph = new AdjacencyDataGraph<int>(6);

            graph.Vertices[0].Data = 0;
            graph.Vertices[1].Data = 1;
            graph.Vertices[2].Data = 2;
            graph.Vertices[3].Data = 3;
            graph.Vertices[4].Data = 4;
            graph.Vertices[5].Data = 5;

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

            var flow = AdjacencyGraphSearch.FordFulkersonMaxFlow(graph, source, sink);

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
   
        private AdjacencyDataGraph<string> CreateCitiesGraph()
        {
            var graph = new AdjacencyDataGraph<string>(12);

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

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBVertexTest
    {
        [TestMethod]
        public void EdgeCount()
        {
            var mesh = HBMeshHelper.CreateCross();
            var vertex = mesh.Vertices[4];

            Assert.AreEqual(4, vertex.EdgeCount);
        }

        [TestMethod]
        public void EmptyEnumerateEdges()
        {
            var vertex = new HBVertex2f();
            var edges = new List<HBEdge>();

            foreach (var edge in vertex.EnumerateEdges(true))
                edges.Add(edge);

            Assert.AreEqual(0, edges.Count);

            vertex = new HBVertex2f();
            edges = new List<HBEdge>();

            foreach (var edge in vertex.EnumerateEdges(false))
                edges.Add(edge);

            Assert.AreEqual(0, edges.Count);
        }

        /// <summary>
        /// See Cross.png
        /// </summary>
        [TestMethod]
        public void EnumerateEdges()
        {
            var mesh = HBMeshHelper.CreateCross();
            var vertex = mesh.Vertices[4];
            var edges = new List<HBEdge>();

            foreach (var edge in vertex.EnumerateEdges(true))
                edges.Add(edge);

            Assert.AreEqual(4, edges.Count);
            Assert.AreEqual(mesh.Edges[0], edges[0]);
            Assert.AreEqual(mesh.Edges[6], edges[1]);
            Assert.AreEqual(mesh.Edges[4], edges[2]);
            Assert.AreEqual(mesh.Edges[2], edges[3]);

            mesh = HBMeshHelper.CreateCross();
            vertex = mesh.Vertices[4];
            edges = new List<HBEdge>();

            foreach (var edge in vertex.EnumerateEdges(false))
                edges.Add(edge);

            Assert.AreEqual(4, edges.Count);
            Assert.AreEqual(mesh.Edges[0], edges[0]);
            Assert.AreEqual(mesh.Edges[2], edges[1]);
            Assert.AreEqual(mesh.Edges[4], edges[2]);
            Assert.AreEqual(mesh.Edges[6], edges[3]);
        }

    }
}

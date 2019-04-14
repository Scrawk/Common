using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBMeshTest
    {
        [TestMethod]
        public void Clear()
        {
            var mesh = HBMeshHelper.CreateSquareWithCenter();

            Assert.AreEqual(4, mesh.Faces.Count);
            Assert.AreEqual(5, mesh.Vertices.Count);
            Assert.AreEqual(12, mesh.Edges.Count);

            mesh.Clear();

            Assert.AreEqual(0, mesh.Faces.Count);
            Assert.AreEqual(0, mesh.Vertices.Count);
            Assert.AreEqual(0, mesh.Edges.Count);
        }

        [TestMethod]
        public void RemoveFaces()
        {

            var mesh = HBMeshHelper.CreateSquareWithCenter();

            Assert.AreEqual(4, mesh.Faces.Count);
            mesh.RemoveFaces();
            Assert.AreEqual(0, mesh.Faces.Count);
            Assert.AreEqual(12, mesh.Edges.Count);

            foreach (var edge in mesh.Edges)
                Assert.IsNull(edge.Face);
        }

        [TestMethod]
        public void Fill()
        {
            var mesh = new HBMesh2f();
            mesh.Fill(3, 3, 3);

            Assert.AreEqual(3, mesh.Vertices.Count);
            Assert.AreEqual(3, mesh.Edges.Count);
            Assert.AreEqual(3, mesh.Faces.Count);
        }

        [TestMethod]
        public void IndexOf()
        {
            var mesh = new HBMesh2f();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var e0 = mesh.NewEdge();
            var e1 = mesh.NewEdge();
            var e2 = mesh.NewEdge();

            var f0 = mesh.NewFace();
            var f1 = mesh.NewFace();
            var f2 = mesh.NewFace();

            Assert.AreEqual(0, mesh.IndexOf(v0));
            Assert.AreEqual(1, mesh.IndexOf(v1));
            Assert.AreEqual(2, mesh.IndexOf(v2));

            Assert.AreEqual(0, mesh.IndexOf(e0));
            Assert.AreEqual(1, mesh.IndexOf(e1));
            Assert.AreEqual(2, mesh.IndexOf(e2));

            Assert.AreEqual(0, mesh.IndexOf(f0));
            Assert.AreEqual(1, mesh.IndexOf(f1));
            Assert.AreEqual(2, mesh.IndexOf(f2));
        }

        [TestMethod]
        public void FindEdge()
        {
            var mesh = new HBMesh2f();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();
            var v3 = mesh.NewVertex();

            var e0 = mesh.NewEdge();
            var e1 = mesh.NewEdge();

            e0.Opposite = e1;
            e1.Opposite = e0;

            e0.From = v0;
            e1.From = v1;

            Assert.AreEqual(e0, mesh.FindEdge(v0, v1));
            Assert.AreEqual(e1, mesh.FindEdge(v1, v0));
        }

    }
}

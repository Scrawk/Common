using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class HBMeshTest
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

            var v0 = new HBVertex2f();
            var v1 = new HBVertex2f();
            var v2 = new HBVertex2f();
            mesh.Vertices.Add(v0, v1, v2);

            var e0 = new HBEdge();
            var e1 = new HBEdge();
            var e2 = new HBEdge();
            mesh.Edges.Add(e0, e1, e2);

            var f0 = new HBFace();
            var f1 = new HBFace();
            var f2 = new HBFace();
            mesh.Faces.Add(f0, f1, f2);

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

            var v0 = new HBVertex2f();
            var v1 = new HBVertex2f();
            var v2 = new HBVertex2f();
            var v3 = new HBVertex2f();
            mesh.Vertices.Add(v0, v1, v2, v3);

            var e0 = new HBEdge();
            var e1 = new HBEdge();
            mesh.Edges.Add(e0, e1);

            e0.Opposite = e1;
            e1.Opposite = e0;

            e0.From = v0;
            e1.From = v1;

            Assert.AreEqual(e0, mesh.FindEdge(v0, v1));
            Assert.AreEqual(e1, mesh.FindEdge(v1, v0));
        }

    }
}

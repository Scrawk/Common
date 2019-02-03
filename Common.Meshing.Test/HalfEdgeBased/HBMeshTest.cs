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

        [TestMethod]
        public void AddBoundaryEdges()
        {
            var mesh = HBMeshHelper.CreateTriangle();

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: -1);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);

            mesh.AddBoundaryEdges();

            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 1, face: -1, previous: 4, next: 5, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 2, face: -1, previous: 5, next: 3, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: -1, previous: 3, next: 4, opposite: 2);
        }

        [TestMethod]
        public void AppendMesh()
        {
            var mesh0 = HBMeshHelper.CreateTriangle();
            var mesh1 = HBMeshHelper.CreateTriangle();

            var mesh = new HBMesh<HBVertex, HBEdge, HBFace>();

            mesh.AppendMesh(mesh0);
            mesh.AppendMesh(mesh1);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: -1);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);

            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 4, edge: 4);
            HBMeshHelper.CheckVertex(mesh, vertex: 5, edge: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 3, face: 1, previous: 5, next: 4, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 4, face: 1, previous: 3, next: 5, opposite: -1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 5, face: 1, previous: 4, next: 3, opposite: -1);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 3);

            HBMeshHelper.PrintMesh(mesh);

        }

    }
}

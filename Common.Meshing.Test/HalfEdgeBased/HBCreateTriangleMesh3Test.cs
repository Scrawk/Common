using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBCreateTriangleMesh3Test
    {
        [TestMethod]
        public void FromTriangle()
        {
            var a = new Vector3d(-1, -1, 0);
            var b = new Vector3d(1, -1, 0);
            var c = new Vector3d(0, 1, 0);
            var mesh = HBCreateTriangleMesh3.FromTriangle(a, b, c);

            Assert.AreEqual(3, mesh.Vertices.Count);
            Assert.AreEqual(6, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
            Assert.AreEqual(a, mesh.Vertices[0].Position);
            Assert.AreEqual(b, mesh.Vertices[1].Position);
            Assert.AreEqual(c, mesh.Vertices[2].Position);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 1, face: -1, previous: 4, next: 5, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 2, face: -1, previous: 5, next: 3, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: -1, previous: 3, next: 4, opposite: 2);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);

            //HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

        [TestMethod]
        public void FromTetrahedron()
        {
            var a = new Vector3d(-1, -1, 0);
            var b = new Vector3d(1, -1, 0);
            var c = new Vector3d(0, 1, 0);
            var d = new Vector3d(0, 0, 1);
            var mesh = HBCreateTriangleMesh3.FromTetrahedron(a, b, c, d);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(4 * 3, mesh.Edges.Count);
            Assert.AreEqual(4, mesh.Faces.Count);

            //HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

        [TestMethod]
        public void FromBox()
        {
            var min = new Vector3d(-1);
            var max = new Vector3d(1);
            var mesh = HBCreateTriangleMesh3.FromBox(min, max);

            Assert.AreEqual(8, mesh.Vertices.Count);
            Assert.AreEqual(12*3, mesh.Edges.Count);
            Assert.AreEqual(12, mesh.Faces.Count);

            //HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

        [TestMethod]
        public void FromIcosahedron()
        {
            var mesh = HBCreateTriangleMesh3.FromIcosahedron(1);

            Assert.AreEqual(12, mesh.Vertices.Count);
            Assert.AreEqual(20 * 3, mesh.Edges.Count);
            Assert.AreEqual(20, mesh.Faces.Count);

            //HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

    }
}

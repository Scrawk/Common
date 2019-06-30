using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;


namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBCreateTriangularMesh2Test
    { 
        [TestMethod]
        public void FromTriangle()
        {
            var a = new Vector2d(-1, -1);
            var b = new Vector2d(1, -1);
            var c = new Vector2d(0, 1);
            var mesh = HBCreateTriangularMesh2.FromTriangle(a, b, c);

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

            HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

        [TestMethod]
        public void FromBox()
        {
            var min = new Vector2d(-1, -1);
            var max = new Vector2d(1, 1);
            var mesh = HBCreateTriangularMesh2.FromBox(min, max);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(10, mesh.Edges.Count);
            Assert.AreEqual(2, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2d(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2d(min.x, max.y), mesh.Vertices[3].Position);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 5);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 2, face: 1, previous: 5, next: 4, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 3, face: 1, previous: 3, next: 5, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: 1, previous: 4, next: 3, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 1, face: -1, previous: 7, next: 9, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 2, face: -1, previous: 8, next: 6, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 3, face: -1, previous: 9, next: 7, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 0, face: -1, previous: 6, next: 8, opposite: 4);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 3);

            HBMeshHelper.CheckAllTrianglesCCW(mesh);
        }

        [TestMethod]
        public void FromCircle()
        {
            var mesh = HBCreateTriangularMesh2.FromCircle(Vector2d.Zero, 1.0f, 4);

            Assert.AreEqual(5, mesh.Vertices.Count);
            Assert.AreEqual(16, mesh.Edges.Count);
            Assert.AreEqual(4, mesh.Faces.Count);

            HBMeshHelper.CheckAllTrianglesCCW(mesh);

        }

        [TestMethod]
        public void FromGrid()
        {

            var mesh = HBCreateTriangularMesh2.FromGrid(3, 3, 1.0);

            Assert.AreEqual(9, mesh.Vertices.Count);
            Assert.AreEqual(32, mesh.Edges.Count);
            Assert.AreEqual(8, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 9);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 10);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 15);
            HBMeshHelper.CheckVertex(mesh, vertex: 4, edge: 21);
            HBMeshHelper.CheckVertex(mesh, vertex: 5, edge: 22);
            HBMeshHelper.CheckVertex(mesh, vertex: 6, edge: 14);
            HBMeshHelper.CheckVertex(mesh, vertex: 7, edge: 20);
            HBMeshHelper.CheckVertex(mesh, vertex: 8, edge: 23);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 4, face: 0, previous: 0, next: 2, opposite: 15);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 3, face: 0, previous: 1, next: 0, opposite: 24);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 0, face: 1, previous: 5, next: 4, opposite: 25);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 1, face: 1, previous: 3, next: 5, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 4, face: 1, previous: 4, next: 3, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 1, face: 2, previous: 8, next: 7, opposite: 11);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 5, face: 2, previous: 6, next: 8, opposite: 21);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 4, face: 2, previous: 7, next: 6, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 1, face: 3, previous: 11, next: 10, opposite: 26);
            HBMeshHelper.CheckEdge(mesh, edge: 10, from: 2, face: 3, previous: 9, next: 11, opposite: 27);
            HBMeshHelper.CheckEdge(mesh, edge: 11, from: 5, face: 3, previous: 10, next: 9, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 12, from: 3, face: 4, previous: 14, next: 13, opposite: 17);
            HBMeshHelper.CheckEdge(mesh, edge: 13, from: 7, face: 4, previous: 12, next: 14, opposite: 28);
            HBMeshHelper.CheckEdge(mesh, edge: 14, from: 6, face: 4, previous: 13, next: 12, opposite: 29);
            HBMeshHelper.CheckEdge(mesh, edge: 15, from: 3, face: 5, previous: 17, next: 16, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 16, from: 4, face: 5, previous: 15, next: 17, opposite: 20);
            HBMeshHelper.CheckEdge(mesh, edge: 17, from: 7, face: 5, previous: 16, next: 15, opposite: 12);
            HBMeshHelper.CheckEdge(mesh, edge: 18, from: 4, face: 6, previous: 20, next: 19, opposite: 23);
            HBMeshHelper.CheckEdge(mesh, edge: 19, from: 8, face: 6, previous: 18, next: 20, opposite: 30);
            HBMeshHelper.CheckEdge(mesh, edge: 20, from: 7, face: 6, previous: 19, next: 18, opposite: 16);
            HBMeshHelper.CheckEdge(mesh, edge: 21, from: 4, face: 7, previous: 23, next: 22, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 22, from: 5, face: 7, previous: 21, next: 23, opposite: 31);
            HBMeshHelper.CheckEdge(mesh, edge: 23, from: 8, face: 7, previous: 22, next: 21, opposite: 18);
            HBMeshHelper.CheckEdge(mesh, edge: 24, from: 0, face: -1, previous: 25, next: 29, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 25, from: 1, face: -1, previous: 26, next: 24, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 26, from: 2, face: -1, previous: 27, next: 25, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 27, from: 5, face: -1, previous: 31, next: 26, opposite: 10);
            HBMeshHelper.CheckEdge(mesh, edge: 28, from: 6, face: -1, previous: 29, next: 30, opposite: 13);
            HBMeshHelper.CheckEdge(mesh, edge: 29, from: 3, face: -1, previous: 24, next: 28, opposite: 14);
            HBMeshHelper.CheckEdge(mesh, edge: 30, from: 7, face: -1, previous: 28, next: 31, opposite: 19);
            HBMeshHelper.CheckEdge(mesh, edge: 31, from: 8, face: -1, previous: 30, next: 27, opposite: 22);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 3);
            HBMeshHelper.CheckFace(mesh, face: 2, edge: 6);
            HBMeshHelper.CheckFace(mesh, face: 3, edge: 9);
            HBMeshHelper.CheckFace(mesh, face: 4, edge: 12);
            HBMeshHelper.CheckFace(mesh, face: 5, edge: 15);
            HBMeshHelper.CheckFace(mesh, face: 6, edge: 18);
            HBMeshHelper.CheckFace(mesh, face: 7, edge: 21);

        }

    }
}

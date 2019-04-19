﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;
using Common.Meshing.Test.HalfEdgeBased;

namespace Common.Meshing.Test.Constructors
{
    [TestClass]
    public class Meshing_Constructors_CreateGeneralMesh2Test
    {
        [TestMethod]
        public void FromTriangle()
        {

            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);
            var mesh = CreateGeneralMesh2.FromTriangle(a, b, c);

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
        public void FromBox2f()
        {
            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            var mesh = CreateGeneralMesh2.FromBox(min, max);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(8, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2f(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2f(min.x, max.y), mesh.Vertices[3].Position);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 3, next: 1, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 3, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 3, face: 0, previous: 2, next: 0, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 1, face: -1, previous: 5, next: 7, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 2, face: -1, previous: 6, next: 4, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 3, face: -1, previous: 7, next: 5, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 0, face: -1, previous: 4, next: 6, opposite: 3);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);
        }

        [TestMethod]
        public void FromBox2d()
        {
            var min = new Vector2d(-1, -1);
            var max = new Vector2d(1, 1);

            var mesh = CreateGeneralMesh2.FromBox(min, max);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(8, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2d(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2d(min.x, max.y), mesh.Vertices[3].Position);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 3, next: 1, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 3, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 3, face: 0, previous: 2, next: 0, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 1, face: -1, previous: 5, next: 7, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 2, face: -1, previous: 6, next: 4, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 3, face: -1, previous: 7, next: 5, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 0, face: -1, previous: 4, next: 6, opposite: 3);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);
        }

        [TestMethod]
        public void FromCircle()
        {
            var mesh = CreateGeneralMesh2.FromCircle(Vector2f.Zero, 1.0f, 4);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(8, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
        }
    }
}

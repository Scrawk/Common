﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.FaceBased;
using Common.Meshing.Constructors;
using Common.Meshing.Test.HalfEdgeBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class Meshing_FaceBased_FBMesh2fTest
    {

        [TestMethod]
        public void GetPositions()
        {
            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);

            var mesh = new FBMesh2f();
            mesh.Vertices.Add(new FBVertex2f(a));
            mesh.Vertices.Add(new FBVertex2f(b));
            mesh.Vertices.Add(new FBVertex2f(c));

            var positions = new List<Vector2f>();
            mesh.GetPositions(positions);

            Assert.AreEqual(positions[0], a);
            Assert.AreEqual(positions[1], b);
            Assert.AreEqual(positions[2], c);
        }

        [TestMethod]
        public void ToHBTriangleMesh2f()
        {
            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            var constructor = new FBMeshConstructor2f();
            CreateTriangleMesh2.FromBox(constructor, min, max);
            var mesh = constructor.PopMesh().ToHBTriangleMesh2f();

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(10, mesh.Edges.Count);
            Assert.AreEqual(2, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2f(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2f(min.x, max.y), mesh.Vertices[3].Position);

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
    }
}

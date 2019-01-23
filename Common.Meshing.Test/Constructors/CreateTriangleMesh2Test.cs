using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Geometry.Shapes;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;
using Common.Meshing.Test.HalfEdgeBased;

namespace Common.Meshing.Test.Constructors
{
    [TestClass]
    public class Meshing_Constructors_CreateTriangleMesh2Test
    { 
        [TestMethod]
        public void FromTriangle()
        {
            var constructor = new HBMeshConstructor2f();

            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);

            CreateTriangleMesh2.FromTriangle(constructor, a, b, c);
            var mesh = constructor.PopMesh();

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
            var constructor = new HBMeshConstructor2f();

            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            CreateTriangleMesh2.FromBox(constructor, min, max);
            var mesh = constructor.PopMesh();

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

        [TestMethod]
        public void FromCircle()
        {
            var constructor = new HBMeshConstructor2f();

            CreateTriangleMesh2.FromCircle(constructor, Vector2f.Zero, 1.0f, 4);

            constructor.AddBoundary = true;
            var mesh = constructor.PopMesh();

            Assert.AreEqual(5, mesh.Vertices.Count);
            Assert.AreEqual(16, mesh.Edges.Count);
            Assert.AreEqual(4, mesh.Faces.Count);

            HBMeshHelper.CheckAllTrianglesCCW(mesh);

        }

    }
}

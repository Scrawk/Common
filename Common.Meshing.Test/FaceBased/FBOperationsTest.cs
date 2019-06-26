using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.FaceBased;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;
using Common.Meshing.Test.HalfEdgeBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class Meshing_FaceBased_FBOperationsTest
    {
        [TestMethod]
        public void ToHBTriangleMesh()
        {
            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            var constructor1 = new FBMeshConstructor2f();
            CreateTriangularMesh2.FromBox(constructor1, min, max);
            var tmp = constructor1.PopMesh();

            var constructor2 = new HBMeshConstructor2f();
            FBOperations.ToTriangularMesh(constructor2, tmp);
            var mesh = constructor2.PopMesh();

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

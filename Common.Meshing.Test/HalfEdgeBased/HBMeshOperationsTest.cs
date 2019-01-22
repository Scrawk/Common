using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBMeshOperationsTest
    {
        [TestMethod]
        public void SplitEdge()
        {
            var a = new Vector2f(0, 1);
            var b = new Vector2f(-1, -1);
            var c = new Vector2f(1, -1);

            var constructor = new HBMeshConstructor2f();
            CreateTriangleMesh2.FromTriangle(constructor, a, b, c);
            var mesh = constructor.PopMesh();

            HBMeshOperations.SplitEdge(mesh, mesh.Edges[0]);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 7);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 6, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 6, next: 2, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 3, face: -1, previous: 7, next: 5, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 2, face: -1, previous: 5, next: 7, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: -1, previous: 3, next: 4, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 3, face: 0, previous: 0, next: 1, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 1, face: -1, previous: 4, next: 3, opposite: 6);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);

        }
    }
}

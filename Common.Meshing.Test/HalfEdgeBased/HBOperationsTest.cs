using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;
using Common.Meshing.FaceBased;

using Common.Meshing.Test.FaceBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBOperationsTest
    {

        [TestMethod]
        public void FlipEdge()
        {
            var max = new Vector2d(1, 1);
            var min = new Vector2d(-1, -1);

            var mesh = HBCreateTriangularMesh2.FromBox(min, max);

            var edge = mesh.Edges[2];
            //var edge = mesh.Edges[5];

            HBOperations.FlipEdge(mesh, edge);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(10, mesh.Edges.Count);
            Assert.AreEqual(2, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 4);
            HBMeshHelper.CheckEdge(mesh, edge:0, from:0, face:0, previous:4, next:2, opposite:6);
            HBMeshHelper.CheckEdge(mesh, edge:1, from:1, face:1, previous:5, next:3, opposite:7);
            HBMeshHelper.CheckEdge(mesh, edge:2, from:1, face:0, previous:0, next:4, opposite:5);
            HBMeshHelper.CheckEdge(mesh, edge:3, from:2, face:1, previous:1, next:5, opposite:8);
            HBMeshHelper.CheckEdge(mesh, edge:4, from:3, face:0, previous:2, next:0, opposite:9);
            HBMeshHelper.CheckEdge(mesh, edge:5, from:3, face:1, previous:3, next:1, opposite:2);
            HBMeshHelper.CheckEdge(mesh, edge:6, from:1, face:-1, previous:7, next:9, opposite:0);
            HBMeshHelper.CheckEdge(mesh, edge:7, from:2, face:-1, previous:8, next:6, opposite:1);
            HBMeshHelper.CheckEdge(mesh, edge:8, from:3, face:-1, previous:9, next:7, opposite:3);
            HBMeshHelper.CheckEdge(mesh, edge:9, from:0, face:-1, previous:6, next:8, opposite:4);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 2);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 5);
        }

        [TestMethod]
        public void PokeFace()
        {
            var a = new Vector2d(0, 1);
            var b = new Vector2d(-1, -1);
            var c = new Vector2d(1, -1);
            var d = (a + b + c) / 3;

            HBMesh2d mesh = HBCreateTriangularMesh2.FromTriangle(a, b, c);

            HBOperations.PokeFace(mesh, mesh.Faces[0]);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(12, mesh.Edges.Count);
            Assert.AreEqual(3, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0, a);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1, b);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 2, c);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 6, d);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 6, next: 9, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 1, previous: 8, next: 11, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 2, previous: 10, next: 7, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 1, face: -1, previous: 4, next: 5, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 2, face: -1, previous: 5, next: 3, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: -1, previous: 3, next: 4, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 3, face: 0, previous: 9, next: 0, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 0, face: 2, previous: 2, next: 10, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 3, face: 1, previous: 11, next: 1, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 1, face: 0, previous: 0, next: 6, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 10, from: 3, face: 2, previous: 7, next: 2, opposite: 11);
            HBMeshHelper.CheckEdge(mesh, edge: 11, from: 2, face: 1, previous: 1, next: 8, opposite: 10);
            HBMeshHelper.CheckFace(mesh, face: 0, edge : 9);
            HBMeshHelper.CheckFace(mesh, face: 1, edge : 11);
            HBMeshHelper.CheckFace(mesh, face: 2, edge : 7);
        }

        [TestMethod]
        public void JoinEdges()
        {
            var max = new Vector2d(1, 1);
            var min = new Vector2d(-1, -1);
 
            var mesh = HBCreatePolygonalMesh2.FromBox(min, max);
            var mesh2 = HBCreatePolygonalMesh2.FromBox(min + 1, max + 1);
            mesh.RemoveFaces();
            mesh2.RemoveFaces();

            HBOperations.Append(mesh, mesh2, false);

            var edge2 = mesh.Edges[2];
            var edge15 = mesh.Edges[15];

            var edge1 = mesh.Edges[1];
            var edge8 = mesh.Edges[8];

            HBOperations.JoinEdges(mesh, edge2, edge15, 0.5);
            HBOperations.JoinEdges(mesh, edge1, edge8, 0.5);

            Assert.AreEqual(10, mesh.Vertices.Count);
            Assert.AreEqual(24, mesh.Edges.Count);
            Assert.AreEqual(0, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 21);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 17);
            HBMeshHelper.CheckVertex(mesh, vertex: 4, edge: 8);
            HBMeshHelper.CheckVertex(mesh, vertex: 5, edge: 23);
            HBMeshHelper.CheckVertex(mesh, vertex: 6, edge: 10);
            HBMeshHelper.CheckVertex(mesh, vertex: 7, edge: 19);
            HBMeshHelper.CheckVertex(mesh, vertex: 8, edge: 16);
            HBMeshHelper.CheckVertex(mesh, vertex: 9, edge: 20);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: -1, previous: 3, next: 1, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: -1, previous: 0, next: 12, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: -1, previous: 20, next: 11, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 3, face: -1, previous: 16, next: 0, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 1, face: -1, previous: 5, next: 7, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 9, face: -1, previous: 23, next: 4, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 8, face: -1, previous: 19, next: 21, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 0, face: -1, previous: 4, next: 17, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 4, face: -1, previous: 11, next: 20, opposite: 12);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 5, face: -1, previous: 22, next: 10, opposite: 13);
            HBMeshHelper.CheckEdge(mesh, edge: 10, from: 6, face: -1, previous: 9, next: 19, opposite: 14);
            HBMeshHelper.CheckEdge(mesh, edge: 11, from: 8, face: -1, previous: 2, next: 8, opposite: 15);
            HBMeshHelper.CheckEdge(mesh, edge: 12, from: 9, face: -1, previous: 1, next: 15, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 13, from: 6, face: -1, previous: 14, next: 23, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 14, from: 7, face: -1, previous: 18, next: 13, opposite: 10);
            HBMeshHelper.CheckEdge(mesh, edge: 15, from: 4, face: -1, previous: 12, next: 16, opposite: 11);
            HBMeshHelper.CheckEdge(mesh, edge: 16, from: 8, face: -1, previous: 15, next: 3, opposite: 17);
            HBMeshHelper.CheckEdge(mesh, edge: 17, from: 3, face: -1, previous: 7, next: 18, opposite: 16);
            HBMeshHelper.CheckEdge(mesh, edge: 18, from: 8, face: -1, previous: 17, next: 14, opposite: 19);
            HBMeshHelper.CheckEdge(mesh, edge: 19, from: 7, face: -1, previous: 10, next: 6, opposite: 18);
            HBMeshHelper.CheckEdge(mesh, edge: 20, from: 9, face: -1, previous: 8, next: 2, opposite: 21);
            HBMeshHelper.CheckEdge(mesh, edge: 21, from: 2, face: -1, previous: 6, next: 22, opposite: 20);
            HBMeshHelper.CheckEdge(mesh, edge: 22, from: 9, face: -1, previous: 21, next: 9, opposite: 23);
            HBMeshHelper.CheckEdge(mesh, edge: 23, from: 5, face: -1, previous: 13, next: 5, opposite: 22);
        }


        [TestMethod]
        public void SplitEdge()
        {
            var a = new Vector2d(0, 1);
            var b = new Vector2d(-1, -1);
            var c = new Vector2d(1, -1);

            HBMesh2d mesh = HBCreateTriangularMesh2.FromTriangle(a, b, c);

            HBOperations.SplitEdge(mesh, mesh.Edges[0], 0.5);

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

        [TestMethod]
        public void AppendMesh()
        {
            var mesh0 = HBMeshHelper.CreateTriangle();
            var mesh1 = HBMeshHelper.CreateTriangle();

            var mesh = new HBMesh2d();

            HBOperations.Append(mesh, mesh0, true);
            HBOperations.Append(mesh, mesh1, true);

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

            HBOperations.AddBoundaryEdges(mesh);

            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 2, next: 1, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 0, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 1, face: -1, previous: 4, next: 5, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 2, face: -1, previous: 5, next: 3, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 0, face: -1, previous: 3, next: 4, opposite: 2);
        }

        [TestMethod]
        public void ToFBTriangleMesh()
        {
            var min = new Vector2d(-1, -1);
            var max = new Vector2d(1, 1);

            var tmp = HBCreateTriangularMesh2.FromBox(min, max);

            var constructor = new FBMeshConstructor2d();
            HBOperations.ToTriangularMesh(constructor, tmp);
            var mesh = constructor.PopMesh();

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(2, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2d(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2d(min.x, max.y), mesh.Vertices[3].Position);

            FBMeshHelper.CheckFace(mesh, face: 0, v0: 0, v1: 1, v2: 2);
            FBMeshHelper.CheckFace(mesh, face: 1, v0: 2, v1: 3, v2: 0);
            FBMeshHelper.CheckAllTrianglesCCW(mesh);
        }
    }
}

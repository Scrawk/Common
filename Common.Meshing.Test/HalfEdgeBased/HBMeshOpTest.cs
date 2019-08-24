using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;
using Common.Meshing.FaceBased;

using Common.Meshing.Test.FaceBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class HBMeshOpTest
    {

        [TestMethod]
        public void RemoveVertex()
        {
            var a = new Vector2d(0, 1);
            var b = new Vector2d(-1, -1);
            var c = new Vector2d(1, -1);

            HBMesh2d mesh = HBCreateTriangleMesh2.FromTriangle(a, b, c);
            mesh.PokeFace(mesh.Faces[0]);

            mesh.RemoveVertex(mesh.Vertices[3], true);

            Assert.AreEqual(3, mesh.Vertices.Count);
            Assert.AreEqual(6, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex : 0, edge : 0);
            HBMeshHelper.CheckVertex(mesh, vertex : 1, edge : 1);
            HBMeshHelper.CheckVertex(mesh, vertex : 2, edge : 2);
            HBMeshHelper.CheckEdge(mesh, edge : 0, from : 0, face : 0, previous : 2, next : 1, opposite : 3);
            HBMeshHelper.CheckEdge(mesh, edge : 1, from : 1, face : 0, previous : 0, next : 2, opposite : 4);
            HBMeshHelper.CheckEdge(mesh, edge : 2, from : 2, face : 0, previous : 1, next : 0, opposite : 5);
            HBMeshHelper.CheckEdge(mesh, edge : 3, from : 1, face : -1, previous : 4, next : 5, opposite : 0);
            HBMeshHelper.CheckEdge(mesh, edge : 4, from : 2, face : -1, previous : 5, next : 3, opposite : 1);
            HBMeshHelper.CheckEdge(mesh, edge : 5, from : 0, face : -1, previous : 3, next : 4, opposite : 2);
            HBMeshHelper.CheckFace(mesh, face : 0, edge : 0);
        }

        [TestMethod]
        public void CollapseEdge()
        {

            var constructor = new HBMeshConstructor2d();
            constructor.PushTriangleMesh(10, 10);

            constructor.AddVertex(new Vector2d(-3, 0));
            constructor.AddVertex(new Vector2d(-2, 1));
            constructor.AddVertex(new Vector2d(0, 1));
            constructor.AddVertex(new Vector2d(2, 1));
            constructor.AddVertex(new Vector2d(3, 0));
            constructor.AddVertex(new Vector2d(2, -1));
            constructor.AddVertex(new Vector2d(0, -1));
            constructor.AddVertex(new Vector2d(-2, -1));
            constructor.AddVertex(new Vector2d(-1, 0));
            constructor.AddVertex(new Vector2d(1, 0));

            constructor.AddFace(8, 1, 0);
            constructor.AddFace(1, 8, 2);
            constructor.AddFace(2, 8, 9);
            constructor.AddFace(2, 9, 3);
            constructor.AddFace(3, 9, 4);
            constructor.AddFace(9, 5, 4);
            constructor.AddFace(9, 6, 5);
            constructor.AddFace(8, 6, 9);
            constructor.AddFace(8, 7, 6);
            constructor.AddFace(0, 7, 8);

            constructor.AddFaceConnection(0, -1, 9, 1);
            constructor.AddFaceConnection(1, 0, 2, -1);
            constructor.AddFaceConnection(2, 1, 7, 3);
            constructor.AddFaceConnection(3, 2, 4, -1);
            constructor.AddFaceConnection(4, 3, 5, -1);
            constructor.AddFaceConnection(5, 4, 6, -1);
            constructor.AddFaceConnection(6, 7, -1, 5);
            constructor.AddFaceConnection(7, 2, 8, 6);
            constructor.AddFaceConnection(8, 9, -1, 7);
            constructor.AddFaceConnection(9, 0, -1, 8);

            var mesh = constructor.PopMesh();
            var edge = mesh.Edges[7];
            mesh.CollapseEdge(edge, edge.GetPosition(0.5), true);

            Assert.AreEqual(9, mesh.Vertices.Count);
            Assert.AreEqual(32, mesh.Edges.Count);
            Assert.AreEqual(8, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 21);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 6);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 9);
            HBMeshHelper.CheckVertex(mesh, vertex: 4, edge: 14);
            HBMeshHelper.CheckVertex(mesh, vertex: 5, edge: 17);
            HBMeshHelper.CheckVertex(mesh, vertex: 6, edge: 20);
            HBMeshHelper.CheckVertex(mesh, vertex: 7, edge: 22);
            HBMeshHelper.CheckVertex(mesh, vertex: 8, edge: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 8, face: 0, previous: 2, next: 1, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 0, next: 2, opposite: 24);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 0, face: 0, previous: 1, next: 0, opposite: 23);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 1, face: 1, previous: 5, next: 4, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 8, face: 1, previous: 3, next: 5, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 2, face: 1, previous: 4, next: 3, opposite: 25);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 2, face: 2, previous: 8, next: 7, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 8, face: 2, previous: 6, next: 8, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 3, face: 2, previous: 7, next: 6, opposite: 26);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 3, face: 3, previous: 11, next: 10, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 10, from: 8, face: 3, previous: 9, next: 11, opposite: 14);
            HBMeshHelper.CheckEdge(mesh, edge: 11, from: 4, face: 3, previous: 10, next: 9, opposite: 27);
            HBMeshHelper.CheckEdge(mesh, edge: 12, from: 8, face: 4, previous: 14, next: 13, opposite: 17);
            HBMeshHelper.CheckEdge(mesh, edge: 13, from: 5, face: 4, previous: 12, next: 14, opposite: 28);
            HBMeshHelper.CheckEdge(mesh, edge: 14, from: 4, face: 4, previous: 13, next: 12, opposite: 10);
            HBMeshHelper.CheckEdge(mesh, edge: 15, from: 8, face: 5, previous: 17, next: 16, opposite: 20);
            HBMeshHelper.CheckEdge(mesh, edge: 16, from: 6, face: 5, previous: 15, next: 17, opposite: 29);
            HBMeshHelper.CheckEdge(mesh, edge: 17, from: 5, face: 5, previous: 16, next: 15, opposite: 12);
            HBMeshHelper.CheckEdge(mesh, edge: 18, from: 8, face: 6, previous: 20, next: 19, opposite: 22);
            HBMeshHelper.CheckEdge(mesh, edge: 19, from: 7, face: 6, previous: 18, next: 20, opposite: 30);
            HBMeshHelper.CheckEdge(mesh, edge: 20, from: 6, face: 6, previous: 19, next: 18, opposite: 15);
            HBMeshHelper.CheckEdge(mesh, edge: 21, from: 0, face: 7, previous: 23, next: 22, opposite: 31);
            HBMeshHelper.CheckEdge(mesh, edge: 22, from: 7, face: 7, previous: 21, next: 23, opposite: 18);
            HBMeshHelper.CheckEdge(mesh, edge: 23, from: 8, face: 7, previous: 22, next: 21, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 24, from: 0, face: -1, previous: 31, next: 25, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 25, from: 1, face: -1, previous: 24, next: 26, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 26, from: 2, face: -1, previous: 25, next: 27, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 27, from: 3, face: -1, previous: 26, next: 28, opposite: 11);
            HBMeshHelper.CheckEdge(mesh, edge: 28, from: 4, face: -1, previous: 27, next: 29, opposite: 13);
            HBMeshHelper.CheckEdge(mesh, edge: 29, from: 5, face: -1, previous: 28, next: 30, opposite: 16);
            HBMeshHelper.CheckEdge(mesh, edge: 30, from: 6, face: -1, previous: 29, next: 31, opposite: 19);
            HBMeshHelper.CheckEdge(mesh, edge: 31, from: 7, face: -1, previous: 30, next: 24, opposite: 21);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 0);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 3);
            HBMeshHelper.CheckFace(mesh, face: 2, edge: 6);
            HBMeshHelper.CheckFace(mesh, face: 3, edge: 9);
            HBMeshHelper.CheckFace(mesh, face: 4, edge: 12);
            HBMeshHelper.CheckFace(mesh, face: 5, edge: 15);
            HBMeshHelper.CheckFace(mesh, face: 6, edge: 18);
            HBMeshHelper.CheckFace(mesh, face: 7, edge: 21);
        }

        [TestMethod]
        public void SplitEdge()
        {
            var max = new Vector2d(1, 1);
            var min = new Vector2d(-1, -1);

            var mesh = HBCreateTriangleMesh2.FromBox(min, max);

            var edge = mesh.Edges[2];

            mesh.SplitEdge(edge);

            Assert.AreEqual(5, mesh.Vertices.Count);
            Assert.AreEqual(16, mesh.Edges.Count);
            Assert.AreEqual(4, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 11);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 13);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 14);
            HBMeshHelper.CheckVertex(mesh, vertex: 4, edge: 15);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 2, previous: 10, next: 13, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 0, previous: 12, next: 2, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 2, face: 0, previous: 1, next: 12, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 2, face: 1, previous: 5, next: 14, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 3, face: 3, previous: 15, next: 11, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 4, face: 1, previous: 14, next: 3, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 1, face: -1, previous: 7, next: 9, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 2, face: -1, previous: 8, next: 6, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 3, face: -1, previous: 9, next: 7, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 0, face: -1, previous: 6, next: 8, opposite: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 10, from: 4, face: 2, previous: 13, next: 0, opposite: 11);
            HBMeshHelper.CheckEdge(mesh, edge: 11, from: 0, face: 3, previous: 4, next: 15, opposite: 10);
            HBMeshHelper.CheckEdge(mesh, edge: 12, from: 4, face: 0, previous: 2, next: 1, opposite: 13);
            HBMeshHelper.CheckEdge(mesh, edge: 13, from: 1, face: 2, previous: 0, next: 10, opposite: 12);
            HBMeshHelper.CheckEdge(mesh, edge: 14, from: 3, face: 1, previous: 3, next: 5, opposite: 15);
            HBMeshHelper.CheckEdge(mesh, edge: 15, from: 4, face: 3, previous: 11, next: 4, opposite: 14);
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 1);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 14);
            HBMeshHelper.CheckFace(mesh, face: 2, edge: 13);
            HBMeshHelper.CheckFace(mesh, face: 3, edge: 4);
        }

        [TestMethod]
        public void flipEdge()
        {
            var max = new Vector2d(1, 1);
            var min = new Vector2d(-1, -1);

            var mesh = HBCreateTriangleMesh2.FromBox(min, max);

            var edge = mesh.Edges[2];
            //var edge = mesh.Edges[5];

            mesh.FlipEdge(edge);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(10, mesh.Edges.Count);
            Assert.AreEqual(2, mesh.Faces.Count);

            HBMeshHelper.CheckVertex(mesh, vertex: 0, edge: 0);
            HBMeshHelper.CheckVertex(mesh, vertex: 1, edge: 1);
            HBMeshHelper.CheckVertex(mesh, vertex: 2, edge: 3);
            HBMeshHelper.CheckVertex(mesh, vertex: 3, edge: 4);
            HBMeshHelper.CheckEdge(mesh, edge: 0, from: 0, face: 0, previous: 4, next: 2, opposite: 6);
            HBMeshHelper.CheckEdge(mesh, edge: 1, from: 1, face: 1, previous: 5, next: 3, opposite: 7);
            HBMeshHelper.CheckEdge(mesh, edge: 2, from: 1, face: 0, previous: 0, next: 4, opposite: 5);
            HBMeshHelper.CheckEdge(mesh, edge: 3, from: 2, face: 1, previous: 1, next: 5, opposite: 8);
            HBMeshHelper.CheckEdge(mesh, edge: 4, from: 3, face: 0, previous: 2, next: 0, opposite: 9);
            HBMeshHelper.CheckEdge(mesh, edge: 5, from: 3, face: 1, previous: 3, next: 1, opposite: 2);
            HBMeshHelper.CheckEdge(mesh, edge: 6, from: 1, face: -1, previous: 7, next: 9, opposite: 0);
            HBMeshHelper.CheckEdge(mesh, edge: 7, from: 2, face: -1, previous: 8, next: 6, opposite: 1);
            HBMeshHelper.CheckEdge(mesh, edge: 8, from: 3, face: -1, previous: 9, next: 7, opposite: 3);
            HBMeshHelper.CheckEdge(mesh, edge: 9, from: 0, face: -1, previous: 6, next: 8, opposite: 4);
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

            HBMesh2d mesh = HBCreateTriangleMesh2.FromTriangle(a, b, c);

            mesh.PokeFace(mesh.Faces[0]);

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
            HBMeshHelper.CheckFace(mesh, face: 0, edge: 9);
            HBMeshHelper.CheckFace(mesh, face: 1, edge: 11);
            HBMeshHelper.CheckFace(mesh, face: 2, edge: 7);
        }

        [TestMethod]
        public void JoinEdges()
        {
            var max = new Vector2d(1, 1);
            var min = new Vector2d(-1, -1);

            var mesh = HBCreatePolygonMesh2.FromBox(min, max);
            var mesh2 = HBCreatePolygonMesh2.FromBox(min + 1, max + 1);
            mesh.RemoveFaces();
            mesh2.RemoveFaces();

            mesh.Append(mesh2, false);

            var edge2 = mesh.Edges[2];
            var edge15 = mesh.Edges[15];

            var edge1 = mesh.Edges[1];
            var edge8 = mesh.Edges[8];

            mesh.JoinEdges(edge2, edge15, 0.5);
            mesh.JoinEdges(edge1, edge8, 0.5);

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
        public void PokeEdge()
        {
            var a = new Vector2d(0, 1);
            var b = new Vector2d(-1, -1);
            var c = new Vector2d(1, -1);

            HBMesh2d mesh = HBCreateTriangleMesh2.FromTriangle(a, b, c);

            mesh.PokeEdge(mesh.Edges[0], 0.5);

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

            mesh.Append(mesh0, true);
            mesh.Append(mesh1, true);

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
        public void ToFBTriangleMesh()
        {
            var min = new Vector2d(-1, -1);
            var max = new Vector2d(1, 1);

            var tmp = HBCreateTriangleMesh2.FromBox(min, max);

            var constructor = new FBMeshConstructor2d();
            tmp.ToTriangularMesh(constructor);
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

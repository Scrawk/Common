using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    /// <summary>
    /// Creates a mesh for testing.
    /// All faces CCW.
    /// </summary>
    public static class HBMeshHelper
    {

        public static void PrintMesh(HBMesh2f mesh)
        {
            Console.WriteLine(mesh);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var e in mesh.Edges)
                Console.WriteLine(e.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));
        }

        public static void PrintMesh(HBMesh3f mesh)
        {
            Console.WriteLine(mesh);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var e in mesh.Edges)
                Console.WriteLine(e.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));
        }

        public static void CheckVertex(HBMesh2f mesh, int vertex, int edge)
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Edge), edge);
        }

        public static void CheckVertex(HBMesh3f mesh, int vertex, int edge)
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Edge), edge);
        }

        public static void CheckVertex(HBMesh2f mesh, int vertex, int edge, Vector2f pos)
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Edge), edge);
            Assert.AreEqual(v.Position, pos);
        }

        public static void CheckVertex(HBMesh3f mesh, int vertex, int edge, Vector2f pos)
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Edge), edge);
            Assert.AreEqual(v.Position, pos);
        }

        public static void CheckEdge(HBMesh2f mesh, int edge, int from, int face, int previous, int next, int opposite)
        {
            var e = mesh.Edges[edge];
            Assert.AreEqual(mesh.IndexOf(e.From), from);
            Assert.AreEqual(mesh.IndexOf(e.Face), face);
            Assert.AreEqual(mesh.IndexOf(e.Previous), previous);
            Assert.AreEqual(mesh.IndexOf(e.Next), next);
            Assert.AreEqual(mesh.IndexOf(e.Opposite), opposite);
        }

        public static void CheckEdge(HBMesh3f mesh, int edge, int from, int face, int previous, int next, int opposite)
        {
            var e = mesh.Edges[edge];
            Assert.AreEqual(mesh.IndexOf(e.From), from);
            Assert.AreEqual(mesh.IndexOf(e.Face), face);
            Assert.AreEqual(mesh.IndexOf(e.Previous), previous);
            Assert.AreEqual(mesh.IndexOf(e.Next), next);
            Assert.AreEqual(mesh.IndexOf(e.Opposite), opposite);
        }

        public static void CheckFace(HBMesh2f mesh, int face, int edge)
        {
            var f = mesh.Faces[face];
            Assert.AreEqual(mesh.IndexOf(f.Edge), edge);
        }

        public static void CheckFace(HBMesh3f mesh, int face, int edge)
        {
            var f = mesh.Faces[face];
            Assert.AreEqual(mesh.IndexOf(f.Edge), edge);
        }

        public static void CheckAllTrianglesCCW(HBMesh2f mesh)
        {
            var list = new List<HBVertex>();
            foreach (var f in mesh.Faces)
            {
                list.Clear();
                f.Edge.GetVertices(list);

                var a = list[0].GetPosition().xy;
                var b = list[1].GetPosition().xy;
                var c = list[2].GetPosition().xy;

                Assert.AreEqual(3, list.Count);
                var tri = new Triangle2f(a, b, c);
                Assert.IsTrue(tri.IsCCW);
            }
        }

        public static HBMesh2f CreateTriangle()
        {
            var mesh = new HBMesh2f();
            mesh.Fill(3, 3, 1);

            var E = mesh.Edges;
            var V = mesh.Vertices;
            var F = mesh.Faces;

            F[0].Edge = E[0];

            V[0].Edge = E[0];
            V[1].Edge = E[1];
            V[2].Edge = E[2];

            E[0].Set(V[0], F[0], E[2], E[1], null);
            E[1].Set(V[1], F[0], E[0], E[2], null);
            E[2].Set(V[2], F[0], E[1], E[0], null);

            return mesh;
        }

        public static HBMesh2f CreateTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var mesh = new HBMesh2f();
            mesh.Fill(3, 3, 1);

            var E = mesh.Edges;
            var V = mesh.Vertices;
            var F = mesh.Faces;

            F[0].Edge = E[0];

            V[0].Edge = E[0];
            V[1].Edge = E[1];
            V[2].Edge = E[2];

            V[0].Position = A;
            V[1].Position = B;
            V[2].Position = C;

            E[0].Set(V[0], F[0], E[2], E[1], null);
            E[1].Set(V[1], F[0], E[0], E[2], null);
            E[2].Set(V[2], F[0], E[1], E[0], null);

            return mesh;
        }

        public static HBMesh3f CreateTriangle(Vector3f A, Vector3f B, Vector3f C)
        {
            var mesh = new HBMesh3f();
            mesh.Fill(3, 3, 1);

            var E = mesh.Edges;
            var V = mesh.Vertices;
            var F = mesh.Faces;

            F[0].Edge = E[0];

            V[0].Edge = E[0];
            V[1].Edge = E[1];
            V[2].Edge = E[2];

            V[0].Position = A;
            V[1].Position = B;
            V[2].Position = C;

            E[0].Set(V[0], F[0], E[2], E[1], null);
            E[1].Set(V[1], F[0], E[0], E[2], null);
            E[2].Set(V[2], F[0], E[1], E[0], null);

            return mesh;
        }

        /// <summary>
        /// See CGALCSharp.Test/Meshes/HalfEdgeBased/Cross.png
        /// </summary>
        /// <returns></returns>
        public static HBMesh2f CreateCross()
        {
            var mesh = new HBMesh2f();
            mesh.Fill(5, 8, 0);

            var E = mesh.Edges;
            var V = mesh.Vertices;

            V[0].Edge = E[1];
            V[1].Edge = E[3];
            V[2].Edge = E[5];
            V[3].Edge = E[7];
            V[4].Edge = E[0];

            E[0].Set(V[4], null, E[7], null, E[1]);
            E[1].Set(V[0], null, null, E[2], E[0]);
            E[2].Set(V[4], null, E[1], null, E[3]);
            E[3].Set(V[1], null, null, E[4], E[2]);
            E[4].Set(V[4], null, E[3], null, E[5]);
            E[5].Set(V[2], null, null, E[6], E[4]);
            E[6].Set(V[4], null, E[5], null, E[7]);
            E[7].Set(V[3], null, null, E[0], E[6]);

            return mesh;
        }

        /// <summary>
        /// See CGALCSharp.Test/Meshes/HalfEdgeBased/SquareWithCenter.png
        /// </summary>
        /// <returns></returns>
        public static HBMesh2f CreateSquareWithCenter()
        {
            var mesh = new HBMesh2f();
            mesh.Fill(5, 12, 4);

            var E = mesh.Edges;
            var V = mesh.Vertices;
            var F = mesh.Faces;

            V[0].Edge = E[1];
            V[1].Edge = E[3];
            V[2].Edge = E[5];
            V[3].Edge = E[7];
            V[4].Edge = E[0];

            F[0].Edge = E[8];
            F[1].Edge = E[9];
            F[2].Edge = E[10];
            F[3].Edge = E[11];

            E[0].Set(V[4], F[3], E[7], E[11], E[1]);
            E[1].Set(V[0], F[0], E[8], E[2], E[0]);
            E[2].Set(V[4], F[0], E[1], E[8], E[3]);
            E[3].Set(V[1], F[1], E[9], E[4], E[2]);
            E[4].Set(V[4], F[1], E[3], E[9], E[5]);
            E[5].Set(V[2], F[2], E[10], E[6], E[4]);
            E[6].Set(V[4], F[2], E[5], E[10], E[7]);
            E[7].Set(V[3], F[3], E[11], E[0], E[6]);

            E[8].Set(V[1], F[0], E[2], E[1], null);
            E[9].Set(V[2], F[1], E[4], E[3], null);
            E[10].Set(V[3], F[2], E[6], E[5], null);
            E[11].Set(V[0], F[3], E[0], E[7], null);

            return mesh;
        }

    }
}

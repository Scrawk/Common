using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    /// <summary>
    /// Creates a mesh for testing.
    /// </summary>
    public static class FBMeshHelper
    {

        public static void PrintMesh(FBMesh2f mesh)
        {
            Console.WriteLine(mesh);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));
        }

        public static void CheckFace(FBMesh2f mesh, int face, int v0, int v1, int v2)
        {
            var f = mesh.Faces[face];
            Assert.AreEqual(mesh.IndexOf(f.Vertices[0]), v0);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[1]), v1);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[2]), v2);
        }

        public static void CheckAllTrianglesCCW(FBMesh2f mesh)
        {
            foreach (var f in mesh.Faces)
            {
                Assert.AreEqual(3, f.NumVertices);

                var a =  f.Vertices[0].GetPosition().xy;
                var b = f.Vertices[1].GetPosition().xy;
                var c = f.Vertices[2].GetPosition().xy;

                var tri = new Triangle2f(a, b, c);
                Assert.IsTrue(tri.IsCCW);
            }
        }

        public static FBMesh2f CreateTriangle()
        {
            var mesh = new FBMesh2f();

            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();
            var v2 = new FBVertex2f();
            mesh.Vertices.Add(v0, v1, v2);

            var f = new FBFace();
            mesh.Faces.Add(f);

            v0.AddFace(f);
            v1.AddFace(f);
            v2.AddFace(f);

            f.SetVerticesSize(3);
            f.Vertices[0] = v0;
            f.Vertices[1] = v1;
            f.Vertices[2] = v2;

            return mesh;
        }

        public static FBMesh2f CreateTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var mesh = new FBMesh2f();

            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();
            var v2 = new FBVertex2f();
            mesh.Vertices.Add(v0, v1, v2);

            var f = new FBFace();
            mesh.Faces.Add(f);

            v0.AddFace(f);
            v0.Position = A;
            v1.AddFace(f);
            v1.Position = B;
            v2.AddFace(f);
            v2.Position = C;

            f.SetVerticesSize(3);
            f.Vertices[0] = v0;
            f.Vertices[1] = v1;
            f.Vertices[2] = v2;

            return mesh;
        }

        public static FBMesh2f CreateSquareWithCenter()
        {
            var mesh = new FBMesh2f();

            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();
            var v2 = new FBVertex2f();
            var v3 = new FBVertex2f();
            var v4 = new FBVertex2f();
            mesh.Vertices.Add(v0, v1, v2, v3);
            mesh.Vertices.Add(v4);

            var f0 = new FBFace();
            var f1 = new FBFace();
            var f2 = new FBFace();
            var f3 = new FBFace();
            mesh.Faces.Add(f0, f1, f2, f3);

            v0.AddFace(f0, f1, f2, f3);
            v1.AddFace(f0, f3);
            v2.AddFace(f0, f1);
            v3.AddFace(f1, f2);
            v4.AddFace(f2, f3);

            f0.SetVerticesSize(3);
            f0.SetVertex(v0, v1, v2);

            f1.SetVerticesSize(3);
            f1.SetVertex(v0, v2, v3);

            f2.SetVerticesSize(3);
            f2.SetVertex(v0, v3, v4);

            f3.SetVerticesSize(3);
            f3.SetVertex(v0, v1, v4);

            return mesh;
        }

    }
}

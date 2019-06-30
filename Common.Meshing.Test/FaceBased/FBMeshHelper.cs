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

        public static void PrintMesh(FBMesh2d mesh)
        {
            Console.WriteLine(mesh);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));
        }

        public static void CheckFace(FBMesh2d mesh, int face, int v0, int v1, int v2)
        {
            var f = mesh.Faces[face];
            Assert.AreEqual(mesh.IndexOf(f.Vertices[0]), v0);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[1]), v1);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[2]), v2);
        }

        public static void CheckAllTrianglesCCW(FBMesh2d mesh)
        {
            foreach (var f in mesh.Faces)
            {
                Assert.AreEqual(3, f.NumVertices);

                var a =  f.GetVertex<FBVertex2d>(0).Position;
                var b = f.GetVertex<FBVertex2d>(1).Position;
                var c = f.GetVertex<FBVertex2d>(2).Position;

                
                var tri = new Triangle2d(a, b, c);
                Assert.IsTrue(tri.IsCCW);
            }
        }

        public static FBMesh2d CreateTriangle()
        {
            var mesh = new FBMesh2d();
 
            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var f = mesh.NewFace();

            v0.AddFace(f);
            v1.AddFace(f);
            v2.AddFace(f);

            f.SetVerticesSize(3);
            f.Vertices[0] = v0;
            f.Vertices[1] = v1;
            f.Vertices[2] = v2;

            return mesh;
        }

        public static FBMesh2d CreateTriangle(Vector2d A, Vector2d B, Vector2d C)
        {
            var mesh = new FBMesh2d();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var f = mesh.NewFace();

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

        public static FBMesh2d CreateSquareWithCenter()
        {
            var mesh = new FBMesh2d();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();
            var v3 = mesh.NewVertex();
            var v4 = mesh.NewVertex();

            var f0 = mesh.NewFace();
            var f1 = mesh.NewFace();
            var f2 = mesh.NewFace();
            var f3 = mesh.NewFace();

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

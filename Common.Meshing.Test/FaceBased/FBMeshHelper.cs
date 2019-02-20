using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Geometry.Shapes;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    /// <summary>
    /// Creates a mesh for testing.
    /// </summary>
    public static class FBMeshHelper
    {

        public static void PrintMesh<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            Console.WriteLine(mesh);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));
        }

        public static void CheckVertex<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh, int vertex, int face)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Face), face);
        }

        public static void CheckVertex<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh, int vertex, int face, Vector2f pos)
            where VERTEX : FBVertex2f, new()
            where FACE : FBFace, new()
        {
            var v = mesh.Vertices[vertex];
            Assert.AreEqual(mesh.IndexOf(v.Face), face);
            Assert.AreEqual(v.Position, pos);
        }

        public static void CheckFace<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh, int face, int e0, int e1, int e2)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            var f = mesh.Faces[face];
            Assert.AreEqual(mesh.IndexOf(f.Vertices[0]), e0);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[1]), e1);
            Assert.AreEqual(mesh.IndexOf(f.Vertices[2]), e2);
        }

        public static void CheckAllTrianglesCCW<VERTEX, EDGE, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex2f, new()
            where FACE : FBFace, new()
        {
            foreach (var f in mesh.Faces)
            {
                Assert.AreEqual(3, f.NumVertices);

                var a =  f.GetVertex< FBVertex2f>(0).Position;
                var b = f.GetVertex<FBVertex2f>(1).Position;
                var c = f.GetVertex<FBVertex2f>(2).Position;

                
                var tri = new Triangle2f(a, b, c);
                Assert.IsTrue(tri.IsCCW);
            }
        }

        public static FBMesh<FBVertex, FBFace> CreateTriangle()
        {
            var mesh = new FBMesh<FBVertex, FBFace>();
 
            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var f = mesh.NewFace();

            v0.Face = f;
            v1.Face = f;
            v2.Face = f;

            f.SetSize(3);
            f.Vertices[0] = v0;
            f.Vertices[1] = v1;
            f.Vertices[2] = v2;

            return mesh;
        }

        public static FBMesh<FBVertex2f, FBFace> CreateTriangle(Vector2f A, Vector2f B, Vector2f C)
        {
            var mesh = new FBMesh<FBVertex2f, FBFace>();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var f = mesh.NewFace();

            v0.Face = f;
            v0.Position = A;
            v1.Face = f;
            v1.Position = B;
            v2.Face = f;
            v2.Position = C;

            f.SetSize(3);
            f.Vertices[0] = v0;
            f.Vertices[1] = v1;
            f.Vertices[2] = v2;

            return mesh;
        }

        public static FBMesh<FBVertex, FBFace> CreateSquareWithCenter()
        {
            var mesh = new FBMesh<FBVertex, FBFace>();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();
            var v3 = mesh.NewVertex();
            var v4 = mesh.NewVertex();

            var f0 = mesh.NewFace();
            var f1 = mesh.NewFace();
            var f2 = mesh.NewFace();
            var f3 = mesh.NewFace();

            v0.Face = f0;
            v1.Face = f0;
            v2.Face = f1;
            v3.Face = f2;
            v4.Face = f3;

            f0.SetSize(3);
            f0.Vertices[0] = v0;
            f0.Vertices[1] = v1;
            f0.Vertices[2] = v2;

            f1.SetSize(3);
            f1.Vertices[0] = v0;
            f1.Vertices[1] = v2;
            f1.Vertices[2] = v3;

            f2.SetSize(3);
            f2.Vertices[0] = v0;
            f2.Vertices[1] = v3;
            f2.Vertices[2] = v4;

            f3.SetSize(3);
            f3.Vertices[0] = v0;
            f3.Vertices[1] = v1;
            f3.Vertices[2] = v4;

            return mesh;
        }

    }
}

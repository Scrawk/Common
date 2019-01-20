using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.Constructors;

namespace Common.Meshing.Test.Constructors
{
    [TestClass]
    public class Meshing_Constructors_CreateGeneralMesh2Test
    {
        [TestMethod]
        public void FromTriangle()
        {
            var constructor = new HBMeshConstructor<HBVertex2f, HBEdge, HBFace>();

            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);
            CreateGeneralMesh2.FromTriangle(constructor, a, b, c);

            constructor.AddBoundary = true;
            var mesh = constructor.PopMesh();

            Assert.AreEqual(3, mesh.Vertices.Count);
            Assert.AreEqual(6, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
            Assert.AreEqual(a, mesh.Vertices[0].Position);
            Assert.AreEqual(b, mesh.Vertices[1].Position);
            Assert.AreEqual(c, mesh.Vertices[2].Position);
        }

        [TestMethod]
        public void FromBox()
        {
            var constructor = new HBMeshConstructor<HBVertex2f, HBEdge, HBFace>();

            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            CreateGeneralMesh2.FromBox(constructor, min, max);

            constructor.AddBoundary = true;
            var mesh = constructor.PopMesh();

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(8, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2f(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2f(min.x, max.y), mesh.Vertices[3].Position);
        }

        [TestMethod]
        public void FromCircle()
        {
            var constructor = new HBMeshConstructor<HBVertex2f, HBEdge, HBFace>();

            CreateGeneralMesh2.FromCircle(constructor, Vector2f.Zero, 1.0f, 4);

            constructor.AddBoundary = true;
            var mesh = constructor.PopMesh();

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(8, mesh.Edges.Count);
            Assert.AreEqual(1, mesh.Faces.Count);
        }
    }
}

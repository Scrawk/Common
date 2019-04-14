using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;
using Common.Meshing.FaceBased;
using Common.Meshing.Constructors;
using Common.Meshing.Test.FaceBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBOperations2fTest
    {
        [TestMethod]
        public void ToFBTriangleMesh2f()
        {
            var min = new Vector2f(-1, -1);
            var max = new Vector2f(1, 1);

            var tmp = CreateTriangleMesh2.FromBox(min, max);
            var mesh = HBOperations2f.ToFBTriangleMesh2f(tmp);

            Assert.AreEqual(4, mesh.Vertices.Count);
            Assert.AreEqual(2, mesh.Faces.Count);
            Assert.AreEqual(min, mesh.Vertices[0].Position);
            Assert.AreEqual(new Vector2f(max.x, min.y), mesh.Vertices[1].Position);
            Assert.AreEqual(max, mesh.Vertices[2].Position);
            Assert.AreEqual(new Vector2f(min.x, max.y), mesh.Vertices[3].Position);

            FBMeshHelper.CheckFace(mesh, face: 0, v0: 0, v1: 1, v2: 2);
            FBMeshHelper.CheckFace(mesh, face: 1, v0: 2, v1: 3, v2: 0);
            FBMeshHelper.CheckAllTrianglesCCW(mesh);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class Meshing_FaceBased_FBMeshTest
    {
        [TestMethod]
        public void Clear()
        {
            var mesh = FBMeshHelper.CreateSquareWithCenter();

            Assert.AreEqual(4, mesh.Faces.Count);
            Assert.AreEqual(5, mesh.Vertices.Count);

            mesh.Clear();

            Assert.AreEqual(0, mesh.Faces.Count);
            Assert.AreEqual(0, mesh.Vertices.Count);
        }

        [TestMethod]
        public void Fill()
        {
            var mesh = new FBMesh2f();
            mesh.Fill(3, 3);

            Assert.AreEqual(3, mesh.Vertices.Count);
            Assert.AreEqual(3, mesh.Faces.Count);
        }

        [TestMethod]
        public void IndexOf()
        {
            var mesh = new FBMesh2f();

            var v0 = mesh.NewVertex();
            var v1 = mesh.NewVertex();
            var v2 = mesh.NewVertex();

            var f0 = mesh.NewFace();
            var f1 = mesh.NewFace();
            var f2 = mesh.NewFace();

            Assert.AreEqual(0, mesh.IndexOf(v0));
            Assert.AreEqual(1, mesh.IndexOf(v1));
            Assert.AreEqual(2, mesh.IndexOf(v2));

            Assert.AreEqual(0, mesh.IndexOf(f0));
            Assert.AreEqual(1, mesh.IndexOf(f1));
            Assert.AreEqual(2, mesh.IndexOf(f2));
        }
    }
}

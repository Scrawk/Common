using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class FBMeshTest
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

            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();
            var v2 = new FBVertex2f();
            mesh.Vertices.Add(v0, v1, v2);

            var f0 = new FBFace();
            var f1 = new FBFace();
            var f2 = new FBFace();
            mesh.Faces.Add(f0, f1, f2);

            Assert.AreEqual(0, mesh.IndexOf(v0));
            Assert.AreEqual(1, mesh.IndexOf(v1));
            Assert.AreEqual(2, mesh.IndexOf(v2));

            Assert.AreEqual(0, mesh.IndexOf(f0));
            Assert.AreEqual(1, mesh.IndexOf(f1));
            Assert.AreEqual(2, mesh.IndexOf(f2));
        }
    }
}

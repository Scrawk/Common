using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBMeshTest
    {
        [TestMethod]
        public void RemoveFaces()
        {

            var mesh = HBMeshHelper.CreateSquareWithCenter();
            mesh.RemoveFaces();

            Assert.AreEqual(0, mesh.Faces.Count);
            Assert.AreEqual(12, mesh.Edges.Count);

            foreach (var edge in mesh.Edges)
                Assert.IsNull(edge.Face);
        }

    }
}

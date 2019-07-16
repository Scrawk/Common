using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;


namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class HBMesh2fTest
    {
        [TestMethod]
        public void GetPositions()
        {
            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);

            var mesh = new HBMesh2f();
            mesh.Vertices.Add(new HBVertex2f(a));
            mesh.Vertices.Add(new HBVertex2f(b));
            mesh.Vertices.Add(new HBVertex2f(c));

            var positions = new List<Vector2f>();
            mesh.GetPositions(positions);

            Assert.AreEqual(positions[0], a);
            Assert.AreEqual(positions[1], b);
            Assert.AreEqual(positions[2], c);
        }
    }
}

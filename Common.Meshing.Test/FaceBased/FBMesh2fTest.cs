using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class FBMesh2fTest
    {

        [TestMethod]
        public void GetPositions()
        {
            var a = new Vector2f(-1, -1);
            var b = new Vector2f(1, -1);
            var c = new Vector2f(0, 1);

            var mesh = new FBMesh2f();
            mesh.Vertices.Add(new FBVertex2f(a));
            mesh.Vertices.Add(new FBVertex2f(b));
            mesh.Vertices.Add(new FBVertex2f(c));

            var positions = new List<Vector2f>();
            mesh.GetPositions(positions);

            Assert.AreEqual(positions[0], a);
            Assert.AreEqual(positions[1], b);
            Assert.AreEqual(positions[2], c);
        }
    }
}

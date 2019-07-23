using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Box2dTest
    {
        [TestMethod]
        public void Closest()
        {
            var box = new Box2d(new Vector2d(-1), new Vector2d(1));

            var p = new Vector2d(0, 0);
            var c = box.Closest(p);

        }
    }
}

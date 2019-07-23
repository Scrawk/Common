using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Triangle2dTest
    {
        [TestMethod]
        public void Closest()
        {
            var tri = new Triangle2d(new Vector2d(-1,0), new Vector2d(1,0), new Vector2d(0, 1));

            var p = new Vector2d(0, 0);
            var c = tri.Closest(p);

            Console.Write(c);
        }
    }
}

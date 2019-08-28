using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Circle2fTest
    {
        [TestMethod]
        public void Closest()
        {
            var circle = new Circle2f(new Vector2f(0, 0), 1);

            var p = new Vector2f(0, 0);
            var c = circle.Closest(p);

            Console.Write(c);
        }
    }
}

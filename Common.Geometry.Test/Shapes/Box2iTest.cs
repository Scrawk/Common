using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Box2iTest
    {
        [TestMethod]
        public void EnumeratePerimeter()
        {

            Box2i box = new Box2i(0, 3);

            foreach(var index in box.EnumeratePerimeter())
            {
               // Console.WriteLine(index);
            }

        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;
using Common.Geometry.Collections;
using System.Collections.Generic;

namespace Common.Geometry.Test.Collections
{
    [TestClass]
    public class BVHTree2fTest
    {
        [TestMethod]
        public void Containing()
        {

            var shape = new Circle2f((0, 0), 10);

            var bvh = new BVHTree2f<IShape2f>();
            bvh.Add(shape);

            var containing = new List<IShape2f>();
            bvh.Containing((100,0), containing);

            foreach (var s in containing)
                Console.WriteLine(s);

        }
    }
}

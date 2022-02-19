using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Shapes;
using Common.Geometry.Collections;
using System.Collections.Generic;

namespace Common.Geometry.Test.Collections
{
    [TestClass]
    public class BVHTree2fTest
    {
        [TestMethod]
        public void Remove()
        {

            var shape0 = new Circle2f((0, 0), 10);
            var shape1 = new Circle2f((1, 2), 2);

            var bvh = new BVHTree2f<IShape2f>();
            //bvh.Add(shape0);
            //bvh.Add(shape1);

            //var removed = bvh.Remove(shape0);

            //Console.WriteLine("Removed = " + removed);

        }
    }
}

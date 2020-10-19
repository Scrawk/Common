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
        public void Remove()
        {

            IShape2f shape0 = new Circle2f((0, 0), 10);

            IShape2f shape1 = new Circle2f((1, 2), 2);

            var bvh = new BVHTree2f<IShape2f>();
            bvh.Add(shape0);
            //bvh.Add(shape1);

            Console.WriteLine(bvh.Root.Shape);

            var removed = bvh.Remove(shape0);

            Console.WriteLine("Removed = " + removed);

        }
    }
}

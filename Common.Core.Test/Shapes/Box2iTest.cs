using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Core.Test.Shapes
{
    [TestClass]
    public class Box2iTest
    {
        [TestMethod]
        public void EnumeratePerimeter()
        {

            var box = new Box2i(0, 10);
            var set = new HashSet<Point2i>();

            foreach (var p in box.EnumeratePerimeter(1))
            {
                Assert.IsFalse(set.Contains(p));
                Assert.IsTrue(box.Contains(p));
                set.Add(p);
            }

            set.Clear();

            foreach (var p in box.EnumeratePerimeter(2))
            {
                Assert.IsFalse(set.Contains(p));
                Assert.IsTrue(box.Contains(p));
                set.Add(p);
            }

        }
    }
}

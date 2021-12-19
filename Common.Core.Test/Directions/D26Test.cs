using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Directions;
using Common.Core.Numerics;

namespace Common.Core.Test.Directions
{
    [TestClass]
    public class D26Test
    {
        [TestMethod]
        public void Offsets()
        {

            Assert.AreEqual(26, D26.OFFSETS.GetLength(0));

            var set = new HashSet<Point3i>();

            for (int i = 0; i < 26; i++)
            {
                int x = D26.OFFSETS[i, 0];
                int y = D26.OFFSETS[i, 1];
                int z = D26.OFFSETS[i, 2];

                var idx = new Point3i(x, y, z);

                Assert.IsFalse(set.Contains(idx));

                set.Add(idx);
            }
        }
    }
}

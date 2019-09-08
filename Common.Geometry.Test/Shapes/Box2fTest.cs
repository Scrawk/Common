﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Shapes
{
    [TestClass]
    public class Box2fTest
    {
        [TestMethod]
        public void Closest()
        {
            var box = new Box2f(new Vector2f(-1), new Vector2f(1));

            var p = new Vector2f(0, 0);
            var c = box.Closest(p);

        }
    }
}
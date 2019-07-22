using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Geometry.Nurbs;

namespace Common.Geometry.Test.Nurbs
{
    [TestClass]
    public class NurbsCurve3dTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Vector3d center = new Vector3d(0, 0, 0);
            Vector3d xaxis = new Vector3d(1, 0, 0);
            Vector3d yaxis = new Vector3d(0, 1, 0);

            var arc = new Arc3d(center, xaxis, yaxis, 5, 0, 3 * Math.PI / 2 );
        }
    }
}

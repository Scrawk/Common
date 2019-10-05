using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Geometry.Nurbs;

namespace Common.Geometry.Test.Nurbs
{
    [TestClass]
    public class NurbsFunctionsTest
    {
        [TestMethod]
        public void FindSpan()
        {
            int p = 2;
            float u = 5.0f / 2.0f;
            float[] U = new float[] { 0, 0, 0, 1, 2, 3, 4, 5, 5, 5 };

            int i = NurbsFunctions.FindSpan(u, p, U);

            Assert.AreEqual(4, i);
        }

        [TestMethod]
        public void BasisFunction()
        {
            int p = 2;
            float u = 5.0f / 2.0f;
            float[] U = new float[] { 0, 0, 0, 1, 2, 3, 4, 5, 5, 5 };

            var N = NurbsFunctions.BasisFunctions(u, p, U);

            //foreach (var n in N)
            //    Console.WriteLine(n);
        }

        [TestMethod]
        public void DerivativeBasisFunction()
        {
            int p = 2;
            float u = 5.0f / 2.0f;
            float[] U = new float[] { 0, 0, 0, 1, 2, 3, 4, 5, 5, 5 };

            var der = NurbsFunctions.DerivativeBasisFunctions(u, p, U);

            //foreach (var d in der)
            //    Console.WriteLine(d);
        }

    }
}

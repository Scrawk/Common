using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Polynomials;

namespace Common.Mathematics.Test.Polynomials
{
    [TestClass]
    public class Mathematics_Polynomials_Polynomial2dTest
    {
        [TestMethod]
        public void Solve()
        {

            Polynomial2d poly = new Polynomial2d(1, 0, 0);
            PolynomialRoots2d roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(0.0, Math.Round(roots.x0, 6));

            poly = new Polynomial2d(1, 1, 1);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(0.0, Math.Round(roots.x0, 6));

            poly = new Polynomial2d(1, 0, -9);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(3.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(-3.0, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(1, 0, -100);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(10.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(-10.0, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(20, 0, -125);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(5.0 / 2.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(-5.0 / 2.0, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(1, -2, -15);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(5.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(-3.0, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(3, -25, 28);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(7.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(1.333333, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(4, 0, -9);
            roots = poly.Solve();

            Assert.AreEqual(2, roots.real);
            Assert.AreEqual(3.0 / 2.0, Math.Round(roots.x0, 6));
            Assert.AreEqual(-3.0 / 2.0, Math.Round(roots.x1, 6));

            poly = new Polynomial2d(1, -16, 64);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(8.0, Math.Round(roots.x0, 6));

            poly = new Polynomial2d(1, 21, 0);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(-21.0, Math.Round(roots.x0, 6));

            poly = new Polynomial2d(12, -24, 0);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(2.0, Math.Round(roots.x0, 6));

            poly = new Polynomial2d(15, 14, 0);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(-0.933333, Math.Round(roots.x0, 6));
 
            poly = new Polynomial2d(1.0 / 4.0, -2.0 / 3.0, 0);
            roots = poly.Solve();

            Assert.AreEqual(1, roots.real);
            Assert.AreEqual(2.666667, Math.Round(roots.x0, 6));
    
        }
    }
}

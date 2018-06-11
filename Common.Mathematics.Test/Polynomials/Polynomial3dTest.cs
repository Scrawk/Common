using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Polynomials;

namespace Common.Mathematics.Test.Polynomials
{
    [TestClass]
    public class Mathematics_Polynomials_Polynomial3dTest
    {
        [TestMethod]
        public void Solve()
        {

             Polynomial3d poly = new Polynomial3d(1, 0, 0, 0);
             PolynomialRoots3d roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(0.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, 1, 1, 1);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(-1.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, 1, 1, 0);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(0.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, -5, -2, 24);
             roots = poly.Solve();

             Assert.AreEqual(3, roots.real);
             Assert.AreEqual(-2.0, Math.Round(roots.x0, 6));
             Assert.AreEqual(4.0, Math.Round(roots.x1, 6));
             Assert.AreEqual(3.0, Math.Round(roots.x2, 6));

             poly = new Polynomial3d(1, 0, -7, -6);
             roots = poly.Solve();

             Assert.AreEqual(3, roots.real);
             Assert.AreEqual(-2.0, Math.Round(roots.x0, 6));
             Assert.AreEqual(3.0, Math.Round(roots.x1, 6));
             Assert.AreEqual(-1.0, Math.Round(roots.x2, 6));

             poly = new Polynomial3d(1, -6, -6, -7);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(7.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, 3, 3, 1);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(-1.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(2, -3, -4, -35);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(3.5, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, 0, 0, -8);
             roots = poly.Solve();

             Assert.AreEqual(1, roots.real);
             Assert.AreEqual(2.0, Math.Round(roots.x0, 6));

             poly = new Polynomial3d(1, 3, 2, 0);
             roots = poly.Solve();

             Assert.AreEqual(3, roots.real);
             Assert.AreEqual(-2.0, Math.Round(roots.x0, 6));
             Assert.AreEqual(0.0, Math.Round(roots.x1, 6));
             Assert.AreEqual(-1.0, Math.Round(roots.x2, 6));
             
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class SumFuncTest
    {

        [TestMethod]
        public new void ToString()
        {
            Function func, func1, func2;

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(0);
            func = new SumFunc(func1, func2);
            Assert.AreEqual("0", func.ToString());

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(1);
            func = new SumFunc(func1, func2);
            Assert.AreEqual("1", func.ToString());

            func1 = new ConstFunc(1);
            func2 = new ConstFunc(1);
            func = new SumFunc(func1, func2);
            Assert.AreEqual("1 + 1", func.ToString());

            func1 = new ConstFunc(-1);
            func2 = new ConstFunc(-1);
            func = new SumFunc(func1, func2);
            Assert.AreEqual("-1 - 1", func.ToString());
        }

        [TestMethod]
        public void Evalulate()
        {
            Function func, func1, func2, func3;

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(0);
            func3 = new ConstFunc(0);
            func = new SumFunc(func1, func2, func3);
            Assert.AreEqual(0, func.Evalulate(1));

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(1);
            func3 = new ConstFunc(0);
            func = new SumFunc(func1, func2, func3);
            Assert.AreEqual(1, func.Evalulate(1));

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(-1);
            func3 = new ConstFunc(0);
            func = new SumFunc(func1, func2, func3);
            Assert.AreEqual(-1, func.Evalulate(1));

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(1);
            func3 = new ConstFunc(1);
            func = new SumFunc(func1, func2, func3);
            Assert.AreEqual(2, func.Evalulate(1));

            func1 = new ConstFunc(0);
            func2 = new ConstFunc(-1);
            func3 = new ConstFunc(-1);
            func = new SumFunc(func1, func2, func3);
            Assert.AreEqual(-2, func.Evalulate(1));
        }

        [TestMethod]
        public void HigherOrderDerivative()
        {
            var func1 = new PowFunc(1.0 / 5.0, 3);
            var func2 = new PowFunc(1, 2);
            var func3 = new LinearFunc(1);
            var func4 = new ConstFunc(-12);

            var func = new SumFunc(func1, func2, func3, func4);
            var derivtive0 = func.Derivative(0);
            var derivtive1 = func.Derivative(1);
            var derivtive2 = func.Derivative(2);
            var derivtive3 = func.Derivative(3);
            var derivtive4 = func.Derivative(4);
            var derivtive5 = func.Derivative(5);

            Assert.AreEqual("0.2x^3 + x^2 + x - 12", derivtive0.ToString());
            Assert.AreEqual("0.6x^2 + 2x + 1", derivtive1.ToString());
            Assert.AreEqual("1.2x + 2", derivtive2.ToString());
            Assert.AreEqual("1.2", derivtive3.ToString());
            Assert.AreEqual("0", derivtive4.ToString());
            Assert.AreEqual("0", derivtive5.ToString());
        }

    }
}

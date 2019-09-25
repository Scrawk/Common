using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Mathematics.Functions;

namespace Common.Mathematics.Test.Functions
{
    [TestClass]
    public class LogChainFuncTest
    {
        [TestMethod]
        public void Constuctor()
        {
            Function con = new ConstFunc(3);
            Function lin = new LinearFunc(1);
            ProductFunc prod = new ProductFunc(con, lin);

            var func = new LogChainFunc(prod);

            Console.WriteLine(func);


        }

        [TestMethod]
        public void Derivative()
        {

        }
    }
}

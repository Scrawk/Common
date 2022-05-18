using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.Core.Extensions;
using Common.GraphTheory.MatrixGraphs;

namespace Common.GraphTheory.Test.MatrixGraphs
{
    [TestClass]
    public class MatrixGraphTest
    {
        [TestMethod]
        public void PushRelabel()
        {

            int[,] flow = new int[6, 6];
            int[,] capacities = new int[6, 6];

            capacities[0,1] = 2;
            capacities[0,2] = 9;
            capacities[1,2] = 1;
            capacities[1,3] = 0;
            capacities[1,4] = 0;
            capacities[2,4] = 7;
            capacities[3,5] = 7;
            capacities[4,5] = 4;

            int maxFlow = MatrixGraph.PushRelabel(capacities, flow, 0, 5);

            Assert.AreEqual(4, maxFlow);

        }
    }
}

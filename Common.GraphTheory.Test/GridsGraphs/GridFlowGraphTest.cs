using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Directions;
using Common.Core.Numerics;
using Common.Core.Extensions;
using Common.GraphTheory.GridGraphs;

namespace Common.GraphTheory.Test.GridsGraphs
{
    [TestClass]
    public class GridFlowGraphTest
    {
        [TestMethod]
        public void PushRelabel()
        {
            var graph = new GridFlowGraph(6, 6);

            //int maxFlow = graph.PushRelabel(0, 5);
            //Assert.AreEqual(4, maxFlow);
        }
    }
}

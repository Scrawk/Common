using System;
using System.Collections.Generic;
using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    public class TestEdge : HBEdge
    {
        public string Name;

        public TestEdge(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

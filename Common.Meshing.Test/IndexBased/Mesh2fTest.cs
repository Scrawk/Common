using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.IndexBased;

namespace Common.Meshing.Test.IndexBased
{
    [TestClass]
    public class Meshing_IndexBased_Mesh2fTest
    {
        [TestMethod]
        public void SetPositions()
        {
            Mesh2f mesh = new Mesh2f(1);

            int size = 10;

            Vector2f[] positions = new Vector2f[size];
            for (int i = 0; i < size; i++)
                positions[i] = new Vector2f(i);

            mesh.SetPositions(positions);

            CollectionAssert.AreEqual(positions, mesh.Positions);
        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Test.HalfEdgeBased
{
    [TestClass]
    public class Meshing_HalfEdgeBased_HBMeshOperationsTest
    {
        [TestMethod]
        public void SplitEdge()
        {
            var a = new Vector2f(0, 1);
            var b = new Vector2f(-1, -1);
            var c = new Vector2f(1, -1);
            var mesh = CreateTestMesh.CreateTriangle(a, b, c);

            mesh.AddBoundaryEdges();

            HBMeshOperations.SplitEdge(mesh, mesh.Edges[0]);

            foreach (var v in mesh.Vertices)
                Console.WriteLine(v.ToString(mesh));

            foreach (var e in mesh.Edges)
                Console.WriteLine(e.ToString(mesh));

            foreach (var f in mesh.Faces)
                Console.WriteLine(f.ToString(mesh));

        }
    }
}

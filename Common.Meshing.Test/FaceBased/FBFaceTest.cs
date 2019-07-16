using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Numerics;
using Common.Meshing.FaceBased;

namespace Common.Meshing.Test.FaceBased
{
    [TestClass]
    public class FBFaceTest
    {
        [TestMethod]
        public void NumVertices()
        {
            var face = new FBFace();

            Assert.AreEqual(0, face.NumVertices);

            face.SetVerticesSize(2);
            Assert.AreEqual(2, face.NumVertices);

            face.SetVerticesSize(3);
            Assert.AreEqual(3, face.NumVertices);
        }

        [TestMethod]
        public void NumNeighbours()
        {
            var face = new FBFace();

            Assert.AreEqual(0, face.NumNeighbours);

            face.SetVerticesSize(2);
            Assert.AreEqual(0, face.NumNeighbours);

            face.Neighbours[0] = new FBFace();
            Assert.AreEqual(1, face.NumNeighbours);

            face.Neighbours[1] = new FBFace();
            Assert.AreEqual(2, face.NumNeighbours);
        }

        [TestMethod]
        public void SetSize()
        {
            var face = new FBFace();

            face.SetVerticesSize(2);
            Assert.AreEqual(2, face.Vertices.Length);

            face.SetVerticesSize(1);
            Assert.AreEqual(1, face.Vertices.Length);
        }

        [TestMethod]
        public void IndexOf()
        {
            var face = new FBFace();

            face.SetVerticesSize(2);
            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();
            var v2 = new FBVertex2f();

            face.Vertices[0] = v0;
            face.Vertices[1] = v1;

            Assert.AreEqual(0, face.IndexOf(v0));
            Assert.AreEqual(1, face.IndexOf(v1));
            Assert.AreEqual(-1, face.IndexOf(v2));
        }

        [TestMethod]
        public void GetVertex()
        {
            var face = new FBFace();

            face.SetVerticesSize(2);
            var v0 = new FBVertex2f();
            var v1 = new FBVertex2f();

            face.Vertices[0] = v0;
            face.Vertices[1] = v1;

            Assert.AreEqual(v0, face.GetVertex<FBVertex2f>(0));
            Assert.AreEqual(v1, face.GetVertex<FBVertex2f>(1));
        }

        [TestMethod]
        public void GetNeighbour()
        {
            var face = new FBFace();

            face.SetVerticesSize(2);
            var f0 = new FBFace();
            var f1 = new FBFace();

            face.Neighbours[0] = f0;
            face.Neighbours[1] = f1;

            Assert.AreEqual(f0, face.GetNeighbour<FBFace>(0));
            Assert.AreEqual(f1, face.GetNeighbour<FBFace>(1));
        }
    }
}

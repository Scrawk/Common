using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Arrays;

namespace Common.Collections.Test.Arrays
{
    [TestClass]
    public class Collections_Arrays_SparseArray2Test
    {
        [TestMethod]
        public void SetIntTest()
        {

            int width = 32;
            int height = 32;
            int gridSize = 4;

            var array = new SparseArray2<int>(width, height, gridSize);

            array[3, 1] = 7;

            Assert.AreEqual(1, array.GridCount);
            Assert.AreEqual(1, array.Count);
            Assert.AreEqual(7, array[3,1]);

            array[3, 1] = 8;

            Assert.AreEqual(1, array.GridCount);
            Assert.AreEqual(1, array.Count);
            Assert.AreEqual(8, array[3, 1]);

            array[3, 1] = 0;

            Assert.AreEqual(0, array.GridCount);
            Assert.AreEqual(0, array.Count);
            Assert.AreEqual(0, array[3, 1]);

            array[31, 8] = 9;
            array[12, 23] = -1;

            Assert.AreEqual(2, array.GridCount);
            Assert.AreEqual(2, array.Count);
            Assert.AreEqual(9, array[31, 8]);
            Assert.AreEqual(-1, array[12, 23]);
        }

        [TestMethod]
        public void SetObjTest()
        {
            int width = 32;
            int height = 32;
            int gridSize = 4;

            var array = new SparseArray2<object>(width, height, gridSize);

            var o1 = new object();
            var o2 = new object();

            array[3, 1] = o1;

            Assert.AreEqual(1, array.GridCount);
            Assert.AreEqual(1, array.Count);
            Assert.AreEqual(o1, array[3, 1]);

            array[3, 1] = o2;

            Assert.AreEqual(1, array.GridCount);
            Assert.AreEqual(1, array.Count);
            Assert.AreEqual(o2, array[3, 1]);

            array[3, 1] = null;

            Assert.AreEqual(0, array.GridCount);
            Assert.AreEqual(0, array.Count);
            Assert.AreEqual(null, array[3, 1]);

        }

        [TestMethod]
        public void FillTest()
        {
            int width = 32;
            int height = 32;
            int gridSize = 4;

            var array = new SparseArray2<int>(width, height, gridSize);

            int idx = 1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[x, y] = idx++;
                }
            }

            Assert.AreEqual(64, array.GridCount);
            Assert.AreEqual(32*32, array.Count);

            idx = 1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Assert.AreEqual(idx++, array[x, y]);
                    array[x, y] = 0;
                }
            }

            Assert.AreEqual(0, array.GridCount);
            Assert.AreEqual(0, array.Count);
        }

        [TestMethod]
        public void ClearTest()
        {
            int width = 32;
            int height = 32;
            int gridSize = 4;

            var array = new SparseArray2<int>(width, height, gridSize);

            int idx = 1;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    array[x, y] = idx++;
                }
            }

            array.Clear();

            Assert.AreEqual(0, array.GridCount);
            Assert.AreEqual(0, array.Count);
        }
    }
}

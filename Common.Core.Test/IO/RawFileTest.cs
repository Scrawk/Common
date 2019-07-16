using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.IO;

namespace Common.Core.Test.IO
{
    [TestClass]
    public class RawFileTest
    {

        const string fileName = "D:/Projects/Visual Studio Projects/Common/Common.Core.Test/IO/TestRawFile.raw";

        [TestMethod]
        public void SaveLoad_32Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.NextDouble();

            try
            {
                RawFile.Save32Bit(fileName, data);
                RawFile raw = new RawFile(fileName, 32, BYTE_ORDER.WINDOWS);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount, "Element count incorrect");
                Assert.AreEqual(size * 4, raw.ByteCount, "Byte count incorrect");
                Assert.AreEqual(32, raw.BitDepth);
                Assert.AreEqual(BYTE_ORDER.WINDOWS, raw.ByteOrder);

                CollectionAssert.AreEqual(data, ToArray(raw));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void StaticSaveLoad_32Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.NextDouble();

            try
            {
                RawFile.Save32Bit(fileName, data);
                CollectionAssert.AreEqual(data, RawFile.Load32Bit(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void SaveLoad_16Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, ushort.MaxValue) / (float)ushort.MaxValue;

            try
            {
                RawFile.Save16Bit(fileName, data, BYTE_ORDER.WINDOWS);
                RawFile raw = new RawFile(fileName, 16, BYTE_ORDER.WINDOWS);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount, "Element count incorrect");
                Assert.AreEqual(size * 2, raw.ByteCount, "Byte count incorrect");
                Assert.AreEqual(16, raw.BitDepth);
                Assert.AreEqual(BYTE_ORDER.WINDOWS, raw.ByteOrder);

                CollectionAssert.AreEqual(data, ToArray(raw));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void StaticSaveLoad_16Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, ushort.MaxValue) / ushort.MaxValue;

            try
            {
                RawFile.Save16Bit(fileName, data, BYTE_ORDER.WINDOWS);
                CollectionAssert.AreEqual(data, RawFile.Load16Bit(fileName, BYTE_ORDER.WINDOWS));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void SaveLoad_8Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, byte.MaxValue) / byte.MaxValue;

            try
            {
                RawFile.Save8Bit(fileName, data);
                RawFile raw = new RawFile(fileName, 8, BYTE_ORDER.WINDOWS);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount);
                Assert.AreEqual(size, raw.ByteCount);
                Assert.AreEqual(8, raw.BitDepth);
                Assert.AreEqual(BYTE_ORDER.WINDOWS, raw.ByteOrder);

                CollectionAssert.AreEqual(data, ToArray(raw));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void StaticSaveLoad_8Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, byte.MaxValue) / (float)byte.MaxValue;

            try
            {
                RawFile.Save8Bit(fileName, data);
                CollectionAssert.AreEqual(data, RawFile.Load8Bit(fileName));
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        float[] ToArray(RawFile raw)
        {
            int size = raw.ElementCount;
            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = raw.Read(i);

            return data;
        }

        int FindSize(List<float[]> data)
        {
            int size = 0;
            int count = data.Count;

            for (int i = 0; i < count; i++)
                size += data[i].Length;

            return size;
        }

    }
}

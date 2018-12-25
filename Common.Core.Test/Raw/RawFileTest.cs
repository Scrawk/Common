using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Raw;

namespace Common.Core.Test.Raw
{
    [TestClass]
    public class Core_Raw_RawFileTest
    {

        const string fileName = "E:/VisualStudio Projects/Common/Common.Core.Test/Raw/TestRawFile.raw";

        [TestMethod]
        public void SaveLoad_32Bit()
        {
            Random rnd = new Random(0);

            int size = 1024;
            int stripSize = 32;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.NextDouble();

            try
            {
                RawFile.Save32Bit(fileName, data);
                RawFile raw = new RawFile(fileName, 32, BYTE_ORDER.WINDOWS, stripSize);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount, "Element count incorrect");
                Assert.AreEqual(size * 4, raw.ByteCount, "Byte count incorrect");
                Assert.AreEqual(stripSize, raw.StripSize, "Strip size incorrect");
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
            int stripSize = 32;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, ushort.MaxValue) / (float)ushort.MaxValue;

            try
            {
                RawFile.Save16Bit(fileName, data, BYTE_ORDER.WINDOWS);
                RawFile raw = new RawFile(fileName, 16, BYTE_ORDER.WINDOWS, stripSize);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount, "Element count incorrect");
                Assert.AreEqual(size * 2, raw.ByteCount, "Byte count incorrect");
                Assert.AreEqual(stripSize, raw.StripSize, "Strip size incorrect");
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
            int stripSize = 32;

            float[] data = new float[size];
            for (int i = 0; i < size; i++)
                data[i] = (float)rnd.Next(0, byte.MaxValue) / byte.MaxValue;

            try
            {
                RawFile.Save8Bit(fileName, data);
                RawFile raw = new RawFile(fileName, 8, BYTE_ORDER.WINDOWS, stripSize);

                Assert.AreEqual(fileName, raw.FileName);
                Assert.AreEqual(size, raw.ElementCount);
                Assert.AreEqual(size, raw.ByteCount);
                Assert.AreEqual(stripSize, raw.StripSize);
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

        //[TestMethod]
        public void BigFileTest()
        {

            string directory = "C:/Users/Justin/Desktop/Residuals/";
            string fileName = directory + "YellowStone_5m_Ortho_8192_8_3.raw";

            RawFileFormat fileFormat = new RawFileFormat();
            fileFormat.bitDepth = 8;
            fileFormat.byteOrder = BYTE_ORDER.WINDOWS;
            fileFormat.fileName = fileName;

            RawFile raw = new RawFile(fileFormat);

            int count = raw.ElementCount;

            for (int i = 0; i < count; i++)
                raw.Read(i);

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

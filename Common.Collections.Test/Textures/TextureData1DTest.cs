using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Textures;
using Common.Collections.Textures.Data1D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class TextureData1DTest
    {

        [TestMethod]
        public void  DimensionsCorrect()
        {
            int width = 64;
            int channels = 3;
            int bitDepth = 8;

            TextureData1D data = TextureData1D.CreateData(width, channels, bitDepth);

            Assert.AreEqual(width, data.GetWidth());
            Assert.AreEqual(channels, data.Channels);
        }

        [TestMethod]
        public void  DataTypeCorrect()
        {

            int width = 512;
            int channels = 3;
            TextureData1D data = null;

            data = TextureData1D.CreateData(width, channels, 8);
            Assert.IsInstanceOfType(data, typeof(TextureData1D8));

            data = TextureData1D.CreateData(width, channels, 16);
            Assert.IsInstanceOfType(data, typeof(TextureData1D16));

            data = TextureData1D.CreateData(width, channels, 32);
            Assert.IsInstanceOfType(data, typeof(TextureData1D32));

        }

        [TestMethod]
        public void Index8()
        {
            int width = 64;
            int channels = 3;
            int bitDepth = 8;

            TextureData1D data = TextureData1D.CreateData(width, channels, bitDepth);

            float[,] arr = new float[width, channels];

            Random rnd = new Random(0);

            for (int x = 0; x < data.GetWidth(); x++)
            {
                for (int c = 0; c < data.Channels; c++)
                {
                    byte v = (byte)rnd.Next(0, byte.MaxValue);
                    arr[x, c] = v / (float)byte.MaxValue;
                    data[x, c] = arr[x, c];
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index16()
        {
            int width = 64;
            int channels = 3;
            int bitDepth = 16;

            TextureData1D data = TextureData1D.CreateData(width, channels, bitDepth);

            float[,] arr = new float[width, channels];

            Random rnd = new Random(0);
            for (int x = 0; x < data.GetWidth(); x++)
            {
                for (int c = 0; c < data.Channels; c++)
                {
                    short v = (short)rnd.Next(short.MinValue, short.MaxValue);
                    arr[x, c] = v / (float)short.MaxValue;
                    data[x, c] = arr[x, c];
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index32()
        {
            int width = 64;
            int channels = 3;
            int bitDepth = 32;

            TextureData1D data = TextureData1D.CreateData(width, channels, bitDepth);

            float[,] arr = new float[width, channels];

            Random rnd = new Random(0);
            for (int x = 0; x < data.GetWidth(); x++)
            {
                for (int c = 0; c < data.Channels; c++)
                {
                    arr[x, c] = (float)rnd.NextDouble();
                    data[x, c] = arr[x, c];
                }
            }

            EqualsWithError(arr, data, 0);
        }

        void EqualsWithError(float[,] arr, TextureData1D data, float error)
        {

            for (int x = 0; x < data.GetWidth(); x++)
            {
                for (int c = 0; c < data.Channels; c++)
                {
                    float diff = Math.Abs(arr[x, c] - data[x, c]);
                    Assert.IsTrue(diff <= error, arr[x, c] + " and " + data[x, c] + " have a error of " + diff);
                }
            }

        }

    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Textures;
using Common.Collections.Textures.Data2D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Collections_Textures_TextureData2DTest
    {

        [TestMethod]
        public void  DimensionsCorrect()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            int bitDepth = 8;

            TextureData2D data = TextureData2D.CreateData(width, height, channels, bitDepth);

            Assert.AreEqual(width, data.GetWidth());
            Assert.AreEqual(height, data.GetHeight());
            Assert.AreEqual(channels, data.Channels);

        }

        [TestMethod]
        public void  DataTypeCorrect()
        {

            int width = 512;
            int height = 128;
            int channels = 3;
            TextureData2D data = null;

            data = TextureData2D.CreateData(width, height, channels, 8);
            Assert.IsInstanceOfType(data, typeof(TextureData2D8));

            data = TextureData2D.CreateData(width, height, channels, 16);
            Assert.IsInstanceOfType(data, typeof(TextureData2D16));

            data = TextureData2D.CreateData(width, height, channels, 32);
            Assert.IsInstanceOfType(data, typeof(TextureData2D32));

        }

        [TestMethod]
        public void Index8()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            int bitDepth = 8;

            TextureData2D data = TextureData2D.CreateData(width, height, channels, bitDepth);

            float[,,] arr = new float[width, height, channels];

            Random rnd = new Random(0);
            for (int y = 0; y < data.GetHeight(); y++)
            {
                for (int x = 0; x < data.GetWidth(); x++)
                {
                    for (int c = 0; c < data.Channels; c++)
                    {
                        byte v = (byte)rnd.Next(0, byte.MaxValue);
                        arr[x, y, c] = v / (float)byte.MaxValue;
                        data[x, y, c] = arr[x, y, c];
                    }
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index16()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            int bitDepth = 16;

            TextureData2D data = TextureData2D.CreateData(width, height, channels, bitDepth);

            float[,,] arr = new float[width, height, channels];

            Random rnd = new Random(0);
            for (int y = 0; y < data.GetHeight(); y++)
            {
                for (int x = 0; x < data.GetWidth(); x++)
                {
                    for (int c = 0; c < data.Channels; c++)
                    {
                        short v = (short)rnd.Next(short.MinValue, short.MaxValue);
                        arr[x, y, c] = v / (float)short.MaxValue;
                        data[x, y, c] = arr[x, y, c];
                    }
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index32()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            int bitDepth = 32;

            TextureData2D data = TextureData2D.CreateData(width, height, channels, bitDepth);

            float[,,] arr = new float[width, height, channels];

            Random rnd = new Random(0);
            for (int y = 0; y < data.GetHeight(); y++)
            {
                for (int x = 0; x < data.GetWidth(); x++)
                {
                    for (int c = 0; c < data.Channels; c++)
                    {
                        arr[x, y, c] = (float)rnd.NextDouble();
                        data[x, y, c] = arr[x, y, c];
                    }
                }
            }

            EqualsWithError(arr, data, 0);
        }

        void EqualsWithError(float[,,] arr, TextureData2D data, float error)
        {
            for (int y = 0; y < data.GetHeight(); y++)
            {
                for (int x = 0; x < data.GetWidth(); x++)
                {
                    for (int c = 0; c < data.Channels; c++)
                    {
                        float diff = Math.Abs(arr[x, y, c] - data[x, y, c]);
                        Assert.IsTrue(diff <= error, arr[x, y, c] + " and " + data[x, y, c] + " have a error of " + diff);
                    }
                }
            }
        }

    }
}

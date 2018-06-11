using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Textures;
using Common.Collections.Textures.Data3D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Collections_Textures_TextureData3DTest
    {

        [TestMethod]
        public void  DimensionsCorrect()
        {
            int width = 8;
            int height = 16;
            int depth = 32;
            int channels = 3;
            int bitDepth = 8;

            TextureData3D data = TextureData3D.CreateData(width, height, depth, channels, bitDepth);

            Assert.AreEqual(width, data.GetWidth());
            Assert.AreEqual(height, data.GetHeight());
            Assert.AreEqual(depth, data.GetDepth());
            Assert.AreEqual(channels, data.Channels);

        }

        [TestMethod]
        public void  DataTypeCorrect()
        {

            int width = 1;
            int height = 1;
            int depth = 1;
            int channels = 3;
            TextureData3D data = null;

            data = TextureData3D.CreateData(width, height, depth, channels, 8);
            Assert.IsInstanceOfType(data, typeof(TextureData3D8));

            data = TextureData3D.CreateData(width, height, depth, channels, 16);
            Assert.IsInstanceOfType(data, typeof(TextureData3D16));

            data = TextureData3D.CreateData(width, height, depth, channels, 32);
            Assert.IsInstanceOfType(data, typeof(TextureData3D32));

        }

        [TestMethod]
        public void Index8()
        {
            int width = 8;
            int height = 16;
            int depth = 32;
            int channels = 3;
            int bitDepth = 8;

            TextureData3D data = TextureData3D.CreateData(width, height, depth, channels, bitDepth);

            float[,,,] arr = new float[width, height, depth, channels];

            Random rnd = new Random(0);
            for (int z = 0; z < data.GetDepth(); z++)
            {
                for (int y = 0; y < data.GetHeight(); y++)
                {
                    for (int x = 0; x < data.GetWidth(); x++)
                    {
                        for (int c = 0; c < data.Channels; c++)
                        {
                            byte v = (byte)rnd.Next(0, byte.MaxValue);
                            arr[x, y, z, c] = v / (float)byte.MaxValue;
                            data[x, y, z, c] = arr[x, y, z, c];
                        }
                    }
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index16()
        {
            int width = 8;
            int height = 16;
            int depth = 32;
            int channels = 3;
            int bitDepth = 16;

            TextureData3D data = TextureData3D.CreateData(width, height, depth, channels, bitDepth);

            float[,,,] arr = new float[width, height, depth, channels];

            Random rnd = new Random(0);
            for (int z = 0; z < data.GetDepth(); z++)
            {
                for (int y = 0; y < data.GetHeight(); y++)
                {
                    for (int x = 0; x < data.GetWidth(); x++)
                    {
                        for (int c = 0; c < data.Channels; c++)
                        {
                            short v = (short)rnd.Next(short.MinValue, short.MaxValue);
                            arr[x, y, z, c] = v / (float)short.MaxValue;
                            data[x, y, z, c] = arr[x, y, z, c];
                        }
                    }
                }
            }

            EqualsWithError(arr, data, 1e-4f);
        }

        [TestMethod]
        public void Index32()
        {
            int width = 8;
            int height = 16;
            int depth = 32;
            int channels = 3;
            int bitDepth = 32;

            TextureData3D data = TextureData3D.CreateData(width, height, depth, channels, bitDepth);

            float[,,,] arr = new float[width, height, depth, channels];

            Random rnd = new Random(0);
            for (int z = 0; z < data.GetDepth(); z++)
            {
                for (int y = 0; y < data.GetHeight(); y++)
                {
                    for (int x = 0; x < data.GetWidth(); x++)
                    {
                        for (int c = 0; c < data.Channels; c++)
                        {
                            arr[x, y, z, c] = (float)rnd.NextDouble();
                            data[x, y, z, c] = arr[x, y, z, c];
                        }
                    }
                }
            }

            EqualsWithError(arr, data, 0);
        }

        void EqualsWithError(float[,,,] arr, TextureData3D data, float error)
        {
            for (int z = 0; z < data.GetDepth(); z++)
            {
                for (int y = 0; y < data.GetHeight(); y++)
                {
                    for (int x = 0; x < data.GetWidth(); x++)
                    {
                        for (int c = 0; c < data.Channels; c++)
                        {
                            float diff = Math.Abs(arr[x, y, z, c] - data[x, y, z, c]);
                            Assert.IsTrue(diff <= error, arr[x, y, z, c] + " and " + data[x, y, z, c] + " have a error of " + diff);
                        }
                    }
                }
            }
        }

    }
}

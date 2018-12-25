using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;
using Common.Collections.Textures;
using Common.Collections.Textures.Data3D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Collections_Textures_Texture3DTest
    {

        [TestMethod]
        public void  DimensionsCorrect()
        {

            int width = 512;
            int height = 128;
            int depth = 32;
            int channels = 3;
            int bitDepth = 8;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
            Assert.AreEqual(height, tex.GetHeight());
            Assert.AreEqual(depth, tex.GetDepth());
            Assert.AreEqual(channels, tex.Channels);
            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        [TestMethod]
        public void  DataTypeCorrect()
        {

            int width = 512;
            int height = 128;
            int depth = 32;
            int channels = 3;
            Texture3D tex = null;

            tex = new Texture3D(width, height, depth, channels, 8);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData3D8));
            Assert.AreEqual(8, tex.BitDepth);

            tex = new Texture3D(width, height, depth, channels, 16);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData3D16));
            Assert.AreEqual(16, tex.BitDepth);

            tex = new Texture3D(width, height, depth, channels, 32);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData3D32));
            Assert.AreEqual(32, tex.BitDepth);

        }

        [TestMethod]
        public void  BilinearFilter_DoesWrap()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(0.5, tex.GetChannel(-0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(-0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(1.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(1.5f, 0.0f, 0.0f, 0));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, -0.5f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, -0.25f, 0.0f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 0.0f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 0.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 0.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 0.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 1.25f, 0.0f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 1.5f, 0.0f, 1));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, -0.5f, 2));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.0f, -0.25f, 2));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 2));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.0f, 0.25f, 2));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, 0.5f, 2));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.0f, 0.75f, 2));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 0.0f, 1.0f, 2));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.0f, 1.25f, 2));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, 1.5f, 2));

        }

        [TestMethod]
        public void  BilinearFilter_DoesClamp()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0.0, tex.GetChannel(-0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(-0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.5f, 0.0f, 0.0f, 0));

            Assert.AreEqual(0.0, tex.GetChannel(0.0f, -0.5f, 0.0f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, -0.25f, 0.0f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 0.0f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 0.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 0.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 0.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.25f, 0.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.5f, 0.0f, 1));

            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, -0.0f, 2));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, -0.25f, 2));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 2));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.0f, 0.25f, 2));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, 0.5f, 2));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.0f, 0.75f, 2));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 0.0f, 1.0f, 2));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 0.0f, 1.25f, 2));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 0.0f, 1.5f, 2));

        }

        [TestMethod]
        public void  BilinearFilter_DoesMirror()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.MIRROR);
      
            Assert.AreEqual(0.5, tex.GetChannel(-0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(-0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(1.25f, 0.0f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(1.5f, 0.0f, 0.0f, 0));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, -0.5f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, -0.25f, 0.0f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 0.0f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 0.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 0.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 0.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 1.25f, 0.0f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 1.5f, 0.0f, 1));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, -0.5f, 2));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.0f, -0.25f, 2));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0.0f, 2));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.0f, 0.25f, 2));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, 0.5f, 2));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.0f, 0.75f, 2));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 0.0f, 1.0f, 2));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.0f, 1.25f, 2));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.0f, 1.5f, 2));

        }

        [TestMethod]
        public void  SetChannel()
        {

            Texture3D tex = new Texture3D(2, 2, 2, 4, 32);

            tex.SetChannel(0, 0, 0, 0, 1);
            tex.SetChannel(0, 0, 0, 1, 2);
            tex.SetChannel(0, 0, 0, 2, 3);
            tex.SetChannel(0, 0, 0, 3, 4);

            Assert.AreEqual(1, tex.GetChannel(0, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 0, 0, 3));
        }

        [TestMethod]
        public void  SetPixel()
        {

            Texture3D tex = new Texture3D(2, 2, 2, 4, 32);

            tex.SetPixel(0, 0, 0, new ColorRGBA(1, 2, 3, 4));

            Assert.AreEqual(1, tex.GetChannel(0, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 0, 0, 3));
        }

        [TestMethod]
        public void  GetChannel_IsCorrect()
        {
            int width = 33;
            int height = 32;
            int depth = 32;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = CreateIndexFilledTexture(width, height, depth, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        for (int c = 0; c < channels; c++)
                        {
                            int idx = (x + y * width + z * width * height) * channels + c;
                            Assert.AreEqual(idx, tex.GetChannel(x, y, z, c));
                        }
                    }
                }
            }

        }

        [TestMethod]
        public void  GetChannelBilinear_IsCorrect()
        {

            int width = 33;
            int height = 32;
            int depth = 32;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = CreateIndexFilledTexture(width, height, depth, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        float fx = x / (width - 1.0f);
                        float fy = y / (height - 1.0f);
                        float fz = z / (depth - 1.0f);

                        for (int c = 0; c < channels; c++)
                        {
                            int idx = (x + y * width + z * width * height) * channels + c;
                            Assert.AreEqual(idx, tex.GetChannel(fx, fy, fz, c));
                        }
                    }
                }
            }

        }

        [TestMethod]
        public void  GetPixel_IsCorrect()
        {

            int width = 33;
            int height = 32;
            int depth = 32;
            int channels = 4;
            int bitDepth = 32;

            Texture3D tex = CreateIndexFilledTexture(width, height, depth, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        ColorRGBA col = tex.GetPixel(x, y, z);

                        Assert.AreEqual((x + y * width + z * width * height) * channels + 0, col.r);
                        Assert.AreEqual((x + y * width + z * width * height) * channels + 1, col.g);
                        Assert.AreEqual((x + y * width + z * width * height) * channels + 2, col.b);
                    }
                }
            }

        }

        [TestMethod]
        public void  GetPixelBilinear_IsCorrect()
        {

            int width = 33;
            int height = 32;
            int depth = 32;
            int channels = 4;
            int bitDepth = 32;

            Texture3D tex = CreateIndexFilledTexture(width, height, depth, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        float fx = x / (width - 1.0f);
                        float fy = y / (height - 1.0f);
                        float fz = z / (depth - 1.0f);

                        ColorRGBA col = tex.GetPixel(fx, fy, fz);

                        Assert.AreEqual((x + y * width + z * width * height) * channels + 0, col.r);
                        Assert.AreEqual((x + y * width + z * width * height) * channels + 1, col.g);
                        Assert.AreEqual((x + y * width + z * width * height) * channels + 2, col.b);
                    }
                }
            }

        }

        [TestMethod]
        public void  GetChannel_DoesWrap()
        {
            int width = 4;
            int height = 4;
            int depth = 4;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(2, tex.GetChannel(-2, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(-1, 0, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(4, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0, 0, 0));

            Assert.AreEqual(2, tex.GetChannel(0, -2, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, -1, 0, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 0, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 0, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 4, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 5, 0, 1));

            Assert.AreEqual(2, tex.GetChannel(0, 0, -2, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, -1, 2));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, 1, 2));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 2, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 3, 2));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 4, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, 5, 2));

        }

        [TestMethod]
        public void  GetChannel_DoesClamp()
        {
            int width = 4;
            int height = 4;
            int depth = 4;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetChannel(-2, 0, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(-1, 0, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(4, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(5, 0, 0, 0));

            Assert.AreEqual(0, tex.GetChannel(0, -2, 0, 1));
            Assert.AreEqual(0, tex.GetChannel(0, -1, 0, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 0, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 4, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 5, 0, 1));

            Assert.AreEqual(0, tex.GetChannel(0, 0, -2, 2));
            Assert.AreEqual(0, tex.GetChannel(0, 0, -1, 2));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, 1, 2));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 2, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 3, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 4, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 5, 2));

        }

        [TestMethod]
        public void  GetChannel_DoesMirror()
        {
            int width = 4;
            int height = 4;
            int depth = 4;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.MIRROR);
      
            Assert.AreEqual(2, tex.GetChannel(-2, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(-1, 0, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(4, 0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0, 0, 0));

            Assert.AreEqual(2, tex.GetChannel(0, -2, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, -1, 0, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 0, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 0, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 4, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 5, 0, 1));

            Assert.AreEqual(2, tex.GetChannel(0, 0, -2, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, -1, 2));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, 1, 2));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 2, 2));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 3, 2));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 4, 2));
            Assert.AreEqual(1, tex.GetChannel(0, 0, 5, 2));

        }

        [TestMethod]
        public void  GetPixel_DoesWrap()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(1, tex.GetPixel(-1, 0, 0, 0).r);
            Assert.AreEqual(0, tex.GetPixel(2, 0, 0, 0).r);
            Assert.AreEqual(1, tex.GetPixel(0, -1, 0, 1).g);
            Assert.AreEqual(0, tex.GetPixel(0, 2, 0, 1).g);
            Assert.AreEqual(1, tex.GetPixel(0, 0, -1, 2).b);
            Assert.AreEqual(0, tex.GetPixel(0, 0, 2, 2).b);

        }

        [TestMethod]
        public void  GetPixel_DoesClamp()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int bitDepth = 32;

            Texture3D tex = CreateXYZFilledTexture(width, height, depth, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetPixel(-1, 0, 0, 0).r);
            Assert.AreEqual(1, tex.GetPixel(2, 0, 0, 0).r);
            Assert.AreEqual(0, tex.GetPixel(0, -1, 0, 1).g);
            Assert.AreEqual(1, tex.GetPixel(0, 2, 0, 1).g);
            Assert.AreEqual(0, tex.GetPixel(0, 0, -1, 2).b);
            Assert.AreEqual(1, tex.GetPixel(0, 0, 2, 2).b);

        }

        [TestMethod]
        public void  ResizeWidth_DoesResize()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            width = 3;
            tex.Resize(width, height, depth, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
        }

        [TestMethod]
        public void  ResizeHeight_DoesResize()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            height = 3;
            tex.Resize(width, height, depth, channels, bitDepth);

            Assert.AreEqual(height, tex.GetHeight());
        }

        [TestMethod]
        public void  ResizeDepth_DoesResize()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            depth = 3;
            tex.Resize(width, height, depth, channels, bitDepth);

            Assert.AreEqual(depth, tex.GetDepth());
        }

        [TestMethod]
        public void  ResizeChannels_DoesResize()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            channels = 4;
            tex.Resize(width, height, depth, channels, bitDepth);

            Assert.AreEqual(channels, tex.Channels);
        }

        [TestMethod]
        public void  ResizeBitDepth_DoesResize()
        {
            int width = 2;
            int height = 2;
            int depth = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            bitDepth = 8;
            tex.Resize(width, height, depth, channels, bitDepth);

            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        Texture3D CreateIndexFilledTexture(int width, int height, int depth, int channels, int bitDepth)
        {
            Texture3D tex = new Texture3D(width, height, depth, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        for (int c = 0; c < channels; c++)
                        {
                            int idx = (x + y * width + z * width * height) * channels + c;
                            tex.SetChannel(x, y, z, c, idx);
                        }
                    }
                }
            }

            return tex;
        }

        Texture3D CreateXYZFilledTexture(int width, int height, int depth, int bitDepth, TEXTURE_WRAP wrap)
        {
            Texture3D tex = new Texture3D(width, height, depth, 3, bitDepth);
            tex.Wrap = wrap;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        tex.SetChannel(x, y, z, 0, x);
                        tex.SetChannel(x, y, z, 1, y);
                        tex.SetChannel(x, y, z, 2, z);
                    }
                }
            }

            return tex;
        }

    }
}

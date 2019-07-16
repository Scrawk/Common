using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;
using Common.Collections.Textures;
using Common.Collections.Textures.Data2D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Texture2DTest
    {

        [TestMethod]
        public void  DimensionsCorrect()
        {

            int width = 512;
            int height = 128;
            int channels = 3;
            int bitDepth = 8;

            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
            Assert.AreEqual(height, tex.GetHeight());
            Assert.AreEqual(channels, tex.Channels);
            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        [TestMethod]
        public void  DataTypeCorrect()
        {

            int width = 512;
            int height = 128;
            int channels = 3;
            Texture2D tex = null;

            tex = new Texture2D(width, height, channels, 8);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData2D8));
            Assert.AreEqual(8, tex.BitDepth);

            tex = new Texture2D(width, height, channels, 16);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData2D16));
            Assert.AreEqual(16, tex.BitDepth);

            tex = new Texture2D(width, height, channels, 32);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData2D32));
            Assert.AreEqual(32, tex.BitDepth);

        }

        [TestMethod]
        public void  CreatedWithMipmaps_UsesMipmaps()
        {
            Texture2D tex = new Texture2D(128, 128, 1, 32, TEXTURE_MIPMAP.BOX);

            Assert.AreEqual(TEXTURE_MIPMAP.BOX, tex.MipmapMode);
            Assert.IsInstanceOfType(tex.Data, typeof(Mipmap2D));
        }

        [TestMethod]
        public void  BilinearFilter_DoesWrap()
        {


            int width = 2;
            int height = 2;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(0.5, tex.GetChannel(-0.5f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(-0.25f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(1.25f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(1.5f, 0.0f, 0));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, -0.5f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, -0.25f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 1.25f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 1.5f, 1));

        }

        [TestMethod]
        public void  BilinearFilter_DoesClamp()
        {


            int width = 2;
            int height = 2;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0.0, tex.GetChannel(-0.5f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(-0.25f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.25f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.5f, 0.0f, 0));

            Assert.AreEqual(0.0, tex.GetChannel(0.0f, -0.5f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, -0.25f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.25f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.5f, 1));

        }

        [TestMethod]
        public void  BilinearFilter_DoesMirror()
        {

            int width = 2;
            int height = 2;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.MIRROR);
    
            Assert.AreEqual(0.5, tex.GetChannel(-0.5f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(-0.25f, 0.0f, 0));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 0));
            Assert.AreEqual(0.25, tex.GetChannel(0.25f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(0.5f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(0.75f, 0.0f, 0));
            Assert.AreEqual(1.0, tex.GetChannel(1.0f, 0.0f, 0));
            Assert.AreEqual(0.75, tex.GetChannel(1.25f, 0.0f, 0));
            Assert.AreEqual(0.5, tex.GetChannel(1.5f, 0.0f, 0));

            Assert.AreEqual(0.5, tex.GetChannel(0.0f, -0.5f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, -0.25f, 1));
            Assert.AreEqual(0.0, tex.GetChannel(0.0f, 0.0f, 1));
            Assert.AreEqual(0.25, tex.GetChannel(0.0f, 0.25f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 0.5f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 0.75f, 1));
            Assert.AreEqual(1.0, tex.GetChannel(0.0f, 1.0f, 1));
            Assert.AreEqual(0.75, tex.GetChannel(0.0f, 1.25f, 1));
            Assert.AreEqual(0.5, tex.GetChannel(0.0f, 1.5f, 1));

        }

        [TestMethod]
        public void  SetChannel()
        {

            Texture2D tex = new Texture2D(2, 2, 4, 32);

            tex.SetChannel(0, 0, 0, 1);
            tex.SetChannel(0, 0, 1, 2);
            tex.SetChannel(0, 0, 2, 3);
            tex.SetChannel(0, 0, 3, 4);

            Assert.AreEqual(1, tex.GetChannel(0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 0, 3));
        }

        [TestMethod]
        public void  SetPixel()
        {

            Texture2D tex = new Texture2D(2, 2, 4, 32);

            tex.SetPixel(0, 0, new ColorRGBA(1, 2, 3, 4));

            Assert.AreEqual(1, tex.GetChannel(0, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 0, 3));
        }

        [TestMethod]
        public void  GetChannel_IsCorrect()
        {

            int width = 65;
            int height = 65;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = CreateIndexFilledTexture(width, height, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int c = 0; c < channels; c++)
                    {
                        int idx = (x + y * width) * channels + c;
                        Assert.AreEqual(idx, tex.GetChannel(x, y, c));
                    }
                }
            }

        }

        [TestMethod]
        public void  GetChannelBilinear_IsCorrect()
        {

            int width = 65;
            int height = 65;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = CreateIndexFilledTexture(width, height, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {

                    float fx = x / (width - 1.0f);
                    float fy = y / (height - 1.0f);

                    for (int c = 0; c < channels; c++)
                    {
                        int idx = (x + y * width) * channels + c;
                        Assert.AreEqual(idx, tex.GetChannel(fx, fy, c));
                    }
                }
            }

        }

        [TestMethod]
        public void  GetPixel_IsCorrect()
        {

            int width = 65;
            int height = 65;
            int channels = 4;
            int bitDepth = 32;

            Texture2D tex = CreateIndexFilledTexture(width, height, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    ColorRGBA col = tex.GetPixel(x, y);

                    Assert.AreEqual((x + y * width) * channels + 0, col.r);
                    Assert.AreEqual((x + y * width) * channels + 1, col.g);
                }
            }

        }

        [TestMethod]
        public void  GetPixelBilinear_IsCorrect()
        {

            int width = 65;
            int height = 65;
            int channels = 4;
            int bitDepth = 32;

            Texture2D tex = CreateIndexFilledTexture(width, height, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float fx = x / (width - 1.0f);
                    float fy = y / (height - 1.0f);

                    ColorRGBA col = tex.GetPixel(fx, fy);

                    Assert.AreEqual((x + y * width) * channels + 0, col.r);
                    Assert.AreEqual((x + y * width) * channels + 1, col.g);
                }
            }

        }

        [TestMethod]
        public void  GetChannel_DoesWrap()
        {
            int width = 4;
            int height = 4;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(2, tex.GetChannel(-2, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(-1, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(4, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0, 0));

            Assert.AreEqual(2, tex.GetChannel(0, -2, 1));
            Assert.AreEqual(3, tex.GetChannel(0, -1, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 4, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 5, 1));

        }

        [TestMethod]
        public void  GetChannel_DoesClamp()
        {
            int width = 4;
            int height = 4;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetChannel(-2, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(-1, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(4, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(5, 0, 0));

            Assert.AreEqual(0, tex.GetChannel(0, -2, 1));
            Assert.AreEqual(0, tex.GetChannel(0, -1, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 4, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 5, 1));

        }

        [TestMethod]
        public void  GetChannel_DoesMirror()
        {
            int width = 4;
            int height = 4;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.MIRROR);
        
            Assert.AreEqual(2, tex.GetChannel(-2, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(-1, 0, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0, 0));
            Assert.AreEqual(2, tex.GetChannel(4, 0, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0, 0));

            Assert.AreEqual(2, tex.GetChannel(0, -2, 1));
            Assert.AreEqual(1, tex.GetChannel(0, -1, 1));
            Assert.AreEqual(0, tex.GetChannel(0, 0, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 1, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 2, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 3, 1));
            Assert.AreEqual(2, tex.GetChannel(0, 4, 1));
            Assert.AreEqual(1, tex.GetChannel(0, 5, 1));

        }

        [TestMethod]
        public void  GetPixel_DoesWrap()
        {
            int width = 2;
            int height = 2;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(1, tex.GetPixel(-1, 0, 0).r);
            Assert.AreEqual(0, tex.GetPixel(2, 0, 0).r);
            Assert.AreEqual(1, tex.GetPixel(0, -1, 1).g);
            Assert.AreEqual(0, tex.GetPixel(0, 2, 1).g);

        }

        [TestMethod]
        public void  GetPixel_DoesClamp()
        {
            int width = 2;
            int height = 2;
            int bitDepth = 32;

            Texture2D tex = CreateXYFilledTexture(width, height, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetPixel(-1, 0, 0).r);
            Assert.AreEqual(1, tex.GetPixel(2, 0, 0).r);
            Assert.AreEqual(0, tex.GetPixel(0, -1, 1).g);
            Assert.AreEqual(1, tex.GetPixel(0, 2, 1).g);

        }

        [TestMethod]
        public void  ResizeWidth_DoesResize()
        {
            int width = 2;
            int height = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            width = 3;
            tex.Resize(width, height, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
        }

        [TestMethod]
        public void  ResizeHeight_DoesResize()
        {
            int width = 2;
            int height = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            height = 3;
            tex.Resize(width, height, channels, bitDepth);

            Assert.AreEqual(height, tex.GetHeight());
        }

        [TestMethod]
        public void  ResizeChannels_DoesResize()
        {
            int width = 2;
            int height = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            channels = 4;
            tex.Resize(width, height, channels, bitDepth);

            Assert.AreEqual(channels, tex.Channels);

        }

        [TestMethod]
        public void  ResizeBitDepth_DoesResize()
        {
            int width = 2;
            int height = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            bitDepth = 8;
            tex.Resize(width, height, channels, bitDepth);

            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        Texture2D CreateIndexFilledTexture(int width, int height, int channels, int bitDepth)
        {
            Texture2D tex = new Texture2D(width, height, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int c = 0; c < channels; c++)
                    {
                        int idx = (x + y * width) * channels + c;
                        tex.SetChannel(x, y, c, idx);
                    }
                }
            }

            return tex;
        }

        Texture2D CreateXYFilledTexture(int width, int height, int bitDepth, TEXTURE_WRAP wrap)
        {
            Texture2D tex = new Texture2D(width, height, 2, bitDepth);
            tex.Wrap = wrap;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    tex.SetChannel(x, y, 0, x);
                    tex.SetChannel(x, y, 1, y);
                }
            }

            return tex;
        }

    }
}

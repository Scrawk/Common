using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.Colors;
using Common.Collections.Textures;
using Common.Collections.Textures.Data1D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Collections_Textures_Texture1DTest
    {

        [TestMethod]
        public void DimensionsCorrect()
        {

            int width = 512;
            int channels = 3;
            int bitDepth = 8;

            Texture1D tex = new Texture1D(width, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
            Assert.AreEqual(channels, tex.Channels);
            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        [TestMethod]
        public void DataTypeCorrect()
        {

            int width = 512;
            int channels = 3;
            Texture1D tex = null;

            tex = new Texture1D(width, channels, 8);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData1D8));
            Assert.AreEqual(8, tex.BitDepth);

            tex = new Texture1D(width, channels, 16);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData1D16));
            Assert.AreEqual(16, tex.BitDepth);

            tex = new Texture1D(width, channels, 32);
            Assert.IsInstanceOfType(tex.Data, typeof(TextureData1D32));
            Assert.AreEqual(32, tex.BitDepth);

        }

        [TestMethod]
        public void BilinearFilter_DoesWrap()
        {
            int width = 2;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(0.5f, tex.GetChannel(-0.5f, 0));
            Assert.AreEqual(0.25f, tex.GetChannel(-0.25f, 0));
            Assert.AreEqual(0.0f, tex.GetChannel(0.0f, 0));
            Assert.AreEqual(0.25f, tex.GetChannel(0.25f, 0));
            Assert.AreEqual(0.5f, tex.GetChannel(0.5f, 0));
            Assert.AreEqual(0.75f, tex.GetChannel(0.75f, 0));
            Assert.AreEqual(1.0f, tex.GetChannel(1.0f, 0));
            Assert.AreEqual(0.75f, tex.GetChannel(1.25f, 0));
            Assert.AreEqual(0.5f, tex.GetChannel(1.5f, 0));

        }

        [TestMethod]
        public void BilinearFilter_DoesClamp()
        {
            int width = 2;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0.0f, tex.GetChannel(-0.5f, 0));
            Assert.AreEqual(0.0f, tex.GetChannel(-0.25f, 0));
            Assert.AreEqual(0.0f, tex.GetChannel(0.0f, 0));
            Assert.AreEqual(0.25f, tex.GetChannel(0.25f, 0));
            Assert.AreEqual(0.5f, tex.GetChannel(0.5f, 0));
            Assert.AreEqual(0.75f, tex.GetChannel(0.75f, 0));
            Assert.AreEqual(1.0f, tex.GetChannel(1.0f, 0));
            Assert.AreEqual(1.0f, tex.GetChannel(1.25f, 0));
            Assert.AreEqual(1.0f, tex.GetChannel(1.5f, 0));

        }

        [TestMethod]
        public void BilinearFilter_DoesMirror()
        {
            int width = 2;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.MIRROR);
 
            Assert.AreEqual(0.5f, tex.GetChannel(-0.5f, 0));
            Assert.AreEqual(0.25f, tex.GetChannel(-0.25f, 0));
            Assert.AreEqual(0.0f, tex.GetChannel(0.0f, 0));
            Assert.AreEqual(0.25f, tex.GetChannel(0.25f, 0));
            Assert.AreEqual(0.5f, tex.GetChannel(0.5f, 0));
            Assert.AreEqual(0.75f, tex.GetChannel(0.75f, 0));
            Assert.AreEqual(1.0f, tex.GetChannel(1.0f, 0));
            Assert.AreEqual(0.75f, tex.GetChannel(1.25f, 0));
            Assert.AreEqual(0.5f, tex.GetChannel(1.5f, 0));

        }

        [TestMethod]
        public void SetChannel()
        {

            Texture1D tex = new Texture1D(2, 4, 32);

            tex.SetChannel(0, 0, 1);
            tex.SetChannel(0, 1, 2);
            tex.SetChannel(0, 2, 3);
            tex.SetChannel(0, 3, 4);

            Assert.AreEqual(1, tex.GetChannel(0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 3));
        }

        [TestMethod]
        public void SetPixel()
        {

            Texture1D tex = new Texture1D(2, 4, 32);

            tex.SetPixel(0, new ColorRGBA(1,2,3,4));

            Assert.AreEqual(1, tex.GetChannel(0, 0));
            Assert.AreEqual(2, tex.GetChannel(0, 1));
            Assert.AreEqual(3, tex.GetChannel(0, 2));
            Assert.AreEqual(4, tex.GetChannel(0, 3));
        }

        [TestMethod]
        public void GetChannel()
        {
            int width = 65;
            int channels = 3;
            int bitDepth = 32;

            Texture1D tex = CreateIndexFilledTexture(width, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    int idx = (x * channels) + c;
                    Assert.AreEqual(idx, tex.GetChannel(x, c));
                }
            }

        }

        [TestMethod]
        public void GetChannelBilinear()
        {

            int width = 65;
            int channels = 3;
            int bitDepth = 32;

            Texture1D tex = CreateIndexFilledTexture(width, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                float fx = x / (width - 1.0f);

                for (int c = 0; c < channels; c++)
                {
                    int idx = (x * channels) + c;
                    Assert.AreEqual(idx, tex.GetChannel(fx, c));
                }
            }

        }

        [TestMethod]
        public void GetPixel()
        {

            int width = 65;
            int channels = 4;
            int bitDepth = 32;

            Texture1D tex = CreateIndexFilledTexture(width, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {

                ColorRGBA col = tex.GetPixel(x);

                Assert.AreEqual((x * channels) + 0, col.r);
                Assert.AreEqual((x * channels) + 1, col.g);
                Assert.AreEqual((x * channels) + 2, col.b);
                Assert.AreEqual((x * channels) + 3, col.a);
            }

        }

        [TestMethod]
        public void GetPixelBilinear()
        {

            int width = 65;
            int channels = 4;
            int bitDepth = 32;

            Texture1D tex = CreateIndexFilledTexture(width, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                float fx = x / (width - 1.0f);

                ColorRGBA col = tex.GetPixel(fx);

                Assert.AreEqual((x * channels) + 0, col.r);
                Assert.AreEqual((x * channels) + 1, col.g);
                Assert.AreEqual((x * channels) + 2, col.b);
                Assert.AreEqual((x * channels) + 3, col.a);
            }

        }

        [TestMethod]
        public void GetChannel_DoesWrap()
        {
            int width = 4;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(2, tex.GetChannel(-2, 0));
            Assert.AreEqual(3, tex.GetChannel(-1, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0));
            Assert.AreEqual(0, tex.GetChannel(4, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0));

        }

        [TestMethod]
        public void GetChannel_DoesClamp()
        {
            int width = 4;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetChannel(-2, 0));
            Assert.AreEqual(0, tex.GetChannel(-1, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0));
            Assert.AreEqual(3, tex.GetChannel(4, 0));
            Assert.AreEqual(3, tex.GetChannel(5, 0));

        }

        [TestMethod]
        public void GetChannel_DoesMirror()
        {
            int width = 4;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.MIRROR);
            tex.Wrap = TEXTURE_WRAP.MIRROR;

            Assert.AreEqual(2, tex.GetChannel(-2, 0));
            Assert.AreEqual(1, tex.GetChannel(-1, 0));
            Assert.AreEqual(0, tex.GetChannel(0, 0));
            Assert.AreEqual(1, tex.GetChannel(1, 0));
            Assert.AreEqual(2, tex.GetChannel(2, 0));
            Assert.AreEqual(3, tex.GetChannel(3, 0));
            Assert.AreEqual(2, tex.GetChannel(4, 0));
            Assert.AreEqual(1, tex.GetChannel(5, 0));

        }

        [TestMethod]
        public void GetPixel_DoesWrap()
        {
            int width = 2;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.WRAP);

            Assert.AreEqual(1, tex.GetPixel(-1, 0).r);
            Assert.AreEqual(0, tex.GetPixel(2, 0).r);

        }

        [TestMethod]
        public void GetPixel_DoesClamp()
        {
            int width = 2;
            int bitDepth = 32;

            Texture1D tex = CreateXFilledTexture(width, bitDepth, TEXTURE_WRAP.CLAMP);

            Assert.AreEqual(0, tex.GetPixel(-1, 0).r);
            Assert.AreEqual(1, tex.GetPixel(2, 0).r);

        }

        [TestMethod]
        public void ResizeWidth_DoesResize()
        {
            int width = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture1D tex = new Texture1D(width, channels, bitDepth);

            width = 3;
            tex.Resize(width, channels, bitDepth);

            Assert.AreEqual(width, tex.GetWidth());
        }

        [TestMethod]
        public void ResizeChannels_DoesResize()
        {
            int width = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture1D tex = new Texture1D(width, channels, bitDepth);

            channels = 4;
            tex.Resize(width, channels, bitDepth);

            Assert.AreEqual(channels, tex.Channels);
        }

        [TestMethod]
        public void ResizeBitDepth_DoesResize()
        {
            int width = 2;
            int channels = 3;
            int bitDepth = 32;

            Texture1D tex = new Texture1D(width, channels, bitDepth);

            bitDepth = 8;
            tex.Resize(width, channels, bitDepth);

            Assert.AreEqual(bitDepth, tex.BitDepth);
        }

        Texture1D CreateIndexFilledTexture(int width, int channels, int bitDepth)
        {
            Texture1D tex = new Texture1D(width, channels, bitDepth);

            for (int x = 0; x < width; x++)
            {
                for (int c = 0; c < channels; c++)
                {
                    int idx = (x * channels) + c;
                    tex.SetChannel(x, c, idx);
                }
            }

            return tex;
        }

        Texture1D CreateXFilledTexture(int width, int bitDepth, TEXTURE_WRAP wrap)
        {
            Texture1D tex = new Texture1D(width, 2, bitDepth);
            tex.Wrap = wrap;

            for (int x = 0; x < width; x++)
            {
                tex.SetChannel(x, 0, x);
            }

            return tex;
        }

    }
}

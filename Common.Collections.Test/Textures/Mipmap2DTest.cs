using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Textures;
using Common.Collections.Textures.Data2D;

namespace Common.Collections.Test.Textures
{
    [TestClass]
    public class Mipmap2DTest
    {

        [TestMethod]
        public void DimensionsCorrect()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            int bitDepth = 8;
            TEXTURE_MIPMAP mode = TEXTURE_MIPMAP.BOX;

            Mipmap2D mipmap = new Mipmap2D(width, height, channels, bitDepth, mode);

            Assert.AreEqual(width, mipmap.GetWidth());
            Assert.AreEqual(height, mipmap.GetHeight());
            Assert.AreEqual(channels, mipmap.Channels);
            Assert.AreEqual(bitDepth, mipmap.BitDepth);
            Assert.AreEqual(mode, mipmap.MipmapMode);
            Assert.IsInstanceOfType(mipmap.GetData(0), typeof(TextureData2D8));
   
        }

        [TestMethod]
        public void DataTypeCorrect()
        {
            int width = 64;
            int height = 128;
            int channels = 3;
            Mipmap2D mipmap = null;

            mipmap = new Mipmap2D(width, height, channels, 8);
            Assert.IsInstanceOfType(mipmap.GetData(0), typeof(TextureData2D8));

            mipmap = new Mipmap2D(width, height, channels, 16);
            Assert.IsInstanceOfType(mipmap.GetData(0), typeof(TextureData2D16));

            mipmap = new Mipmap2D(width, height, channels, 32);
            Assert.IsInstanceOfType(mipmap.GetData(0), typeof(TextureData2D32));

        }

        [TestMethod]
        public void LevelDimensionsCorrect()
        {
            int width = 32;
            int height = 32;

            Mipmap2D mipmap = new Mipmap2D(width, height, 1, 32);
            mipmap.GenerateMipmaps();

            Assert.AreEqual(6, mipmap.Levels);

            for (int i = 0; i < mipmap.Levels; i++)
            {
                TextureData2D data = mipmap.GetData(i);

                Assert.IsNotNull(data);
                Assert.AreEqual(mipmap.Channels, data.Channels);
                Assert.AreEqual(mipmap.BitDepth, data.BitDepth);
                Assert.AreEqual(width, data.GetWidth());
                Assert.AreEqual(height, data.GetHeight());
   
                width /= 2;
                height /= 2;
            }

        }

 
    }
}

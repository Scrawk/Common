using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data1D
{
    public abstract class TextureData1D : TextureData
    {

        public abstract float this[int x, int c, int mipmap = 0] { get; set; }

        protected int Width { get; private set; }
        public virtual int GetWidth(int mipmap = 0) { return Width; }

        internal TextureData1D(int width, int channels, int bitDepth) : base(channels, bitDepth)
        {
            Width = width;
        }

        public static TextureData1D CreateData(int width, int channels, int bitDepth)
        {

            TextureData1D data = null;

            switch (bitDepth)
            {
                case 32:
                    data = new TextureData1D32(width, channels);
                    break;

                case 16:
                    data = new TextureData1D16(width, channels);
                    break;

                case 8:
                    data = new TextureData1D8(width, channels);
                    break;

                default:
                    throw new ArgumentException("Bit depth " + bitDepth + " is invalid");
            }

            return data;
        }

    }

}

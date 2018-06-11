using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data2D
{
    public abstract class TextureData2D : TextureData
    {

        public abstract float this[int x, int y, int c, int mipmap = 0] { get; set; }

        protected int Width { get; private set; }
        public virtual int GetWidth(int mipmap = 0) { return Width; }

        protected int Height { get; private set; }
        public virtual int GetHeight(int mipmap = 0) { return Height; }

        internal TextureData2D(int width, int height, int channels, int bitDepth) : base(channels, bitDepth)
        {
            Width = width;
            Height = height;
        }

        public static TextureData2D CreateData(int width, int height, int channels, int bitDepth)
        {

            TextureData2D data = null;

            switch (bitDepth)
            {
                case 32:
                    data = new TextureData2D32(width, height, channels);
                    break;

                case 16:
                    data = new TextureData2D16(width, height, channels);
                    break;

                case 8:
                    data = new TextureData2D8(width, height, channels);
                    break;

                default:
                    throw new ArgumentException("Bit depth " + bitDepth + " is invalid");
            }

            return data;
        }

    }

}

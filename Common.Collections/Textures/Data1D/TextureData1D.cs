using System;
using System.Collections.Generic;

using Common.Core.Colors;

namespace Common.Collections.Textures.Data1D
{
    public abstract class TextureData1D : TextureData
    {

        public abstract float this[int x, int c, int mipmap = 0] { get; set; }

        protected int Width { get; private set; }
        public virtual int GetWidth(int mipmap = 0) { return Width; }

        public TextureData1D(int width, int channels, int bitDepth) : base(channels, bitDepth)
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

        public void SetPixels(float[] pixels, int mipmap = 0)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int c = 0; c < Channels; c++)
                {
                    float p = pixels[x * Channels + c];
                    this[x, c, mipmap] = p;
                }
            }
        }

        public void SetPixels(float[,] pixels, int mipmap = 0)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int c = 0; c < Channels; c++)
                {
                    float p = pixels[x, c];
                    this[x, c, mipmap] = p;
                }
            }
        }

        public void SetPixels(ColorRGBA[] pixels, int mipmap = 0)
        {
            for (int x = 0; x < Width; x++)
            {
                ColorRGBA p = pixels[x];
                if (Channels > 0) this[x, 0, mipmap] = p.r;
                if (Channels > 1) this[x, 1, mipmap] = p.g;
                if (Channels > 2) this[x, 2, mipmap] = p.b;
                if (Channels > 3) this[x, 3, mipmap] = p.a;
            }
        }

    }

}

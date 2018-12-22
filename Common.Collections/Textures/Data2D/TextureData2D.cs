using System;
using System.Collections.Generic;

using Common.Core.Colors;

namespace Common.Collections.Textures.Data2D
{
    public abstract class TextureData2D : TextureData
    {

        public abstract float this[int x, int y, int c, int mipmap = 0] { get; set; }

        protected int Width { get; private set; }
        public virtual int GetWidth(int mipmap = 0) { return Width; }

        protected int Height { get; private set; }
        public virtual int GetHeight(int mipmap = 0) { return Height; }

        public TextureData2D(int width, int height, int channels, int bitDepth) : base(channels, bitDepth)
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

        public void SetPixels(float[] pixels, int mipmap = 0)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int c = 0; c < Channels; c++)
                    {
                        float p = pixels[(x + y * Width) * Channels + c];
                        this[x, y, c, mipmap] = p;
                    }
                }
            }
        }

        public void SetPixels(float[,,] pixels, int mipmap = 0)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int c = 0; c < Channels; c++)
                    {
                        float p = pixels[x,y,c];
                        this[x, y, c, mipmap] = p;
                    }
                }
            }
        }

        public void SetPixels(ColorRGBA[] pixels, int mipmap = 0)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ColorRGBA p = pixels[x + y * Width];
                    if (Channels > 0) this[x, y, 0, mipmap] = p.r;
                    if (Channels > 1) this[x, y, 1, mipmap] = p.g;
                    if (Channels > 2) this[x, y, 2, mipmap] = p.b;
                    if (Channels > 3) this[x, y, 3, mipmap] = p.a;
                }
            }
        }

        public void SetPixels(ColorRGBA[,] pixels, int mipmap = 0)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ColorRGBA p = pixels[x,y];
                    if (Channels > 0) this[x, y, 0, mipmap] = p.r;
                    if (Channels > 1) this[x, y, 1, mipmap] = p.g;
                    if (Channels > 2) this[x, y, 2, mipmap] = p.b;
                    if (Channels > 3) this[x, y, 3, mipmap] = p.a;
                }
            }
        }
    }

}

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
                        float p = pixels[x, y, c];
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
                    ColorRGBA p = pixels[x, y];
                    if (Channels > 0) this[x, y, 0, mipmap] = p.r;
                    if (Channels > 1) this[x, y, 1, mipmap] = p.g;
                    if (Channels > 2) this[x, y, 2, mipmap] = p.b;
                    if (Channels > 3) this[x, y, 3, mipmap] = p.a;
                }
            }
        }

        public void SetChannel(float[,] channel, int c, int mipmap = 0)
        {
            if (Channels < c) return;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    this[x, y, c, mipmap] = channel[x, y];
                }
            }

        }

        public ColorRGBA[,] GetPixels(int mipmap = 0)
        {
            ColorRGBA[,] pixels = new ColorRGBA[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ColorRGBA p = new ColorRGBA();
                    if (Channels > 0) p.r = this[x, y, 0, mipmap];
                    if (Channels > 1) p.g = this[x, y, 1, mipmap];
                    if (Channels > 2) p.b = this[x, y, 2, mipmap];
                    if (Channels > 3) p.a = this[x, y, 3, mipmap];

                    pixels[x, y] = p;
                }
            }

            return pixels;
        }

        public float[,] GetChannel(int c, int mipmap = 0)
        {
            float[,] channel = new float[Width, Height];
            if (Channels < c) return channel;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    channel[x, y] = this[x, y, c, mipmap];
                }
            }

            return channel;
        }

    }

}

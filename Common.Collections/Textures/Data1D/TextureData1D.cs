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

        public void SetChannels(float[] channel, int c, int mipmap = 0)
        {
            if (Channels < c) return;

            for (int x = 0; x < Width; x++)
            {
                this[x, c, mipmap] = channel[x];
            }
        }

        public ColorRGBA[] GetPixels(int mipmap = 0)
        {
            ColorRGBA[] pixels = new ColorRGBA[Width];

            for (int x = 0; x < Width; x++)
            {
                ColorRGBA p = new ColorRGBA();
                if (Channels > 0) p.r = this[x, 0, mipmap];
                if (Channels > 1) p.g = this[x, 1, mipmap];
                if (Channels > 2) p.b = this[x, 2, mipmap];
                if (Channels > 3) p.a = this[x, 3, mipmap];

                pixels[x] = p;
            }

            return pixels;
        }

        public float[] GetChannels(int c, int mipmap = 0)
        {
            float[] channel = new float[Width];
            if (Channels < c) return channel;

            for (int x = 0; x < Width; x++)
            {
                channel[x] = this[x, c, mipmap];
            }

            return channel;
        }

    }

}

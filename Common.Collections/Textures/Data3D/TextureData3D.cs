using System;
using System.Collections.Generic;

using Common.Core.Colors;

namespace Common.Collections.Textures.Data3D
{
    public abstract class TextureData3D : TextureData
    {

        public abstract float this[int x, int y, int z, int c, int mipmap = 0] { get; set; }

        protected int Width { get; private set; }
        public virtual int GetWidth(int mipmap = 0) { return Width; }

        protected int Height { get; private set; }
        public virtual int GetHeight(int mipmap = 0) { return Height; }

        protected int Depth { get; private set; }
        public virtual int GetDepth(int mipmap = 0) { return Depth; }

        public TextureData3D(int width, int height, int depth, int channels, int bitDepth) : base(channels, bitDepth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public static TextureData3D CreateData(int width, int height, int depth, int channels, int bitDepth)
        {

            TextureData3D data = null;

            switch (bitDepth)
            {
                case 32:
                    data = new TextureData3D32(width, height, depth, channels);
                    break;

                case 16:
                    data = new TextureData3D16(width, height, depth, channels);
                    break;

                case 8:
                    data = new TextureData3D8(width, height, depth, channels);
                    break;

                default:
                    throw new ArgumentException("Bit depth " + bitDepth + " is invalid");
            }

            return data;
        }

        public void SetPixels(ColorRGBA[,,] pixels, int mipmap = 0)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        ColorRGBA p = pixels[x, y, z];
                        if (Channels > 0) this[x, y, z, 0, mipmap] = p.r;
                        if (Channels > 1) this[x, y, z, 1, mipmap] = p.g;
                        if (Channels > 2) this[x, y, z, 2, mipmap] = p.b;
                        if (Channels > 3) this[x, y, z, 3, mipmap] = p.a;
                    }
                }
            }
        }

        public void SetChannels(float[,,] channel, int c, int mipmap = 0)
        {
            if (Channels < c) return;

             for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        this[x, y, z, c, mipmap] = channel[x,y,z];
                    }
                }
            }
        }

        public ColorRGBA[,,] GetPixels(int mipmap = 0)
        {
            ColorRGBA[,,] pixels = new ColorRGBA[Width, Height, Depth];
            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        ColorRGBA p = new ColorRGBA();
                        if (Channels > 0) p.r = this[x, y, z, 0, mipmap];
                        if (Channels > 1) p.g = this[x, y, z, 1, mipmap];
                        if (Channels > 2) p.b = this[x, y, z, 2, mipmap];
                        if (Channels > 3) p.a = this[x, y, z, 3, mipmap];

                        pixels[x, y, z] = p;
                    }
                }
            }

            return pixels;
        }

        public float[,,] GetChannels(int c, int mipmap = 0)
        {
            float[,,] channel = new float[Width, Height, Depth];
            if (Channels < c) return channel;

            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        channel[x,y,z] = this[x, y, z, c, mipmap];
                    }
                }
            }

            return channel;
        }

    }

}

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

        public void SetPixels(float[] pixels, int mipmap = 0)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        for (int c = 0; c < Channels; c++)
                        {
                            float p = pixels[(x + y * Width + z * Width * Height) * Channels + c];
                            this[x, y, z, c, mipmap] = p;
                        }
                    }
                }
            }
        }

        public void SetPixels(float[,,,] pixels, int mipmap = 0)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        for (int c = 0; c < Channels; c++)
                        {
                            float p = pixels[x, y, z, c];
                            this[x, y, z, c, mipmap] = p;
                        }
                    }
                }
            }
        }

        public void SetPixels(ColorRGBA[] pixels, int mipmap = 0)
        {
            for (int z = 0; z < Depth; z++)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        ColorRGBA p = pixels[x + y * Width];
                        if (Channels > 0) this[x, y, z, 0, mipmap] = p.r;
                        if (Channels > 1) this[x, y, z, 1, mipmap] = p.g;
                        if (Channels > 2) this[x, y, z, 2, mipmap] = p.b;
                        if (Channels > 3) this[x, y, z, 3, mipmap] = p.a;
                    }
                }
            }
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

    }

}

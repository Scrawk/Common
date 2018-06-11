using System;
using System.Collections.Generic;

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

        internal TextureData3D(int width, int height, int depth, int channels, int bitDepth) : base(channels, bitDepth)
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

    }

}

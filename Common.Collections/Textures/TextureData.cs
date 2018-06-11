using System;
using System.Collections.Generic;

namespace Common.Collections.Textures
{
    public abstract class TextureData
    {

        public int Levels { get; protected set; }

        public TEXTURE_MIPMAP MipmapMode { get; set; }

        internal int Channels { get; set; }

        internal int BitDepth { get; set; }

        internal TextureData()
        {
            Levels = 1;
            MipmapMode = TEXTURE_MIPMAP.NONE;
        }

        internal TextureData(int channels, int bitDepth)
        {
            Channels = channels;
            BitDepth = bitDepth;
            Levels = 1;
            MipmapMode = TEXTURE_MIPMAP.NONE;
        }

        public virtual void GenerateMipmaps()
        {

        }

        public abstract void Clear();

    }
}

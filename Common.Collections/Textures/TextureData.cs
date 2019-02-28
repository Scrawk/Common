using System;
using System.Collections.Generic;

namespace Common.Collections.Textures
{
    public abstract class TextureData
    {

        public int Levels { get; protected set; }

        public TEXTURE_MIPMAP MipmapMode { get; set; }

        public int Channels { get; set; }

        public int BitDepth { get; set; }

        public TextureData()
        {
            Levels = 1;
            MipmapMode = TEXTURE_MIPMAP.NONE;
        }

        public TextureData(int channels, int bitDepth)
        {
            Channels = channels;
            BitDepth = bitDepth;
            Levels = 1;
            MipmapMode = TEXTURE_MIPMAP.NONE;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[TextureData: Channels={0}, BitDepth={1}, Levels]", Channels, BitDepth, Levels);
        }

        public virtual void GenerateMipmaps()
        {

        }

        public abstract void Clear();

    }
}

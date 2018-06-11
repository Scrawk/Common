using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data1D
{
    public class TextureData1D32 : TextureData1D
    {

        public float[,] Raw { get; private set; }

        internal TextureData1D32(int width, int channels) : base(width, channels, 32)
        {

            Raw = new float[width, channels];

        }

        public override float this[int x, int c, int mipmap = 0]
        {
            get { return Raw[x, c]; }
            set { Raw[x, c] = value; }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data1D
{
    public class TextureData1D8 : TextureData1D
    {

        public byte[,] Raw { get; private set; }

        readonly static float FACTOR = 1.0f / byte.MaxValue;

        internal TextureData1D8(int width, int channels) : base(width, channels, 8)
        {

            Raw = new byte[width, channels];

        }

        public override float this[int x, int c, int mipmap = 0]
        {
            get
            {
                return Raw[x, c] * FACTOR;
            }
            set
            {
                float v = value;
                if (v > 1.0f) v = 1.0f;
                if (v < 0.0f) v = 0.0f;
                v *= byte.MaxValue;

                Raw[x, c] = (byte)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

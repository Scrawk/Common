using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data1D
{
    public class TextureData1D16 : TextureData1D
    {

        public short[,] Raw { get; private set; }

        readonly static float FACTOR = 1.0f / short.MaxValue;

        internal TextureData1D16(int width, int channels) : base(width, channels, 16)
        {

            Raw = new short[width, channels];

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
                if (v < -1.0f) v = -1.0f;
                v *= short.MaxValue;

                Raw[x, c] = (short)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

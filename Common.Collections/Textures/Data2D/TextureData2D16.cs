using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data2D
{
    public class TextureData2D16 : TextureData2D
    {

        private readonly static float FACTOR = 1.0f / short.MaxValue;

        public short[,,] Raw;

        internal TextureData2D16(int width, int height, int channels) : base(width, height, channels, 16)
        {
            Raw = new short[width, height, channels];
        }

        public override float this[int x, int y, int c, int mipmap = 0]
        {
            get { return Raw[x,y,c] * FACTOR; }
            set
            {
                float v = value;
                if (v > 1.0f) v = 1.0f;
                if (v < -1.0f) v = -1.0f;
                v *= short.MaxValue;

                Raw[x,y,c] = (short)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

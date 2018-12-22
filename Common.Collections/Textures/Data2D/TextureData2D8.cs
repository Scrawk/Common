using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data2D
{
    public class TextureData2D8 : TextureData2D
    {

        private readonly static float FACTOR = 1.0f / byte.MaxValue;

        public byte[,,] Raw;

        public TextureData2D8(int width, int height, int channels) : base(width, height, channels, 8)
        {
            Raw = new byte[width, height, channels];
        }

        public override float this[int x, int y, int c, int mipmap = 0]
        {
            get { return Raw[x,y,c] * FACTOR; }
            set
            {
                float v = value;
                if (v > 1.0f) v = 1.0f;
                if (v < 0.0f) v = 0.0f;
                v *= byte.MaxValue;

                Raw[x, y, c] = (byte)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

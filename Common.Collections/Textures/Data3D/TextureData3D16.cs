using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data3D
{
    public class TextureData3D16 : TextureData3D
    {
        readonly static float FACTOR = 1.0f / short.MaxValue;

        public short[,,,] Raw { get; private set; }

        public TextureData3D16(int width, int height, int depth, int channels) 
            : base(width, height, depth, channels, 16)
        {

            Raw = new short[width, height, depth, channels];

        }

        public override float this[int x, int y, int z, int c, int mipmap = 0]
        {
            get
            {
                return Raw[x, y, z, c] * FACTOR;
            }
            set
            {
                float v = value;
                if (v > 1.0f) v = 1.0f;
                if (v < -1.0f) v = -1.0f;
                v *= short.MaxValue;

                Raw[x, y, z, c] = (short)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

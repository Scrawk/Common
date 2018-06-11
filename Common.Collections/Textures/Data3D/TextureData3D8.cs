using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data3D
{
    public class TextureData3D8 : TextureData3D
    {

        readonly static float FACTOR = 1.0f / byte.MaxValue;

        public byte[,,,] Raw { get; private set; }

        internal TextureData3D8(int width, int height, int depth, int channels) 
            : base(width, height, depth, channels, 8)
        {

            Raw = new byte[width, height, depth, channels];

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
                if (v < 0.0f) v = 0.0f;
                v *= byte.MaxValue;

                Raw[x, y, z, c] = (byte)v;
            }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

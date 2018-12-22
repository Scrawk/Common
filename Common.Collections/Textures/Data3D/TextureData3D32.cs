using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data3D
{
    public class TextureData3D32 : TextureData3D
    {

        public float[,,,] Raw { get; private set; }

        public TextureData3D32(int width, int height, int depth, int channels) 
            : base(width, height, depth, channels, 32)
        {

            Raw = new float[width, height, depth, channels];

        }

        public override float this[int x, int y, int z, int c, int mipmap = 0]
        {
            get { return Raw[x, y, z, c]; }
            set { Raw[x, y, z, c] = value; }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

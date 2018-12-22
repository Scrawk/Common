using System;
using System.Collections.Generic;

namespace Common.Collections.Textures.Data2D
{
    public class TextureData2D32 : TextureData2D
    {

        public float[,,] Raw;

        public TextureData2D32(int width, int height, int channels) : base(width, height, channels, 32)
        {
            Raw = new float[width, height, channels];
        }

        public override float this[int x, int y, int c, int mipmap = 0]
        {
            get {  return Raw[x,y,c]; }
            set { Raw[x,y,c] = value; }
        }

        public override void Clear()
        {
            Array.Clear(Raw, 0, Raw.Length);
        }

    }
}

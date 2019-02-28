using System;
using System.Collections.Generic;

using Common.Core.Mathematics;

namespace Common.Collections.Textures
{

    public enum TEXTURE_WRAP { CLAMP, WRAP, MIRROR };

    public enum TEXTURE_MIPMAP {  NONE, BOX, TRIANGLE, QUADRATIC, KAISER };

    public enum TEXTURE_INTERPOLATION { POINT, BILINEAR, BICUBIC };

    public abstract class Texture
    {

        protected struct BilinearIndex
        {
            internal float fi;
            internal int i0, i1, size;
        }

        protected struct BicubicIndex
        {
            internal float fi;
            internal int i0, i1, i2, i3, size;
        }

        public abstract int Channels { get; }

        public abstract int BitDepth { get; }

        public TEXTURE_WRAP Wrap { get; set; }

        public TEXTURE_INTERPOLATION Interpolation { get; set; }

        public Texture()
        {
            Interpolation = TEXTURE_INTERPOLATION.BILINEAR;
            Wrap = TEXTURE_WRAP.CLAMP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Texture: Channels={0}, BitDepth={1}, Wrap={2}, Interpolation={3}]", 
                Channels, BitDepth, Wrap, Interpolation);
        }

        public abstract void Clear();

        protected int Index(int i, int size)
        {
            if (Wrap == TEXTURE_WRAP.WRAP)
                return IMath.Wrap(i, size);
            else if(Wrap == TEXTURE_WRAP.CLAMP)
                return IMath.Clamp(i, 0, size-1);
            else //MIRROR
                return IMath.Mirror(i, size);
        }

        protected BilinearIndex NewBilinearIndex(float i, int size)
        {

            float fi = i - (int)i;
            if (fi < 0) fi = 1.0f + fi;

            int i0 = (int)i;
            if (i < 0.0) i0--;

            int i1 = i0 + 1;

            if (Wrap == TEXTURE_WRAP.WRAP)
            {
                i0 = IMath.Wrap(i0, size);
                i1 = IMath.Wrap(i1, size);
            }
            else if (Wrap == TEXTURE_WRAP.CLAMP)
            {
                i0 = IMath.Clamp(i0, 0, size - 1);
                i1 = IMath.Clamp(i1, 0, size - 1);
            }
            else //MIRROR
            {
                i0 = IMath.Mirror(i0, size);
                i1 = IMath.Mirror(i1, size);
            }

            BilinearIndex idx;
            idx.fi = fi;
            idx.i0 = i0;
            idx.i1 = i1;
            idx.size = size;

            return idx;
        }

        protected BicubicIndex NewBicubicIndex(float i, int size)
        {

            float fi = i - (int)i;
            if (fi < 0) fi = 1.0f + fi;
   
            int i1 = (int)i;
            if (i < 0.0) i1--;

            int i0 = i1 - 1;
            int i2 = i1 + 1;
            int i3 = i1 + 2;

            if (Wrap == TEXTURE_WRAP.WRAP)
            {
                i0 = IMath.Wrap(i0, size);
                i1 = IMath.Wrap(i1, size);
                i2 = IMath.Wrap(i2, size);
            }
            else if (Wrap == TEXTURE_WRAP.CLAMP)
            {
                i0 = IMath.Clamp(i0, 0, size - 1);
                i1 = IMath.Clamp(i1, 0, size - 1);
                i2 = IMath.Clamp(i2, 0, size - 1);
            }
            else //MIRROR
            {
                i0 = IMath.Mirror(i0, size);
                i1 = IMath.Mirror(i1, size);
                i2 = IMath.Mirror(i2, size);
            }

            BicubicIndex idx;
            idx.fi = fi;
            idx.i0 = i0;
            idx.i1 = i1;
            idx.i2 = i2;
            idx.i3 = i3;
            idx.size = size;

            return idx;
        }

        protected float Bicubic(float fx, float v0, float v1, float v2, float v3)
        {
            return v1 + 0.5f * fx * (v2 - v0 + fx * (2.0f * v0 - 5.0f * v1 + 4.0f * v2 - v3 + fx * (3.0f * (v1 - v2) + v3 - v0)));
        }

    }
}

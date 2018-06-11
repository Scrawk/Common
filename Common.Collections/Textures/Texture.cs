using System;
using System.Collections.Generic;

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

        public abstract void Clear();

        protected int Index(int i, int size)
        {
            int size1 = size - 1;

            if (Wrap == TEXTURE_WRAP.WRAP)
            {
                i = i % size;
                return (i < 0) ? i + size : i;
            }
            else if(Wrap == TEXTURE_WRAP.CLAMP)
            {
                if (i < 0) i = 0;
                if (i >= size) i = size1;
                return i;
            }
            else //MIRROR
            {
                int j = Math.Abs(i);

                i = j % (size1 * 2);
                if (i >= size1) i = size1 - j % size1;

                return i;
            }

        }

        protected BilinearIndex NewBilinearIndex(float i, int size)
        {

            float fi = i - (int)i;
            if (fi < 0) fi = 1.0f + fi;

            int i0 = (int)i;
            if (i < 0.0) i0--;

            int i1 = i0 + 1;
            int size1 = size - 1;

            if (Wrap == TEXTURE_WRAP.WRAP)
            {
                if (i0 >= size || i0 <= -size) i0 = i0 % size;
                if (i0 < 0) i0 = size - -i0;

                if (i1 >= size || i1 <= -size) i1 = i1 % size;
                if (i1 < 0) i1 = size - -i1;
            }
            else if (Wrap == TEXTURE_WRAP.CLAMP)
            {
                if (i0 < 0) i0 = 0;
                else if (i0 >= size) i0 = size1;

                if (i1 < 0) i1 = 0;
                else if (i1 >= size) i1 = size1;
            }
            else //MIRROR
            {
                int j0 = Math.Abs(i0);

                i0 = j0 % (size1 * 2);
                if (i0 >= size1) i0 = size1 - j0 % size1;

                int j1 = Math.Abs(i1);

                i1 = j1 % (size1 * 2);
                if (i1 >= size1) i1 = size1 - j1 % size1;
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
            int size1 = size - 1;

            if (Wrap == TEXTURE_WRAP.WRAP)
            {
                if (i0 >= size || i0 <= -size) i0 = i0 % size;
                if (i0 < 0) i0 = size - -i0;

                if (i1 >= size || i1 <= -size) i1 = i1 % size;
                if (i1 < 0) i1 = size - -i1;

                if (i2 >= size || i2 <= -size) i2 = i2 % size;
                if (i2 < 0) i2 = size - -i2;

                if (i3 >= size || i3 <= -size) i3 = i3 % size;
                if (i3 < 0) i3 = size - -i3;
            }
            else if (Wrap == TEXTURE_WRAP.CLAMP)
            {
                if (i0 < 0) i0 = 0;
                else if (i0 >= size) i0 = size1;

                if (i1 < 0) i1 = 0;
                else if (i1 >= size) i1 = size1;

                if (i2 < 0) i2 = 0;
                else if (i2 >= size) i2 = size1;

                if (i3 < 0) i3 = 0;
                else if (i3 >= size) i3 = size1;
            }
            else //MIRROR
            {
                int j0 = Math.Abs(i0);
                i0 = j0 % (size1 * 2);
                if (i0 >= size1) i0 = size1 - j0 % size1;

                int j1 = Math.Abs(i1);
                i1 = j1 % (size1 * 2);
                if (i1 >= size1) i1 = size1 - j1 % size1;

                int j2 = Math.Abs(i2);
                i2 = j2 % (size1 * 2);
                if (i2 >= size1) i2 = size1 - j2 % size1;

                int j3 = Math.Abs(i3);
                i3 = j3 % (size1 * 2);
                if (i3 >= size1) i3 = size1 - j3 % size1;
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

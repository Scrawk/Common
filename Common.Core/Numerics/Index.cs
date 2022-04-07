using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Common.Core.Numerics
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Index2
    {
        public int x, y;

        public Index2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        unsafe public int this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Index2 index out of range.");

                fixed (Index2* array = &this) { return ((int*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Index2 index out of range.");

                fixed (int* array = &x) { array[i] = value; }
            }
        }

        public override string ToString()
        {
            return string.Format("[Index2: x={0}, y={1}]", x, y);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Index3
    {
        public int x, y, z;

        public Index3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        unsafe public int this[int i]
        {
            get
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Index3 index out of range.");

                fixed (Index3* array = &this) { return ((int*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 3)
                    throw new IndexOutOfRangeException("Index3 index out of range.");

                fixed (int* array = &x) { array[i] = value; }
            }
        }

        public override string ToString()
        {
            return string.Format("[Index3: x={0}, y={1}, z={2}]", x, y, z);
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Index4
    {
        public int x, y, z, w;

        public Index4(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        unsafe public int this[int i]
        {
            get
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Index4 index out of range.");

                fixed (Index4* array = &this) { return ((int*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 4)
                    throw new IndexOutOfRangeException("Index4 index out of range.");

                fixed (int* array = &x) { array[i] = value; }
            }
        }

        public override string ToString()
        {
            return string.Format("[Index4: x={0}, y={1}, z={2}, w={3}]", x, y, x, w);
        }
    }
}

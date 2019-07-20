using System;
using System.Collections.Generic;

namespace Common.Core.Bits
{
    public static class Bit
    {

        public static void Set(ref int value, int position)
        {
            value |= (1 << position);
        }

        public static bool IsSet(int value, int position)
        {
            return (value & (1 << position)) != 0;
        }

        public static void Clear(ref int value, int position)
        {
            value &= ~(1 << position);
        }

        public static void Flip(ref int value, int position)
        {
            value ^= (1 << position);
        }
    }
}

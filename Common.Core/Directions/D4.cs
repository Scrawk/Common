using System;
using System.Collections.Generic;

namespace Common.Core.Directions
{
    public static class D4
    {
        public const int LEFT = 0;
        public const int TOP = 1;
        public const int RIGHT = 2;
        public const int BOTTOM = 3;

        public static readonly int[,] OFFSETS = new int[,]
        {
                {-1,0},
                {0,1},
                {1,0},
                {0,-1}
        };

        public static readonly int[] OPPOSITES = new int[]
        {
            RIGHT,
            BOTTOM,
            LEFT,
            TOP
        };

        public static readonly int[,] ADJACENT = new int[,]
        {
            { BOTTOM, TOP },
            { LEFT, RIGHT },
            { TOP, BOTTOM },
            { RIGHT, LEFT }
        };

    }
}

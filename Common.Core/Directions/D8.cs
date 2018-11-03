using System;
using System.Collections.Generic;

namespace Common.Core.Directions
{
    public static class D8
    {
        public const int LEFT = 0;
        public const int LEFT_TOP = 1;
        public const int TOP = 2;
        public const int RIGHT_TOP = 3;
        public const int RIGHT = 4;
        public const int RIGHT_BOTTOM = 5;
        public const int BOTTOM = 6;
        public const int LEFT_BOTTOM = 7;

        public static bool IsDiagonal(int i)
        {
            return i % 2 != 0;
        }

        public static readonly int[,] OFFSETS = new int[,]
        {
                {-1,0},
                {-1,1},
                {0,1},
                {1,1},
                {1,0},
                {1,-1},
                {0,-1},
                {-1,-1},
        };

        public static readonly int[,] DIRECTION = new int[,]
        {
            {7, 0, 1 },
            {6, -1, 2 },
            {5, 4, 3 }
        };

        public static readonly int[] OPPOSITES = new int[]
        {
            RIGHT,
            RIGHT_BOTTOM,
            BOTTOM,
            LEFT_BOTTOM,
            LEFT,
            LEFT_TOP,
            TOP,
            RIGHT_TOP
        };

        public static readonly int[,] ADJACENT = new int[,]
        {
            { LEFT_BOTTOM, LEFT_TOP },
            { LEFT, TOP },
            { LEFT_TOP, RIGHT_TOP },
            { TOP, RIGHT },
            { RIGHT_TOP, RIGHT_BOTTOM },
            { RIGHT, BOTTOM },
            { RIGHT_BOTTOM, LEFT_BOTTOM },
            { BOTTOM, LEFT }
        };

        public static readonly int[] DIAGONAL = new int[]
        {
            LEFT_TOP,
            RIGHT_TOP,
            RIGHT_BOTTOM,
            LEFT_BOTTOM
        };

        public static readonly int[] ORTHOGONAL = new int[]
        {
            LEFT,
            TOP,
            RIGHT,
            BOTTOM
        };

    }
}

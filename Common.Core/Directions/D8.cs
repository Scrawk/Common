using System;
using System.Collections.Generic;

namespace Common.Core.Directions
{
    /// <summary>
    /// Represents the 8 directions around a 2D array
    /// including diagonals.
    /// </summary>
    public static class D8
    {
        /// <summary>
        /// Assign a number to each direction.
        /// </summary>
        public const int LEFT = 0;
        public const int LEFT_TOP = 1;
        public const int TOP = 2;
        public const int RIGHT_TOP = 3;
        public const int RIGHT = 4;
        public const int RIGHT_BOTTOM = 5;
        public const int BOTTOM = 6;
        public const int LEFT_BOTTOM = 7;

        /// <summary>
        /// Is this a diagonal direction.
        /// IsDiagonal(RIGHT_TOP) == true
        /// </summary>
        public static bool IsDiagonal(int i)
        {
            return i % 2 != 0;
        }

        /// <summary>
        /// All the direction values.
        /// </summary>
        public static readonly int[] ALL = new int[]
        {
            LEFT,
            LEFT_TOP,
            TOP,
            RIGHT_TOP,
            RIGHT,
            RIGHT_BOTTOM,
            BOTTOM,
            LEFT_BOTTOM
        };

        /// <summary>
        /// The directions name.
        /// </summary>
        public static readonly string[] NAME = new string[]
        {
                "LEFT",
                "LEFT_TOP",
                "TOP",
                "RIGHT_TOP",
                "RIGHT",
                "RIGHT_BOTTOM",
                "BOTTOM",
                "LEFT_BOTTOM"
        };

        /// <summary>
        /// The offset needed to be applied to a
        /// index to move in that direction.
        /// </summary>
        public static readonly int[,] OFFSETS = new int[,]
        {
                {-1,0},
                {-1,1},
                {0,1},
                {1,1},
                {1,0},
                {1,-1},
                {0,-1},
                {-1,-1}
        };

        /// <summary>
        /// Given a 2D index from 0-2 what direction does it represent.
        /// </summary>
        public static readonly int[,] DIRECTION = new int[,]
        {
            {7, 0, 1 },
            {6, -1, 2 },
            {5, 4, 3 }
        };

        /// <summary>
        /// The opposite direction.
        /// OPPOSITE[LEFT] == RIGHT
        /// </summary>
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

        /// <summary>
        /// In a byte flag which bit does a direction represents.
        /// </summary>
        public static readonly int[] BITS = new int[]
        {
            1,
            2,
            4,
            8,
            16,
            32,
            64,
            128
        };

        /// <summary>
        /// The two adjacent directions.
        /// ADJACENT[LEFT, 0] == LEFT_BOTTOM
        /// ADJACENT[LEFT, 1] == LEFT_TOP
        /// </summary>
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

        /// <summary>
        /// A list of only the diagonal directions.
        /// </summary>
        public static readonly int[] DIAGONAL = new int[]
        {
            LEFT_TOP,
            RIGHT_TOP,
            RIGHT_BOTTOM,
            LEFT_BOTTOM
        };

        /// <summary>
        /// A list of only the orthogonal directions.
        /// </summary>
        public static readonly int[] ORTHOGONAL = new int[]
        {
            LEFT,
            TOP,
            RIGHT,
            BOTTOM
        };

    }
}

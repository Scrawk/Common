using System;
using System.Collections.Generic;

namespace Common.Core.Directions
{
    /// <summary>
    /// Represents the 4 directions around a 2D array
    /// not including diagonals.
    /// </summary>
    public static class D4
    {
        /// <summary>
        /// Assign a number to each direction.
        /// </summary>
        public const int LEFT = 0;
        public const int TOP = 1;
        public const int RIGHT = 2;
        public const int BOTTOM = 3;

        /// <summary>
        /// The offset needed to be applied to a
        /// index to move in that direction.
        /// </summary>
        public static readonly int[,] OFFSETS = new int[,]
        {
                {-1,0},
                {0,1},
                {1,0},
                {0,-1}
        };

        /// <summary>
        /// The opposite direction.
        /// OPPOSITE[LEFT] == RIGHT
        /// </summary>
        public static readonly int[] OPPOSITES = new int[]
        {
            RIGHT,
            BOTTOM,
            LEFT,
            TOP
        };

        /// <summary>
        /// The two adjacent directions.
        /// ADJACENT[LEFT, 0] == BOTTOM
        /// ADJACENT[LEFT, 1] == TOP
        /// </summary>
        public static readonly int[,] ADJACENT = new int[,]
        {
            { BOTTOM, TOP },
            { LEFT, RIGHT },
            { TOP, BOTTOM },
            { RIGHT, LEFT }
        };

    }
}

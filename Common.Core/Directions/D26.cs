using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Directions
{
    /// <summary>
    /// Represents the 26 directions around a 3D array
    /// including diagonals.
    /// </summary>
    public static class D26
    {
        /// <summary>
        /// Assign a number to each direction.
        /// </summary>
        public const int LEFT_TOP = 0;
        public const int LEFT_TOP_FRONT = 1;
        public const int TOP_FRONT = 2;
        public const int RIGHT_TOP_FRONT = 3;
        public const int RIGHT_TOP = 4;
        public const int RIGHT_TOP_BACK = 5;
        public const int TOP_BACK = 6;
        public const int LEFT_TOP_BACK = 7;
        public const int TOP = 8;

        public const int LEFT = 9;
        public const int LEFT_FRONT = 10;
        public const int FRONT = 11;
        public const int RIGHT_FRONT = 12;
        public const int RIGHT = 13;
        public const int RIGHT_BACK = 14;
        public const int BACK = 15;
        public const int LEFT_BACK = 16;

        public const int LEFT_BOTTOM = 17;
        public const int LEFT_BOTTOM_FRONT = 18;
        public const int BOTTOM_FRONT = 19;
        public const int RIGHT_BOTTOM_FRONT = 20;
        public const int RIGHT_BOTTOM = 21;
        public const int RIGHT_BOTTOM_BACK = 22;
        public const int BOTTOM_BACK = 23;
        public const int LEFT_BOTTOM_BACK = 24;
        public const int BOTTOM = 25;

        /// <summary>
        /// The offset needed to be applied to a
        /// index to move in that direction.
        /// </summary>
        public static readonly int[,] OFFSETS = new int[,]
        {
            {-1,1,0}, //LEFT_TOP = 0;
            {-1,1,1}, //LEFT_TOP_FRONT = 1;
            {0,1,1}, //TOP_FRONT = 2;
            {1,1,1}, //RIGHT_TOP_FRONT = 3;
            {1,1,0}, //RIGHT_TOP = 4;
            {1,1,-1}, //RIGHT_TOP_BACK = 5;
            {0,1,-1}, //TOP_BACK = 6;
            {-1,1,-1}, //LEFT_TOP_BACK = 7;
            {0,1,0}, //TOP = 8;

            {-1,0,0}, //LEFT = 9;
            {-1,0,1}, //LEFT_FRONT = 10;
            {0,0,1}, //FRONT = 11;
            {1,0,1}, //RIGHT_FRONT = 12;
            {1,0,0}, //RIGHT = 13;
            {1,0,-1}, //RIGHT_BACK = 14;
            {0,0,-1}, //BACK = 15;
            {-1,0,-1}, //LEFT_BACK = 16;

            {-1,-1,0}, //LEFT_BOTTOM = 17;
            {-1,-1,1}, //LEFT_BOTTOM_FRONT = 18;
            {0,-1,1}, //BOTTOM_FRONT = 19;
            {1,-1,1}, //RIGHT_BOTTOM_FRONT = 20;
            {1,-1,0}, //RIGHT_BOTTOM = 21;
            {1,-1,-1}, //RIGHT_BOTTOM_BACK = 22;
            {0,-1,-1}, //BOTTOM_BACK = 23;
            {-1,-1,-1}, //LEFT_BOTTOM_BACK = 24;
            {0,-1,0}  //BOTTOM = 25;
        };
    }
}

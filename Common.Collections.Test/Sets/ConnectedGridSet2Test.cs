using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Collections.Sets;

namespace Common.Collections.Test.Sets
{

    [TestClass]
    public class ConnectedGridSet2Test
    {
        int[,] map = new int[,]
        {
                {1, 1, 0, 1, 1},
                {1, 1, 0, 1, 0},
                {0, 0, 0, 0, 0},
                {1, 1, 1, 1, 0},
                {1, 1, 0, 0, 1}
        };

        private bool IsForeGround(int x, int y)
        {
            return map[x, y] == ConnectedGridSet2.FOREGROUND;
        }

        [TestMethod]
        public void TestMethod1()
        {

            int width = 5;
            int height = 5;

            var set = new ConnectedGridSet2(width, height, IsForeGround);

            set.Label();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Console.Write(set.GetLabel(y, x) + " ");
                }

                //Console.WriteLine();
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core.Threading
{
    public struct Block1D
    {
        public Block1D(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min;

        public int Max;
    }

    public class ParallelAlgorithm1D : ParallelAlgorithm
    {
        public static IList<Block1D> CreateBlocks(int width, int blockSize, bool single = false)
        {
            if (single)
            {
                var blocks = new List<Block1D>(1);
                blocks.Add(new Block1D(0, width));
                return blocks;
            }
            else
            {
                int size = width / blockSize + 1;
                var blocks = new List<Block1D>(size);

                for (int x = 0; x < width; x += blockSize)
                {
                    var box = new Block1D();
                    box.Min = x;
                    box.Max = Math.Min(x + blockSize, width);

                    blocks.Add(box);
                }

                return blocks;
            }
        }
    }
}

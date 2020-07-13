﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;

namespace Common.Core.Threading
{
    public struct ThreadingBlock1D
    {
        public ThreadingBlock1D(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int Min;

        public int Max;

        public static List<ThreadingBlock1D> CreateBlocks(int width, int blockSize, bool single = false)
        {
            if (single)
            {
                var blocks = new List<ThreadingBlock1D>(1);
                blocks.Add(new ThreadingBlock1D(0, width));
                return blocks;
            }
            else
            {
                int sizeX = width / blockSize + 1;
                var blocks = new List<ThreadingBlock1D>(sizeX);

                for (int x = 0; x < width; x += blockSize)
                {
                    var box = new ThreadingBlock1D();
                    box.Min = x;
                    box.Max = Math.Min(x + blockSize, width);
                    blocks.Add(box);
                }

                return blocks;
            }
        }

        public static void ParallelAction(int width, int blockSize, Action<int> action)
        {
            var blocks = CreateBlocks(width, blockSize);

            Parallel.ForEach(blocks, (block) =>
            {
                for (int x = block.Min; x < block.Max; x++)
                {
                    action(x);
                }
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;

namespace Common.Core.Threading
{
    public struct ThreadingBlock2D
    {
        public ThreadingBlock2D(int minX, int maxX, int minY, int maxY)
        {
            Min = new Vector2i(minX, minY);
            Max = new Vector2i(maxX, maxY);
        }

        public ThreadingBlock2D(Vector2i min, Vector2i max)
        {
            Min = min;
            Max = max;
        }

        public Vector2i Min;

        public Vector2i Max;

        public static int BlockSize(Vector2i size, int divisions = 4)
        {
            if (divisions <= 0) divisions = 4;
            return Math.Max(64, Math.Max(size.x, size.y) / divisions);
        }

        public static int BlockSize(int width, int height, int divisions = 4)
        {
            if (divisions <= 0) divisions = 4;
            return Math.Max(64, Math.Max(width, height) / divisions);
        }

        public static List<ThreadingBlock2D> CreateBlocks(Vector2i size, int blockSize)
        {
            return CreateBlocks(size.x, size.y, blockSize);
        }

        public static List<ThreadingBlock2D> CreateBlocks(int width, int height, int blockSize)
        {
            int sizeX = width / blockSize + 1;
            int sizeY = height / blockSize + 1;
            var blocks = new List<ThreadingBlock2D>(sizeX * sizeY);

            for (int y = 0; y < height; y += blockSize)
            {
                for (int x = 0; x < width; x += blockSize)
                {
                    var box = new ThreadingBlock2D();
                    box.Min = new Vector2i(x, y);
                    box.Max.x = Math.Min(x + blockSize, width);
                    box.Max.y = Math.Min(y + blockSize, height);

                    blocks.Add(box);
                }
            }

            return blocks;
        }

        public static void ParallelAction(Vector2i size, int blockSize, Action<int, int> action)
        {
            ParallelAction(size.x, size.y, blockSize, action);
        }

        public static void ParallelAction(int width, int height, int blockSize, Action<int, int> action)
        {
            var blocks = CreateBlocks(width, height, blockSize);

            Parallel.ForEach(blocks, (block) =>
            {
                for (int y = block.Min.y; y < block.Max.y; y++)
                {
                    for (int x = block.Min.x; x < block.Max.x; x++)
                    {
                        action(x, y);
                    }
                }

            });
        }
        

    }
}


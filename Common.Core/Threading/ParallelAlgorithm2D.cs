using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Colors;

namespace Common.Core.Threading
{
    public struct Block2D
    {
        public Block2D(int minX, int maxX, int minY, int maxY)
        {
            Min = new Vector2i(minX, minY);
            Max = new Vector2i(maxX, maxY);
        }

        public Block2D(Vector2i min, Vector2i max)
        {
            Min = min;
            Max = max;
        }

        public Vector2i Min;

        public Vector2i Max;
    }

    public class ParallelAlgorithm2D : ParallelAlgorithm
    {

        public static IList<Block2D> CreateBlocks(int width, int height, int blockSize, bool single = false)
        {
            if (single)
            {
                var blocks = new List<Block2D>(1);
                blocks.Add(new Block2D(0, width, 0, height));
                return blocks;
            }
            else
            {
                int sizeX = width / blockSize + 1;
                int sizeY = height / blockSize + 1;
                var blocks = new Block2D[sizeX * sizeY];

                int i = 0;
                for (int y = 0; y < height; y += blockSize)
                {
                    for (int x = 0; x < width; x += blockSize)
                    {
                        var box = new Block2D();
                        box.Min = new Vector2i(x, y);
                        box.Max.x = Math.Min(x + blockSize, width);
                        box.Max.y = Math.Min(y + blockSize, height);

                        blocks[i++] = box;
                    }
                }

                return blocks;
            }

        }
    }
}

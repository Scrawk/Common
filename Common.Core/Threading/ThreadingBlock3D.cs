using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.Threading
{
    public struct ThreadingBlock3D
    {
        public ThreadingBlock3D(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            Min = new Vector3i(minX, minY, minZ);
            Max = new Vector3i(maxX, maxY, maxZ);
        }

        public ThreadingBlock3D(Vector3i min, Vector3i max)
        {
            Min = min;
            Max = max;
        }

        public Vector3i Min;

        public Vector3i Max;

        public static List<ThreadingBlock3D> CreateBlocks(Vector3i size, int blockSize, bool single = false)
        {
            return CreateBlocks(size.x, size.y, size.z, blockSize, single);
        }

        public static List<ThreadingBlock3D> CreateBlocks(int width, int height, int depth, int blockSize, bool single = false)
        {
            if (single)
            {
                var blocks = new List<ThreadingBlock3D>(1);
                blocks.Add(new ThreadingBlock3D(0, width, 0, height, 0, depth));
                return blocks;
            }
            else
            {
                int sizeX = width / blockSize + 1;
                int sizeY = height / blockSize + 1;
                int sizeZ = depth / blockSize + 1;
                var blocks = new List<ThreadingBlock3D>(sizeX * sizeY * sizeZ);

                for (int z = 0; z < depth; z += blockSize)
                {
                    for (int y = 0; y < height; y += blockSize)
                    {
                        for (int x = 0; x < width; x += blockSize)
                        {
                            var box = new ThreadingBlock3D();
                            box.Min = new Vector3i(x, y, z);
                            box.Max.x = Math.Min(x + blockSize, width);
                            box.Max.y = Math.Min(y + blockSize, height);
                            box.Max.z = Math.Min(z + blockSize, depth);

                            blocks.Add(box);
                        }
                    }
                }

                return blocks;
            }
        }
    }
}


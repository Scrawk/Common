﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;

namespace Common.Core.Threading
{
    public struct ThreadingBlock3D
    {
        public ThreadingBlock3D(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            Min = new Point3i(minX, minY, minZ);
            Max = new Point3i(maxX, maxY, maxZ);
        }

        public ThreadingBlock3D(Point3i min, Point3i max)
        {
            Min = min;
            Max = max;
        }

        public Point3i Min;

        public Point3i Max;

        public static int BlockSize(Point3i size, int divisions = 2)
        {
            if (divisions <= 0) divisions = 2;
            return Math.Min(16, MathUtil.Max(size.x, size.y, size.z) / divisions);
        }

        public static int BlockSize(int width, int height, int depth, int divisions = 2)
        {
            if (divisions <= 0) divisions = 2;
            return Math.Min(16, MathUtil.Max(width, height, depth) / divisions);
        }

        public static List<ThreadingBlock3D> CreateBlocks(Point3i size, int blockSize)
        {
            return CreateBlocks(size.x, size.y, size.z, blockSize);
        }

        public static List<ThreadingBlock3D> CreateBlocks(int width, int height, int depth, int blockSize)
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
                        box.Min = new Point3i(x, y, z);
                        box.Max.x = Math.Min(x + blockSize, width);
                        box.Max.y = Math.Min(y + blockSize, height);
                        box.Max.z = Math.Min(z + blockSize, depth);

                        blocks.Add(box);
                    }
                }
            }

            return blocks;
        }

        public static void ParallelAction(Point3i size, int blockSize, Action<int, int, int> action)
        {
            ParallelAction(size.x, size.y, size.z, blockSize, action);
        }

        public static void ParallelAction(int width, int height, int depth, int blockSize, Action<int, int, int> action, bool disableThreading = false)
        {

            if (disableThreading)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            action(x, y, z);
                        }
                    }
                }
            }
            else
            {
                var blocks = CreateBlocks(width, height, depth, blockSize);
                Parallel.ForEach(blocks, (block) =>
                {
                    for (int z = block.Min.z; z < block.Max.z; z++)
                    {
                        for (int y = block.Min.y; y < block.Max.y; y++)
                        {
                            for (int x = block.Min.x; x < block.Max.x; x++)
                            {
                                action(x, y, z);
                            }
                        }
                    }

                });
            }
        }

    }
}


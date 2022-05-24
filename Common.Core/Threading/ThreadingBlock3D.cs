using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;

namespace Common.Core.Threading
{
    /// <summary>
    /// A helper object to break a iteration into smaller 
    /// blocks that can be run on seperate tasks. 
    /// </summary>
    public struct ThreadingBlock3D
    {
        /// <summary>
        /// Create a new block.
        /// </summary>
        /// <param name="start">The blocks start index in the iteration.</param>
        /// <param name="end">The blocks end index in the iteration.</param>
        public ThreadingBlock3D(Point3i start, Point3i end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The blocks start index in the iteration.
        /// </summary>
        public Point3i Start;

        /// <summary>
        /// The blocks end index in the iteration.
        /// </summary>
        public Point3i End;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[ThreadingBlock3D: Start={0}, End={1}]", Start, End);
        }

        /// <summary>
        /// The block size is the number of iterarations in the loop each thread will be assigned.
        /// A reconmended maximum block size of 16 will be enforced.
        /// </summary>
        /// <param name="size">The number of iterations in the loop.</param>
        /// <param name="divisions">The number of blocks the size is divided into.</param>
        /// <returns></returns>
        public static int BlockSize(Point3i size, int divisions = 2)
        {
            if (divisions <= 0) 
                divisions = 2;

            int count = MathUtil.Max(size.x, size.y, size.z);

            if (divisions >= count)
                return 1;

            return Math.Min(64, count / divisions);
        }

        /// <summary>
        /// The block size is the number of iterarations in the loop each thread will be assigned.
        /// A reconmended maximum block size of 64 will be enforced.
        /// </summary>
        /// <param name="width">The number of iterations in the loop on the x axis.</param>
        /// <param name="height">The number of iterations in the loop on the y axis.</param>
        /// <param name="depth">The number of iterations in the loop on the z axis.</param>
        /// <param name="divisions">The number of blocks the size is divided into.</param>
        /// <returns></returns>
        public static int BlockSize(int width, int height, int depth, int divisions = 2)
        {
            if (divisions <= 0) 
                divisions = 2;

            int count = MathUtil.Max(width, height, depth);

            if (divisions >= count)
                return 1;

            return Math.Min(64, count / divisions);
        }

        /// <summary>
        /// Create the blocks the parallel action will be performed on.
        /// </summary>
        /// <param name="size">The number of iterations in the loop.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <returns></returns>
        public static List<ThreadingBlock3D> CreateBlocks(Point3i size, int blockSize)
        {
            return CreateBlocks(size.x, size.y, size.z, blockSize);
        }

        /// <summary>
        /// Create the blocks the parallel action will be performed on.
        /// </summary>
        /// <param name="width">The number of iterations in the loop on the x axis.</param>
        /// <param name="height">The number of iterations in the loop on the y axis.</param>
        /// <param name="depth">The number of iterations in the loop on the z axis.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <returns></returns>
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
                        box.Start = new Point3i(x, y, z);
                        box.End.x = Math.Min(x + blockSize - 1, width - 1);
                        box.End.y = Math.Min(y + blockSize - 1, height - 1);
                        box.End.z = Math.Min(z + blockSize - 1, depth - 1);

                        blocks.Add(box);
                    }
                }
            }

            return blocks;
        }

        /// <summary>
        /// Run a action in parallel.
        /// </summary>
        /// <param name="size">The number of iterations in the loop.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="token">A optional helper token.</param>
        public static double ParallelAction(Point3i size, int blockSize, Action<int, int, int> action, ThreadingToken token = null)
        {
           return ParallelAction(size.x, size.y, size.z, blockSize, action, token);
        }

        /// <summary>
        /// Run a action in parallel.
        /// </summary>
        /// <param name="width">The number of iterations in the loop on the x axis.</param>
        /// <param name="height">The number of iterations in the loop on the y axis.</param>
        /// <param name="depth">The number of iterations in the loop on the z axis.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="token">A optional helper token.</param>
        public static double ParallelAction(int width, int height, int depth, int blockSize, Action<int, int, int> action, ThreadingToken token = null)
        {

            if (token != null && !token.UseThreading)
            {
                for (int z = 0; z < depth; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            action(x, y, z);

                            if (token != null)
                            {
                                if (token.Cancelled)
                                    return 0;

                                token.IncrementProgess();
                            }
                        }
                    }
                }
            }
            else
            {
                var blocks = CreateBlocks(width, height, depth, blockSize);
                Parallel.ForEach(blocks, (block) =>
                {
                    for (int z = block.Start.z; z <= block.End.z; z++)
                    {
                        for (int y = block.Start.y; y <= block.End.y; y++)
                        {
                            for (int x = block.Start.x; x <= block.End.x; x++)
                            {
                                action(x, y, z);

                                if (token != null)
                                {
                                    if (token.Cancelled)
                                        return;

                                    token.IncrementProgess();
                                }
                            }
                        }
                    }

                });
            }

            if (token != null)
                return token.ElapsedTime();
            else
                return 0;
        }

    }
}


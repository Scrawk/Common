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
    public struct ThreadingBlock2D
    {
        /// <summary>
        /// Create a new block.
        /// </summary>
        /// <param name="start">The blocks start index in the iteration.</param>
        /// <param name="end">The blocks end index in the iteration.</param>
        public ThreadingBlock2D(Point2i start, Point2i end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The blocks start index in the iteration.
        /// </summary>
        public Point2i Start;

        /// <summary>
        /// The blocks end index in the iteration.
        /// </summary>
        public Point2i End;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[ThreadingBlock2D: Start={0}, End={1}]", Start, End);
        }

        /// <summary>
        /// The block size is the number of iterarations in the loop each thread will be assigned.
        /// A reconmended maximum block size of 64 will be enforced.
        /// </summary>
        /// <param name="size">The number of iterations in the loop.</param>
        /// <param name="divisions">The number of blocks the size is divided into.</param>
        /// <returns></returns>
        public static int BlockSize(Point2i size, int divisions = 4)
        {
            if (divisions <= 0) 
                divisions = 4;

            int count = Math.Max(size.x, size.y);

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
        /// <param name="divisions">The number of blocks the size is divided into.</param>
        /// <returns></returns>
        public static int BlockSize(int width, int height, int divisions = 4)
        {
            if (divisions <= 0) 
                divisions = 4;

            int count = Math.Max(width, height);

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
        public static List<ThreadingBlock2D> CreateBlocks(Point2i size, int blockSize)
        {
            return CreateBlocks(size.x, size.y, blockSize);
        }

        /// <summary>
        /// Create the blocks the parallel action will be performed on.
        /// </summary>
        /// <param name="width">The number of iterations in the loop on the x axis.</param>
        /// <param name="height">The number of iterations in the loop on the y axis.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <returns></returns>
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
                    box.Start = new Point2i(x, y);
                    box.End.x = Math.Min(x + blockSize - 1, width - 1);
                    box.End.y = Math.Min(y + blockSize - 1, height - 1);

                    blocks.Add(box);
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
        public static void ParallelAction(Point2i size, int blockSize, Action<int, int> action, ThreadingToken token = null)
        {
            ParallelAction(size.x, size.y, blockSize, action,  token);
        }

        /// <summary>
        /// Run a action in parallel.
        /// </summary>
        /// <param name="width">The number of iterations in the loop on the x axis.</param>
        /// <param name="height">The number of iterations in the loop on the y axis.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="token">A optional helper token.</param>
        public static double ParallelAction(int width, int height, int blockSize, Action<int, int> action, ThreadingToken token = null)
        {

            if (token != null && !token.UseThreading)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        action(x, y);

                        if (token != null)
                        {
                            if (token.Cancelled)
                                return 0;

                            token.IncrementProgess();
                        }
                    }
                }
            }
            else
            {
                var blocks = CreateBlocks(width, height, blockSize);

                Parallel.ForEach(blocks, (block) =>
                {
                    for (int y = block.Start.y; y <= block.End.y; y++)
                    {
                        for (int x = block.Start.x; x <= block.End.x; x++)
                        {
                            action(x, y);

                            if (token != null)
                            {
                                if (token.Cancelled)
                                    return;

                                token.IncrementProgess();
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


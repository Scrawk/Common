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
    public struct ThreadingBlock1D
    {

        /// <summary>
        /// Create a new block.
        /// </summary>
        /// <param name="start">The blocks start index in the iteration.</param>
        /// <param name="end">The blocks end index in the iteration.</param>
        public ThreadingBlock1D(int start, int end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// The blocks start index in the iteration.
        /// </summary>
        public int Start;

        /// <summary>
        /// The blocks end index in the iteration.
        /// </summary>
        public int End;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[ThreadingBlock1D: Start={0}, End={1}]", Start, End);
        }

        /// <summary>
        /// The block size is the number of iterarations in the loop each thread will be assigned.
        /// A reconmended maximum block size of 4096 will be enforced.
        /// </summary>
        /// <param name="count">The number of iterations in the loop.</param>
        /// <param name="divisions">The number of blocks the count is divided into.</param>
        /// <returns></returns>
        public static int BlockSize(int count, int divisions = 16)
        {
            if (divisions <= 0) divisions = 16;
            return Math.Min(4096, count / divisions);
        }

        /// <summary>
        /// Create the blocks the parallel action will be performed on.
        /// </summary>
        /// <param name="count">The number of iterations in the loop.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <returns></returns>
        public static List<ThreadingBlock1D> CreateBlocks(int count, int blockSize)
        {
            int size = count / blockSize + 1;
            var blocks = new List<ThreadingBlock1D>(size);

            for (int x = 0; x < count; x += blockSize)
            {
                var box = new ThreadingBlock1D();
                box.Start = x;
                box.End = Math.Min(x + blockSize - 1, count);
                blocks.Add(box);
            }

            return blocks;
        }

        /// <summary>
        /// Run a action in parallel.
        /// </summary>
        /// <param name="count">The number of iterations in the loop.</param>
        /// <param name="blockSize">The block size is the number of iterarations in the loop each thread will be assigned.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="token">A optional helper token.</param>
        public static double ParallelAction(int count, int blockSize, Action<int> action, ThreadingToken token = null)
        {
            if(token != null && !token.UseThreading)
            {
                for (int i = 0; i < count; i++)
                {
                    action(i);

                    if(token != null)
                    {
                        if (token.Cancelled)
                            return 0;

                        token.IncrementProgess();
                    }
                }
                    
            }
            else
            {
                var blocks = CreateBlocks(count, blockSize);
                Parallel.ForEach(blocks, (block) =>
                {
                    for (int x = block.Start; x <= block.End; x++)
                    {
                        action(x);

                        if (token != null)
                        {
                            if (token.Cancelled)
                                return;

                            token.IncrementProgess();
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


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;
using Common.Core.Threading;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// Wrapper for system array using the general IArray2 interface.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class Array2<T> : IArray2<T>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        public Array2(int width, int height)
        {
            Data = new T[width, height];
        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        public Array2(Vector2i size)
        {
            Data = new T[size.x, size.y];
        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public Array2(T[,] data)
        {
            Data = data.Copy();
        }

        public T[,] Data { get; protected set; }

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int Count {  get { return Data.Length; } }

        /// <summary>
        /// The size of the arrays 1st dimention.
        /// </summary>
        public int Width { get { return Data.GetLength(0); } }

        /// <summary>
        /// The size of the arrays 2st dimention.
        /// </summary>
        public int Height { get { return Data.GetLength(1); } }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        public T this[int x, int y]
        {
            get { return Data[x, y]; }
            set { Data[x, y] = value; }
        }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        public T this[Vector2i i]
        {
            get { return Data[i.x, i.y]; }
            set { Data[i.x, i.y] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Array2: Width={0}, Height={1}, Count={2}]", Width, Height, Count);
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in Data)
                yield return t;
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Sets all elements in the array to default value.
        /// </summary>
        public void Clear()
        {
            Data.Clear();
        }

        /// <summary>
        /// Get the element at clamped index x,y.
        /// </summary>
        public T GetClamped(int x, int y)
        {
            x = MathUtil.Clamp(x, 0, Width - 1);
            y = MathUtil.Clamp(y, 0, Height - 1);
            return Data[x, y];
        }

        /// <summary>
        /// Get the element at wrapped index x,y.
        /// </summary>
        public T GetWrapped(int x, int y)
        {
            x = MathUtil.Wrap(x, Width);
            y = MathUtil.Wrap(y, Height);
            return Data[x, y];
        }

        /// <summary>
        /// Fill the array with the value.
        /// </summary>
        public void Fill(T value)
        {
            Data.Fill(value);
        }

        /// <summary>
        /// Fill the array with the value from the function.
        /// </summary>
        public void Fill(Func<int, int, T> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Data[x, y] = func(x, y);
                }
            }
        }

        /// <summary>
        /// Fill the array with the value from the function in parallel.
        /// </summary>
        public void ParallelFill(int blockSize, Func<int, int, T> func)
        {
            var blocks = ThreadingBlock2D.CreateBlocks(Width, Height, blockSize);

            Parallel.ForEach(blocks, (block) =>
            {
                for (int y = block.Min.y; y < block.Max.y; y++)
                {
                    for (int x = block.Min.x; x < block.Max.x; x++)
                    {
                        Data[x, y] = func(x, y);
                    }
                }
            });
        }
    }
}

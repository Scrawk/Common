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
        /// Is this array the same size as the other array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public bool IsSameSize<S>(IArray2<S> array)
        {
            if (Width != array.Width) return false;
            if (Height != array.Height) return false;
            return true;
        }

        /// <summary>
        /// Are the x and y index in the bounds of the array.
        /// </summary>
        public bool InBounds(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;
            return true;
        }

        /// <summary>
        /// Are the x and y index not in the bounds of the array.
        /// </summary>
        public bool NotInBounds(int x, int y)
        {
            return !InBounds(x, y);
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
        /// Resize the array. Will clear any existing data.
        /// </summary>
        public void Resize(int width, int height)
        {
            Data = new T[width, height];
        }

        /// <summary>
        /// Resize the array. Will clear any existing data.
        /// </summary>
        public void Resize(Vector2i size)
        {
            Data = new T[size.x, size.y];
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
        /// Get the element at mirrored index x,y.
        /// </summary>
        public T GetMirrored(int x, int y)
        {
            x = MathUtil.Mirror(x, Width);
            y = MathUtil.Mirror(y, Height);
            return Data[x, y];
        }

        /// <summary>
        /// Recommended blocks for parallel processing.
        /// </summary>
        /// <param name="divisions">Number of divisions on each axis to make.</param>
        /// <returns></returns>
        public int BlockSize(int divisions = 4)
        {
            return ThreadingBlock2D.BlockSize(Width, Height, divisions);
        }

        /// <summary>
        /// Iterate over the array with the action.
        /// </summary>
        public void Iterate(Action<int, int> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    func(x, y);
                }
            }
        }

        /// <summary>
        /// Iterate over the array with the action in parallel.
        /// </summary>
        public void ParallelIterate(Action<int, int> func)
        {
            ParallelIterate(BlockSize(), func);
        }

        /// <summary>
        /// Iterate over the array with the action in parallel.
        /// </summary>
        public void ParallelIterate(int blockSize, Action<int, int> func)
        {
            var blocks = ThreadingBlock2D.CreateBlocks(Width, Height, blockSize);
            Parallel.ForEach(blocks, (block) =>
            {
                for (int y = block.Min.y; y < block.Max.y; y++)
                {
                    for (int x = block.Min.x; x < block.Max.x; x++)
                    {
                        func(x, y);
                    }
                }
            });
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
        public void ParallelFill(Func<int, int, T> func)
        {
            ParallelFill(BlockSize(), func);
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

        /// <summary>
        /// Modify the array with the function.
        /// </summary>
        public void Modify(Func<T, T> func)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Data[x, y] = func(Data[x, y]);
                }
            }
        }

        /// <summary>
        /// Modify the array with the function in parallel.
        /// </summary>
        public void ParallelModify(Func<T, T> func)
        {
            ParallelModify(BlockSize(), func);
        }

        /// <summary>
        /// Modify the array with the function in parallel.
        /// </summary>
        public void ParallelModify(int blockSize, Func<T, T> func)
        {
            var blocks = ThreadingBlock2D.CreateBlocks(Width, Height, blockSize);
            Parallel.ForEach(blocks, (block) =>
            {
                for (int y = block.Min.y; y < block.Max.y; y++)
                {
                    for (int x = block.Min.x; x < block.Max.x; x++)
                    {
                        Data[x, y] = func(Data[x, y]);
                    }
                }
            });
        }

    }
}

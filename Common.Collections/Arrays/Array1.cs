using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Common.Core.Numerics;
using Common.Core.Threading;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// Wrapper for system array using the general IArray1 interface.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class Array1<T> : IArray1<T>
    {

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="count">The size of the arrays 1st dimention.</param>
        public Array1(int count)
        {
            Data = new T[count];
        }

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="data">The data form the array. Will be deep copied.</param>
        public Array1(T[] data)
        {
            Data = data.Copy();
        }

        public T[] Data { get; protected set; }

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int Count {  get { return Data.Length; } }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        public T this[int x]
        {
            get { return Data[x]; }
            set { Data[x] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Array1: Count={0}]", Count);
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
        /// Get the element at clamped index x.
        /// </summary>
        public T GetClamped(int x)
        {
            x = MathUtil.Clamp(x, 0, Count - 1);
            return Data[x];
        }

        /// <summary>
        /// Get the element at wrapped index x.
        /// </summary>
        public T GetWrapped(int x)
        {
            x = MathUtil.Wrap(x, Count);
            return Data[x];
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
        public void Fill(Func<int, T> func)
        {
                for (int x = 0; x < Count; x++)
                {
                    Data[x] = func(x);
                }
        }

        /// <summary>
        /// Fill the array with the value from the function in parallel.
        /// </summary>
        public void ParallelFill(int blockSize, Func<int, T> func)
        {
            var blocks = ThreadingBlock1D.CreateBlocks(Count, blockSize);

            Parallel.ForEach(blocks, (block) =>
            {
                    for (int x = block.Min; x < block.Max; x++)
                    {
                        Data[x] = func(x);
                    }
            });
        }
    }
}

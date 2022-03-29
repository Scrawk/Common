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
        /// Is this index in the bounds of the array.
        /// </summary>
        public bool InBounds(int x)
        {
            if (x < 0 || x >= Count) return false;
            return true;
        }

        /// <summary>
        /// Is this index not in the bounds of the array.
        /// </summary>
        public bool NotInBounds(int x)
        {
            return !InBounds(x);
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
        /// <param name="count"></param>
        public void Resize(int count)
        {
            Data = new T[count];
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
        /// Get the element at mirrored index x.
        /// </summary>
        public T GetMirrored(int x)
        {
            x = MathUtil.Mirror(x, Count);
            return Data[x];
        }

        /// <summary>
        /// Set the element at clamped index x.
        /// </summary>
        public void SetClamped(int x, T value)
        {
            x = MathUtil.Clamp(x, 0, Count - 1);
            Data[x] = value;
        }

        /// <summary>
        /// Set the element at wrapped index x.
        /// </summary>
        public void SetWrapped(int x, T value)
        {
            x = MathUtil.Wrap(x, Count);
            Data[x] = value;
        }

        /// <summary>
        /// Set the element at mirred index x.
        /// </summary>
        public void SetMirrored(int x, T value)
        {
            x = MathUtil.Mirror(x, Count);
            Data[x] = value;
        }

        /// <summary>
        /// Recommended blocks for parallel processing.
        /// </summary>
        /// <param name="divisions">Number of divisions on each axis to make.</param>
        /// <returns></returns>
        public int BlockSize(int divisions = 16)
        {
            return ThreadingBlock2D.BlockSize(Count, divisions);
        }

        /// <summary>
        /// Iterate over the array with the action.
        /// </summary>
        public void Iterate(Action<int> func)
        {
            for (int x = 0; x < Count; x++)
            {
                func(x);
            }
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
        /// Modify the array with the function.
        /// </summary>
        public void Modify(Func<T, T> func)
        {
            for (int x = 0; x < Count; x++)
            {
                Data[x] = func(Data[x]);
            }
        }

    }
}

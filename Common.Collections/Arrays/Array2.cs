using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// Wrapper for system array using the general array2 interface.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class Array2<T> : IArray2<T>
    {

        private T[,] m_array;

        /// <summary>
        /// Create a new array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        public Array2(int width, int height)
        {
            m_array = new T[width, height];
        }

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        public int Count {  get { return m_array.Length; } }

        /// <summary>
        /// The size of the arrays 1st dimention.
        /// </summary>
        public int Width { get { return m_array.GetLength(0); } }

        /// <summary>
        /// The size of the arrays 2st dimention.
        /// </summary>
        public int Height { get { return m_array.GetLength(1); } }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        public T this[int x, int y]
        {
            get { return m_array[x, y]; }
            set { m_array[x, y] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[SystemArray: Width={0}, Height={1}, Count={2}]", Width, Height, Count);
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in m_array)
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
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    m_array[x, y] = default(T);
                }
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    public class SystemArray2<T> : IArray2<T>
    {

        private T[,] m_array;

        public SystemArray2(int width, int height)
        {
            m_array = new T[width, height];
        }

        public int Count {  get { return m_array.Length; } }

        public int Width { get { return m_array.GetLength(0); } }

        public int Height { get { return m_array.GetLength(1); } }

        public T this[int x, int y]
        {
            get { return m_array[x, y]; }
            set { m_array[x, y] = value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var t in m_array)
                yield return t;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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

using System;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    public interface IArray2<T> : IEnumerable<T>
    {

        int Count { get; }

        int Width { get; }

        int Height { get; }

        T this[int x, int y] { get; set; }

        void Clear();

    }
}

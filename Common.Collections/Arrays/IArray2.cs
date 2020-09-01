using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// General interface for a 2 dimensional array.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface IArray2<T> : IEnumerable<T>
    {
        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The size of the arrays 1st dimention.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// The size of the arrays 2st dimention.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        T this[int x, int y] { get; set; }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
        T this[Vector2i i] { get; set; }

        /// <summary>
        /// Sets all elements in the array to default value.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the element at clamped index x,y.
        /// </summary>
        T GetClamped(int x, int y);

        /// <summary>
        /// Get the element at wrapped index x,y.
        /// </summary>
        T GetWrapped(int x, int y);

        /// <summary>
        /// Get the element at mirrored index x,y.
        /// </summary>
        T GetMirrored(int x, int y);

    }
}

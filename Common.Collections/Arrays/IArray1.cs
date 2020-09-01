using System;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// General interface for a 1 dimensional array.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public interface IArray1<T> : IEnumerable<T>
    {
        /// <summary>
        /// The number of elements in the array.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Access a element at index x.
        /// </summary>
        T this[int x] { get; set; }

        /// <summary>
        /// Sets all elements in the array to default value.
        /// </summary>
        void Clear();

        /// <summary>
        /// Get the element at clamped index x.
        /// </summary>
        T GetClamped(int x);

        /// <summary>
        /// Get the element at wrapped index x.
        /// </summary>
        T GetWrapped(int x);

        /// <summary>
        /// Get the element at mirrored index x.
        /// </summary>
        T GetMirrored(int x);

    }
}

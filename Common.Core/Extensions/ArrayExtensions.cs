using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class ArrayExtensions
    {
        public static void Clear(this Array array)
        {
            Array.Clear(array, 0, array.Length);
        }

        public static int IndexOf<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                if (EqualityComparer<T>.Default.Equals(array[i], value))
                    return i;

            return -1;
        }

        public static S[] ConvertAll<T, S>(this T[] array, Func<T, S> func)
        {
            var dest = new S[array.Length];
            for (int i = 0; i < array.Length; i++)
                dest[i] = func(array[i]);
            return dest;
        }

        public static T[] Copy<T>(this T[] array)
        {
            var dest = new T[array.Length];
            Array.Copy(array, dest, array.Length);
            return dest;
        }

        public static T[,] Copy<T>(this T[,] array)
        {
            var dest = new T[array.GetLength(0), array.GetLength(1)];
            Array.Copy(array, dest, array.Length);
            return dest;
        }

        public static T[,,] Copy<T>(this T[,,] array)
        {
            var dest = new T[array.GetLength(0), array.GetLength(1), array.GetLength(2)];
            Array.Copy(array, dest, array.Length);
            return dest;
        }

        public static void Fill<T>(this T[] array, T value)
        {
            for (int x = 0; x < array.Length; x++)
                array[x] = value;
        }

        public static void Fill<T>(this T[] array, Func<int, T> func)
        {
            for (int x = 0; x < array.Length; x++)
                array[x] = func(x);
        }

        public static void Fill<T>(this T[,] array, T value)
        {
            for (int y = 0; y < array.GetLength(1); y++)
                for (int x = 0; x < array.GetLength(0); x++)
                    array[x,y] = value;
        }

        public static void Fill<T>(this T[,] array, Func<int, int, T> func)
        {
            for (int y = 0; y < array.GetLength(1); y++)
                for (int x = 0; x < array.GetLength(0); x++)
                    array[x, y] = func(x, y);
        }

        public static void Fill<T>(this T[,,] array, T value)
        {
            for (int z = 0; z < array.GetLength(2); z++)
                for (int y = 0; y < array.GetLength(1); y++)
                    for (int x = 0; x < array.GetLength(0); x++)
                        array[x, y, z] = value;
        }

        public static void Fill<T>(this T[,,] array, Func<int, int, int, T> func)
        {
            for (int z = 0; z < array.GetLength(2); z++)
                for (int y = 0; y < array.GetLength(1); y++)
                    for (int x = 0; x < array.GetLength(0); x++)
                        array[x, y, z] = func(x,y,z);
        }

    }
}

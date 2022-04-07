using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Core.Extensions
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

        public static T GetWrapped<T>(this T[] array, int x)
        {
            x = MathUtil.Wrap(x, array.Length);
            return array[x];
        }

        public static T GetClamped<T>(this T[] array, int x)
        {
            x = MathUtil.Clamp(x, 0, array.Length - 1);
            return array[x];
        }

        public static T GetWrapped<T>(this T[,] array, int x, int y)
        {
            x = MathUtil.Wrap(x, array.GetLength(0));
            y = MathUtil.Wrap(y, array.GetLength(1));
            return array[x, y];
        }

        public static T GetClamped<T>(this T[,] array, int x, int y)
        {
            x = MathUtil.Clamp(x, 0, array.GetLength(0) - 1);
            y = MathUtil.Clamp(y, 0, array.GetLength(1) - 1);
            return array[x, y];
        }

        public static T GetWrapped<T>(this T[,,] array, int x, int y, int z)
        {
            x = MathUtil.Wrap(x, array.GetLength(0));
            y = MathUtil.Wrap(y, array.GetLength(1));
            z = MathUtil.Wrap(z, array.GetLength(2));

            return array[x, y, z];
        }

        public static T GetClamped<T>(this T[,,] array, int x, int y, int z)
        {
            x = MathUtil.Clamp(x, 0, array.GetLength(0) - 1);
            y = MathUtil.Clamp(y, 0, array.GetLength(1) - 1);
            z = MathUtil.Clamp(z, 0, array.GetLength(2) - 1);
            return array[x, y, z];
        }

    }
}

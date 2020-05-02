﻿using System;
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

        public static void Fill<T>(this T[] array, T value)
        {
            for (int x = 0; x < array.Length; x++)
                array[x] = value;
        }

        public static void Fill<T>(this T[,] array, T value)
        {
            for (int y = 0; y < array.GetLength(1); y++)
                for (int x = 0; x < array.GetLength(0); x++)
                    array[x,y] = value;
        }

        public static void Fill<T>(this T[,,] array, T value)
        {
            for (int z = 0; z < array.GetLength(2); z++)
                for (int y = 0; y < array.GetLength(1); y++)
                    for (int x = 0; x < array.GetLength(0); x++)
                        array[x, y, z] = value;
        }
    }
}
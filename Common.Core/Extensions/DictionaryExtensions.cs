﻿using System;
using System.Collections.Generic;

namespace Common.Core.Extensions
{
    public static class KeyValuePairExtensions
    {
        public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
        {
            key = tuple.Key;
            value = tuple.Value;
        }
    }

    public static class DictionaryExtensions
    {

    }
}

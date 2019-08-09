using System;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static bool TryAdd<K,T>(this Dictionary<K, T> dic, K key, T value)
        {
            if (dic.ContainsKey(key)) return false;
            dic.Add(key, value);
            return true;
        }

    }
}

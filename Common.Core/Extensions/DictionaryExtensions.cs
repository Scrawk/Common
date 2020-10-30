using System;

namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Try and and a new value to the dictionary.
        /// </summary>
        /// <param name="dic">The dictionary.</param>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the value added, false if not
        /// added because another value with the same key aready exists.</returns>
        public static bool TryAdd<K,T>(this Dictionary<K, T> dic, K key, T value)
        {
            if (dic.ContainsKey(key)) return false;
            dic.Add(key, value);
            return true;
        }

    }
}

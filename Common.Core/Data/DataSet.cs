using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Core.Data
{
    /// <summary>
    /// A set of data and keys.
    /// Different data types may have the same key.
    /// </summary>
    /// <typeparam name="KEY"></typeparam>
    public class DataSet<KEY>
    {

        private Dictionary<KEY, bool> m_bools;

        private Dictionary<KEY, int> m_ints;

        private Dictionary<KEY, float> m_floats;

        private Dictionary<KEY, string> m_strings;

        private Dictionary<KEY, Vector3f> m_vectors;

        private Dictionary<KEY, object> m_objects;

        private Dictionary<KEY, List<object>> m_lists;

        /// <summary>
        /// The number of data elements in the set.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                if (m_bools != null) count += m_bools.Count;
                if (m_ints != null) count += m_ints.Count;
                if (m_floats != null) count += m_floats.Count;
                if (m_strings != null) count += m_strings.Count;
                if (m_vectors != null) count += m_vectors.Count;
                if (m_objects != null) count += m_objects.Count;
                if (m_lists != null) count += m_lists.Count;

                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[DataSet: Count={0}]", Count);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public bool GetBool(KEY key)
        {
            bool value = false;
            if (m_bools == null || !m_bools.TryGetValue(key, out value))
                    throw new Exception("Bool named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetBool(KEY key, out bool value)
        {
            value = false;
            if (m_bools == null)
                return false;
            else
                return m_bools.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetBool(KEY key, bool value)
        {
            if (m_bools == null)
                m_bools = new Dictionary<KEY, bool>();

            m_bools[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveBool(KEY key)
        {
            if (m_bools == null)
                return false;

            return m_bools.Remove(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public int GetInt(KEY key)
        {
            int value = 0;
            if (m_ints == null || !m_ints.TryGetValue(key, out value))
                    throw new Exception("Int named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetInt(KEY key, out int value)
        {
            value = 0;
            if (m_ints == null)
                return false;
            else
                return m_ints.TryGetValue(key, out value);
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveInt(KEY key)
        {
            if (m_ints == null)
                return false;

            return m_ints.Remove(key);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetInt(KEY key, int value)
        {
            if (m_ints == null)
                m_ints = new Dictionary<KEY, int>();

            m_ints[key] = value;
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public float GetFloat(KEY key)
        {
            float value = 0;
            if (m_floats == null || !m_floats.TryGetValue(key, out value))
                    throw new Exception("Float named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetFloat(KEY key, out float value)
        {
            value = 0;
            if (m_floats == null)
                return false;
            else
                return m_floats.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetFloat(KEY key, float value)
        {
            if (m_floats == null)
                m_floats = new Dictionary<KEY, float>();

            m_floats[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveFloat(KEY key)
        {
            if (m_floats == null)
                return false;

            return m_floats.Remove(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public string GetString(KEY key)
        {
            string value = "";
            if (m_strings == null || !m_strings.TryGetValue(key, out value))
                    throw new Exception("String named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetString(KEY key, out string value)
        {
            value = "";
            if (m_strings == null)
                return false;
            else
                return m_strings.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetString(KEY key, string value)
        {
            if (m_strings == null)
                m_strings = new Dictionary<KEY, string>();

            m_strings[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveString(KEY key)
        {
            if (m_strings == null)
                return false;

            return m_strings.Remove(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public Vector3f GetVector3f(KEY key)
        {
            Vector3f value = Vector3f.Zero;
            if (m_vectors == null || !m_vectors.TryGetValue(key, out value))
                    throw new Exception("Vector3f named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetVector3f(KEY key, out Vector3f value)
        {
            value = Vector3f.Zero;
            if (m_vectors == null)
                return false;
            else
                return m_vectors.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetVector3f(KEY key, Vector3f value)
        {
            if (m_vectors == null)
                m_vectors = new Dictionary<KEY, Vector3f>();

            m_vectors[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveVector3f(KEY key)
        {
            if (m_vectors == null)
                return false;

            return m_vectors.Remove(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public object GetObject(KEY key)
        {
            object value = null;
            if (m_objects == null || !m_objects.TryGetValue(key, out value))
                    throw new Exception("Object named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetObject(KEY key, out object value)
        {
            value = null;
            if (m_objects == null)
                return false;
            else
                return m_objects.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public T GetObject<T>(KEY key) where T : class
        {
            object obj = null;
            if (m_objects == null || !m_objects.TryGetValue(key, out obj))
                    throw new Exception("Object named " + key + " does not exist.");
            else if (obj == null)
                return null;
            else
            {
                var value = obj as T;
                if (value == null)
                    throw new InvalidCastException("Object is not the correct type");

                return value;
            }
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetObject<T>(KEY key, out T value) where T : class
        {
            value = null;
            object obj = null;
            if (m_objects == null || !m_objects.TryGetValue(key, out obj))
                return false;
            else if (obj == null)
                return true;
            else
            {
                value = obj as T;
                return value != null;
            }
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetObject<T>(KEY key, T value) where T : class
        {
            if (m_objects == null)
                m_objects = new Dictionary<KEY, object>();

            m_objects[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveObject(KEY key)
        {
            if (m_objects == null)
                return false;

            return m_objects.Remove(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public List<object> GetList(KEY key)
        {
            List<object> value = null;
            if (m_lists == null || !m_lists.TryGetValue(key, out value))
                throw new Exception("List named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetList(KEY key, out List<object> value)
        {
            value = null;
            if (m_lists == null)
                return false;
            else
                return m_lists.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetList(KEY key, List<object> value)
        {
            if (m_lists == null)
                m_lists = new Dictionary<KEY, List<object>>();

            m_lists[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveList(KEY key)
        {
            if (m_lists == null)
                return false;

            return m_lists.Remove(key);
        }

        /// <summary>
        /// Is this set a subset of the other set.
        /// ie the other set contains the same data as this set and maybe more.
        /// </summary>
        /// <param name="set">The other set.</param>
        /// <returns></returns>
        public bool IsSubsetOf(DataSet<KEY> set)
        {

            if (Count > set.Count)
                return false;

            if (m_bools != null)
            {
                if (set.m_bools == null)
                    return false;

                foreach (var kvp in m_bools)
                {
                    if (set.TryGetBool(kvp.Key, out bool value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if (m_ints != null)
            {
                if (set.m_ints == null)
                    return false;

                foreach (var kvp in m_ints)
                {
                    if (set.TryGetInt(kvp.Key, out int value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if (m_floats != null)
            {
                if (set.m_floats == null)
                    return false;

                foreach (var kvp in m_floats)
                {
                    if (set.TryGetFloat(kvp.Key, out float value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if (m_strings != null)
            {
                if (set.m_strings == null)
                    return false;

                foreach (var kvp in m_strings)
                {
                    if (set.TryGetString(kvp.Key, out string value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if (m_vectors != null)
            {
                if (set.m_vectors == null)
                    return false;

                foreach (var kvp in m_vectors)
                {
                    if (set.TryGetVector3f(kvp.Key, out Vector3f value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if (m_objects != null)
            {
                if (set.m_objects == null)
                    return false;

                foreach (var kvp in m_objects)
                {
                    if (set.TryGetObject(kvp.Key, out object value))
                        return value == kvp.Value;
                    else
                        return false;
                }
            }

            if(m_lists != null)
            {
                if (set.m_lists == null)
                    return false;

                foreach (var kvp in m_lists)
                {
                    List<object> list1 = kvp.Value;
                    List<object> list2;

                    if (!set.TryGetList(kvp.Key, out list2))
                        return false;

                    int count1 = list1 != null ? list1.Count : 0;
                    int count2 = list2 != null ? list2.Count : 0;
                    if (count1 > count2) return false;

                    for(int i = 0; i < count1; i++)
                    {
                        if (list1[i] != list2[i])
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Print the sets data into the string builder.
        /// </summary>
        /// <param name="builder"></param>
        public void Print(StringBuilder builder)
        {
            if (m_bools != null)
            {
                foreach (var kvp in m_bools)
                    builder.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            if (m_ints != null)
            {
                foreach (var kvp in m_ints)
                    builder.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            if (m_floats != null)
            {
                foreach (var kvp in m_floats)
                    builder.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            if (m_strings != null)
            {
                foreach (var kvp in m_strings)
                    builder.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            if (m_vectors != null)
            {
                foreach (var kvp in m_vectors)
                    builder.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            if (m_objects != null)
            {
                foreach (var kvp in m_objects)
                {
                    if (kvp.Value == null)
                        builder.AppendLine(kvp.Key + ": Null");
                    else
                        builder.AppendLine(kvp.Key + ": " + kvp.Value);
                }
            }

            if (m_lists != null)
            {
                foreach (var kvp in m_lists)
                {
                    if (kvp.Value == null)
                        builder.AppendLine(kvp.Key + ": Null");
                    else
                        builder.AppendLine(kvp.Key + ": " + kvp.Value.Count);
                }
            }
        }
    }
}




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

        public Dictionary<KEY, bool> Bools { get; private set; }

        public Dictionary<KEY, int> Ints { get; private set; }

        public Dictionary<KEY, float> Floats { get; private set; }

        public Dictionary<KEY, string> Strings { get; private set; }

        public Dictionary<KEY, Vector3f> Vectors { get; private set; }

        public Dictionary<KEY, object> Objects { get; private set; }

        public Dictionary<KEY, List<object>> Lists { get; private set; }

        /// <summary>
        /// The number of data elements in the set.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                if (Bools != null) count += Bools.Count;
                if (Ints != null) count += Ints.Count;
                if (Floats != null) count += Floats.Count;
                if (Strings != null) count += Strings.Count;
                if (Vectors != null) count += Vectors.Count;
                if (Objects != null) count += Objects.Count;
                if (Lists != null) count += Lists.Count;

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
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasBool(KEY key)
        {
            if (Bools == null)
                return false;
            else
                return Bools.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public bool GetBool(KEY key)
        {
            bool value = false;
            if (Bools == null || !Bools.TryGetValue(key, out value))
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
            if (Bools == null)
                return false;
            else
                return Bools.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetBool(KEY key, bool value)
        {
            if (Bools == null)
                Bools = new Dictionary<KEY, bool>();

            Bools[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveBool(KEY key)
        {
            if (Bools == null)
                return false;

            return Bools.Remove(key);
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasInt(KEY key)
        {
            if (Ints == null)
                return false;
            else
                return Ints.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public int GetInt(KEY key)
        {
            int value = 0;
            if (Ints == null || !Ints.TryGetValue(key, out value))
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
            if (Ints == null)
                return false;
            else
                return Ints.TryGetValue(key, out value);
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveInt(KEY key)
        {
            if (Ints == null)
                return false;

            return Ints.Remove(key);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetInt(KEY key, int value)
        {
            if (Ints == null)
                Ints = new Dictionary<KEY, int>();

            Ints[key] = value;
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasFloat(KEY key)
        {
            if (Floats == null)
                return false;
            else
                return Floats.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public float GetFloat(KEY key)
        {
            float value = 0;
            if (Floats == null || !Floats.TryGetValue(key, out value))
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
            if (Floats == null)
                return false;
            else
                return Floats.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetFloat(KEY key, float value)
        {
            if (Floats == null)
                Floats = new Dictionary<KEY, float>();

            Floats[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveFloat(KEY key)
        {
            if (Floats == null)
                return false;

            return Floats.Remove(key);
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasString(KEY key)
        {
            if (Strings == null)
                return false;
            else
                return Strings.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public string GetString(KEY key)
        {
            string value = "";
            if (Strings == null || !Strings.TryGetValue(key, out value))
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
            if (Strings == null)
                return false;
            else
                return Strings.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetString(KEY key, string value)
        {
            if (Strings == null)
                Strings = new Dictionary<KEY, string>();

            Strings[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveString(KEY key)
        {
            if (Strings == null)
                return false;

            return Strings.Remove(key);
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasVector(KEY key)
        {
            if (Vectors == null)
                return false;
            else
                return Vectors.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public Vector3f GetVector(KEY key)
        {
            Vector3f value = Vector3f.Zero;
            if (Vectors == null || !Vectors.TryGetValue(key, out value))
                    throw new Exception("Vector named " + key + " does not exist.");
            else
                return value;
        }

        /// <summary>
        /// Try and get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        /// <returns>True if the set contains the key value pair.</returns>
        public bool TryGetVector(KEY key, out Vector3f value)
        {
            value = Vector3f.Zero;
            if (Vectors == null)
                return false;
            else
                return Vectors.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetVector(KEY key, Vector3f value)
        {
            if (Vectors == null)
                Vectors = new Dictionary<KEY, Vector3f>();

            Vectors[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveVector(KEY key)
        {
            if (Vectors == null)
                return false;

            return Vectors.Remove(key);
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasObject(KEY key)
        {
            if (Objects == null)
                return false;
            else
                return Objects.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public object GetObject(KEY key)
        {
            object value = null;
            if (Objects == null || !Objects.TryGetValue(key, out value))
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
            if (Objects == null)
                return false;
            else
                return Objects.TryGetValue(key, out value);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public T GetObject<T>(KEY key) where T : class
        {
            object obj = null;
            if (Objects == null || !Objects.TryGetValue(key, out obj))
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
            if (Objects == null || !Objects.TryGetValue(key, out obj))
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
        public void SetObject(KEY key, object value)
        {
            if (Objects == null)
                Objects = new Dictionary<KEY, object>();

            Objects[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveObject(KEY key)
        {
            if (Objects == null)
                return false;

            return Objects.Remove(key);
        }

        /// <summary>
        /// Is there a value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>True if there is a value for this key</returns>
        public bool HasList(KEY key)
        {
            if (Lists == null)
                return false;
            else
                return Lists.ContainsKey(key);
        }

        /// <summary>
        /// Get the value associated with a key.
        /// </summary>
        /// <param name="key">The values key.</param>
        /// <returns>The value.</returns>
        public List<object> GetList(KEY key)
        {
            List<object> value = null;
            if (Lists == null || !Lists.TryGetValue(key, out value))
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
            if (Lists == null)
                return false;
            else
                return Lists.TryGetValue(key, out value);
        }

        /// <summary>
        /// Set the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <param name="value">The value.</param>
        public void SetList(KEY key, List<object> value)
        {
            if (Lists == null)
                Lists = new Dictionary<KEY, List<object>>();

            Lists[key] = value;
        }

        /// <summary>
        /// Remove the value associated with a key.
        /// <param name="key">The values key.</param>
        /// <returns>True if the value was removed.</returns>
        public bool RemoveList(KEY key)
        {
            if (Lists == null)
                return false;

            return Lists.Remove(key);
        }

    }
}




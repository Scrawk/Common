using System;
using System.Collections.Generic;

namespace Common.Collections.Caching
{
    public class ObjectCache<T> where T : class, ICacheable<T>, new()
    {

        private List<T> m_cache;

        public ObjectCache()
        {
            m_cache = new List<T>();
        }

        public int Count => m_cache.Count;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ObjectCache: Count={0}]", Count);
        }

        public void Clear()
        {
            m_cache.Clear();
        }

        public void Cache(T obj)
        {
            if (obj == null) return;
            obj.Clear();

            for(int i = 0; i < m_cache.Count; i++)
            {
                if(m_cache[i] == null)
                {
                    m_cache[i] = obj;
                    return;
                }
            }

            m_cache.Add(obj);
        }

        public T GetObject()
        {
            for (int i = 0; i < m_cache.Count; i++)
            {
                if (m_cache[i] != null)
                {
                    var obj = m_cache[i];
                    m_cache[i] = null;
                    return obj;
                }
            }

            return new T();
        }


    }
}

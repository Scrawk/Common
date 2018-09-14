using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Knowledge
{

    public interface ICategory
    {
        string Name { get; }

        bool Contains(string name);
    }

    public class Category : ICategory
    {

        private HashSet<string> m_set;

        public Category(string name)
        {
            Name = name;
            m_set = new HashSet<string>();
        }

        public string Name { get; private set; }

        public void Add(string name)
        {
            m_set.Add(name);
        }

        public bool Contains(string name)
        {
            return m_set.Contains(name);
        }

    }
}

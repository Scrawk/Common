using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Knowledge
{

    public class Category
    {

        private HashSet<string> m_set;

        public Category(string name)
        {
            Name = name;
            m_set = new HashSet<string>();
            m_set.Add(name);
        }

        public string Name { get; private set; }

        public IEnumerable<string> Set { get { return m_set; } }

        public void Add(string name)
        {
            m_set.Add(name);
        }

        public void Add(Category cat)
        {
            foreach (var name in cat.Set)
                m_set.Add(name);
        }

        public bool IsA(string name)
        {
            return m_set.Contains(name);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Knowledge
{
    public class SubCategory : ICategory
    {

        private List<ICategory> m_subsets;

        public SubCategory(string name)
        {
            Name = name;
            m_subsets = new List<ICategory>();
        }

        public string Name { get; private set; }

        public void Add(ICategory catergory)
        {
            m_subsets.Add(catergory);
        }

        public bool Contains(string name)
        {
            for (int i = 0; i < m_subsets.Count; i++)
            {
                if (m_subsets[i].Contains(name))
                    return true;
            }

            return false;
        }

    }
}

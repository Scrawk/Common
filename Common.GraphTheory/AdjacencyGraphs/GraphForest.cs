using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphForest
    {

        private Dictionary<int, GraphTree> m_forest;

        public GraphForest()
        {
            m_forest = new Dictionary<int, GraphTree>();
        }

        public GraphForest(int size)
        {
            m_forest = new Dictionary<int, GraphTree>(size);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => m_forest.Count;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<int> Roots => m_forest.Keys;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<GraphTree> Trees => m_forest.Values;


        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[GraphForest: Count={0}]", Count);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ContainsTree(int root)
        {
            return m_forest.ContainsKey(root);
        }

        /// <summary>
        /// 
        /// </summary>
        public GraphTree GetTree(int root)
        {
            return m_forest[root];
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddTree(GraphTree tree)
        {
            m_forest.Add(tree.Root, tree);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveTree(int root)
        {
            m_forest.Remove(root);
        }

    }
}

using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A list of graph trees.
    /// </summary>
    public class GraphForest
    {

        private Dictionary<int, GraphTree> m_forest;

        public GraphForest(AdjacencyGraph graph)
        {
            Graph = graph;
            m_forest = new Dictionary<int, GraphTree>();
        }

        public GraphForest(AdjacencyGraph graph, int size)
        {
            Graph = graph;
            m_forest = new Dictionary<int, GraphTree>(size);
        }

        /// <summary>
        /// The number of trees in the forest.
        /// </summary>
        public int Count => m_forest.Count;

        /// <summary>
        /// The graph the trees were constructed from.
        /// </summary>
        public AdjacencyGraph Graph { get; private set; }

        /// <summary>
        /// The vertex indices of the tree roots.
        /// </summary>
        public IEnumerable<int> Roots => m_forest.Keys;

        /// <summary>
        /// The trees in the forest.
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
        /// Does the forest contain this tree.
        /// </summary>
        public bool ContainsTree(int root)
        {
            return m_forest.ContainsKey(root);
        }

        /// <summary>
        /// Get the tree with this root.
        /// </summary>
        public GraphTree GetTree(int root)
        {
            return m_forest[root];
        }

        /// <summary>
        /// Add a tree to the forest.
        /// </summary>
        public void AddTree(GraphTree tree)
        {
            m_forest.Add(tree.Root, tree);
        }

        /// <summary>
        /// Remove a tree from the forest.
        /// </summary>
        public bool RemoveTree(int root)
        {
            return m_forest.Remove(root);
        }

    }
}

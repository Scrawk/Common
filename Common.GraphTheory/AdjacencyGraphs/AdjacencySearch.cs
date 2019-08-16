using System;
using System.Collections.Generic;

namespace Common.GraphTheory.AdjacencyGraphs
{
    /// <summary>
    /// A data structure to store the results 
    /// from a search algorithm on a adjaceny graph.
    /// </summary>
    public class AdjacencySearch
    {

        public AdjacencySearch(int size)
        {
            Order = new List<int>(size);
            Parent = new int[size];
            IsVisited = new bool[size];

            for (int i = 0; i < size; i++)
                Parent[i] = -1;
        }

        public int Count { get { return Order.Count; } }

        public int Root { get { return Order[0]; } }

        public IList<int> Order { get; private set; }

        public IList<int> Parent { get; private set; }

        public IList<bool> IsVisited { get; private set; }

        public override string ToString()
        {
            return string.Format("[AdjacencySearch: Count={0}, Root={1}]", 
                Count, (Count > 0) ? Root : -1);
        }

        public void Clear()
        {
            Order.Clear();

            for (int i = 0; i < Parent.Count; i++)
            {
                Parent[i] = -1;
                IsVisited[i] = false;
            }
        }

        public void GetPath(int dest, List<int> path)
        {
            while (dest != Parent[dest] && Parent[dest] != -1)
            {
                path.Add(dest);
                dest = Parent[dest];
            }
        }

    }
}

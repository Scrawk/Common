using System;
using System.Collections.Generic;

namespace Common.GraphTheory.Adjacency
{
    public class AdjacencySearch
    {

        public int Count {  get { return Order.Count; } }

        public int Root { get { return Order[0]; } }

        public IList<int> Order { get; private set; }

        public IList<int> Parent { get; private set; }

        public IList<bool> IsVisited { get; private set; }

        public AdjacencySearch(int size)
        {
            Order = new List<int>(size);
            Parent = new int[size];
            IsVisited = new bool[size];

            for (int i = 0; i < size; i++)
                Parent[i] = -1;
        }

        public void Clear()
        {
            Order.Clear();

            for (int i = 0; i < Count; i++)
            {
                Parent[i] = -1;
                IsVisited[i] = false;
            }
        }

        public void GetPath(int dest, List<int> path)
        {
            path.Clear();
            while (dest != Parent[dest] && Parent[dest] != -1)
            {
                path.Add(dest);
                dest = Parent[dest];
            }

        }

    }
}

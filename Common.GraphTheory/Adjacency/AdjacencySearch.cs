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

        public AdjacencySearch(int size)
        {
            Order = new List<int>(size);
            Parent = new int[size];

            for (int i = 0; i < size; i++)
                Parent[i] = -1;
        }

        public void Clear()
        {
            Order.Clear();

            for (int i = 0; i < Parent.Count; i++)
                Parent[i] = -1;
        }

        public AdjacencySearch(IList<int> order, IList<int> parent)
        {
            Order = order;
            Parent = parent;
        }

        public List<int> GetPathEdges(int i)
        {
            List<int> path = new List<int>();

            while (i != Parent[i] && Parent[i] != -1)
            {
                path.Add(i);
                i = Parent[i];
            }

            return path;
        }

    }
}

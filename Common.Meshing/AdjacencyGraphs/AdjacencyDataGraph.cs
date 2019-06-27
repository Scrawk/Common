using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency graph where the vertices data can be any object.
    /// </summary>
    /// <typeparam name="T">The type of object the data represents</typeparam>
    public class AdjacencyDataGraph<T> : AdjacencyGraph<AdjacencyDataVertex<T>, AdjacencyEdge>
    {
        public AdjacencyDataGraph(int size) : base(size)
        {
        }

        public AdjacencyDataGraph(IEnumerable<AdjacencyDataVertex<T>> vertices) : base(vertices)
        {
        }

        public AdjacencyDataGraph(IEnumerable<T> vertices)
        {
            Vertices = new List<AdjacencyDataVertex<T>>();
            Edges = new List<List<AdjacencyEdge>>(Vertices.Count);

            foreach (var data in vertices)
            {
                var v = new AdjacencyDataVertex<T>();
                v.Index = Vertices.Count;
                v.Data = data;
                Vertices.Add(v);
                Edges.Add(null);
            }

            
        }

        /// <summary>
        /// Find the vertex index belonging to this data.
        /// </summary>
        public int IndexOf(T data)
        {
            foreach (var v in Vertices)
            {
                if (ReferenceEquals(data, v.Data))
                    return v.Index;
            }

            return -1;
        }

    }
}

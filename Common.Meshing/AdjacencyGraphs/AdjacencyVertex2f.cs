using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex that represents a 2D position.
    /// </summary>
    public class AdjacencyVertex2f : AdjacencyVertex
    {

        public AdjacencyVertex2f()
        {
            Index = -1;
        }

        public AdjacencyVertex2f(int index, Vector2f position)
        {
            Index = index;
            Position = position;
        }

        public Vector2f Position { get; set; }

        public override void Initialize(Vector2f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos.xy;
        }

        public override void Initialize(Vector2d pos)
        {
            Position = (Vector2f)pos;
        }

        public override void Initialize(Vector3d pos)
        {
            Position = (Vector2f)pos.xy;
        }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex2f: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }
    }
}

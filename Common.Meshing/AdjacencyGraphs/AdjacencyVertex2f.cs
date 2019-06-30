using System;
using System.Collections.Generic;

using Common.Core.Numerics;

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

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex2f: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector2f)pos.xy;
        }

        public override Vector3d GetPosition()
        {
            return Position.xy0;
        }
    }
}

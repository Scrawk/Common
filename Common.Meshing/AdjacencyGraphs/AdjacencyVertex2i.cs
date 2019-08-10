using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex that represents a 2D position.
    /// </summary>
    public class AdjacencyVertex2i : AdjacencyVertex
    {

        public AdjacencyVertex2i()
        {
            Index = -1;
        }

        public AdjacencyVertex2i(int index, Vector2i position)
        {
            Index = index;
            Position = position;
        }

        public Vector2i Position { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex2i: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector2i)pos.xy;
        }

        public override Vector3d GetPosition()
        {
            return Position.xy0;
        }
    }
}

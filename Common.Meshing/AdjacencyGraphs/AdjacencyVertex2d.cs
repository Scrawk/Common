using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex that represents a 2D position.
    /// </summary>
    public class AdjacencyVertex2d : AdjacencyVertex
    {

        public AdjacencyVertex2d()
        {
            Index = -1;
        }

        public AdjacencyVertex2d(int index, Vector2d position)
        {
            Index = index;
            Position = position;
        }

        public Vector2d Position { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex2d: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = pos.xy;
        }

        public override Vector3d GetPosition()
        {
            return Position.xy0;
        }
    }
}

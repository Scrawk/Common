using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex that represents a 3D position.
    /// </summary>
    public class AdjacencyVertex3d : AdjacencyVertex
    {

        public AdjacencyVertex3d()
        {
            Index = -1;
        }

        public AdjacencyVertex3d(int index, Vector3d position)
        {
            Index = index;
            Position = position;
        }

        public Vector3d Position { get; set; }

        public override void Initialize(Vector2f pos)
        {
            Position = pos.xy0;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector2d pos)
        {
            Position = pos.xy0;
        }

        public override void Initialize(Vector3d pos)
        {
            Position = pos;
        }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex3d: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }
    }
}

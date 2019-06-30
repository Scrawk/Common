using System;
using System.Collections.Generic;

using Common.Core.Numerics;

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

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex3d: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = pos;
        }

        public override Vector3d GetPosition()
        {
            return Position;
        }
    }
}

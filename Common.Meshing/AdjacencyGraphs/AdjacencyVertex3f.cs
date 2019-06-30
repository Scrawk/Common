using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.AdjacencyGraphs
{
    /// <summary>
    /// A adjacency vertex that represents a 3D position.
    /// </summary>
    public class AdjacencyVertex3f : AdjacencyVertex
    {

        public AdjacencyVertex3f()
        {
            Index = -1;
        }

        public AdjacencyVertex3f(int index, Vector3f position)
        {
            Index = index;
            Position = position;
        }

        public Vector3f Position { get; set; }

        public override string ToString()
        {
            return string.Format("[AdjacencyVertex3f: Index={0}, Cost={1}, Position={2}]", Index, Cost, Position);
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector3f)pos;
        }

        public override Vector3d GetPosition()
        {
            return Position;
        }

    }
}

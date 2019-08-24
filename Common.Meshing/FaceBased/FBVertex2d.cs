using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public sealed class FBVertex2d : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 2;

        public Vector2d Position { get; set; }

        public FBVertex2d()
        {

        }

        public FBVertex2d(Vector2d position)
        {
            Position = position;
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex2d).Position;
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

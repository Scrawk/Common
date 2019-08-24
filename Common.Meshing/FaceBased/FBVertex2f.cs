using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public sealed class FBVertex2f : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 2;

        public Vector2f Position { get; set; }

        public FBVertex2f()
        {

        }

        public FBVertex2f(Vector2f position)
        {
            Position = position;
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex2f).Position;
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

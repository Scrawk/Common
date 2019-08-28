using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public sealed class FBVertex3f : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 3;

        public Vector3f Position { get; set; }

        public FBVertex3f()
        {

        }

        public FBVertex3f(Vector3f position)
        {
            Position = position;
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex3f).Position;
        }

        public override void SetPosition(Vector3f pos)
        {
            Position = pos;
        }

        public override Vector3f GetPosition()
        {
            return Position;
        }

    }
}

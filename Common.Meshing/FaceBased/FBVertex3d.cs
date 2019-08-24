using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public sealed class FBVertex3d : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 3;

        public Vector3d Position { get; set; }

        public FBVertex3d()
        {

        }

        public FBVertex3d(Vector3d position)
        {
            Position = position;
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex3d).Position;
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

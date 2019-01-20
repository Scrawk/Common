using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex3f : FBVertex
    {
        public Vector3f Position { get; set; }

        public FBVertex3f()
        {

        }

        public FBVertex3f(Vector3f position)
        {
            Position = position;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos;
        }

    }
}

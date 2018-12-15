using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex2f : FBVertex
    {
        public Vector2f Position { get; set; }

        public FBVertex2f()
        {

        }

        public override void Initialize(Vector2f pos)
        {
            Position = pos;
        }

    }
}

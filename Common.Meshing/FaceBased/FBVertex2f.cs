﻿using System;
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

        public FBVertex2f(Vector2f position)
        {
            Position = position;
        }

        public override void Initialize(Vector2f pos)
        {
            Position = pos;
        }

    }
}

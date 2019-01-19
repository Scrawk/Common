using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex with 2D position.
    /// Presumes edges are connected in CCW order.
    /// </summary>
    public class HBVertex2f : HBVertex
    {
        public Vector2f Position;

        public HBVertex2f()
        {

        }

        public HBVertex2f(Vector2f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector2f pos)
        {
            Position = pos;
        }

        public override void Transform(Matrix2x2f m)
        {
            Position = m * Position;
        }

    }
}

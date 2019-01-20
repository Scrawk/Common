using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex with 3D position.
    /// Presumes edges are connected in CCW order.
    /// </summary>
    public class HBVertex3f : HBVertex
    {
        public Vector3f Position;

        public HBVertex3f()
        {

        }

        public HBVertex3f(Vector3f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos;
        }

    }
}

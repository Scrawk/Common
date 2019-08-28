﻿using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex with 3D position.
    /// Presumes edges are connected in CCW order.
    /// </summary>
    public sealed class HBVertex3f : HBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 3;

        public Vector3f Position;

        public HBVertex3f()
        {

        }

        public HBVertex3f(Vector3f pos)
        {
            Position = pos;
        }

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public override string ToString<VERTEX>(HBMesh<VERTEX> mesh)
        {
            return string.Format("[HBVertex: Id={0}, Edge={1}]", mesh.IndexOf(this), mesh.IndexOf(Edge));
        }

        public override void SetPosition(HBVertex vertex)
        {
            Position = (vertex as HBVertex3f).Position;
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

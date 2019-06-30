using System;
using System.Xml;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge vertex with 3D position.
    /// Presumes edges are connected in CCW order.
    /// </summary>
    public class HBVertex3f : HBVertex
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
        public override string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
        {
            return string.Format("[HBVertex3f: Id={0}, Edge={1}, Position={2}]",
                mesh.IndexOf(this), mesh.IndexOf(Edge), Position);
        }

        public override void SetPosition(HBVertex vertex)
        {
            Position = (vertex as HBVertex3f).Position;
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector3f)pos;
        }

        public override Vector3d GetPosition()
        {
            return Position;
        }

    }
}

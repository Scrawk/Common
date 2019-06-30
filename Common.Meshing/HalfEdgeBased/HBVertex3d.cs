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
    public class HBVertex3d : HBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 3;

        public Vector3d Position;

        public HBVertex3d()
        {

        }

        public HBVertex3d(Vector3d pos)
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
            return string.Format("[HBVertex3d: Id={0}, Edge={1}, Position={2}]",
                mesh.IndexOf(this), mesh.IndexOf(Edge), Position);
        }

        public override void SetPosition(HBVertex vertex)
        {
            Position = (vertex as HBVertex3d).Position;
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

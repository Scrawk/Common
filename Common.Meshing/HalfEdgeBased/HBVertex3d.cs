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
    public class HBVertex3d : HBVertex
    {
        public Vector3d Position;

        public HBVertex3d()
        {

        }

        public override void Initialize(HBVertex vertex)
        {
            var v3 = vertex as HBVertex3d;
            if (v3 == null) return;
            Position = v3.Position;
        }

        public HBVertex3d(Vector3d pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector3d pos)
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

        /// <summary>
        /// Create a vertex that is a interpolation 
        /// from this to the other vertex.
        /// </summary>
        /// <param name="to">other vertex</param>
        /// <returns>interpolation< between the two/returns>
        public override HBVertex Interpolate(HBVertex to, double t)
        {
            var pos = Vector3d.Lerp(Position, (to as HBVertex3d).Position, t);
            return new HBVertex3d(pos);
        }

    }
}

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

        public override void Initialize(HBVertex vertex)
        {
            Position = (vertex as HBVertex3f).Position;
        }

        public override void Initialize(Vector2f pos)
        {
            Position = pos.xy0;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector2d pos)
        {
            Position = (Vector3f)pos.xy0;
        }

        public override void Initialize(Vector3d pos)
        {
            Position = (Vector3f)pos;
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

        /// <summary>
        /// Create a vertex that is a interpolation 
        /// from this to the other vertex.
        /// </summary>
        /// <param name="to">other vertex</param>
        /// <returns>interpolation< between the two/returns>
        public override HBVertex Interpolate(HBVertex to, double t)
        {
            var pos = Vector3f.Lerp(Position, (to as HBVertex3f).Position, (float)t);
            return new HBVertex3f(pos);
        }

    }
}

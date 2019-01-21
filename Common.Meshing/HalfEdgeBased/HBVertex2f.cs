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

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public override string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
        {
            return string.Format("[HBVertex2f: Id={0}, Edge={1}, Position={2}]", 
                mesh.IndexOf(this), mesh.IndexOf(Edge), Position);
        }

        /// <summary>
        /// Create a vertex that is a interpolation 
        /// from this to the other vertex.
        /// </summary>
        /// <param name="to">other vertex</param>
        /// <returns>interpolation< between the two/returns>
        public override HBVertex Interpolate(HBVertex to, float t)
        {
            var pos = Vector2f.Lerp(Position, (to as HBVertex2f).Position, t);
            return new HBVertex2f(pos);
        }

    }
}

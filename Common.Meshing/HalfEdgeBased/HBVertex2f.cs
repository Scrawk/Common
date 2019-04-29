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
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 2;

        public Vector2f Position;

        public HBVertex2f()
        {

        }

        public HBVertex2f(Vector2f pos)
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

        public override void SetPosition(HBVertex vertex)
        {
            Position = (vertex as HBVertex2f).Position;
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector2f)pos.xy;
        }

        public override Vector3d GetPosition()
        {
            return Position.xy0;
        }

        /// <summary>
        /// Create a vertex that is a interpolation 
        /// from this to the other vertex.
        /// </summary>
        /// <param name="to">other vertex</param>
        /// <returns>interpolation< between the two/returns>
        public override HBVertex Interpolate(HBVertex to, double t)
        {
            var pos = Vector2f.Lerp(Position, (to as HBVertex2f).Position, (float)t);
            return new HBVertex2f(pos);
        }

    }
}

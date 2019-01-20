using System;
using System.Collections.Generic;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge based face. Presumes face is CCW.
    /// </summary>
    public class HBFace
    {
        /// <summary>
        /// The edge this face connects to. 
        /// </summary>
        public HBEdge Edge { get; set; }

        public HBFace()
        {

        }

        /// <summary>
        /// Convert face to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Face as string</returns>
        public string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            return string.Format("[HBFace: Edge={0}]", mesh.IndexOf(Edge));
        }

        /// <summary>
        /// The number of edges in face.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                if (Edge == null) return 0;
                return Edge.EdgeCount;
            }
        }

        public EDGE GetEdge<EDGE>() where EDGE : HBEdge
        {
            if (Edge == null) return null;

            EDGE edge = Edge as EDGE;
            if (edge == null)
                throw new InvalidCastException("Edge is not a " + typeof(EDGE));

            return edge;
        }

        /// <summary>
        /// Clear face of all connections.
        /// </summary>
        public virtual void Clear()
        {
            Edge = null;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge based face. Presumes face is CCW.
    /// </summary>
    public class HBFace
    {
        public int Tag;

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
        public virtual string ToString<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            return string.Format("[HBFace: Id={0}, Edge={1}]", mesh.IndexOf(this), mesh.IndexOf(Edge));
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

        /// <summary>
        /// Clear face of all connections.
        /// </summary>
        public virtual void Clear()
        {
            Edge = null;
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
        /// Check the face is valid.
        /// </summary>
        /// <returns>A list of errors</returns>
        public virtual string Check<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            var builder = new StringBuilder();

            if (Edge == null)
                builder.AppendLine("Edge is null.");
            else
            {
                if (Edge.Face != this)
                    builder.Append("Edge is not part of this face.");

                if (mesh.IndexOf(Edge) == -1)
                    builder.Append("Edge is not found in mesh.");
            }

            return builder.ToString();
        }

    }
}

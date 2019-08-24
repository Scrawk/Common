using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge based face. Presumes face is CCW.
    /// </summary>
    public sealed class HBFace
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
        public string ToString<VERTEX>(HBMesh<VERTEX> mesh)
            where VERTEX : HBVertex, new()
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
        public void Clear()
        {
            Edge = null;
        }

        /// <summary>
        /// Check the face is valid.
        /// </summary>
        /// <returns>A list of errors</returns>
        public string Check()
        {
            var builder = new StringBuilder();

            if (Edge == null)
                builder.AppendLine("Edge is null.");
            else
            {
                if (Edge.Face != this)
                    builder.AppendLine("Edge is not part of this face.");
            }

            return builder.ToString();
        }

    }
}

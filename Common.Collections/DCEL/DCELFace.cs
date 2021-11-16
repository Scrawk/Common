using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Collections.DCEL
{
    /// <summary>
    /// A half edge based face. Presumes face is CCW.
    /// </summary>
    public partial class DCELFace
    {

        internal DCELFace()
        {

        }

        /// <summary>
        /// Used for temporary making the face.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// The faces index in the mesh.
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// The edge this face connects to. 
        /// </summary>
        public DCELHalfedge Edge { get; internal set; }

        /// <summary>
        /// The faces optional data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Convert face to string.
        /// </summary>
        /// <returns>Face as string</returns>
        public override string ToString()
        {
            return string.Format("[DCELFace: Id={0}, Edge={1},]",
                Tag, DCELMesh.IndexOrDefault(Edge));
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
        internal void Clear()
        {
            Data = null;
            Edge = null;
        }

    }
}

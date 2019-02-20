using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex2f : FBVertex
    {
        public Vector2f Position { get; set; }

        public FBVertex2f()
        {

        }

        public FBVertex2f(Vector2f position)
        {
            Position = position;
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
        public override string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
        {
            return string.Format("[FBVertex2f: Id={0}, Face={1}, Position={2}]", 
                mesh.IndexOf(this), mesh.IndexOf(Face), Position);
        }

    }
}

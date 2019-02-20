using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{

    public class FBVertex
    {
        public int Tag;

        public FBFace Face { get; set; }

        public FBVertex()
        {
            
        }

        public virtual void Initialize(Vector2f pos)
        {

        }

        public virtual void Initialize(Vector3f pos)
        {

        }

        public virtual void Initialize(Vector4f pos)
        {

        }

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public virtual string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            return string.Format("[FBVertex: Id={0}, Face={1}]", mesh.IndexOf(this), mesh.IndexOf(Face));
        }

        public FACE GetFace<FACE>() where FACE : FBFace
        {
            if (Face == null) return null;

            FACE face = Face as FACE;
            if (face == null)
                throw new InvalidCastException("Face is not a " + typeof(FACE));

            return face;
        }

        public virtual void Clear()
        {
            Face = null;
        }

    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex3f : FBVertex
    {
        public Vector3f Position { get; set; }

        public FBVertex3f()
        {

        }

        public FBVertex3f(Vector3f position)
        {
            Position = position;
        }

        public override void Initialize(FBVertex vert)
        {
            Position = (vert as FBVertex3f).Position;
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
        public override string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
        {
            return string.Format("[FBVertex3f: Id={0}, Faces={1}, Position={2}]",
                mesh.IndexOf(this), NumFaces, Position);
        }

    }
}

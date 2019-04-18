using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex3d : FBVertex
    {
        public Vector3d Position { get; set; }

        public FBVertex3d()
        {

        }

        public FBVertex3d(Vector3d position)
        {
            Position = position;
        }

        public override void Initialize(Vector3d pos)
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
            return string.Format("[FBVertex3d: Id={0}, Faces={1}, Position={2}]",
                mesh.IndexOf(this), NumFaces, Position);
        }

    }
}

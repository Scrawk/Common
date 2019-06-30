using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public class FBVertex3i : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 3;

        public Vector3i Position { get; set; }

        public FBVertex3i()
        {

        }

        public FBVertex3i(Vector3i position)
        {
            Position = position;
        }

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public override string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
        {
            return string.Format("[FBVertex3i: Id={0}, Faces={1}, Position={2}]",
                mesh.IndexOf(this), NumFaces, Position);
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex3i).Position;
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector3i)pos;
        }

        public override Vector3d GetPosition()
        {
            return Position;
        }

    }
}

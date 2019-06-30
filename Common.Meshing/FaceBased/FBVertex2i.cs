using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public class FBVertex2i : FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public override int Dimension => 2;

        public Vector2i Position { get; set; }

        public FBVertex2i()
        {

        }

        public FBVertex2i(Vector2i position)
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
            return string.Format("[FBVertex2i: Id={0}, Faces={1}, Position={2}]",
                mesh.IndexOf(this), NumFaces, Position);
        }

        public override void SetPosition(FBVertex vert)
        {
            Position = (vert as FBVertex2i).Position;
        }

        public override void SetPosition(Vector3d pos)
        {
            Position = (Vector2i)pos.xy;
        }

        public override Vector3d GetPosition()
        {
            return Position.xy0;
        }

    }
}

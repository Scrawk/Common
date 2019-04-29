﻿using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{
    public class FBVertex2d : FBVertex
    {
        public Vector2d Position { get; set; }

        public FBVertex2d()
        {

        }

        public FBVertex2d(Vector2d position)
        {
            Position = position;
        }

        public override void Initialize(FBVertex vert)
        {
            Position = (vert as FBVertex2f).Position;
        }

        public override void Initialize(Vector2f pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector3f pos)
        {
            Position = pos.xy;
        }

        public override void Initialize(Vector2d pos)
        {
            Position = pos;
        }

        public override void Initialize(Vector3d pos)
        {
            Position = pos.xy;
        }

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public override string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
        {
            return string.Format("[FBVertex2d: Id={0}, Faces={1}, Position={2}]",
                mesh.IndexOf(this), NumFaces, Position);
        }

    }
}
using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.FaceBased
{

    public class FBVertex
    {
        public int Tag;

        public List<FBFace> Faces { get; set; }

        public FBVertex()
        {
            
        }

        public int NumFaces
        {
            get
            {
                return (Faces != null) ? Faces.Count : 0;
            }
        }

        public virtual void Initialize(Vector2f pos)
        {

        }

        public virtual void Initialize(Vector3f pos)
        {

        }

        public virtual void Initialize(Vector2d pos)
        {

        }

        public virtual void Initialize(Vector3d pos)
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
            return string.Format("[FBVertex: Id={0}, Faces={1}]", mesh.IndexOf(this), NumFaces);
        }

        public FACE GetFace<FACE>(int i) where FACE : FBFace
        {
            if (Faces == null) return null;

            FACE face = Faces[i] as FACE;
            if (face == null)
                throw new InvalidCastException("Face is not a " + typeof(FACE));

            return face;
        }

        public void AddFace(FBFace face)
        {
            if (Faces == null)
                Faces = new List<FBFace>();

            Faces.Add(face);
        }
        public void AddFace(FBFace f0, FBFace f1)
        {
            if (Faces == null)
                Faces = new List<FBFace>(2);

            Faces.Add(f0, f1);
        }

        public void AddFace(FBFace f0, FBFace f1, FBFace f2)
        {
            if (Faces == null)
                Faces = new List<FBFace>(3);

            Faces.Add(f0, f1, f2);
        }

        public void AddFace(FBFace f0, FBFace f1, FBFace f2, FBFace f3)
        {
            if (Faces == null)
                Faces = new List<FBFace>(4);

            Faces.Add(f0, f1, f2, f3);
        }

        /// <summary>
        /// A vertex is closed if all the faces surounding it 
        /// dont have a null neighbour that would inculde this vertex.
        /// </summary>
        /// <param name="minFaces">The minimum number of neighbours a face 
        /// should have the include this vertex.</param>
        public bool IsClosed(int minFaces)
        {
            if (Faces == null) return false;

            for (int i = 0; i < Faces.Count; i++)
            {
                var face = Faces[i];
                int count = 0;
                for (int j = 0; j < face.Neighbours.Length; j++)
                {
                    var n = face.Neighbours[j];
                    if (n == null) continue;
                    if (n.IndexOf(this) != -1) count++;
                }

                if (count < minFaces) return false;
            }

            return true;
        }

        public virtual void Clear()
        {
            Faces = null;
        }

    }
}

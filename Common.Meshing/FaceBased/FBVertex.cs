using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{

    public abstract class FBVertex
    {
        /// <summary>
        /// The dimension of the vertex, ie 2D, 3D.
        /// </summary>
        public abstract int Dimension { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Tag;

        /// <summary>
        /// A list of all faces surrounding vertex.
        /// Null by default.
        /// </summary>
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

        /// <summary>
        /// Convert vertex to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Vertex as string</returns>
        public virtual string ToString<VERTEX>(FBMesh<VERTEX> mesh)
            where VERTEX : FBVertex, new()
        {
            return string.Format("[FBVertex: Id={0}, Faces={1}]", mesh.IndexOf(this), NumFaces);
        }

        public abstract void SetPosition(FBVertex vert);

        public abstract void SetPosition(Vector3d pos);

        public abstract Vector3d GetPosition();

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

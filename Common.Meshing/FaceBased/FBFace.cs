using System;
using System.Linq;

using Common.Core.Numerics;

namespace Common.Meshing.FaceBased
{
    public sealed class FBFace
    {

        public int Tag;

        /// <summary>
        /// A list of vertices that make up the face.
        /// No vertex will be null.
        /// </summary>
        public FBVertex[] Vertices { get; set; }

        /// <summary>
        /// A list of each of the faces neighbours.
        /// Boundary faces may have a null neighbour.
        /// </summary>
        public FBFace[] Neighbours { get; set; }

        public FBFace()
        {

        }

        public FBFace(int size)
        {
            Vertices = new FBVertex[size];
            Neighbours = new FBFace[size];
        }

        /// <summary>
        /// The number of vertices a face has.
        /// </summary>
        public int NumVertices
        {
            get { return (Vertices != null) ? Vertices.Length : 0; }
        }

        /// <summary>
        /// The number of neighbours a face has.
        /// Null neigbour dont count.
        /// </summary>
        public int NumNeighbours
        {
            get
            {
                if (Neighbours == null) return 0;

                int count = 0;
                foreach (var n in Neighbours)
                    if (n != null) count++;

                return count;
            }
        }

        /// <summary>
        /// Calculate the average position of the vertices.
        /// </summary>
        public Vector3f Centriod
        {
            get
            {
                if (Vertices == null)
                    return Vector3f.Zero;

                int count = 0;
                Vector3f centroid = Vector3f.Zero;
                foreach (var v in Vertices)
                {
                    centroid += v.GetPosition();
                    count++;
                }

                if (count == 0)
                    return centroid;
                else
                    return centroid / count;
            }
        }

        /// <summary>
        /// Convert face to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Face as string</returns>
        public string ToString<VERTEX>(FBMesh<VERTEX> mesh)
            where VERTEX : FBVertex, new()
        {
            return string.Format("[FBFace: Id={0}, NumVertices={1}, NumNeighbours={2}]", 
                mesh.IndexOf(this), NumVertices, NumNeighbours);
        }

        /// <summary>
        /// Sets the size of the face.
        /// A faces size is determined by its number of vertices.
        /// </summary>
        /// <param name="size"></param>
        public void SetVerticesSize(int size)
        {
            if(Vertices == null || Vertices.Length != size)
                Vertices = new FBVertex[size];

            if (Neighbours == null || Neighbours.Length != size)
                Neighbours = new FBFace[size];

            for (int i = 0; i < size; i++)
            {
                Vertices[i] = null;
                Neighbours[i] = null;
            }
        }

        /// <summary>
        /// Returns the index of vertex in faces array.
        /// </summary>
        /// <returns>The index of the vertex or -1 if not found.</returns>
        public int IndexOf(FBVertex v)
        {
            if (Vertices == null) return -1;
            for (int i = 0; i < NumVertices; i++)
            {
                if (ReferenceEquals(v, Vertices[i]))
                    return i;
            }

            return -1;
        }

        public void SetVertex(FBVertex v0, FBVertex v1, FBVertex v2)
        {
            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
        }

        public void SetVertex(FBVertex v0, FBVertex v1, FBVertex v2, FBVertex v3)
        {
            Vertices[0] = v0;
            Vertices[1] = v1;
            Vertices[2] = v2;
            Vertices[3] = v3;
        }

        public void SetNeighbour(FBFace f0, FBFace f1, FBFace f2)
        {
            Neighbours[0] = f0;
            Neighbours[1] = f1;
            Neighbours[2] = f2;
        }

        public void SetNeighbour(FBFace f0, FBFace f1, FBFace f2, FBFace f3)
        {
            Neighbours[0] = f0;
            Neighbours[1] = f1;
            Neighbours[2] = f2;
            Neighbours[3] = f3;
        }

        public void Clear()
        {
            Vertices = null;
            Neighbours = null;
        }

    }
}

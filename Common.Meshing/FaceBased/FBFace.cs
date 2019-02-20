using System;
using System.Linq;

namespace Common.Meshing.FaceBased
{
    public class FBFace
    {

        public int Tag;

        public FBVertex[] Vertices { get; set; }

        public FBFace[] Neighbours { get; set; }

        public FBFace()
        {

        }

        public FBFace(int size)
        {
            Vertices = new FBVertex[size];
            Neighbours = new FBFace[size];
        }

        public int NumVertices
        {
            get { return (Vertices != null) ? Vertices.Length : 0; }
        }

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
        /// Convert face to string.
        /// </summary>
        /// <param name="mesh">Parent mesh</param>
        /// <returns>Face as string</returns>
        public virtual string ToString<VERTEX, FACE>(FBMesh<VERTEX, FACE> mesh)
            where VERTEX : FBVertex, new()
            where FACE : FBFace, new()
        {
            return string.Format("[FBFace: Id={0}, NumVertices={1}, NumNeighbours={2}]", 
                mesh.IndexOf(this), NumVertices, NumNeighbours);
        }

        public void SetSize(int size)
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

        public int IndexOf<VERTEX>(VERTEX v)
        {
            if (Vertices == null) return -1;
            for (int i = 0; i < NumVertices; i++)
            {
                if (ReferenceEquals(v, Vertices[i]))
                    return i;
            }

            return -1;
        }

        public VERTEX GetVertex<VERTEX>(int i) where VERTEX : FBVertex
        {
            if (Vertices == null) return null;
            if (Vertices[i] == null) return null;

            VERTEX vert = Vertices[i] as VERTEX;
            if (vert == null)
                throw new InvalidCastException("Vertex is not a " + typeof(VERTEX));

            return vert;
        }

        public FACE GetNeighbour<FACE>(int i) where FACE : FBFace
        {
            if (Neighbours == null) return null;
            if (Neighbours[i] == null) return null;

            FACE face = Neighbours[i] as FACE;
            if (face == null)
                throw new InvalidCastException("Neighbor is not a " + typeof(FACE));

            return face;
        }

        public virtual void Clear()
        {
            Vertices = null;
            Neighbours = null;
        }

    }
}

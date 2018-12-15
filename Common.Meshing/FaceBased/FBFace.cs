using System;
using System.Collections.Generic;

namespace Common.Meshing.FaceBased
{
    public class FBFace
    {

        public FBVertex[] Vertices { get; set; }

        public FBFace[] Neighbors { get; set; }

        public FBFace()
        {

        }

        public FBFace(int size)
        {
            Vertices = new FBVertex[size];
            Neighbors = new FBFace[size];
        }

        public int NumVertices
        {
            get { return (Vertices != null) ? Vertices.Length : 0; }
        }

        public int NumNeighbors
        {
            get
            {
                if (Neighbors == null) return 0;

                int count = 0;
                foreach (var n in Neighbors)
                    if (n != null) count++;

                return count;
            }
        }

        public void SetSize(int size)
        {
            if(Vertices == null || Vertices.Length != size)
                Vertices = new FBVertex[size];

            if (Neighbors == null || Neighbors.Length != size)
                Neighbors = new FBFace[size];
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

        public FACE GetNeighbor<FACE>(int i) where FACE : FBFace
        {
            if (Neighbors == null) return null;
            if (Neighbors[i] == null) return null;

            FACE face = Neighbors[i] as FACE;
            if (face == null)
                throw new InvalidCastException("Neighbor is not a " + typeof(FACE));

            return face;
        }

        public virtual void Clear()
        {
            Vertices = null;
            Neighbors = null;
        }

    }
}

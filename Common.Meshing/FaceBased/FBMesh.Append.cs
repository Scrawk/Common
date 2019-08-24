using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Meshing.FaceBased
{
    public partial class FBMesh<VERTEX>
           where VERTEX : FBVertex, new()
    {

        /// <summary>
        /// Append the contents of source mesh into source mesh as a deep copy.
        /// </summary>
        /// <param name="source">mesh that get appended to</param>
        /// <param name="dest">mesh that gets appended from</param>
        /// <param name="incudeFaces">should the mesh faces also be appended</param>
        public void Append(FBMesh<VERTEX> source)
        {
            int vStart = Vertices.Count;
            int fStart = Faces.Count;

            source.TagAll();

            for (int i = 0; i < source.Vertices.Count; i++)
            {
                var v = new VERTEX();
                v.SetPosition(source.Vertices[i]);
                Vertices.Add(v);
            }

            for (int i = 0; i < source.Faces.Count; i++)
            {
                var f = new FBFace();
                Faces.Add(f);
            }

            for (int i = 0; i < source.Faces.Count; i++)
            {
                var f0 = source.Faces[i];
                var f1 = Faces[fStart + i];

                int size = f0.NumVertices;
                f1.SetVerticesSize(size);

                for (int j = 0; j < size; j++)
                {
                    f1.Vertices[j] = Vertices[vStart + f0.Vertices[j].Tag];
                }

            }
        }
    }
}
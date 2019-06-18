using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static void SubdivideTriangularMesh<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, int iterations)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            int count = mesh.Faces.Count;
            for (int i = 1; i < iterations; i++)
                count *= 3;

            var faces = new List<FACE>(count);

            for (int i = 0; i < iterations; i++)
            {
                mesh.TagEdges(0);
                faces.Clear();
                faces.AddRange(mesh.Faces);

                foreach (var face in faces)
                {
                    var v = HBOperations.PokeFace(mesh, face);

                    foreach (var edge in v.EnumerateEdges())
                    {
                        edge.Tag = 1;
                        edge.Opposite.Tag = 1;
                    }
                }

                foreach (var edge in mesh.Edges)
                {
                    if (edge.Tag == 1) continue;
                    if (edge.Face == null) continue;
                    if (edge.Opposite == null) continue;
                    if (edge.Opposite.Face == null) continue;

                    HBOperations.FlipEdge(mesh, edge);

                    edge.Tag = 1;
                    edge.Opposite.Tag = 1;
                }

            }

        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static void Subdivide<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, int iterations)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            int count = mesh.Faces.Count;
            for (int i = 1; i < iterations; i++)
                count *= 3;

            var edges = new List<EDGE>(9);
            var faces = new List<FACE>(count);

            for (int i = 0; i < iterations; i++)
            {
                mesh.TagVertices(0);
                mesh.TagEdges(0);
                faces.Clear();
                faces.AddRange(mesh.Faces);

                foreach (var face in faces)
                {
                    edges.Clear();
                    foreach (var edge in face.Edge.EnumerateEdges())
                    {
                        if (edge.Tag == 1) continue;
                        edges.Add(edge as EDGE);
                    }

                    if (edges.Count == 0) continue;

                    foreach(var edge in edges)
                    {
                        var v = SplitEdge(mesh, edge, 0.5);

                        v.Tag = 1;
                        TagEdgeAndOpposite(v.Edge, 1);
                        TagEdgeAndOpposite(v.Edge.Previous, 1);
                    }
                }

                foreach (var face in faces)
                {
                    var centroid = face.Edge.GetCentriod();
                    PokeFace(mesh, face, centroid);
                }
            }
        }

        /// <summary>
        /// Calculate the average position of the vertices.
        /// </summary>
        private static Vector3d FindCentriod(HBFace face, int tag)
        {
            int count = 0;
            Vector3d centroid = Vector3d.Zero;
            foreach (var edge in face.Edge.EnumerateEdges())
            {
                if (edge.From.Tag != tag) continue;
                centroid += edge.From.GetPosition();
                count++;
            }

            if (count == 0)
                return centroid;
            else
                return centroid / count;
        }

        /*
        public static void Subdivide<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, int iterations)
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
        */
    }
}

using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;
using Common.Core.Mathematics;

namespace Common.Meshing.HalfEdgeBased
{
    public static partial class HBOperations
    {
        public static void Subdivide<VERTEX, EDGE, FACE>(HBMesh<VERTEX, EDGE, FACE> mesh, int iterations, bool smooth)
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
        {
            if (iterations <= 0) return;

            int finalFaceCount = mesh.Faces.Count;
            for (int i = 1; i < iterations; i++)
                finalFaceCount *= 3;

            var positions = new List<Vector3d>(finalFaceCount/3);
            var edges = new List<HBEdge>(9);
            var faces = new List<FACE>(finalFaceCount);

            for (int i = 0; i < iterations; i++)
            {
                mesh.TagVertices();
                mesh.TagEdges(0);
                positions.Clear();
                faces.Clear();
                faces.AddRange(mesh.Faces);

                if (i == 0)
                {
                    foreach (var face in faces)
                    {
                        if (face.EdgeCount != 3)
                            throw new NotSupportedException("Can only subdivide triangle faces.");
                    }
                }

                foreach (var v in mesh.Vertices)
                    positions.Add(v.GetPosition());

                foreach (var face in faces)
                {
                    edges.Clear();
                    foreach (var edge in face.Edge.EnumerateEdges())
                    {
                        if (edge.Tag == 1) continue;
                        edges.Add(edge);
                    }

                    if (edges.Count == 0) continue;

                    foreach(var edge in edges)
                    {
                        var v = SplitEdge(mesh, edge as EDGE, 0.5);

                        v.Tag = -1;
                        TagEdgeAndOpposite(v.Edge, 1);
                        TagEdgeAndOpposite(v.Edge.Previous, 1);
                    }
                }

                if (smooth)
                {
                    foreach (var v in mesh.Vertices)
                    {
                        if (v.Tag == -1)
                            ComputeOddInteriorVertex(v);
                        else
                            ComputeEvenInteriorVertex(v, positions);
                    }
                }

                foreach (var face in faces)
                {
                    var edge = face.Edge;
                    if (edge.From.Tag != -1)
                        edge = edge.Next;

                    if (edge.From.Tag != -1)
                        throw new InvalidOperationException("edge.From.Tag != -1");

                    edges.Clear();
                    edge.GetEdges(edges);

                    if (edges.Count != 6)
                        throw new InvalidOperationException("edges.Count != 6");

                    for (int j = 0; j < 3; j++)
                    {
                        NewEdge(out EDGE e0, out EDGE e1);
                        mesh.Edges.Add(e0, e1);
                        edges.Add(e0, e1);
                    }

                    ConnectTriangleEdges(edges);

                    var f0 = face;
                    var f1 = mesh.NewFace();
                    var f2 = mesh.NewFace();
                    var f3 = mesh.NewFace();

                    ConnectTriangleFace(edges[1], f0);
                    ConnectTriangleFace(edges[3], f1);
                    ConnectTriangleFace(edges[5], f2);
                    ConnectTriangleFace(edges[7], f3);
                }
            }
        }

        private static void ComputeOddInteriorVertex(HBVertex v)
        {
            var a = v.Edge.Previous.From.GetPosition();
            var b = v.Edge.To.GetPosition();
            var c = v.Edge.Next.Next.To.GetPosition();
            var d = v.Edge.Opposite.Previous.Previous.From.GetPosition();

            var p = 3.0 / 8.0 * (a + b) + 1.0 / 8.0 * (c + d);
            v.SetPosition(p);
        }

        private static void ComputeEvenInteriorVertex(HBVertex v, List<Vector3d> positions)
        {
            int n = v.EdgeCount;
            Vector3d sum = Vector3d.Zero;

            foreach (var e in v.EnumerateEdges())
                sum += positions[e.Next.To.Tag];

            double u = (n == 3) ? 3.0 / 16.0 : 3.0 / (8.0 * n);

            var p = positions[v.Tag];
            p = p * (1.0 - n * u) + u * sum;

            System.Diagnostics.Debug.WriteLine(p);

            v.SetPosition(p);
        }

        private static void ConnectTriangleEdges(List<HBEdge> edges)
        {
            ConnectTriangleEdge(edges[6], edges[0], edges[1], edges[1].To);
            ConnectTriangleEdge(edges[7], edges[9], edges[11], edges[5].To);
            ConnectTriangleEdge(edges[8], edges[2], edges[3], edges[3].To);
            ConnectTriangleEdge(edges[9], edges[11], edges[7], edges[1].To);
            ConnectTriangleEdge(edges[10], edges[4], edges[5], edges[5].To);
            ConnectTriangleEdge(edges[11], edges[7], edges[9], edges[3].To);
        }

        private static void ConnectTriangleEdge(HBEdge edge, HBEdge next, HBEdge previous, HBVertex v)
        {
            edge.From = v;
            SetNext(edge, next);
            SetPrevious(edge, previous);
        }

        private static void ConnectTriangleFace(HBEdge edge, HBFace face)
        {
            face.Edge = edge;

            foreach (var e in edge.EnumerateEdges())
                e.Face = face;
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

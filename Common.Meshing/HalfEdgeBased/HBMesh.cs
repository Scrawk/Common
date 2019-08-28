﻿using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge based mesh.
    /// </summary>
    public partial class HBMesh<VERTEX> 
        where VERTEX : HBVertex, new()
    {

        public HBMesh()
        {
            Vertices = new List<VERTEX>();
            Edges = new List<HBEdge>();
            Faces = new List<HBFace>();
        }

        public HBMesh(int numVertices, int numEdges, int numFaces)
        {
            Vertices = new List<VERTEX>(numVertices);
            Edges = new List<HBEdge>(numEdges);
            Faces = new List<HBFace>(numFaces);
        }

        /// <summary>
        /// All the vertices in the mesh.
        /// </summary>
        public List<VERTEX> Vertices { get; private set; }

        /// <summary>
        /// All the edges in the mesh.
        /// </summary>
        public List<HBEdge> Edges { get; private set; }

        /// <summary>
        /// All the faces in the mesh.
        /// </summary>
        public List<HBFace> Faces { get; private set; }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        /// <returns>Mesh as string</returns>
        public override string ToString()
        {
            return string.Format("[HBMesh: Vertices={0}, Edges={1}, Faces={2}]", 
                Vertices.Count, Edges.Count, Faces.Count);
        }

        /// <summary>
        /// Clear mesh.
        /// </summary>
        public virtual void Clear()
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Clear();

            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Clear();

            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Clear();

            Vertices.Clear();
            Edges.Clear();
            Faces.Clear();
        }

        /// <summary>
        /// Fill the mesh with vertices, faces and edges.
        /// Connections will need to be set manually.
        /// </summary>
        public void Fill(int numVertices, int numEdges, int numFaces)
        {
            Clear();

            Vertices.Capacity = numVertices;
            Edges.Capacity = numEdges;
            Faces.Capacity = numFaces;

            for (int i = 0; i < numVertices; i++)
                Vertices.Add(new VERTEX());

            for (int i = 0; i < numEdges; i++)
                Edges.Add(new HBEdge());

            for (int i = 0; i < numFaces; i++)
                Faces.Add(new HBFace());
        }

        /// <summary>
        /// Create a new vertex, add to mesh and return.
        /// </summary>
        /// <returns></returns>
        public VERTEX NewVertex()
        {
            var v = new VERTEX();
            Vertices.Add(v);
            return v;
        }

        /// <summary>
        /// Create a new edge, add to mesh and return.
        /// </summary>
        /// <returns></returns>
        public HBEdge NewEdge()
        {
            var e = new HBEdge();
            Edges.Add(e);
            return e;
        }

        /// <summary>
        /// Create a new face, add to mesh and return.
        /// </summary>
        /// <returns></returns>
        public HBFace NewFace()
        {
            var f = new HBFace();
            Faces.Add(f);
            return f;
        }

        /// <summary>
        /// Find the index of this vertex.
        /// </summary>
        /// <returns>The vertex index or -1 if not found.</returns>
        public int IndexOf(HBVertex vertex)
        {
            VERTEX v = vertex as VERTEX;
            if (v == null) return -1;
            return Vertices.IndexOf(v);
        }

        /// <summary>
        /// Find the index of this edge.
        /// </summary>
        /// <returns>The edge index or -1 if not found.</returns>
        public int IndexOf(HBEdge edge)
        {
            HBEdge e = edge as HBEdge;
            if (e == null) return -1;
            return Edges.IndexOf(e);
        }

        /// <summary>
        /// Find the index of this face.
        /// </summary>
        /// <returns>The face index or -1 if not found.</returns>
        public int IndexOf(HBFace face)
        {
            HBFace f = face as HBFace;
            if (f == null) return -1;
            return Faces.IndexOf(f);
        }

        /// <summary>
        /// Applies the vertex index as a tag.
        /// </summary>
        public void TagVertices()
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = i;
        }

        /// <summary>
        /// Sets all vertex tags.
        /// </summary>
        public void TagVertices(int tag)
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Tag = tag;
        }

        /// <summary>
        /// Applies the edge index as a tag.
        /// </summary>
        public void TagEdges()
        {
            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Tag = i;
        }

        /// <summary>
        /// Sets all edge tags.
        /// </summary>
        public void TagEdges(int tag)
        {
            for (int i = 0; i < Edges.Count; i++)
                Edges[i].Tag = tag;
        }

        /// <summary>
        /// Applies the face index as a tag.
        /// </summary>
        public void TagFaces()
        {
            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Tag = i;
        }

        /// <summary>
        /// Sets all face tags.
        /// </summary>
        public void TagFaces(int tag)
        {
            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Tag = tag;
        }

        /// <summary>
        /// Sets all vertex, edge and face tags.
        /// </summary>
        public void TagAll()
        {
            TagVertices();
            TagEdges();
            TagFaces();
        }

        /// <summary>
        /// Sets all vertex, edge and face tags.
        /// </summary>
        public void TagAll(int tag)
        {
            TagVertices(tag);
            TagEdges(tag);
            TagFaces(tag);
        }

        /// <summary>
        /// Get the position at index i.
        /// </summary>
        public Vector3f GetPosition(int i)
        {
            return Vertices[i].GetPosition();
        }

        /// <summary>
        /// Get the position at index i.
        /// </summary>
        public void SetPosition(int i, Vector3f p)
        {
            Vertices[i].SetPosition(p);
        }

        /// <summary>
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3f> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].GetPosition());
        }

        /// <summary>
        /// Copy all vertex normals into list.
        /// </summary>
        public void GetVertexNormals(List<Vector3f> normals)
        {
            for (int i = 0; i < Vertices.Count; i++)
                normals.Add(Vertices[i].Normal);
        }

        /// <summary>
        /// Copy all vertex centroids into list.
        /// </summary>
        public void GetVertexCentroids(List<Vector3f> centroids)
        {
            for (int i = 0; i < Vertices.Count; i++)
                centroids.Add(Vertices[i].Centriod);
        }

        /// <summary>
        /// Copy all face centroids into list.
        /// </summary>
        public void GetFaceCentroids(List<Vector3f> centroids)
        {
            for (int i = 0; i < Faces.Count; i++)
                centroids.Add(Faces[i].Edge.FaceCentriod);
        }

        /// <summary>
        /// Copy all face normals into list.
        /// </summary>
        public void GetFaceNormals(List<Vector3f> normals)
        {
            for (int i = 0; i < Faces.Count; i++)
                normals.Add(Faces[i].Edge.FaceNormal);
        }

        /// <summary>
        /// Get the vertex indices of a triangle face.
        /// Presumes vertices have been tagged.
        /// </summary>
        public Vector3i GetTriangle(int i)
        {
            var face = Faces[i];
            int a = face.Edge.From.Tag;
            int b = face.Edge.Next.From.Tag;
            int c = face.Edge.Next.Next.From.Tag;
            return new Vector3i(a, b, c);
        }

        /// <summary>
        /// Find the edge that uses these two vertices.
        /// Presumes all edges have opposites.
        /// </summary>
        /// <param name="from">The from vertex</param>
        /// <param name="to">The to vertex</param>
        /// <returns>The edge index or null if not found</returns>
        public HBEdge FindEdge(HBVertex from, HBVertex to)
        {
            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
                if (edge.From == null) continue;
                if (edge.To == null) continue;

                if (ReferenceEquals(edge.From, from) &&
                    ReferenceEquals(edge.To, to))
                    return edge;
            }

            return null;
        }

        /// <summary>
        /// Remove all faces.
        /// Will clear faces from all edges.
        /// </summary>
        public void RemoveFaces()
        {
            Faces.Clear();

            int numEdges = Edges.Count;
            for (int i = 0; i < numEdges; i++)
                Edges[i].Face = null;
        }

        /// <summary>
        /// Creates a index list representing the vertices of each face.
        /// </summary>
        /// <param name="faceVertices">The number of vertices each face has</param>
        /// <param name="indices">list representing the vertices of each face</param>
        public void GetFaceIndices(List<int> indices, int faceVertices)
        {
            if (faceVertices < 3)
                throw new ArgumentException("faceVertices can not be less than 3.");

            TagVertices();
            var vertices = new List<HBVertex>(faceVertices);

            int count = Faces.Count;
            for (int i = 0; i < count; i++)
            {
                var face = Faces[i];

                vertices.Clear();
                face.Edge.GetVertices(vertices);

                if (vertices.Count != faceVertices)
                    throw new InvalidOperationException("Face does not contain the required number of vertices.");

                for (int j = 0; j < faceVertices; j++)
                    indices.Add(vertices[j].Tag);
            }
        }

        /// <summary>
        /// Creates a index list representing the vertices of each edge.
        /// </summary>
        /// <param name="indices">list representing the vertices of each edge</param>
        public void GetEdgeIndices(List<int> indices)
        {
            TagVertices();
            TagEdges(0);

            int count = Edges.Count;
            for (int i = 0; i < count; i++)
            {
                var edge = Edges[i];
                if (edge.Tag == 1) continue;
                if (edge.Opposite == null) continue;

                indices.Add(edge.From.Tag);
                indices.Add(edge.To.Tag);

                edge.Tag = 1;
                edge.Opposite.Tag = 1;
            }
        }

        /// <summary>
        /// Translate all vertices.
        /// </summary>
        public void Translate(Vector3f translate)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p + translate);
            }
        }

        /// <summary>
        /// Rotate all vertices.
        /// </summary>
        public void Rotate(Vector3f rotate)
        {
            var q = Quaternion3f.FromEuler(rotate);
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p * q);
            }
        }

        /// <summary>
        /// Scale all vertices.
        /// </summary>
        public void Scale(Vector3f scale)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p * scale);
            }
        }

        /// <summary>
        /// Transform all vertices.
        /// </summary>
        public void Transform(Matrix4x4f matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition((matrix * p.xyz1).xyz);
            }
        }

        /// <summary>
        /// Print the mesh for debugging.
        /// </summary>
        public string Print()
        {
            var builder = new StringBuilder();

            builder.AppendLine(this.ToString());

            foreach (var v in Vertices)
                builder.AppendLine(v.ToString(this));

            foreach (var e in Edges)
                builder.AppendLine(e.ToString(this));

            foreach (var f in Faces)
                builder.AppendLine(f.ToString(this));

            return builder.ToString();
        }

        /// <summary>
        /// Check the mesh for debugging.
        /// </summary>
        public string Check()
        {
            int count = 0;
            var builder = new StringBuilder();

            for (int i = 0; i < Vertices.Count; i++)
            {
                var errors = Vertices[i].Check();

                if (!string.IsNullOrEmpty(errors))
                {
                    builder.AppendLine("Vertex " + i + " contains errors:");
                    builder.AppendLine(errors);
                    count++;
                }
            }

            for (int i = 0; i < Edges.Count; i++)
            {
                var errors = Edges[i].Check();
                if (!string.IsNullOrEmpty(errors))
                {
                    builder.AppendLine("Edge " + i + " contains errors:");
                    builder.AppendLine(errors);
                    count++;
                }
            }

            for (int i = 0; i < Faces.Count; i++)
            {
                var errors = Faces[i].Check();
                if (!string.IsNullOrEmpty(errors))
                {
                    builder.AppendLine("Face " + i + " contains errors:");
                    builder.AppendLine(errors);
                    count++;
                }
            }

            var msg = builder.ToString();

            if (string.IsNullOrEmpty(msg))
                msg = "Mesh contains no errors";

            return msg;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.LinearAlgebra;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    /// <summary>
    /// A half edge based mesh.
    /// </summary>
    public class HBMesh<VERTEX, EDGE, FACE> : ITriangularMesh
            where VERTEX : HBVertex, new()
            where EDGE : HBEdge, new()
            where FACE : HBFace, new()
    {

        public HBMesh()
        {
            Vertices = new List<VERTEX>();
            Edges = new List<EDGE>();
            Faces = new List<FACE>();
        }

        public HBMesh(int numVertices, int numEdges, int numFaces)
        {
            Vertices = new List<VERTEX>(numVertices);
            Edges = new List<EDGE>(numEdges);
            Faces = new List<FACE>(numFaces);
        }

        /// <summary>
        /// All the vertices in the mesh.
        /// </summary>
        public List<VERTEX> Vertices { get; private set; }

        /// <summary>
        /// The number of vertices in mesh.
        /// </summary>
        public int VertexCount => Vertices.Count;

        /// <summary>
        /// All the edges in the mesh.
        /// </summary>
        public List<EDGE> Edges { get; private set; }

        /// <summary>
        /// All the faces in the mesh.
        /// </summary>
        public List<FACE> Faces { get; private set; }

        /// <summary>
        /// The number of faces in mesh.
        /// </summary>
        public int FaceCount => Faces.Count;

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
            EDGE e = edge as EDGE;
            if (e == null) return -1;
            return Edges.IndexOf(e);
        }

        /// <summary>
        /// Find the index of this face.
        /// </summary>
        /// <returns>The face index or -1 if not found.</returns>
        public int IndexOf(HBFace face)
        {
            FACE f = face as FACE;
            if (f == null) return -1;
            return Faces.IndexOf(f);
        }

        /// <summary>
        /// Creates a new vertex, adds it to 
        /// vertex list add returns.
        /// </summary>
        /// <returns>The new vertex</returns>
        public VERTEX NewVertex()
        {
            var v = new VERTEX();
            Vertices.Add(v);
            return v;
        }

        /// <summary>
        /// Creates a new edge, adds it to 
        /// edge list add returns.
        /// </summary>
        /// <returns>The new edge</returns>
        public EDGE NewEdge()
        {
            var e = new EDGE();
            Edges.Add(e);
            return e;
        }

        /// <summary>
        /// Creates a new face, adds it to 
        /// face list add returns.
        /// </summary>
        /// <returns>The new face</returns>
        public FACE NewFace()
        {
            var f = new FACE();
            Faces.Add(f);
            return f;
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
        /// Copy all vertex positions into list.
        /// </summary>
        public void GetPositions(List<Vector3d> positions)
        {
            for (int i = 0; i < Vertices.Count; i++)
                positions.Add(Vertices[i].GetPosition());
        }

        /// <summary>
        /// Get the position at index i.
        /// </summary>
        public Vector3d GetPosition(int i)
        {
            return Vertices[i].GetPosition();
        }

        /// <summary>
        /// Gets the triangle at index i.
        /// Presumes faces are triangles 
        /// and vertices have been tagged.
        /// </summary>
        public Vector3i GetTriangle(int i)
        {
            var face = Faces[i];
            int a = face.Edge.Previous.From.Tag;
            int b = face.Edge.From.Tag;
            int c = face.Edge.Next.From.Tag;
            return new Vector3i(a, b, c);
        }

        /// <summary>
        /// Find the edge that uses these two vertices.
        /// Presumes all edges have opposites.
        /// </summary>
        /// <param name="from">The from vertex</param>
        /// <param name="to">The to vertex</param>
        /// <returns>The edge index or null if not found</returns>
        public EDGE FindEdge(HBVertex from, HBVertex to)
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
                Edges.Add(new EDGE());

            for (int i = 0; i < numFaces; i++)
                Faces.Add(new FACE());
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
            List<VERTEX> vertices = new List<VERTEX>(faceVertices);

            int count = Faces.Count;
            for (int i = 0; i < count; i++)
            {
                var face = Faces[i];

                vertices.Clear();
                face.GetVertices(vertices);

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

        public void Translate(Vector3d translate)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p + translate);
            }
        }

        public void Scale(Vector3d scale)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p * scale);
            }
        }

        public void Transform(Matrix4x4d matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition((matrix * p.xyz1).xyz);
            }
        }

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

    }
}

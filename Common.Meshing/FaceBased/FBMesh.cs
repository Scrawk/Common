using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.FaceBased
{

    /// <summary>
    /// A faced based mesh.
    /// </summary>
    /// <typeparam name="VERTEX">The element type for the vertices</typeparam>
    /// <typeparam name="FACE">The element type for the faces</typeparam>
    public class FBMesh<VERTEX, FACE>
           where VERTEX : FBVertex, new()
           where FACE : FBFace, new()
    {

        /// <summary>
        /// Create a new mesh.
        /// </summary>
        public FBMesh()
        {
            Vertices = new List<VERTEX>();
            Faces = new List<FACE>();
        }

        /// <summary>
        /// Create a new mesh.
        /// </summary>
        /// <param name="numVertices">The capacity of the vertex list</param>
        /// <param name="numFaces">The capacity of the face list</param>
        public FBMesh(int numVertices, int numFaces)
        {
            Vertices = new List<VERTEX>(numVertices);
            Faces = new List<FACE>(numFaces);
        }

        /// <summary>
        /// All the vertices in the mesh.
        /// </summary>
        public List<VERTEX> Vertices { get; private set; }

        /// <summary>
        /// All the faces in the mesh.
        /// </summary>
        public List<FACE> Faces { get; private set; }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[FBMesh: Vertices={0}, Faces={1}]",
                Vertices.Count, Faces.Count);
        }

        /// <summary>
        /// Clears the vertex and face lists.
        /// </summary>
        public virtual void Clear()
        {
            for (int i = 0; i < Vertices.Count; i++)
                Vertices[i].Clear();

            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Clear();

            Vertices.Clear();
            Faces.Clear();
        }

        /// <summary>
        /// Find the index of this vertex.
        /// </summary>
        /// <returns>The vertex index or -1 if not found.</returns>
        public int IndexOf(FBVertex vertex)
        {
            VERTEX v = vertex as VERTEX;
            if (v == null) return -1;
            return Vertices.IndexOf(v);
        }

        /// <summary>
        /// Find the index of this face.
        /// </summary>
        /// <returns>The face index or -1 if not found.</returns>
        public int IndexOf(FBFace face)
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
        /// Sets all vertex and face tags.
        /// </summary>
        public void TagAll()
        {
            TagVertices();
            TagFaces();
        }

        /// <summary>
        /// Sets all vertex and face tags.
        /// </summary>
        public void TagAll(int tag)
        {
            TagVertices(tag);
            TagFaces(tag);
        }

        /// <summary>
        /// Get the position at index i.
        /// </summary>
        public Vector3d GetPosition(int i)
        {
            return Vertices[i].GetPosition();
        }

        /// <summary>
        /// Get the position at index i.
        /// </summary>
        public void SetPosition(int i, Vector3d p)
        {
            Vertices[i].SetPosition(p);
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
        /// Fills the mesh by creating the require number of vertices and faces.
        /// </summary>
        /// <param name="numVertices">Number of vertices to create</param>
        /// <param name="numFaces">Number of faces to create</param>
        public void Fill(int numVertices, int numFaces)
        {
            Clear();

            Vertices.Capacity = numVertices;
            Faces.Capacity = numFaces;

            for (int i = 0; i < numVertices; i++)
                Vertices.Add(new VERTEX());

            for (int i = 0; i < numFaces; i++)
                Faces.Add(new FACE());
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

            int count = Faces.Count;
            for (int i = 0; i < count; i++)
            {
                var face = Faces[i];
                if (face.Vertices.Length != faceVertices)
                    throw new InvalidOperationException("Face does not contain the required number of vertices.");

                for (int j = 0; j < faceVertices; j++)
                {
                    var v = face.Vertices[j];
                    indices.Add(v.Tag);
                }
            }
        }

        /// <summary>
        /// Finds each face that a vertex is attached to
        /// and stores it in the vertices face list.
        /// </summary>
        public void SetVerticeFaces()
        {
            int count = Vertices.Count;
            for (int i = 0; i < count; i++)
            {
                var vert = Vertices[i];
                if (vert.Faces == null)
                    vert.Faces = new List<FBFace>();
                else
                    vert.Faces.Clear();
            }

            count = Faces.Count;
            for (int i = 0; i < count; i++)
            {
                var face = Faces[i];
                for (int j = 0; j < face.NumVertices; j++)
                {
                    var v = face.Vertices[j];
                    v.Faces.Add(face);
                }
            }
        }

        /// <summary>
        /// Removes each vertices face list.
        /// </summary>
        public void RemoveVerticeFaces()
        {
            int count = Vertices.Count;
            for (int i = 0; i < count; i++)
                Vertices[i].Faces = null;
        }

        /// <summary>
        /// Translate all vertices.
        /// </summary>
        public void Translate(Vector3d translate)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition(p + translate);
            }
        }

        /// <summary>
        /// Scale all vertices.
        /// </summary>
        public void Scale(Vector3d scale)
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
        public void Transform(Matrix4x4d matrix)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var p = Vertices[i].GetPosition();
                Vertices[i].SetPosition((matrix * p.xyz1).xyz);
            }
        }

        /// <summary>
        /// Print mesh for debugging.
        /// </summary>
        public string Print()
        {
            var builder = new StringBuilder();

            builder.AppendLine(this.ToString());

            foreach (var v in Vertices)
                builder.AppendLine(v.ToString(this));

            foreach (var f in Faces)
                builder.AppendLine(f.ToString(this));

            return builder.ToString();
        }

    }
}

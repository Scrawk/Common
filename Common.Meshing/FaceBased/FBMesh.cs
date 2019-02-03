using System;
using System.Collections.Generic;

using Common.Core.LinearAlgebra;

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

        public List<VERTEX> Vertices { get; private set; }

        public List<FACE> Faces { get; private set; }

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
        public void Clear()
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
        /// Applies the face index as a tag.
        /// </summary>
        public void TagFaces()
        {
            for (int i = 0; i < Faces.Count; i++)
                Faces[i].Tag = i;
        }

        public void TagAll()
        {
            TagVertices();
            TagFaces();
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
        /// <returns>list representing the vertices of each face</returns>
        public List<int> CreateFaceIndices(int faceVertices = 3)
        {
            int count = Faces.Count;
            int size = Faces.Count * faceVertices;

            TagVertices();
            List<int> indices = new List<int>(size);

            for(int i = 0; i < count; i++)
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

            return indices;
        }

    }
}

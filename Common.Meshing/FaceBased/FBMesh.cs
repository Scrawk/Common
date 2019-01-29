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
            foreach (var v in Vertices)
                v.Clear();

            foreach (var f in Faces)
                f.Clear();

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
        public List<int> CreateFaceIndices(int faceVertices)
        {
            int count = Faces.Count;
            int size = Faces.Count * faceVertices;

            List<int> indices = new List<int>(size);

            for(int i = 0; i < count; i++)
            {
                for(int j = 0; j < faceVertices; j++)
                {
                    if (Faces[i].Vertices.Length != faceVertices)
                        throw new InvalidOperationException("Face does not contain the required number of vertices.");

                    VERTEX v = Faces[i].Vertices[j] as VERTEX;
                    if (v == null)
                        throw new InvalidOperationException("Face vertex is null.");

                    int index = Vertices.IndexOf(v);
                    indices.Add(index);
                }
            }

            return indices;
        }

    }
}

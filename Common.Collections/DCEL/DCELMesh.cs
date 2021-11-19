using System;
using System.Collections.Generic;
using System.Text;

using Common.Core.Numerics;

namespace Common.Collections.DCEL
{
    /// <summary>
    /// A half edge based mesh.
    /// </summary>
    public partial class DCELMesh
    {

        public DCELMesh()
        {
            Vertices = new List<DCELVertex>();
            Edges = new List<DCELHalfedge>();
            Faces = new List<DCELFace>();
        }

        /// <summary>
        /// The number of vertices in the mesh.
        /// </summary>
        public int VertexCount { get; private set; }

        /// <summary>
        /// The number of edges in the mesh.
        /// </summary>
        public int EdgeCount { get; private set; }

        /// <summary>
        /// The number of faces in the mesh.
        /// </summary>
        public int FaceCount { get; private set; }

        /// <summary>
        /// The vertex capacity in the mesh.
        /// </summary>
        public int VertexCapacity => Vertices.Capacity;

        /// <summary>
        /// The edge capacity in the mesh.
        /// </summary>
        public int EdgeCapacity => Edges.Capacity;

        /// <summary>
        /// The face capacity in the mesh.
        /// </summary>
        public int FaceCapacity => Faces.Capacity;

        /// <summary>
        /// All the vertices in the mesh.
        /// </summary>
        private List<DCELVertex> Vertices { get; set; }

        /// <summary>
        /// All the edges in the mesh.
        /// </summary>
        private List<DCELHalfedge> Edges { get; set; }

        /// <summary>
        /// All the faces in the mesh.
        /// </summary>
        private List<DCELFace> Faces { get; set; }

        /// <summary>
        /// Convert mesh to string.
        /// </summary>
        /// <returns>Mesh as string</returns>
        public override string ToString()
        {
            return string.Format("[DCELMesh: Vertices={0}, Edges={1}, Faces={2}]",
                VertexCount, EdgeCount, FaceCount);
        }

        /// <summary>
        /// Enumerate through all the vertices in the mesh.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DCELVertex> EnumerateVertices()
        {
            foreach(var vert in Vertices)
            {
                if (vert != null)
                    yield return vert;
            }
        }

        /// <summary>
        /// Enumerate through all the half edges in the mesh.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DCELHalfedge> EnumerateHalfedges()
        {
            foreach (var edge in Edges)
            {
                if (edge != null)
                {
                    yield return edge;
                    yield return edge.Opposite;
                }
                    
            }
        }

        /// <summary>
        /// Enumerate through all the edges in the mesh.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DCELHalfedge> EnumerateEdges()
        {
            foreach (var edge in Edges)
            {
                if (edge != null)
                    yield return edge;
            }
        }

        /// <summary>
        /// Enumerate through all the faces in the mesh.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DCELFace> EnumerateFaces()
        {
            foreach (var face in Faces)
            {
                if (face != null)
                    yield return face;
            }
        }

        /// <summary>
        /// Get the vertex at the index.
        /// </summary>
        /// <param name="index">The vertices index.</param>
        /// <returns>The vertex at the index.</returns>
        public DCELVertex GetVertex(int index)
        {
            return Vertices[index];
        }

        /// <summary>
        /// Get the edge at the index.
        /// </summary>
        /// <param name="index">The edges index.</param>
        /// <returns>The edge at the index.</returns>
        public DCELHalfedge GetEdge(int index)
        {
            return Edges[index];
        }

        /// <summary>
        /// Get the face at the index.
        /// </summary>
        /// <param name="index">The faces index.</param>
        /// <returns>The face at the index.</returns>
        public DCELFace GetFace(int index)
        {
            return Faces[index];
        }

        /// <summary>
        /// Clear the mesh.
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
        /// Get the next index to add a new vertex.
        /// </summary>
        /// <returns>The index in the list to add the vertex.</returns>
        private int NextVertexIndex()
        {
            Vertices.Add(null);
            return Vertices.Count - 1;
        }

        /// <summary>
        /// Get the next index to add a new edge.
        /// </summary>
        /// <returns>The index in the list to add the edge.</returns>
        private int NextEdgeIndex()
        {
            Edges.Add(null);
            return Edges.Count - 1;
        }

        /// <summary>
        /// Get the next index to add a new face.
        /// </summary>
        /// <returns>The index in the list to add the face.</returns>
        private int NextFaceIndex()
        {
            Faces.Add(null);
            return Faces.Count - 1;
        }

        /// <summary>
        /// Add a new vertex to the mesh.
        /// </summary>
        /// <param name="point">The vertices point.</param>
        /// <returns>The new vertex.</returns>
        public DCELVertex InsertVertex(Vector2d point)
        {
            int i = NextVertexIndex();
            var vert = new DCELVertex(i, point);

            Vertices[i] = vert;
            VertexCount++;

            return vert;
        }

        /// <summary>
        /// Add a new edge connecting the two vertices.
        /// </summary>
        /// <returns>The new edge that goes from v0 and to v1.</returns>
        public DCELHalfedge InsertEdge(DCELVertex v0, DCELVertex v1)
        {
            if (v0 == v1)
                throw new Exception("Can not connect the same vertex objects.");

            if (v0.Point == v1.Point)
                throw new Exception("Can not connect the same vertex positions.");

            int count0 = v0.EdgeCount;
            int count1 = v1.EdgeCount;

            int i = NextEdgeIndex();
  
            var e0 = new DCELHalfedge(i);
            var e1 = new DCELHalfedge(i);
            e0.Opposite = e1;
            e1.Opposite = e0;

            if (count0 == 0 && count1 == 0)
            {

            }
            else if (count0 <= 1 && count1 <= 1)
            {
                DCELHalfedge.SetPrevious(e0, v0.Edge?.Opposite);
                DCELHalfedge.SetNext(e0, v1?.Edge);

                DCELHalfedge.SetPrevious(e1, v1.Edge?.Opposite);
                DCELHalfedge.SetNext(e1, v0?.Edge);
            }
            else if (count0 > 1 && count1 <= 1)
            {
                var edge = v0.FindInBetweenEdges(v1.Point);
                var previous = edge.Previous;

                DCELHalfedge.SetPrevious(e0, previous);
                DCELHalfedge.SetNext(e0, v1?.Edge);

                DCELHalfedge.SetPrevious(e1, v1.Edge?.Opposite);
                DCELHalfedge.SetNext(e1, edge);
            }
            else if (count0 <= 1 && count1 > 1)
            {
                var edge = v1.FindInBetweenEdges(v0.Point);
                var previous = edge.Previous;

                DCELHalfedge.SetPrevious(e0, v0.Edge?.Opposite);
                DCELHalfedge.SetNext(e0, edge);

                DCELHalfedge.SetPrevious(e1, previous);
                DCELHalfedge.SetNext(e1, v0?.Edge);
            }
            else if (count0 > 1 && count1 > 1)
            {
                var edge0 = v0.FindInBetweenEdges(v1.Point);
                var previous0 = edge0.Previous;

                var edge1 = v1.FindInBetweenEdges(v0.Point);
                var previous1 = edge1.Previous;

                DCELHalfedge.SetPrevious(e0, previous0);
                DCELHalfedge.SetNext(e0, edge1);

                DCELHalfedge.SetPrevious(e1, previous1);
                DCELHalfedge.SetNext(e1, edge0);
            }
            else
            {
                throw new Exception("Unhandled case connecting vertices.");
            }

            DCELHalfedge.SetFrom(e0, v0);
            DCELHalfedge.SetFrom(e1, v1);

            Edges[i] = e0;
            EdgeCount++;

            return e0;
        }

        /// <summary>
        /// Applies the vertex index as a tag.
        /// </summary>
        public void TagVertices()
        {
            int i = 0;
            foreach(var vert in EnumerateVertices())
                vert.Tag = i++;
        }

        /// <summary>
        /// Sets all vertex tags.
        /// </summary>
        public void TagVertices(int tag)
        {
            foreach (var vert in EnumerateVertices())
                vert.Tag = tag;
        }

        /// <summary>
        /// Applies the edge index as a tag.
        /// </summary>
        public void TagEdges()
        {
            int i = 0;
            foreach (var edge in EnumerateHalfedges())
                edge.Tag = i++;
        }

        /// <summary>
        /// Sets all edge tags.
        /// </summary>
        public void TagEdges(int tag)
        {
            foreach (var edge in EnumerateHalfedges())
                edge.Tag = tag;
        }

        /// <summary>
        /// Applies the face index as a tag.
        /// </summary>
        public void TagFaces()
        {
            int i = 0;
            foreach (var face in EnumerateFaces())
                face.Tag = i++;
        }

        /// <summary>
        /// Sets all face tags.
        /// </summary>
        public void TagFaces(int tag)
        {
            foreach (var face in EnumerateFaces())
                face.Tag = tag;
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
        /// Return the vertices index or -1 if vertex is null.
        /// </summary>
        /// <param name="vert">The vertex</param>
        /// <returns>The vertices index or -1 if vertex is null.</returns>
        public static int IndexOrDefault(DCELVertex vert)
        {
            return vert != null ? vert.Index : -1;
        }

        /// <summary>
        /// Return the edges index or -1 if edge is null.
        /// </summary>
        /// <param name="edge">The edge</param>
        /// <returns>The edges index or -1 if edge is null.</returns>
        public static int IndexOrDefault(DCELHalfedge edge)
        {
            return edge != null ? edge.Index : -1;
        }

        /// <summary>
        /// Return the faces index or -1 if face is null.
        /// </summary>
        /// <param name="face">The face</param>
        /// <returns>The faces index or -1 if face is null.</returns>
        public static int IndexOrDefault(DCELFace face)
        {
            return face != null ? face.Index : -1;
        }

        /// <summary>
        /// Clear all vertex data to null.
        /// </summary>
        public void ClearVertexData()
        {
            foreach(var vert in EnumerateVertices())
                vert.Data = null;
        }

        /// <summary>
        /// Clear all edge data to null.
        /// </summary>
        public void ClearEdgeData()
        {
            foreach (var edge in EnumerateHalfedges())
                edge.Data = null;
        }

        /// <summary>
        /// Clear all face data to null.
        /// </summary>
        public void ClearFaceData()
        {
            foreach (var face in EnumerateFaces())
                face.Data = null;
        }

        /// <summary>
        /// Clear all data to null.
        /// </summary>
        public void ClearAllData()
        {
            ClearVertexData();
            ClearEdgeData();
            ClearFaceData();
        }

        /// <summary>
        /// Translate all vertices.
        /// </summary>
        public void Translate(Vector2d translate)
        {
            foreach (var vert in EnumerateVertices())
                vert.Point += translate;

        }

        /// <summary>
        /// Scale all vertices.
        /// </summary>
        public void Scale(Vector2d scale)
        {
            foreach (var vert in EnumerateVertices())
                vert.Point *= scale;

        }

        /// <summary>
        /// Transform all vertices.
        /// </summary>
        public void Transform(Matrix4x4d matrix)
        {
            foreach (var vert in EnumerateVertices())
                vert.Point = (matrix * vert.Point.xy01).xy;
        }

    }
}

using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.AdjacencyGraphs
{
    public class AdjacencyGraphConstructor2f : 
        AdjacencyGraphConstructor<AdjacencyGraph2f, AdjacencyVertex2f, AdjacencyEdge>
    {

    }

    public class AdjacencyGraphConstructor2d :
        AdjacencyGraphConstructor<AdjacencyGraph2d, AdjacencyVertex2d, AdjacencyEdge>
    {

    }

    public class AdjacencyGraphConstructor3f :
    AdjacencyGraphConstructor<AdjacencyGraph3f, AdjacencyVertex3f, AdjacencyEdge>
    {

    }

    public class AdjacencyGraphConstructor3d :
        AdjacencyGraphConstructor<AdjacencyGraph3d, AdjacencyVertex3d, AdjacencyEdge>
    {

    }

    /// <summary>
    /// A adjacency graph constructor.
    /// Only supports edge meshes.
    /// </summary>
    public class AdjacencyGraphConstructor<GRAPH, VERTEX, EDGE> :
            IEdgeMeshConstructor<GRAPH>
            where GRAPH : AdjacencyGraph<VERTEX, EDGE>, new()
            where VERTEX : AdjacencyVertex, new()
            where EDGE : AdjacencyEdge, new()
    {

        private GRAPH m_graph;

        private int m_vertexIndex;

        /// <summary>
        /// Start a new edge mesh.
        /// </summary>
        /// <param name="numVertices">number of vertices in graph</param>
        /// <param name="numEdges">number of edges in graph</param>
        public void PushEdgeMesh(int numVertices, int numEdges)
        {
            if (m_graph != null)
                throw new InvalidOperationException("Graph under construction. Can not push a new graph.");

            m_vertexIndex = 0;
            m_graph = new GRAPH();
            m_graph.Fill(numVertices);
        }

        /// <summary>
        /// Remove and return finished mesh.
        /// </summary>
        public GRAPH PopMesh()
        {
            m_vertexIndex = 0;

            var tmp = m_graph;
            m_graph = null;
            return tmp;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector2d pos)
        {
            CheckGraphIsPushed();

            var v = new VERTEX();
            v.Index = m_vertexIndex;
            v.SetPosition(pos.xy0);
            m_graph.Vertices[m_vertexIndex++] = v;
        }

        /// <summary>
        /// Add a vertex to the mesh with this position.
        /// </summary>
        /// <param name="pos">The vertex position</param>
        public void AddVertex(Vector3d pos)
        {
            CheckGraphIsPushed();

            var v = new VERTEX();
            v.Index = m_vertexIndex;
            v.SetPosition(pos);
            m_graph.Vertices[m_vertexIndex++] = v;
        }

        /// <summary>
        /// Add a edge connecting two vertices.
        /// Edges are directional so add a 
        /// edge for each direction.
        /// </summary>
        /// <param name="from">index of from vertex</param>
        /// <param name="to">index of to vertex</param>
        public void AddEdge(int from, int to)
        {
            CheckGraphIsPushed();

            var e0 = new EDGE();
            e0.From = from;
            e0.To = to;

            var e1 = new EDGE();
            e1.From = to;
            e1.To = from;

            m_graph.AddEdge(e0);
            m_graph.AddEdge(e1);
        }

        /// <summary>
        /// Helper to throw exception if tring to create mesh
        /// with out pushing a mesh first.
        /// </summary>
        private void CheckGraphIsPushed()
        {
            if (m_graph == null)
                throw new InvalidOperationException("Graph has not been pushed.");
        }
    }
}

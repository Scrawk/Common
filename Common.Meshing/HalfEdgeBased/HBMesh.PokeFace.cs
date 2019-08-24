using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.HalfEdgeBased
{
    public partial class HBMesh<VERTEX>
        where VERTEX : HBVertex, new()
    {
        /// <summary>
        /// Adds a vertex inside the face and creates a edge from
        /// that vertex to each other vertex in the face.
        /// Will resolute in the face being subdivided into triangles.
        /// Used the faces centroid as the position.
        /// </summary>
        /// <param name="mesh">The mesh the face belongs to.</param>
        /// <param name="face">Th face to be poked. Must have at least 3 vertices.</param>
        /// <returns>The created vertex.</returns>
        public VERTEX PokeFace(HBFace face)
        {
            int count = face.EdgeCount;
            if (count < 3)
                throw new InvalidOperationException("Face has < 3 vertices.");

            var pos = face.Edge.FaceCentriod;
            return PokeFace(face, pos);
        }

        /// <summary>
        /// Adds a vertex inside the face and creates a edge from
        /// that vertex to each other vertex in the face.
        /// Will resolute in the face being subdivided into triangles.
        /// </summary>
        /// <param name="mesh">The mesh the face belongs to.</param>
        /// <param name="face">Th face to be poked. Must have at least 3 vertices.</param>
        /// <param name="pos">The position of the new vertex.</param>
        /// <returns>The created vertex.</returns>
        public VERTEX PokeFace(HBFace face, Vector3d pos)
        {
            int count = face.EdgeCount;
            if (count < 3)
                throw new InvalidOperationException("Face has < 3 vertices.");

            //Create the new vertex, set the pos and add to mesh.
            var vert = new VERTEX();
            vert.SetPosition(pos);
            Vertices.Add(vert as VERTEX);

            //Create a tmp array to hold the new and old edges.
            var oldEdges = new HBEdge[count];
            var newEdges = new HBEdge[count];

            int index = 0;
            foreach (var edge in face.Edge.EnumerateEdges())
            {
                //Create a new edge. e0 and e1 are the half edges.
                NewEdge(out HBEdge e0, out HBEdge e1);
                Edges.Add(e0);
                Edges.Add(e1);

                oldEdges[index] = edge;
                newEdges[index] = e0;
                index++;
            }

            for (int i = 0; i < count; i++)
            {
                var edge = oldEdges[i];
                var previous = edge.Previous;
                var e0 = newEdges[i];
                var e1 = e0.Opposite;

                //Make the connections with the existing edge.
                //e0 goes from the new vertex and to the existing vert.
                //e1 goes from the existing vert and to the new vert.
                e0.From = vert;
                e0.Next = edge;
                edge.Previous = e0;
                e1.From = edge.From;
                e1.Previous = previous;
                previous.Next = e1;

                //Make the connections with the new edges.
                previous = newEdges[IMath.Wrap(i + 1, count)].Opposite;
                var next = newEdges[IMath.Wrap(i - 1, count)];

                e0.Previous = previous;
                previous.Next = e0;

                e1.Next = next;
                next.Previous = e1;
            }

            //The first created edge should be long to 
            //the existing face.
            newEdges[0].Face = face;
            face.Edge = newEdges[0];
            vert.Edge = newEdges[0];

            for (int i = 0; i < count; i++)
            {
                var f = newEdges[i].Face;

                //If the new edge does not have a face create one.
                if (f == null)
                {
                    f = new HBFace();
                    Faces.Add(f);
                }

                //Enmerate around the edges and set the face.
                foreach (var edge in newEdges[i].EnumerateEdges())
                {
                    edge.Face = f;
                    f.Edge = edge;
                }
            }

            return vert;
        }

    }
}

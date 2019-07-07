using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.HalfEdgeBased;

namespace Common.Meshing.Constructors
{
    /// <summary>
    /// Create general meshes via mesh constructor.
    /// All faces are CCW.
    /// </summary>
    public static class CreatePolygonalMesh2
    {

        public static void FromBox<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2d min, Vector2d max)
        {
            constructor.PushPolygonMesh(4, 1);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2d(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2d(min.x, max.y));

            constructor.AddFace(0, 4);
        }

        public static void FromCircle<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2d center, double radius, int segments)
        {
            constructor.PushPolygonMesh(segments, 1);

            double pi = Math.PI;
            double fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                double theta = 2.0f * pi * i / fseg;

                double x = -radius * Math.Cos(theta);
                double y = -radius * Math.Sin(theta);

                constructor.AddVertex(center + new Vector2d(x, y));
            }

            constructor.AddFace(0, segments);
        }

    }
}

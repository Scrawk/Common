using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Meshing.Constructors
{
    /// <summary>
    /// Create general meshes via mesh constructor.
    /// All faces are CCW.
    /// </summary>
    public static class CreatePolygonMesh2
    {

        public static void FromBox<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2f min, Vector2f max)
        {
            constructor.PushPolygonMesh(4, 1);

            constructor.AddVertex(min);
            constructor.AddVertex(new Vector2f(max.x, min.y));
            constructor.AddVertex(max);
            constructor.AddVertex(new Vector2f(min.x, max.y));

            constructor.AddFace(0, 4);
        }

        public static void FromCircle<MESH>(IPolygonMeshConstructor<MESH> constructor, Vector2f center, float radius, int segments)
        {
            constructor.PushPolygonMesh(segments, 1);

            float pi = FMath.PI;
            float fseg = segments;

            for (int i = 0; i < segments; i++)
            {
                float theta = 2.0f * pi * i / fseg;

                float x = -radius * FMath.Cos(theta);
                float y = -radius * FMath.Sin(theta);

                constructor.AddVertex(center + new Vector2f(x, y));
            }

            constructor.AddFace(0, segments);
        }

    }
}

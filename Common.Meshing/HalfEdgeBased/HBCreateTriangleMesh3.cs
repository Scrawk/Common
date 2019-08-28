using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreateTriangleMesh3
    {
        public static HBMesh3f FromTriangle(Vector3f A, Vector3f B, Vector3f C)
        {
            var constructor = new HBMeshConstructor3f();
            CreateTriangleMesh3.FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh3f FromTetrahedron(Vector3f A, Vector3f B, Vector3f C, Vector3f D)
        {
            var constructor = new HBMeshConstructor3f();
            CreateTriangleMesh3.FromTertahedron(constructor, A, B, C, D);
            return constructor.PopMesh();
        }

        public static HBMesh3f FromBox(Vector3f min, Vector3f max)
        {
            var constructor = new HBMeshConstructor3f();
            CreateTriangleMesh3.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh3f FromMesh(IList<Vector3f> positions, IList<int> indices, bool ccw)
        {
            var constructor = new HBMeshConstructor3f();
            CreateTriangleMesh3.FromMesh(constructor, positions, indices, ccw);
            return constructor.PopMesh();
        }

        public static HBMesh3f FromIcosahedron(float scale)
        {
            var constructor = new HBMeshConstructor3f();
            CreateTriangleMesh3.FromIcosahedron(constructor, scale);
            return constructor.PopMesh();
        }
    }
}

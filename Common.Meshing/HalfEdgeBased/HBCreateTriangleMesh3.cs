using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Meshing.Constructors;

namespace Common.Meshing.HalfEdgeBased
{
    public static class HBCreateTriangleMesh3
    {
        public static HBMesh3d FromTriangle(Vector3d A, Vector3d B, Vector3d C)
        {
            var constructor = new HBMeshConstructor3d();
            CreateTriangleMesh3.FromTriangle(constructor, A, B, C);
            return constructor.PopMesh();
        }

        public static HBMesh3d FromTetrahedron(Vector3d A, Vector3d B, Vector3d C, Vector3d D)
        {
            var constructor = new HBMeshConstructor3d();
            CreateTriangleMesh3.FromTertahedron(constructor, A, B, C, D);
            return constructor.PopMesh();
        }

        public static HBMesh3d FromBox(Vector3d min, Vector3d max)
        {
            var constructor = new HBMeshConstructor3d();
            CreateTriangleMesh3.FromBox(constructor, min, max);
            return constructor.PopMesh();
        }

        public static HBMesh3d FromMesh(IList<Vector3d> positions, IList<int> indices, bool ccw)
        {
            var constructor = new HBMeshConstructor3d();
            CreateTriangleMesh3.FromMesh(constructor, positions, indices, ccw);
            return constructor.PopMesh();
        }

        public static HBMesh3d FromIcosahedron(double scale)
        {
            var constructor = new HBMeshConstructor3d();
            CreateTriangleMesh3.FromIcosahedron(constructor, scale);
            return constructor.PopMesh();
        }
    }
}

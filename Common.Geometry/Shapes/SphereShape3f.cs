using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public class SphereShape3f : IShape3f
    {
        public SphereShape3f(float x, float y, float z, float radius)
        {
            Sphere = new Sphere3f(x,y,z,radius);
        }

        public SphereShape3f(Vector3f center, float radius)
        {
            Sphere = new Sphere3f(center, radius);
        }

        public SphereShape3f(Sphere3f sphere)
        {
            Sphere = sphere;
        }

        public Sphere3f Sphere;

        public Box3f Bounds => Sphere.Bounds;

        public bool Contains(Vector3f p)
        {
            return Sphere.Contains(p);
        }

        public bool Intersects(Box3f box)
        {
            return Sphere.Intersects(box);
        }

        public Vector3f Closest(Vector3f p)
        {
            return Sphere.Closest(p);
        }

        public float SignedDistance(Vector3f p)
        {
            return Sphere.SignedDistance(p);
        }
    }
}

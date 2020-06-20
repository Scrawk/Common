using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    public interface ISignedDistanceFunction3f
    {
        float SignedDistance(Vector3f p);
    }

    public interface IShape3f : ISignedDistanceFunction3f
    {
        Box3f Bounds { get; }

        bool Contains(Vector3f p);

        bool Intersects(Box3f box);

        Vector3f Closest(Vector3f p);

    }

    public abstract class Shape3f : IShape3f
    {
        public abstract Box3f Bounds { get; }

        public Vector3f Closest(Vector3f p)
        {
            throw new NotImplementedException();
        }

        public bool Contains(Vector3f p)
        {
            throw new NotImplementedException();
        }

        public bool Intersects(Box3f box)
        {
            throw new NotImplementedException();
        }

        public float SignedDistance(Vector3f p)
        {
            throw new NotImplementedException();
        }
    }
}

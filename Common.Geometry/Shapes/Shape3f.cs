using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{

    public interface IShape3f
    { 
        Box3f Bounds { get; }

        bool Contains(Vector3f p);

        bool Intersects(Box3f box);

        Vector3f Closest(Vector3f p);

        float SignedDistance(Vector3f p);

    }

    public abstract class Shape3f : IShape3f
    {
        public abstract Box3f Bounds { get; }

        public virtual Vector3f Closest(Vector3f p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains(Vector3f p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Intersects(Box3f box)
        {
            throw new NotImplementedException();
        }

        public virtual float SignedDistance(Vector3f p)
        {
            throw new NotImplementedException();
        }
    }
}

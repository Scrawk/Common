using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Core.Shapes
{

    public interface IShape3d
    {
        Box3d Bounds { get; }

        bool Contains(Point3d p);

        bool Intersects(Box3d box);

        Point3d Closest(Point3d p);

        double SignedDistance(Point3d p);

    }

    public abstract class Shape3d : IShape3d
    {
        public abstract Box3d Bounds { get; }

        public virtual Point3d Closest(Point3d p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains(Point3d p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Intersects(Box3d box)
        {
            throw new NotImplementedException();
        }

        public virtual double SignedDistance(Point3d p)
        {
            throw new NotImplementedException();
        }
    }
}

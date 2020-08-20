﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{

    public interface IShape3d
    {
        Box3d Bounds { get; }

        bool Contains(Vector3d p);

        bool Intersects(Box3d box);

        Vector3d Closest(Vector3d p);

        double SignedDistance(Vector3d p);

    }

    public abstract class Shape3d : IShape3d
    {
        public abstract Box3d Bounds { get; }

        public virtual Vector3d Closest(Vector3d p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Contains(Vector3d p)
        {
            throw new NotImplementedException();
        }

        public virtual bool Intersects(Box3d box)
        {
            throw new NotImplementedException();
        }

        public virtual double SignedDistance(Vector3d p)
        {
            throw new NotImplementedException();
        }
    }
}

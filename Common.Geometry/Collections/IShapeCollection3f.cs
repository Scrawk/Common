﻿using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Collections
{
    public interface IStaticShapeCollection3f<T> : IEnumerable<T>
        where T : class, IShape3f
    {
        int Count { get; }

        float SignedDistance(Point3f point);

       T Contains(Point3f point);

        List<T> ToList();
    }

    public interface IShapeCollection3f<T> : IStaticShapeCollection3f<T>
        where T : class, IShape3f
    {

        void Clear();

        void Add(IEnumerable<T> shapes);

        void Add(T shape);

        bool Remove(T shape);

    }
}

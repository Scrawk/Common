using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{
    public interface IStaticShapeCollection3f : IEnumerable<IShape3f>
    {
        int Count { get; }

        float SignedDistance(Vector3f point);

        bool ContainsPoint(Vector3f point);

        List<IShape3f> ToList();
    }

    public interface IShapeCollection3f : IStaticShapeCollection3f
    {

        void Clear();

        void Add(IEnumerable<IShape3f> shapes);

        void Add(IShape3f shape);

        bool Remove(IShape3f shape);

    }
}

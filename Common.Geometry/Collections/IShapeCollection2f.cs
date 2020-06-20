using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{
    public interface IStaticShapeCollection2f : IEnumerable<IShape2f>
    {
        int Count { get; }

        bool Contains(Vector2f point);

        List<IShape2f> ToList();
    }

    public interface IShapeCollection2f : IStaticShapeCollection2f
    {

        void Clear();

        void Add(IEnumerable<IShape2f> shapes);

        void Add(IShape2f shape);

        bool Remove(IShape2f shape);

    }
}

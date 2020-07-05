using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Collections
{
    public interface IStaticShapeCollection2f<T> : IEnumerable<T>
        where T : class, IShape2f
    {
        int Count { get; }

        bool Contains(Vector2f point);

        List<T> ToList();
    }

    public interface IShapeCollection2f<T> : IStaticShapeCollection2f<T>
        where T : class, IShape2f
    {

        void Clear();

        void Add(IEnumerable<T> shapes);

        void Add(T shape);

        bool Remove(T shape);

    }
}

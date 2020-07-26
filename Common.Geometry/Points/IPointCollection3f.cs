using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection3f<T> : IEnumerable<T>
        where T : IPoint3f
    {
        int Count { get; }

        void Search(Sphere3f region, List<T> points);

        T Closest(T point);

        List<T> ToList();
    }

    public interface IPointCollection3f<T> : IStaticPointCollection3f<T>
        where T : IPoint3f
    {
        void Clear();

        bool Add(IEnumerable<T> points);

        bool Add(T point);

        bool Remove(T point);
    }
}

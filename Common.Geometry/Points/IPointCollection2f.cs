using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection2f<T> : IEnumerable<T>
        where T : IPoint2f
    {

        int Count { get; }

        void Search(Circle2f region, List<T> points);

        T Closest(T point);

        List<T> ToList();

    }

    public interface IPointCollection2f<T> : IStaticPointCollection2f<T>
        where T : IPoint2f
    {

        void Clear();

        bool Add(IEnumerable<T> points);

        bool Add(T point);

        bool Remove(T point);

    }
}

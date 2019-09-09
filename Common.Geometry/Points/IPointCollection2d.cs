using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection2f : IEnumerable<Vector2f>
    {

        int Count { get; }

        void Search(Circle2f region, List<Vector2f> points);

        void Search(Circle2f region, List<int> indices);

        Vector2f Closest(Vector2f point);

        float SignedDistance(Vector2f point);

        List<Vector2f> ToList();

    }

    public interface IPointCollection2f : IStaticPointCollection2f
    {

        void Clear();

        bool Add(IEnumerable<Vector2f> points);

        bool Add(Vector2f point);

        bool Remove(Vector2f point);

    }
}

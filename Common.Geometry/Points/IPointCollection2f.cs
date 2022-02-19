using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection2f : IEnumerable<Point2f>
    {

        int Count { get; }

        void Search(Circle2f region, List<Point2f> points);

        Point2f Closest(Point2f point);

        List<Point2f> ToList();

    }

    public interface IPointCollection2f : IStaticPointCollection2f
    {

        void Clear();

        bool Add(IEnumerable<Point2f> points);

        bool Add(Point2f point);

        bool Remove(Point2f point);

    }
}

using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection3f : IEnumerable<Point3f>
    {
        int Count { get; }

        void Search(Sphere3f region, List<Point3f> points);

        Point3f Closest(Point3f point);

        List<Point3f> ToList();
    }

    public interface IPointCollection3f : IStaticPointCollection3f
    {
        void Clear();

        bool Add(IEnumerable<Point3f> points);

        bool Add(Point3f point);

        bool Remove(Point3f point);
    }
}

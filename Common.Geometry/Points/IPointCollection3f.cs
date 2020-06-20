using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public interface IStaticPointCollection3f : IEnumerable<Vector3f>
    {

        int Count { get; }

        void Search(Sphere3f region, List<Vector3f> points);

        void Search(Sphere3f region, List<int> indices);

        Vector3f Closest(Vector3f point);

        List<Vector3f> ToList();

    }

    public interface IPointCollection3f : IStaticPointCollection3f
    {

        void Clear();

        bool Add(IEnumerable<Vector3f> points);

        bool Add(Vector3f point);

        bool Remove(Vector3f point);

    }
}

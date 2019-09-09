using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{
    public class PoissonPoints2f : IPointCollection2f
    {

        private PointGrid2f m_grid;

        public PoissonPoints2f(float width, float height, float radius)
            : this(new Box2f(0, width, 0, height), radius)
        {

        }

        public PoissonPoints2f(Box2f bounds, float radius)
        {
            m_grid = new PointGrid2f(bounds, radius);
            Radius = radius;
            InitializeAttempts = 100;
            PointAttempts = 30;
        }

        public int InitializeAttempts { get; set; }

        public int PointAttempts { get; set; }

        public float Radius { get; private set; }

        public float Radius2 { get { return Radius * Radius; } }

        public int Count { get { return m_grid.Count; } }

        public Box2f Bounds { get { return m_grid.Bounds; } }

        public override string ToString()
        {
            return string.Format("[PoissonPoints2f: Count={0}, Radius={1}, Bounds={2}]", Count, Radius, Bounds);
        }

        public void Clear()
        {
            m_grid.Clear();
        }

        public bool Add(IEnumerable<Vector2f> points)
        {
            return m_grid.Add(points);
        }

        public bool Add(Vector2f point)
        {
            return m_grid.Add(point);
        }

        public bool Remove(Vector2f point)
        {
            return m_grid.Remove(point);
        }

        public void Search(Circle2f region, List<Vector2f> points)
        {
            m_grid.Search(region, points);
        }

        public void Search(Circle2f region, List<int> indices)
        {
            m_grid.Search(region, indices);
        }

        public float SignedDistance(Vector2f point)
        {
            return m_grid.SignedDistance(point);
        }

        public Vector2f Closest(Vector2f point)
        {
            return m_grid.Closest(point);
        }

        public List<Vector2f> ToList()
        {
            return m_grid.ToList();
        }

        public IEnumerator<Vector2f> GetEnumerator()
        {
            return m_grid.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddBorder(float spacing)
        {
            var min = Bounds.Min;
            var max = Bounds.Max;

            Add(Bounds.Corner00);
            Add(Bounds.Corner10);
            Add(Bounds.Corner11);
            Add(Bounds.Corner01);

            float r = spacing;
            for (float y = min.y + r; y <= max.y - r; y += r)
            {
                Add(new Vector2f(min.x, y));
                Add(new Vector2f(max.x, y));
            }

            for (float x = min.x + r; x <= max.x - r; x += r)
            {
                Add(new Vector2f(x, min.y));
                Add(new Vector2f(x, max.y));
            }
        }

        public void Fill(int seed)
        {
            var rnd = new Random(seed);

            Vector2f initial;
            if (!FindInitialPoint(rnd, out initial)) return;

            List<Vector2f> active = new List<Vector2f>();
            active.Add(initial);
            m_grid.Add(initial);

            while(active.Count > 0)
            {
                int i = rnd.Next(0, active.Count - 1);
                var point = active[i];

                Vector2f next;
                if (FindNextPoint(rnd, point, out next))
                {
                    m_grid.Add(next);
                    active.Add(next);
                }
                else
                {
                    active.Remove(point);
                }
            }
        }

        private bool FindInitialPoint(Random rnd, out Vector2f initial)
        {
            Vector2f min = Bounds.Min;
            Vector2f max = Bounds.Max;
            initial = new Vector2f();

            int k = 0;
            while (k++ < InitializeAttempts)
            {
                Vector2f p = rnd.NextVector2f(min, max);
                Vector2f nearest;

                if (m_grid.Closest(p, out nearest))
                {
                    if (Vector2f.SqrDistance(p, nearest) >= Radius2)
                    {
                        initial = p;
                        return true;
                    }
                }
                else
                {
                    initial = p;
                    return true;
                }
            }

            return false;
        }

        private bool FindNextPoint(Random rnd, Vector2f point, out Vector2f next)
        {
            Vector2f min = Bounds.Min;
            Vector2f max = Bounds.Max;

            int k = 0;
            while(k++ < PointAttempts)
            {
                Vector2f n = rnd.NextVector2f(-1, 1).Normalized;
                Vector2f p = point + n * rnd.NextFloat(Radius, Radius * 2);
                Vector2f nearest;

                p.Clamp(min, max);

                if (m_grid.Closest(p, out nearest))
                {
                    if (Vector2f.SqrDistance(p, nearest) >= Radius2)
                    {
                        next = p;
                        return true;
                    }
                }
                else
                {
                    next = p;
                    return true;
                }
            }

            next = new Vector2f();
            return false;
        }

    }
}

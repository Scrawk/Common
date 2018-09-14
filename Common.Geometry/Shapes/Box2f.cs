using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box2f
    {

        public Vector2f Min;

        public Vector2f Max;

        public Box2f(float min, float max)
        {
            Min = new Vector2f(min);
            Max = new Vector2f(max);
        }

        public Box2f(float minX, float maxX, float minY, float maxY)
        {
            Min = new Vector2f(minX, minY);
            Max = new Vector2f(maxX, maxY);
        }

        public Box2f(Vector2f min, Vector2f max)
        {
            Min = min;
            Max = max;
        }

        public Box2f(Vector2i min, Vector2i max)
        {
            Min = new Vector2f(min.x, min.y);
            Max = new Vector2f(max.x, max.y);
        }

        public Vector2f Center
        {
            get { return (Min + Max) * 0.5f; }
        }

        public Vector2f Size
        {
            get { return new Vector2f(Width, Height); }
        }

        public float Width
        {
            get { return Max.x - Min.x; }
        }

        public float Height
        {
            get { return Max.y - Min.y; }
        }

        public float Area
        {
            get { return (Max.x - Min.x) * (Max.y - Min.y); }
        }

        public override string ToString()
        {
            return string.Format("[Box2f: Min={0}, Max={1}, Width={2}, Height={3}]", Min, Max, Width, Height);
        }

        public void GetCorners(IList<Vector2f> corners)
        {
            corners[0] = new Vector2f(Min.x, Min.y);
            corners[1] = new Vector2f(Min.x, Max.y);
            corners[2] = new Vector2f(Max.x, Max.y);
            corners[3] = new Vector2f(Max.x, Min.y);
        }

        public void GetCornersXZ(IList<Vector3f> corners, float y = 0)
        {
            corners[0] = new Vector3f(Min.x, y, Min.y);
            corners[1] = new Vector3f(Min.x, y, Max.y);
            corners[2] = new Vector3f(Max.x, y, Max.y);
            corners[3] = new Vector3f(Max.x, y, Min.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given point.
        /// </summary>
        public void Enlarge(Vector2f p)
        {
            Min.x = Math.Min(Min.x, p.x);
            Min.y = Math.Min(Min.y, p.y);
            Max.x = Math.Max(Max.x, p.x);
            Max.y = Math.Max(Max.y, p.y);
        }

        /// <summary>
        /// Returns the bounding box containing this box and the given box.
        /// </summary>
        public void Enlarge(Box2f box)
        {
            Min.x = Math.Min(Min.x, box.Min.x);
            Min.y = Math.Min(Min.y, box.Min.y);
            Max.x = Math.Max(Max.x, box.Max.x);
            Max.y = Math.Max(Max.y, box.Max.y);
        }

        public static Box2f CalculateBounds(IList<Vector2f> vertices)
        {
            Vector2f min = Vector2f.PositiveInfinity;
            Vector2f max = Vector2f.NegativeInfinity;

            int count = vertices.Count;
            for (int i = 0; i < count; i++)
            {
                Vector2f v = vertices[i];

                if (v.x < min.x) min.x = v.x;
                if (v.y < min.y) min.y = v.y;

                if (v.x > max.x) max.x = v.x;
                if (v.y > max.y) max.y = v.y;
            }

            return new Box2f(min, max);
        }

        public static Box2f CalculateBounds(Vector2f a, Vector2f b)
        {
            float xmin = Math.Min(a.x, b.x);
            float xmax = Math.Max(a.x, b.x);
            float ymin = Math.Min(a.y, b.y);
            float ymax = Math.Max(a.y, b.y);

            return new Box2f(xmin, xmax, ymin, ymax);
        }

    }

}


















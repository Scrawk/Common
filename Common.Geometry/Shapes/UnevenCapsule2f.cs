using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct UnevenCapsule2f : IEquatable<UnevenCapsule2f>, IShape2f
    {
        public Vector2f A;

        public Vector2f B;

        public float RadiusA;

        public float RadiusB;

        public UnevenCapsule2f(Vector2f a, Vector2f b, float radiusA, float radiusB)
        {
            A = a;
            B = b;
            RadiusA = radiusA;
            RadiusB = radiusB;
        }

        public UnevenCapsule2f(float ax, float ay, float bx, float by, float radiusA, float radiusB)
        {
            A = new Vector2f(ax, ay);
            B = new Vector2f(bx, by);
            RadiusA = radiusA;
            RadiusB = radiusB;
        }

        /// <summary>
        /// The center position of the capsule.
        /// </summary>
        public Vector2f Center
        {
            get { return (A + B) * 0.5f; }
        }

        /// <summary>
        /// Calculate the bounding box.
        /// </summary>
        public Box2f Bounds
        {
            get
            {
                float xmin = Math.Min(A.x - RadiusA, B.x - RadiusB);
                float xmax = Math.Max(A.x + RadiusA, B.x + RadiusB);
                float ymin = Math.Min(A.y - RadiusA, B.y - RadiusB);
                float ymax = Math.Max(A.y + RadiusA, B.y + RadiusB);

                return new Box2f(xmin, xmax, ymin, ymax);
            }
        }

        public static bool operator ==(UnevenCapsule2f c1, UnevenCapsule2f c2)
        {
            return c1.RadiusA == c2.RadiusA && c1.RadiusB == c2.RadiusB && c1.A == c2.A && c1.B == c2.B;
        }

        public static bool operator !=(UnevenCapsule2f c1, UnevenCapsule2f c2)
        {
            return c1.RadiusA != c2.RadiusA || c1.RadiusB != c2.RadiusB || c1.A != c2.A || c1.A != c2.A;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UnevenCapsule2f)) return false;
            UnevenCapsule2f cap = (UnevenCapsule2f)obj;
            return this == cap;
        }

        public bool Equals(UnevenCapsule2f cap)
        {
            return this == cap;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ RadiusA.GetHashCode();
                hash = (hash * 16777619) ^ RadiusB.GetHashCode();
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ToString()
        {
            return string.Format("[Capsule2f: A={0}, B={1}, RadiusA={2}, RadiusB={2}]", A, B, RadiusA, RadiusB);
        }

        /// <summary>
        /// Does the capsule contain the point.
        /// </summary>
        public bool Contains(Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The closest point to the surface of the capsule.
        /// </summary>
        public Vector2f Closest(Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the capsule intersect with the box.
        /// </summary>
        public bool Intersects(Box2f box)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The signed distance to the point.
        /// </summary>
        public float SignedDistance(Vector2f p)
        {
            Vector2f pa = p - A, ba = B - A;
            float h = MathUtil.Clamp01(Vector2f.Dot(pa, ba) / Vector2f.Dot(ba, ba));
            return (pa - ba * h).Magnitude - MathUtil.Lerp(RadiusA, RadiusB, h);
        }

    }
}
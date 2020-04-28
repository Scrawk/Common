using System;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment3f : IEquatable<Segment3f>, IShape3f
    {

        public Vector3f A;

        public Vector3f B;

        public Segment3f(Vector3f a, Vector3f b)
        {
            A = a;
            B = b;
        }

        public Segment3f(float ax, float ay, float az, float bx, float by, float bz)
        {
            A = new Vector3f(ax, ay, az);
            B = new Vector3f(bx, by, bz);
        }

        public Vector3f Center
        {
            get { return (A + B) * 0.5f; }
        }

        public float Length
        {
            get { return Vector3f.Distance(A, B); }
        }

        public float SqrLength
        {
            get { return Vector3f.SqrDistance(A, B); }
        }

        public Box3f Bounds
        {
            get
            {
                float xmin = Math.Min(A.x, B.x);
                float xmax = Math.Max(A.x, B.x);
                float ymin = Math.Min(A.y, B.y);
                float ymax = Math.Max(A.y, B.y);
                float zmin = Math.Min(A.z, B.z);
                float zmax = Math.Max(A.z, B.z);

                return new Box3f(xmin, xmax, ymin, ymax, zmin, zmax);
            }
        }

        unsafe public Vector3f this[int i]
        {
            get
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment3f index out of range.");

                fixed (Segment3f* array = &this) { return ((Vector3f*)array)[i]; }
            }
            set
            {
                if ((uint)i >= 2)
                    throw new IndexOutOfRangeException("Segment3f index out of range.");

                fixed (Vector3f* array = &A) { array[i] = value; }
            }
        }

        public static Segment3f operator +(Segment3f seg, float s)
        {
            return new Segment3f(seg.A + s, seg.B + s);
        }

        public static Segment3f operator +(Segment3f seg, Vector3f v)
        {
            return new Segment3f(seg.A + v, seg.B + v);
        }

        public static Segment3f operator -(Segment3f seg, float s)
        {
            return new Segment3f(seg.A - s, seg.B - s);
        }

        public static Segment3f operator -(Segment3f seg, Vector3f v)
        {
            return new Segment3f(seg.A - v, seg.B - v);
        }

        public static Segment3f operator *(Segment3f seg, float s)
        {
            return new Segment3f(seg.A * s, seg.B * s);
        }

        public static Segment3f operator *(Segment3f seg, Vector3f v)
        {
            return new Segment3f(seg.A * v, seg.B * v);
        }

        public static Segment3f operator /(Segment3f seg, float s)
        {
            return new Segment3f(seg.A / s, seg.B / s);
        }

        public static Segment3f operator /(Segment3f seg, Vector3f v)
        {
            return new Segment3f(seg.A / v, seg.B / v);
        }

        public static Segment3f operator *(Segment3f seg, Matrix3x3f m)
        {
            return new Segment3f(m * seg.A, m * seg.B);
        }

        public static bool operator ==(Segment3f s1, Segment3f s2)
        {
            return s1.A == s2.A && s1.B == s2.B;
        }

        public static bool operator !=(Segment3f s1, Segment3f s2)
        {
            return s1.A != s2.A || s1.B != s2.B;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Segment3f)) return false;
            Segment3f seg = (Segment3f)obj;
            return this == seg;
        }

        public bool Equals(Segment3f seg)
        {
            return this == seg;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ A.GetHashCode();
                hash = (hash * 16777619) ^ B.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Segment3f: A={0}, B={1}]", A, B);
        }

        /// <summary>
        /// Does the point line on the segemnts.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool Contains(Vector3f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the segment intersect this box.
        /// </summary>
        public bool Intersects(Box3f box)
        {
            Vector3f c = box.Center;
            Vector3f e = box.Max - c; //Box half length extents
            Vector3f m = Center;
            Vector3f d = B - m; //Segment halflength vector.
            m = m - c; //translate box and segment to origin.

            //try world coordinate axes as seperating axes.
            float adx = Math.Abs(d.x);
            if (Math.Abs(m.x) > e.x + adx) return false;
            float ady = Math.Abs(d.y);
            if (Math.Abs(m.y) > e.y + ady) return false;
            float adz = Math.Abs(d.z);
            if (Math.Abs(m.z) > e.z + adz) return false;

            //add in an epsilon term to counteract arithmetic errors 
            //when segment is near parallel to a coordinate axis.
            adx += FMath.EPS;
            ady += FMath.EPS;
            adz += FMath.EPS;

            if (Math.Abs(m.y * d.z - m.z * d.y) > e.y * adz + e.z * ady) return false;
            if (Math.Abs(m.z * d.x - m.x * d.z) > e.x * adz + e.z * adx) return false;
            if (Math.Abs(m.x * d.y - m.y * d.x) > e.x * ady + e.y * adx) return false;

            return true;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        public Vector3f Closest(Vector3f p)
        {
            float t;
            Closest(p, out t);
            return A + (B - A) * t;
        }

        /// <summary>
        /// The closest point on segment to point.
        /// </summary>
        /// <param name="p">point</param>
        /// <param name="t">closest point = A + t * (B - A)</param>
        public void Closest(Vector3f p, out float t)
        {
            t = 0.0f;
            Vector3f ab = B - A;
            Vector3f ap = p - A;

            float len = ab.x * ab.x + ab.y * ab.y;
            if (len < FMath.EPS) return;

            t = (ab.x * ap.x + ab.y * ap.y) / len;
            t = FMath.Clamp01(t);
        }

        /// <summary>
        /// Return the signed distance to the point. 
        /// Always positive.
        /// </summary>
        public float SignedDistance(Vector3f p)
        {
            return Vector3f.Distance(Closest(p), p);
        }

    }
}


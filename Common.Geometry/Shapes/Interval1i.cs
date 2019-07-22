using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Interval1i : IEquatable<Interval1i>
    {
        public int Min;

        public int Max;

        public Interval1i(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public Interval1i(Vector2i v)
        {
            Min = v.x;
            Max = v.y;
        }

        public int Length
        {
            get { return Max - Min; }
        }

        public int SqrLength
        {
            get { return (Min - Max) * (Min - Max); }
        }

        public double Center
        {
            get { return (Max + Min) * 0.5; }
        }

        public bool IsConstant
        {
            get { return Max == Min; }
        }

        public static Interval1i operator +(Interval1i a, int f)
        {
            return new Interval1i(a.Min + f, a.Max + f);
        }

        public static Interval1i operator -(Interval1i a, int f)
        {
            return new Interval1i(a.Min - f, a.Max - f);
        }

        public static Interval1i operator *(Interval1i a, int f)
        {
            return new Interval1i(a.Min * f, a.Max * f);
        }

        public static explicit operator Interval1i(Interval1d i)
        {
            return new Interval1i((int)i.Min, (int)i.Max);
        }

        public static explicit operator Interval1i(Interval1f i)
        {
            return new Interval1i((int)i.Min, (int)i.Max);
        }

        public static bool operator ==(Interval1i i1, Interval1i i2)
        {
            return i1.Min == i2.Min && i1.Max == i2.Max;
        }

        public static bool operator !=(Interval1i i1, Interval1i i2)
        {
            return i1.Min != i2.Min || i1.Max != i2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Interval1i)) return false;
            Interval1i box = (Interval1i)obj;
            return this == box;
        }

        public bool Equals(Interval1i box)
        {
            return this == box;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ Min.GetHashCode();
                hash = (hash * 16777619) ^ Max.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("[Interval1i: a={0}, b={1}]", Min, Max);
        }

        public int Clamp(int f)
        {
            return (f < Min) ? Min : (f > Max) ? Max : f;
        }

        public double Interpolate(double t)
        {
            if (t < 0.0f) t = 0.0;
            if (t > 1.0f) t = 1.0;
            return (1 - t) * Min + (t) * Max;
        }

        public void Enlarge(int d)
        {
            if (d < Min) Min = d;
            if (d > Max) Max = d;
        }

        public void Enlarge(Interval1i i)
        {
            Min = Math.Min(Min, i.Min);
            Max = Math.Max(Max, i.Max);
        }

        public bool Intersects(Interval1i i)
        {
            return !(i.Min > Max || i.Max < Min);
        }

        public bool Contains(int d)
        {
            return d >= Min && d <= Max;
        }

        public static bool Intersection(Interval1i i1, Interval1i i2, out Interval1i i)
        {
            if (i2.Min > i1.Max || i2.Max < i1.Min)
            {
                i = new Interval1i(0,0);
                return false;
            }
            else
            {
                i = new Interval1i(Math.Max(i1.Min, i2.Min), Math.Min(i1.Max, i2.Max));
                return true;
            }
        }

        public static int Distance(Interval1i i1, Interval1i i2)
        {
            if (i1.Max < i2.Min)
                return i2.Min - i1.Max;
            else if (i1.Min > i2.Max)
                return i1.Min - i2.Max;
            else
                return 0;
        }

        public static int SqrDistance(Interval1i i1, Interval1i i2)
        {
            if (i1.Max < i2.Min)
                return (i2.Min - i1.Max) * (i2.Min - i1.Max);
            else if (i1.Min > i2.Max)
                return (i1.Min - i2.Max) * (i1.Min - i2.Max);
            else
                return 0;
        }

        public static Interval1i CalculateInterval(IEnumerable<int> indices)
        {
            int min = int.MaxValue;
            int max = int.MinValue;

            foreach (var i in indices)
            {
                if (i < min) min = i;
                if (i > max) max = i;
            }

            return new Interval1i(min, max);
        }

    }
}

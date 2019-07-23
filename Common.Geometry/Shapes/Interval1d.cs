﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Numerics;

using REAL = System.Double;
using VECTOR2 = Common.Core.Numerics.Vector2d;

namespace Common.Geometry.Shapes
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Interval1d : IEquatable<Interval1d>
    {
        public REAL Min;

        public REAL Max;

        public Interval1d(REAL min, REAL max)
        {
            Min = min;
            Max = max;
        }

        public Interval1d(VECTOR2 v)
        {
            Min = v.x;
            Max = v.y;
        }

        public REAL Length
        {
            get { return Max - Min; }
        }

        public REAL SqrLength
        {
            get { return (Min - Max) * (Min - Max); }
        }

        public REAL Center
        {
            get { return (Max + Min) * 0.5; }
        }

        public bool IsConstant
        {
            get { return Max == Min; }
        }

        public static Interval1d operator +(Interval1d a, REAL f)
        {
            return new Interval1d(a.Min + f, a.Max + f);
        }

        public static Interval1d operator -(Interval1d a, REAL f)
        {
            return new Interval1d(a.Min - f, a.Max - f);
        }

        public static Interval1d operator *(Interval1d a, REAL f)
        {
            return new Interval1d(a.Min * f, a.Max * f);
        }

        public static implicit operator Interval1d(Interval1f i)
        {
            return new Interval1d(i.Min, i.Max);
        }

        public static implicit operator Interval1d(Interval1i i)
        {
            return new Interval1d(i.Min, i.Max);
        }

        public static bool operator ==(Interval1d i1, Interval1d i2)
        {
            return i1.Min == i2.Min && i1.Max == i2.Max;
        }

        public static bool operator !=(Interval1d i1, Interval1d i2)
        {
            return i1.Min != i2.Min || i1.Max != i2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Interval1d)) return false;
            Interval1d box = (Interval1d)obj;
            return this == box;
        }

        public bool Equals(Interval1d box)
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
            return string.Format("[Interval1d: a={0}, b={1}]", Min, Max);
        }

        public REAL Clamp(REAL f)
        {
            return (f < Min) ? Min : (f > Max) ? Max : f;
        }

        public REAL Interpolate(REAL t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
            return (1 - t) * Min + (t) * Max;
        }

        public void Enlarge(REAL d)
        {
            if (d < Min) Min = d;
            if (d > Max) Max = d;
        }

        public void Enlarge(Interval1d i)
        {
            Min = Math.Min(Min, i.Min);
            Max = Math.Max(Max, i.Max);
        }

        public bool Intersects(Interval1d i)
        {
            return !(i.Min > Max || i.Max < Min);
        }

        public bool Contains(REAL d)
        {
            return d >= Min && d <= Max;
        }

        public static bool Intersection(Interval1d i1, Interval1d i2, out Interval1d i)
        {
            if (i2.Min > i1.Max || i2.Max < i1.Min)
            {
                i = new Interval1d(0, 0);
                return false;
            }
            else
            {
                i = new Interval1d(Math.Max(i1.Min, i2.Min), Math.Min(i1.Max, i2.Max));
                return true;
            }
        }

        public static REAL Distance(Interval1d i1, Interval1d i2)
        {
            if (i1.Max < i2.Min)
                return i2.Min - i1.Max;
            else if (i1.Min > i2.Max)
                return i1.Min - i2.Max;
            else
                return 0;
        }

        public static REAL SqrDistance(Interval1d i1, Interval1d i2)
        {
            if (i1.Max < i2.Min)
                return (i2.Min - i1.Max) * (i2.Min - i1.Max);
            else if (i1.Min > i2.Max)
                return (i1.Min - i2.Max) * (i1.Min - i2.Max);
            else
                return 0;
        }

        public static Interval1d CalculateInterval(IEnumerable<REAL> indices)
        {
            REAL min = REAL.PositiveInfinity;
            REAL max = REAL.NegativeInfinity;

            foreach (var i in indices)
            {
                if (i < min) min = i;
                if (i > max) max = i;
            }

            return new Interval1d(min, max);
        }

    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.LinearAlgebra;

namespace Common.Geometry.Shapes
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Interval1f : IEquatable<Interval1f>
    {
        public float Min;

        public float Max;

        public Interval1f(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public Interval1f(Vector2i v)
        {
            Min = v.x;
            Max = v.y;
        }

        public float Length
        {
            get { return Max - Min; }
        }

        public float SqrLength
        {
            get { return (Min - Max) * (Min - Max); }
        }

        public float Center
        {
            get { return (Max + Min) * 0.5f; }
        }

        public bool IsConstant
        {
            get { return Max == Min; }
        }

        public static Interval1f operator +(Interval1f a, float f)
        {
            return new Interval1f(a.Min + f, a.Max + f);
        }

        public static Interval1f operator -(Interval1f a, float f)
        {
            return new Interval1f(a.Min - f, a.Max - f);
        }

        public static Interval1f operator *(Interval1f a, float f)
        {
            return new Interval1f(a.Min * f, a.Max * f);
        }

        public static explicit operator Interval1f(Interval1d i)
        {
            return new Interval1f((float)i.Min, (float)i.Max);
        }

        public static implicit operator Interval1f(Interval1i i)
        {
            return new Interval1f(i.Min, i.Max);
        }

        public static bool operator ==(Interval1f i1, Interval1f i2)
        {
            return i1.Min == i2.Min && i1.Max == i2.Max;
        }

        public static bool operator !=(Interval1f i1, Interval1f i2)
        {
            return i1.Min != i2.Min || i1.Max != i2.Max;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Interval1f)) return false;
            Interval1f box = (Interval1f)obj;
            return this == box;
        }

        public bool Equals(Interval1f box)
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
            return string.Format("[Interval1f: a={0}, b={1}]", Min, Max);
        }

        public float Clamp(float f)
        {
            return (f < Min) ? Min : (f > Max) ? Max : f;
        }

        public float Interpolate(float t)
        {
            if (t < 0.0f) t = 0.0f;
            if (t > 1.0f) t = 1.0f;
            return (1 - t) * Min + (t) * Max;
        }

        public void Enlarge(float d)
        {
            if (d < Min) Min = d;
            if (d > Max) Max = d;
        }

        public void Enlarge(Interval1f i)
        {
            Min = Math.Min(Min, i.Min);
            Max = Math.Max(Max, i.Max);
        }

        public bool Intersects(Interval1f i)
        {
            return !(i.Min > Max || i.Max < Min);
        }

        public bool Contains(float d)
        {
            return d >= Min && d <= Max;
        }

        public static bool Intersection(Interval1f i1, Interval1f i2, out Interval1f i)
        {
            if (i2.Min > i1.Max || i2.Max < i1.Min)
            {
                i = new Interval1f(0, 0);
                return false;
            }
            else
            {
                i = new Interval1f(Math.Max(i1.Min, i2.Min), Math.Min(i1.Max, i2.Max));
                return true;
            }
        }

        public static float Distance(Interval1f i1, Interval1f i2)
        {
            if (i1.Max < i2.Min)
                return i2.Min - i1.Max;
            else if (i1.Min > i2.Max)
                return i1.Min - i2.Max;
            else
                return 0;
        }

        public static float SqrDistance(Interval1f i1, Interval1f i2)
        {
            if (i1.Max < i2.Min)
                return (i2.Min - i1.Max) * (i2.Min - i1.Max);
            else if (i1.Min > i2.Max)
                return (i1.Min - i2.Max) * (i1.Min - i2.Max);
            else
                return 0;
        }

        public static Interval1f CalculateInterval(IEnumerable<float> indices)
        {
            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;

            foreach (var i in indices)
            {
                if (i < min) min = i;
                if (i > max) max = i;
            }

            return new Interval1f(min, max);
        }

    }
}
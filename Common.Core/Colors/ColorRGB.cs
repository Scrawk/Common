using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Mathematics;

namespace Common.Core.Colors
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRGB : IEquatable<ColorRGB>
    {

        public readonly static ColorRGB Red = new ColorRGB(1, 0, 0);
        public readonly static ColorRGB Yellow = new ColorRGB(1, 1, 0);
        public readonly static ColorRGB Green = new ColorRGB(0, 1, 0);
        public readonly static ColorRGB Cyan = new ColorRGB(0, 1, 1);
        public readonly static ColorRGB Blue = new ColorRGB(0, 0, 1);
        public readonly static ColorRGB Magenta = new ColorRGB(1, 0, 1);
        public readonly static ColorRGB Black = new ColorRGB(0, 0, 0);
        public readonly static ColorRGB White = new ColorRGB(1, 1, 1);

        public float r, g, b;

        public ColorRGB(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public ColorRGB(float v) : this(v,v,v)
        {

        }

        public ColorRGB(double r, double g, double b) 
            : this((float)r, (float)g, (float)b)
        {

        }

        public ColorRGB(double v) : this(v, v, v)
        {

        }

        public ColorRGB rrr
        {
            get { return new ColorRGB(r, r, r); }
        }

        public ColorRGB bgr
        {
            get { return new ColorRGB(b, g, r); }
        }

        public ColorHSV hsv
        {
            get { return ToHSV(r, g, b); }
        }

        public ColorRGBA rgb1
        {
            get { return new ColorRGBA(r, g, b, 1); }
        }

        public float Magnitude
        {
            get { return (float)Math.Sqrt(SqrMagnitude); }
        }

        public float SqrMagnitude
        {
            get { return (r * r + g * g + b * b); }
        }

        public float Intensity 
        { 
            get { return (r + g + b) / 3.0f; } 
        }

        public float Luminance 
        { 
            get { return 0.2126f * r + 0.7152f * g + 0.0722f * b; } 
        }

        public int Integer
        {
            get
            {
                int R = (int)FMath.Clamp(r * 255.0f, 0.0f, 255.0f);
                int G = (int)FMath.Clamp(g * 255.0f, 0.0f, 255.0f);
                int B = (int)FMath.Clamp(b * 255.0f, 0.0f, 255.0f);
                int A = 255;

                return R | (G << 8) | (B << 16) | (A << 24);
            }
        }

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return r;
                    case 1: return g;
                    case 2: return b;
                    default: throw new IndexOutOfRangeException("ColorRGB index out of range: " + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    default: throw new IndexOutOfRangeException("ColorRGB index out of range: " + i);
                }
            }
        }

        /// <summary>
        /// Add two colors.
        /// </summary>
        public static ColorRGB operator +(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r + v2.r, v1.g + v2.g, v1.b + v2.b);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static ColorRGB operator +(ColorRGB v1, float s)
        {
            return new ColorRGB(v1.r + s, v1.g + s, v1.b + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static ColorRGB operator +(float s, ColorRGB v1)
        {
            return new ColorRGB(v1.r + s, v1.g + s, v1.b + s);
        }

        /// <summary>
        /// Subtract two colors.
        /// </summary>
        public static ColorRGB operator -(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r - v2.r, v1.g - v2.g, v1.b - v2.b);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static ColorRGB operator -(ColorRGB v1, float s)
        {
            return new ColorRGB(v1.r - s, v1.g - s, v1.b - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static ColorRGB operator -(float s, ColorRGB v1)
        {
            return new ColorRGB(v1.r - s, v1.g - s, v1.b - s);
        }

        /// <summary>
        /// Multiply two colors.
        /// </summary>
        public static ColorRGB operator *(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r * v2.r, v1.g * v2.g, v1.b * v2.b);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static ColorRGB operator *(ColorRGB v, float s)
        {
            return new ColorRGB(v.r * s, v.g * s, v.b * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static ColorRGB operator *(float s, ColorRGB v)
        {
            return new ColorRGB(v.r * s, v.g * s, v.b * s);
        }

        /// <summary>
        /// Divide two colors.
        /// </summary>
        public static ColorRGB operator /(ColorRGB v1, ColorRGB v2)
        {
            return new ColorRGB(v1.r / v2.r, v1.g / v2.g, v1.b / v2.b);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        public static ColorRGB operator /(ColorRGB v, float s)
        {
            return new ColorRGB(v.r / s, v.g / s, v.b / s);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public static bool operator ==(ColorRGB v1, ColorRGB v2)
        {
            return (v1.r == v2.r && v1.g == v2.g && v1.b == v2.b);
        }

        /// <summary>
        /// Are these colors not equal.
        /// </summary>
        public static bool operator !=(ColorRGB v1, ColorRGB v2)
        {
            return (v1.r != v2.r || v1.g != v2.g || v1.b != v2.b);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorRGB)) return false;

            ColorRGB v = (ColorRGB)obj;

            return this == v;
        }

        /// <summary>
        /// Are these colors equal given the error.
        /// </summary>
        public bool EqualsWithError(ColorRGB v, float eps)
        {
            if (Math.Abs(r - v.r) > eps) return false;
            if (Math.Abs(g - v.g) > eps) return false;
            if (Math.Abs(b - v.b) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public bool Equals(ColorRGB v)
        {
            return this == v;
        }

        /// <summary>
        /// colors hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = (hash * 16777619) ^ r.GetHashCode();
                hash = (hash * 16777619) ^ g.GetHashCode();
                hash = (hash * 16777619) ^ b.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// color as a string.
        /// </summary>
		public override string ToString()
        {
            return string.Format("{0},{1},{2}", r, g, b);
        }

        /// <summary>
        /// color as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2}", r.ToString(f), g.ToString(f), b.ToString(f));
        }

        /// <summary>
        /// Vector from a string.
        /// </summary>
        static public ColorRGB FromString(string s)
        {
            ColorRGB v = new ColorRGB();
            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.r = float.Parse(result[0]);
                v.g = float.Parse(result[1]);
                v.b = float.Parse(result[2]);
            }
            catch { }

            return v;
        }

        /// <summary>
        /// The minimum value between s and each component in vector.
        /// </summary>
        public void Min(float s)
        {
            r = Math.Min(r, s);
            g = Math.Min(g, s);
            b = Math.Min(b, s);
        }

        /// <summary>
        /// The maximum value between s and each component in vector.
        /// </summary>
        public void Max(float s)
        {
            r = Math.Max(r, s);
            g = Math.Max(g, s);
            b = Math.Max(b, s);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        public void Clamp(float min, float max)
        {
            r = Math.Max(Math.Min(r, max), min);
            g = Math.Max(Math.Min(g, max), min);
            b = Math.Max(Math.Min(b, max), min);
        }

        /// <summary>
        /// Lerp between two vectors.
        /// </summary>
        public static ColorRGB Lerp(ColorRGB v1, ColorRGB v2, float a)
        {
            float a1 = 1.0f - a;
            ColorRGB v = new ColorRGB();
            v.r = v1.r * a1 + v2.r * a;
            v.g = v1.g * a1 + v2.g * a;
            v.b = v1.b * a1 + v2.b * a;
            return v;
        }

        /// <summary>
        /// Convert to HSV color space.
        /// </summary>
        public static ColorHSV ToHSV(float r, float g, float b)
        {
            float delta, min;
            float h = 0, s, v;

            float R = FMath.Clamp(r * 255.0f, 0.0f, 255.0f);
            float G = FMath.Clamp(g * 255.0f, 0.0f, 255.0f);
            float B = FMath.Clamp(b * 255.0f, 0.0f, 255.0f);

            min = Math.Min(Math.Min(R, G), B);
            v = Math.Max(Math.Max(R, G), B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0f;
            else
            {
                if (R == v)
                    h = (G - B) / delta;
                else if (G == v)
                    h = 2 + (B - R) / delta;
                else if (B == v)
                    h = 4 + (R - G) / delta;

                h *= 60;
                if (h < 0.0)
                    h = h + 360;
            }

            return new ColorHSV(h / 360.0f, s, v / 255.0f);
        }



    }

}

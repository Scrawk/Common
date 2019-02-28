using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Mathematics;

namespace Common.Core.Colors
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorRGBA : IEquatable<ColorRGBA>
    {

        public readonly static ColorRGBA Red = new ColorRGBA(1, 0, 0, 1);
        public readonly static ColorRGBA Yellow = new ColorRGBA(1, 1, 0, 1);
        public readonly static ColorRGBA Green = new ColorRGBA(0, 1, 0, 1);
        public readonly static ColorRGBA Cyan = new ColorRGBA(0, 1, 1, 1);
        public readonly static ColorRGBA Blue = new ColorRGBA(0, 0, 1, 1);
        public readonly static ColorRGBA Magenta = new ColorRGBA(1, 0, 1, 1);
        public readonly static ColorRGBA Black = new ColorRGBA(0, 0, 0, 1);
        public readonly static ColorRGBA White = new ColorRGBA(1, 1, 1, 1);
        public readonly static ColorRGBA Clear = new ColorRGBA(0, 0, 0, 0);

        public float r, g, b, a;

        public ColorRGBA(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public ColorRGBA(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1.0f;
        }

        public ColorRGBA(float v)
        {
            this.r = v;
            this.g = v;
            this.b = v;
            this.a = v;
        }

        public ColorRGBA(float v, float a)
        {
            this.r = v;
            this.g = v;
            this.b = v;
            this.a = a;
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
                    case 3: return a;
                    default: throw new IndexOutOfRangeException("ColorRGBA index out of range: " + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0: r = value; break;
                    case 1: g = value; break;
                    case 2: b = value; break;
                    case 3: a = value; break;
                    default: throw new IndexOutOfRangeException("ColorRGBA index out of range: " + i);
                }
            }
        }

        public ColorRGB rgb
        {
            get { return new ColorRGB(r, g, b); }
        }

        public ColorHSV hsv
        {
            get { return ColorRGB.ToHSV(r, g, b); }
        }

        public float Magnitude
        {
            get { return (float)Math.Sqrt(SqrMagnitude); }
        }

        public float SqrMagnitude
        {
            get { return (r * r + g * g + b * b + a*a); }
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
                int A = (int)FMath.Clamp(a * 255.0f, 0.0f, 255.0f);

                return R | (G << 8) | (B << 16) | (A << 24);
            }
        }

        /// <summary>
        /// Add two colors.
        /// </summary>
        public static ColorRGBA operator +(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r + v2.r, v1.g + v2.g, v1.b + v2.b, v1.a + v2.a);
        }

        /// <summary>
        /// Add color and scalar.
        /// </summary>
        public static ColorRGBA operator +(ColorRGBA v1, float s)
        {
            return new ColorRGBA(v1.r + s, v1.g + s, v1.b + s, v1.a + s);
        }

        /// <summary>
        /// Add color and scalar.
        /// </summary>
        public static ColorRGBA operator +(float s, ColorRGBA v1)
        {
            return new ColorRGBA(v1.r + s, v1.g + s, v1.b + s, v1.a + s);
        }

        /// <summary>
        /// Subtract two colors.
        /// </summary>
        public static ColorRGBA operator -(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r - v2.r, v1.g - v2.g, v1.b - v2.b, v1.a - v2.a);
        }

        /// <summary>
        /// Subtract color and scalar.
        /// </summary>
        public static ColorRGBA operator -(ColorRGBA v1, float s)
        {
            return new ColorRGBA(v1.r - s, v1.g - s, v1.b - s, v1.a - s);
        }

        /// <summary>
        /// Subtract color and scalar.
        /// </summary>
        public static ColorRGBA operator -(float s, ColorRGBA v1)
        {
            return new ColorRGBA(v1.r - s, v1.g - s, v1.b - s, v1.a - s);
        }

        /// <summary>
        /// Multiply two colors.
        /// </summary>
        public static ColorRGBA operator *(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r * v2.r, v1.g * v2.g, v1.b * v2.b, v1.a * v2.a);
        }

        /// <summary>
        /// Multiply a color and a scalar.
        /// </summary>
        public static ColorRGBA operator *(ColorRGBA v, float s)
        {
            return new ColorRGBA(v.r * s, v.g * s, v.b * s, v.a * s);
        }

        /// <summary>
        /// Multiply a color and a scalar.
        /// </summary>
        public static ColorRGBA operator *(float s, ColorRGBA v)
        {
            return new ColorRGBA(v.r * s, v.g * s, v.b * s, v.a * s);
        }

        /// <summary>
        /// Divide two colors.
        /// </summary>
        public static ColorRGBA operator /(ColorRGBA v1, ColorRGBA v2)
        {
            return new ColorRGBA(v1.r / v2.r, v1.g / v2.g, v1.b / v2.b, v1.a / v2.a);
        }

        /// <summary>
        /// Divide a color and a scalar.
        /// </summary>
        public static ColorRGBA operator /(ColorRGBA v, float s)
        {
            return new ColorRGBA(v.r / s, v.g / s, v.b / s, v.a / s);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public static bool operator ==(ColorRGBA v1, ColorRGBA v2)
        {
            return (v1.r == v2.r && v1.g == v2.g && v1.b == v2.b && v1.a == v2.a);
        }

        /// <summary>
        /// Are these colors not equal.
        /// </summary>
        public static bool operator !=(ColorRGBA v1, ColorRGBA v2)
        {
            return (v1.r != v2.r || v1.g != v2.g || v1.b != v2.b || v1.a != v2.a);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorRGBA)) return false;

            ColorRGBA v = (ColorRGBA)obj;

            return this == v;
        }

        /// <summary>
        /// Are these colors equal given the error.
        /// </summary>
        public bool EqualsWithError(ColorRGBA v, float eps)
        {
            if (Math.Abs(r - v.r) > eps) return false;
            if (Math.Abs(g - v.g) > eps) return false;
            if (Math.Abs(b - v.b) > eps) return false;
            if (Math.Abs(a - v.a) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public bool Equals(ColorRGBA v)
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
                hash = (hash * 16777619) ^ a.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// color as a string.
        /// </summary>
		public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", r,g,b,a);
        }

        /// <summary>
        /// color as a string.
        /// </summary>
        public string ToString(string f)
        {
            return string.Format("{0},{1},{2},{3}", r.ToString(f), g.ToString(f), b.ToString(f), a.ToString(f));
        }

        /// <summary>
        /// color from a string.
        /// </summary>
        public static ColorRGBA FromString(string s)
        {
            ColorRGBA v = new ColorRGBA();
            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                v.r = float.Parse(result[0]);
                v.g = float.Parse(result[1]);
                v.b = float.Parse(result[2]);
                v.a = float.Parse(result[3]);
            }
            catch { }

            return v;
        }

        /// <summary>
        /// The minimum value between s and each component in color.
        /// </summary>
        public void Min(float s)
        {
            r = Math.Min(r, s);
            g = Math.Min(g, s);
            b = Math.Min(b, s);
            a = Math.Min(a, s);
        }

        /// <summary>
        /// The maximum value between s and each component in color.
        /// </summary>
        public void Max(float s)
        {
            r = Math.Max(r, s);
            g = Math.Max(g, s);
            b = Math.Max(b, s);
            a = Math.Max(a, s);
        }

        /// <summary>
        /// Clamp the each component to specified min and max.
        /// </summary>
        public void Clamp(float min, float max)
        {
            r = Math.Max(Math.Min(r, max), min);
            g = Math.Max(Math.Min(g, max), min);
            b = Math.Max(Math.Min(b, max), min);
            a = Math.Max(Math.Min(a, max), min);
        }

        /// <summary>
        /// Lerp between two colors.
        /// </summary>
        public static ColorRGBA Lerp(ColorRGBA v1, ColorRGBA v2, float a)
        {
            float a1 = 1.0f - a;
            ColorRGBA v = new ColorRGBA();
            v.r = v1.r * a1 + v2.r * a;
            v.g = v1.g * a1 + v2.g * a;
            v.b = v1.b * a1 + v2.b * a;
            v.a = v1.a * a1 + v2.a * a;
            return v;
        }

        /// <summary>
        /// Convert to HSV color space.
        /// </summary>
        public ColorHSV ToHSV()
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

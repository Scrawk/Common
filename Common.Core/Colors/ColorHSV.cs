using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Common.Core.Mathematics;

namespace Common.Core.Colors
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorHSV
    {

        public readonly static ColorHSV Red = new ColorHSV(0, 1, 1);
        public readonly static ColorHSV Green = new ColorHSV(120.0f / 360.0f, 1, 1);
        public readonly static ColorHSV Blue = new ColorHSV(240.0f / 360.0f, 1, 1);
        public readonly static ColorHSV Black = new ColorHSV(0, 0, 0);
        public readonly static ColorHSV White = new ColorHSV(0, 0, 1);

        public float h, s, v;

        public ColorHSV(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
        }

        public ColorRGB rgb
        {
            get { return ToRGB(h, s, v); }
        }

        public ColorRGBA rgb1
        {
            get { return ToRGB(h, s, v).rgb1; }
        }

        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return h;
                    case 1: return s;
                    case 2: return v;
                    default: throw new IndexOutOfRangeException("ColorHSV index out of range: " + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0: h = value; break;
                    case 1: s = value; break;
                    case 2: v = value; break;
                    default: throw new IndexOutOfRangeException("ColorHSV index out of range: " + i);
                }
            }
        }

        /// <summary>
        /// Add two colors.
        /// </summary>
        public static ColorHSV operator +(ColorHSV hsv1, ColorHSV hsv2)
        {
            return new ColorHSV(hsv1.h + hsv2.h, hsv1.s + hsv2.s, hsv1.v + hsv2.v);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static ColorHSV operator +(ColorHSV hsv1, float s)
        {
            return new ColorHSV(hsv1.h + s, hsv1.s + s, hsv1.v + s);
        }

        /// <summary>
        /// Add vector and scalar.
        /// </summary>
        public static ColorHSV operator +(float s, ColorHSV hsv1)
        {
            return new ColorHSV(hsv1.h + s, hsv1.s + s, hsv1.v + s);
        }

        /// <summary>
        /// Subtract two colors.
        /// </summary>
        public static ColorHSV operator -(ColorHSV hsv1, ColorHSV hsv2)
        {
            return new ColorHSV(hsv1.h - hsv2.h, hsv1.s - hsv2.s, hsv1.v - hsv2.v);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static ColorHSV operator -(ColorHSV hsv1, float s)
        {
            return new ColorHSV(hsv1.h - s, hsv1.s - s, hsv1.v - s);
        }

        /// <summary>
        /// Subtract vector and scalar.
        /// </summary>
        public static ColorHSV operator -(float s, ColorHSV hsv1)
        {
            return new ColorHSV(hsv1.h - s, hsv1.s - s, hsv1.v - s);
        }

        /// <summary>
        /// Multiply two colors.
        /// </summary>
        public static ColorHSV operator *(ColorHSV hsv1, ColorHSV hsv2)
        {
            return new ColorHSV(hsv1.h * hsv2.h, hsv1.s * hsv2.s, hsv1.v * hsv2.v);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static ColorHSV operator *(ColorHSV hsv, float s)
        {
            return new ColorHSV(hsv.h * s, hsv.s * s, hsv.v * s);
        }

        /// <summary>
        /// Multiply a vector and a scalar.
        /// </summary>
        public static ColorHSV operator *(float s, ColorHSV v)
        {
            return new ColorHSV(v.h * s, v.s * s, v.v * s);
        }

        /// <summary>
        /// Divide two colors.
        /// </summary>
        public static ColorHSV operator /(ColorHSV hsv1, ColorHSV hsv2)
        {
            return new ColorHSV(hsv1.h / hsv2.h, hsv1.s / hsv2.s, hsv1.v / hsv2.v);
        }

        /// <summary>
        /// Divide a vector and a scalar.
        /// </summary>
        public static ColorHSV operator /(ColorHSV hsv, float s)
        {
            return new ColorHSV(hsv.h / s, hsv.s / s, hsv.v / s);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public static bool operator ==(ColorHSV hsv1, ColorHSV hsv2)
        {
            return (hsv1.h == hsv2.h && hsv1.s == hsv2.s && hsv1.v == hsv2.v);
        }

        /// <summary>
        /// Are these colors not equal.
        /// </summary>
        public static bool operator !=(ColorHSV hsv1, ColorHSV hsv2)
        {
            return (hsv1.h != hsv2.h || hsv1.s != hsv2.s || hsv1.v != hsv2.v);
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is ColorHSV)) return false;

            ColorHSV hsv = (ColorHSV)obj;

            return this == hsv;
        }

        /// <summary>
        /// Are these colors equal given the error.
        /// </summary>
        public bool EqualsWithError(ColorHSV hsv, float eps)
        {
            if (Math.Abs(h - hsv.h) > eps) return false;
            if (Math.Abs(s - hsv.s) > eps) return false;
            if (Math.Abs(v - hsv.v) > eps) return false;
            return true;
        }

        /// <summary>
        /// Are these colors equal.
        /// </summary>
        public bool Equals(ColorHSV hsv)
        {
            return this == hsv;
        }

        /// <summary>
        /// colors hash code. 
        /// </summary>
        public override int GetHashCode()
        {
            float hashcode = 23;
            hashcode = (hashcode * 37) + h;
            hashcode = (hashcode * 37) + s;
            hashcode = (hashcode * 37) + v;

            return unchecked((int)hashcode);
        }

        /// <summary>
        /// Vector as a string.
        /// </summary>
		public override string ToString()
        {
            return h + "," + s + "," + v;
        }

        /// <summary>
        /// Vector from a string.
        /// </summary>
        static public ColorHSV FromString(string s)
        {
            ColorHSV hsv = new ColorHSV();
            try
            {
                string[] separators = new string[] { "," };
                string[] result = s.Split(separators, StringSplitOptions.None);

                hsv.h = float.Parse(result[0]);
                hsv.s = float.Parse(result[1]);
                hsv.v = float.Parse(result[2]);
            }
            catch { }

            return hsv;
        }

        /// <summary>
        /// Convert to RGB color space.
        /// </summary>
        public static ColorRGB ToRGB(float h, float s, float v)
        {
            //Wrap h to 0-1 range.
            h = h - (float)Math.Floor(h);

            float r = 0, g = 0, b = 0;

            if (s == 0)
            {
                r = v;
                g = v;
                b = v;
            }
            else
            {
                int idx;
                float f, p, q, t;

                float H = FMath.Clamp(h, 0.0f, 1.0f) * 360.0f;
                float S = FMath.Clamp(s, 0.0f, 1.0f);
                float V = FMath.Clamp(v, 0.0f, 1.0f);

                if (H == 360)
                    H = 0;
                else
                    H = H / 60;

                idx = (int)Math.Truncate(H);
                f = H - idx;

                p = V * (1.0f - S);
                q = V * (1.0f - (S * f));
                t = V * (1.0f - (S * (1.0f - f)));

                switch (idx)
                {
                    case 0:
                        r = V;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = V;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = V;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = V;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = V;
                        break;

                    default:
                        r = V;
                        g = p;
                        b = q;
                        break;
                }
            }

            return new ColorRGB(r, g, b);
        }

        public static ColorHSV Random(Random rnd)
        {
            ColorHSV c = new ColorHSV();
            c.h = (float)rnd.NextDouble();
            c.s = (float)rnd.NextDouble();
            c.v = (float)rnd.NextDouble();

            return c;
        }

        /// <summary>
        /// Generates a list of colors with hues ranging from 0 360
        /// and a saturation and value of 1. 
        /// </summary>
        public static List<ColorHSV> GenerateSpectrum(int bands)
        {

            List<ColorHSV> colorsList = new List<ColorHSV>(bands);

            int increment = 360 / bands;
            for (int i = 0; i < bands - 1; i++)
            {
                colorsList.Add(new ColorHSV(i * increment, 1, 1));
            }

            colorsList.Add(new ColorHSV(0, 1, 1));

            return colorsList;
        }
    }
}

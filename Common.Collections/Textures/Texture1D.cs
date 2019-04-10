﻿using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Collections.Textures.Data1D;

namespace Common.Collections.Textures
{
    public class Texture1D : Texture
    {

        public TextureData1D Data { get; private set; }

        public override int Channels { get { return Data.Channels; } }

        public override int BitDepth { get { return Data.BitDepth; } }

        public Texture1D(int width, int channels, int bitDepth)
        {
            if (channels < 1) throw new ArgumentException("Channels must be at least 1");
            if (width < 1) throw new ArgumentException("Width must be at least 1");

            CreateData(width, channels, bitDepth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Texture1D: Width={0}, Channels={1}, BitDepth={2}, Wrap={3}, Interpolation={4}]",
                GetWidth(), Channels, BitDepth, Wrap, Interpolation);
        }

        public void Resize(int width, int channels, int bitDepth)
        {
            if (Data.GetWidth() != width || Data.Channels != channels || Data.BitDepth != bitDepth)
            {
                CreateData(width, channels, bitDepth);
            }
        }

        public int GetWidth(int m = 0)
        {
            return Data.GetWidth(m);
        }

        public override void Clear()
        {
            Data.Clear();
        }

        public void SetPixels(ColorRGBA[] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetChannels(float[] channel, int c, int mipmap = 0)
        {
            Data.SetChannels(channel, c, mipmap);
        }

        public ColorRGBA[] GetPixels(int mipmap = 0)
        {
            return Data.GetPixels(mipmap);
        }

        public float[] GetChannels(int c, int mipmap = 0)
        {
            return Data.GetChannels(c, mipmap);
        }

        public void SetChannel(int x, int c, float v, int m = 0)
        {
            if (c >= Data.Channels) return;
            Data[x, c, m] = v;
        }

        public void SetPixel(int x, ColorRGBA pixel, int m = 0)
        {
            if (Data.Channels > 0) Data[x, 0, m] = pixel.r;
            if (Data.Channels > 1) Data[x, 1, m] = pixel.g;
            if (Data.Channels > 2) Data[x, 2, m] = pixel.b;
            if (Data.Channels > 3) Data[x, 3, m] = pixel.a;
        }

        public float GetChannel(int x, int c, int m = 0)
        {
            if (c >= Data.Channels) return 0;

            x = Index(x, Data.GetWidth(m));

            return Data[x, c, m];
        }

        public float GetChannel(double x, int c, int m = 0)
        {
            if (c >= Data.Channels) return 0;

            int W = Data.GetWidth(m);

            x *= (W - 1);

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);
                return GetBilinear(ix, c, m);
            }
            else if (Interpolation == TEXTURE_INTERPOLATION.BICUBIC)
            {
                BicubicIndex ix = NewBicubicIndex(x, W);
                return GetBicubic(ix, c, m);
            }
            else
            {
                return GetChannel((int)x, c, m);
            }

        }

        public ColorRGBA GetPixel(int x, int m = 0)
        {
            x = Index(x, Data.GetWidth(m));

            ColorRGBA pixel = new ColorRGBA();

            if (Data.Channels > 0) pixel.r = Data[x, 0, m];
            if (Data.Channels > 1) pixel.g = Data[x, 1, m];
            if (Data.Channels > 2) pixel.b = Data[x, 2, m];
            if (Data.Channels > 3) pixel.a = Data[x, 3, m];

            return pixel;
        }

        public ColorRGBA GetPixel(double x, int m = 0)
        {
            int W = Data.GetWidth(m);

            x *= (W - 1);

            ColorRGBA pixel = new ColorRGBA();

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);

                if (Data.Channels > 0) pixel.r = GetBilinear(ix, 0, m);
                if (Data.Channels > 1) pixel.g = GetBilinear(ix, 1, m);
                if (Data.Channels > 2) pixel.b = GetBilinear(ix, 2, m);
                if (Data.Channels > 3) pixel.a = GetBilinear(ix, 3, m);
            }
            else if (Interpolation == TEXTURE_INTERPOLATION.BICUBIC)
            {
                BicubicIndex ix = NewBicubicIndex(x, W);

                if (Data.Channels > 0) pixel.r = GetBicubic(ix, 0, m);
                if (Data.Channels > 1) pixel.g = GetBicubic(ix, 1, m);
                if (Data.Channels > 2) pixel.b = GetBicubic(ix, 2, m);
                if (Data.Channels > 3) pixel.a = GetBicubic(ix, 3, m);
            }
            else
            {
                pixel = GetPixel((int)x, m);
            }

            return pixel;
        }

        private float GetBilinear(BilinearIndex x, int c, int m)
        {
            return (float)(Data[x.i0, c, m] * (1.0 - x.fi) + Data[x.i1, c, m] * x.fi);
        }

        private float GetBicubic(BicubicIndex x, int c, int m)
        {
            double fx = x.fi;
            double v0 = Data[x.i0, c, m];
            double v1 = Data[x.i1, c, m];
            double v2 = Data[x.i2, c, m];
            double v3 = Data[x.i3, c, m];

            return (float)Bicubic(fx, v0, v1, v2, v3);
        }

        private void CreateData(int width, int channels, int bitDepth)
        {
            Data = null;
            Data = TextureData1D.CreateData(width, channels, bitDepth);
        }

    }
}

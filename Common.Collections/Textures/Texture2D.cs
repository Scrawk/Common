using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Collections.Textures.Data2D;

namespace Common.Collections.Textures
{
    public class Texture2D : Texture
    {

        public TextureData2D Data { get; private set; }

        public override int Channels {  get { return Data.Channels; } }

        public override int BitDepth { get { return Data.BitDepth; } }

        public int MipmapLevels { get { return Data.Levels; } }

        public TEXTURE_MIPMAP MipmapMode
        {
            get { return Data.MipmapMode; }
            set { Data.MipmapMode = value; }
        }

        public Texture2D(int width, int height, int channels, int bitDepth)
        {
            if (channels < 1) throw new ArgumentException("Channels must be at least 1");
            if (width < 1) throw new ArgumentException("Width must be at least 1");
            if (height < 1) throw new ArgumentException("Height must be at least 1");

            CreateData(width, height, channels, bitDepth, TEXTURE_MIPMAP.NONE);
        }

        public Texture2D(int width, int height, int channels, int bitDepth, TEXTURE_MIPMAP mipmap)
        {
            if (channels < 1) throw new ArgumentException("Channels must be at least 1");
            if (width < 1) throw new ArgumentException("Width must be at least 1");
            if (height < 1) throw new ArgumentException("Height must be at least 1");

            CreateData(width, height, channels, bitDepth, mipmap);
        }

        public Texture2D(TextureData2D data)
        {
            if (data.GetWidth() < 1) throw new ArgumentException("Data width must be at least 1");
            if (data.GetHeight() < 1) throw new ArgumentException("Data height must be at least 1");

            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[Texture2D: Width={0}, Height={1}, Channels={2}, BitDepth={3}, Wrap={4}, Interpolation={5}, MipmapLevels={6}]",
                GetWidth(), GetHeight(), Channels, BitDepth, Wrap, Interpolation, MipmapLevels);
        }

        public void Resize(int width, int height, int channels, int bitDepth)
        {
            if(GetWidth() != width || GetHeight() != height || Channels != channels || BitDepth != bitDepth)
            {
                CreateData(width, height, channels, bitDepth, MipmapMode);
            }
        }

        public int GetWidth(int m = 0) 
        { 
            return Data.GetWidth(m); 
        }

        public int GetHeight(int m = 0) 
        { 
            return Data.GetHeight(m); 
        }

        public override void Clear()
        {
            Data.Clear();
        }

        public void GenerateMipmaps()
        {
            Data.GenerateMipmaps();
        }

        public void SetPixels(ColorRGBA[,] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetChannels(float[,] channel, int c, int mipmap = 0)
        {
            Data.SetChannels(channel, c, mipmap);
        }

        public ColorRGBA[,] GetPixels(int mipmap = 0)
        {
            return Data.GetPixels(mipmap);
        }

        public float[,] GetChannels(int c, int mipmap = 0)
        {
            return Data.GetChannels(c, mipmap);
        }

        public void SetChannel(int x, int y, int c, float v, int m = 0)
        {
            if (c >= Channels) return;
            Data[x, y, c, m] = v;
        }

        public void SetPixel(int x, int y, ColorRGBA pixel, int m = 0)
        {
            int channels = Channels;
            if (channels > 0) Data[x, y, 0, m] = pixel.r;
            if (channels > 1) Data[x, y, 1, m] = pixel.g;
            if (channels > 2) Data[x, y, 2, m] = pixel.b;
            if (channels > 3) Data[x, y, 3, m] = pixel.a;
        }

        public float GetChannel(int x, int y, int c, int m = 0)
        {
            if (c >= Channels) return 0;

            x = Index(x, GetWidth(m));
            y = Index(y, GetHeight(m));

            return Data[x, y, c, m];
        }

        public float GetChannel(double x, double y, int c, int m = 0)
        {
            if (c >= Channels) return 0;

            int W = GetWidth(m);
            int H = GetHeight(m);

            x *= (W - 1);
            y *= (H - 1);

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);
                BilinearIndex iy = NewBilinearIndex(y, H);
                return GetBilinear(ix, iy, c, m);
            }
            else if (Interpolation == TEXTURE_INTERPOLATION.BICUBIC)
            {
                BicubicIndex ix = NewBicubicIndex(x, W);
                BicubicIndex iy = NewBicubicIndex(y, H);
                return GetBicubic(ix, iy, c, m);
            }
            else
            {
                return GetChannel((int)x, (int)y, c, m);
            }

        }

        public ColorRGBA GetPixel(int x, int y, int m = 0)
        {
            x = Index(x, GetWidth(m));
            y = Index(y, GetHeight(m));

            ColorRGBA pixel = new ColorRGBA();

            int channels = Channels;
            if (channels > 0) pixel.r = Data[x, y, 0, m];
            if (channels > 1) pixel.g = Data[x, y, 1, m];
            if (channels > 2) pixel.b = Data[x, y, 2, m];
            if (channels > 3) pixel.a = Data[x, y, 3, m];

            return pixel;
        }

        public ColorRGBA GetPixel(double x, double y, int m = 0)
        {
            int W = GetWidth(m);
            int H = GetHeight(m);

            x *= (W - 1);
            y *= (H - 1);

            ColorRGBA pixel = new ColorRGBA();

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);
                BilinearIndex iy = NewBilinearIndex(y, H);

                int channels = Channels;
                if (channels > 0) pixel.r = GetBilinear(ix, iy, 0, m);
                if (channels > 1) pixel.g = GetBilinear(ix, iy, 1, m);
                if (channels > 2) pixel.b = GetBilinear(ix, iy, 2, m);
                if (channels > 3) pixel.a = GetBilinear(ix, iy, 3, m);
            }
            else if (Interpolation == TEXTURE_INTERPOLATION.BICUBIC)
            {
                BicubicIndex ix = NewBicubicIndex(x, W);
                BicubicIndex iy = NewBicubicIndex(y, H);

                int channels = Channels;
                if (channels > 0) pixel.r = GetBicubic(ix, iy, 0, m);
                if (channels > 1) pixel.g = GetBicubic(ix, iy, 1, m);
                if (channels > 2) pixel.b = GetBicubic(ix, iy, 2, m);
                if (channels > 3) pixel.a = GetBicubic(ix, iy, 3, m);
            }
            else
            {
                pixel = GetPixel((int)x, (int)y, m);
            }

            return pixel;
        }

        private float GetBilinear(BilinearIndex x, BilinearIndex y, int c, int m)
        {
            double v0 = Data[x.i0, y.i0, c, m] * (1.0 - x.fi) + Data[x.i1, y.i0, + c, m] * x.fi;
            double v1 = Data[x.i0, y.i1, c, m] * (1.0 - x.fi) + Data[x.i1, y.i1, c, m] * x.fi;

            return (float)(v0 * (1.0 - y.fi) + v1 * y.fi);
        }

        private float GetBicubic(BicubicIndex x, BicubicIndex y, int c, int m)
        {
            double fx, fy, v0, v1, v2, v3;

            fx = x.fi;
            fy = y.fi;
            v0 = Data[x.i0, y.i0, c, m];
            v1 = Data[x.i1, y.i0, c, m];
            v2 = Data[x.i2, y.i0, c, m];
            v3 = Data[x.i3, y.i0, c, m];

            double v00 = Bicubic(fx, v0, v1, v2, v3);

            v0 = Data[x.i0, y.i1, c, m];
            v1 = Data[x.i1, y.i1, c, m];
            v2 = Data[x.i2, y.i1, c, m];
            v3 = Data[x.i3, y.i1, c, m];

            double v01 = Bicubic(fx, v0, v1, v2, v3);

            v0 = Data[x.i0, y.i2, c, m];
            v1 = Data[x.i1, y.i2, c, m];
            v2 = Data[x.i2, y.i2, c, m];
            v3 = Data[x.i3, y.i2, c, m];

            double v02 = Bicubic(fx, v0, v1, v2, v3);

            v0 = Data[x.i0, y.i3, c, m];
            v1 = Data[x.i1, y.i3, c, m];
            v2 = Data[x.i2, y.i3, c, m];
            v3 = Data[x.i3, y.i3, c, m];

            double v03 = Bicubic(fx, v0, v1, v2, v3);

            return (float)(Bicubic(fy, v00, v01, v02, v03));

        }

        private void CreateData(int width, int height, int channels, int bitDepth, TEXTURE_MIPMAP mode)
        {
            Data = null;

            if (mode != TEXTURE_MIPMAP.NONE)
                Data = new Mipmap2D(width, height, channels, bitDepth, mode);
            else
                Data = TextureData2D.CreateData(width, height, channels, bitDepth);
        }

    }
}

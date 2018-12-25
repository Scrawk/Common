using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Collections.Textures.Data3D;

namespace Common.Collections.Textures
{
    public class Texture3D : Texture
    {

        public TextureData3D Data { get; private set; }

        public override int Channels { get { return Data.Channels; } }

        public override int BitDepth { get { return Data.BitDepth; } }

        /// <summary>
        /// 
        /// </summary>
        public Texture3D(int width, int height, int depth, int channels, int bitDepth)
        {
            if (channels < 1) throw new ArgumentException("Channels must be at least 1");
            if (width < 1) throw new ArgumentException("Width must be at least 1");
            if (height < 1) throw new ArgumentException("Height must be at least 1");
            if (depth < 1) throw new ArgumentException("Depth must be at least 1");

            CreateData(width, height, depth, channels, bitDepth);
        }

        public void Resize(int width, int height, int depth, int channels, int bitDepth)
        {
            if (GetWidth() != width || GetHeight() != height || GetDepth() != depth || Channels != channels || BitDepth != bitDepth)
            {
                CreateData(width, height, depth, channels, bitDepth);
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

        public int GetDepth(int m = 0) 
        { 
            return Data.GetDepth(m); 
        }

        public override void Clear()
        {
            Data.Clear();
        }

        public void SetPixels(float[] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetPixels(float[,,,] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetPixels(ColorRGBA[] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetPixels(ColorRGBA[,,] pixels, int mipmap = 0)
        {
            Data.SetPixels(pixels, mipmap);
        }

        public void SetChannels(float[,,] channel, int c, int mipmap = 0)
        {
            Data.SetChannels(channel, c, mipmap);
        }

        public ColorRGBA[,,] GetPixels(int mipmap = 0)
        {
            return Data.GetPixels(mipmap);
        }

        public float[,,] GetChannels(int c, int mipmap = 0)
        {
            return Data.GetChannels(c, mipmap);
        }

        public void SetChannel(int x, int y, int z, int c, float v, int m = 0)
        {
            if (c >= Channels) return;
            Data[x, y, z, c, m] = v;
        }

        /// <summary>
        /// Set channels from a color.
        /// </summary>
        public void SetPixel(int x, int y, int z, ColorRGBA pixel, int m = 0)
        {
            if (Channels > 0) Data[x, y, z, 0, m] = pixel.r;
            if (Channels > 1) Data[x, y, z, 1, m] = pixel.g;
            if (Channels > 2) Data[x, y, z, 2, m] = pixel.b;
            if (Channels > 3) Data[x, y, z, 3, m] = pixel.a;
        }

        /// <summary>
        /// Get a channel.
        /// </summary>
        public float GetChannel(int x, int y, int z, int c, int m = 0)
        {
            if (c >= Channels) return 0;

            x = Index(x, GetWidth(m));
            y = Index(y, GetHeight(m));
            z = Index(z, GetDepth(m));

            return Data[x, y, z, c, m];
        }

        /// <summary>
        /// Get a channel.
        /// </summary>
        public float GetChannel(float x, float y, float z, int c, int m = 0)
        {
            if (c >= Channels) return 0;

            int W = GetWidth(m);
            int H = GetHeight(m);
            int D = GetDepth(m);

            x *= (W - 1);
            y *= (H - 1);
            z *= (D - 1);

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);
                BilinearIndex iy = NewBilinearIndex(y, H);
                BilinearIndex iz = NewBilinearIndex(z, D);

                return GetBilinear(ix, iy, iz, c, m);
            }
            else
            {
                return GetChannel((int)x, (int)y, (int)z, c, m);
            }
        }

        /// <summary>
        /// Get channels into color.
        /// </summary>
        public ColorRGBA GetPixel(int x, int y, int z, int m = 0)
        {
            x = Index(x, GetWidth(m));
            y = Index(y, GetHeight(m));
            z = Index(z, GetDepth(m));

            ColorRGBA pixel = new ColorRGBA();

            if (Channels > 0) pixel.r = Data[x, y, z, 0, m];
            if (Channels > 1) pixel.g = Data[x, y, z, 1, m];
            if (Channels > 2) pixel.b = Data[x, y, z, 2, m];
            if (Channels > 3) pixel.a = Data[x, y, z, 3, m];

            return pixel;
        }

        /// <summary>
        /// Get channels into color.
        /// </summary>
        public ColorRGBA GetPixel(float x, float y, float z, int m = 0)
        {
            int W = GetWidth(m);
            int H = GetHeight(m);
            int D = GetDepth(m);

            x *= (W - 1);
            y *= (H - 1);
            z *= (D - 1);

            ColorRGBA pixel = new ColorRGBA();

            if (Interpolation == TEXTURE_INTERPOLATION.BILINEAR)
            {
                BilinearIndex ix = NewBilinearIndex(x, W);
                BilinearIndex iy = NewBilinearIndex(y, H);
                BilinearIndex iz = NewBilinearIndex(z, D);

                if (Channels > 0) pixel.r = GetBilinear(ix, iy, iz, 0, m);
                if (Channels > 1) pixel.g = GetBilinear(ix, iy, iz, 1, m);
                if (Channels > 2) pixel.b = GetBilinear(ix, iy, iz, 2, m);
                if (Channels > 3) pixel.a = GetBilinear(ix, iy, iz, 3, m);
            }
            else
            {
                pixel = GetPixel((int)x, (int)y, (int)z, m);
            }

            return pixel;
        }

        /// <summary>
        /// Get a value from the datat array using bilinear filtering.
        /// </summary>
        private float GetBilinear(BilinearIndex x, BilinearIndex y, BilinearIndex z, int c, int m)
        {

            float fx1 = 1.0f - x.fi;
            float fy1 = 1.0f - y.fi;
            float fz1 = 1.0f - z.fi;

            float v00 = Data[x.i0, y.i0, z.i0, c, m] * fx1 + Data[x.i1, y.i0, z.i0, c, m] * x.fi;
            float v10 = Data[x.i0, y.i1, z.i0, c, m] * fx1 + Data[x.i1, y.i1, z.i0, c, m] * x.fi;

            float v01 = Data[x.i0, y.i0, z.i1, c, m] * fx1 + Data[x.i1, y.i0, z.i1, c, m] * x.fi;
            float v11 = Data[x.i0, y.i1, z.i1, c, m] * fx1 + Data[x.i1, y.i1, z.i1, c, m] * x.fi;

            float v0 = v00 * fy1 + v10 * y.fi;
            float v1 = v01 * fy1 + v11 * y.fi;

            return v0 * fz1 + v1 * z.fi;
        }

        /// <summary>
        /// Create the texture data based on the texture settings.
        /// </summary>
        private void CreateData(int width, int height, int depth, int channels, int bitDepth)
        {
            Data = null;
            Data = TextureData3D.CreateData(width, height, depth, channels, bitDepth);
        }

    }
}

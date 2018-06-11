using System;
using System.Collections.Generic;

using Common.Collections.Textures.Data2D;
using Common.Collections.Textures.Filters;

namespace Common.Collections.Textures.Kernals
{
    public class PolyphaseKernel
    {

        public int WindowSize { get; private set; }

        public int Length { get; private set; }

        public float Width { get; private set; }

        public float[] Data { get; private set; }

        public PolyphaseKernel(Filter filter, int srcLength, int dstLength, int samples)
        {

            float scale = dstLength / (float)srcLength;
            float iscale = 1.0f / scale;

            if (scale > 1)
            {
                // Upsampling.
                samples = 1;
                scale = 1;
            }

            Length = dstLength;
            Width = filter.Width * iscale;
            WindowSize = (int)Math.Ceiling(Width * 2) + 1;

            Data = new float[WindowSize * Length];

            for (uint i = 0; i < Length; i++)
            {
                float center = (0.5f + i) * iscale;

                int left = (int)Math.Floor(center - Width);
            
                float total = 0.0f;
                for (int j = 0; j < WindowSize; j++)
                {
                    float sample = filter.SampleBox(left + j - center, scale, samples);

                    Data[i * WindowSize + j] = sample;
                    total += sample;
                }

                // normalize weights.
                for (int j = 0; j < WindowSize; j++)
                {
                    Data[i * WindowSize + j] /= total;
                }
            }

        }

        public float ValueAt(int column, int x)
        {
			return Data[column * WindowSize + x];
		}

        public void ApplyHorizontal(int y, TextureData2D src, TextureData2D dst)
        {
            int srcWidth = src.GetWidth();
            int channels = src.Channels;

            float scale = Length / (float)srcWidth;
            float iscale = 1.0f / scale;

            for (int i = 0; i < Length; i++)
            {
                float center = (0.5f + i) * iscale;

                int left = (int)Math.Floor(center - Width);

                for (int c = 0; c < channels; c++)
                {
                    float sum = 0;
                    for (int j = 0; j < WindowSize; ++j)
                    {
                        int x = j + left;

                        if (x < 0) x = 0;
                        if (x >= srcWidth) x = srcWidth - 1; 

                        sum += ValueAt(i, j) * src[x, y, c];
                    }

                    dst[i, y, c] = sum;
                }
            }
        }

        public void ApplyHorizontal(int y, TextureData2D src, float[] dst, int dstWidth)
        {
            int srcWidth = src.GetWidth();
            int channels = src.Channels;

            float scale = Length / (float)srcWidth;
            float iscale = 1.0f / scale;
            int idx;

            for (int i = 0; i < Length; i++)
            {
                float center = (0.5f + i) * iscale;

                int left = (int)Math.Floor(center - Width);

                for (int c = 0; c < channels; c++)
                {
                    float sum = 0;
                    for (int j = 0; j < WindowSize; ++j)
                    {
                        int x = j + left;

                        if (x < 0) x = 0;
                        if (x >= srcWidth) x = srcWidth - 1;

                        idx = x + y * srcWidth;
                        sum += ValueAt(i, j) * src[x, y, c];
                    }

                    idx = i + y * dstWidth;
                    dst[idx * channels + c] = sum;
                }
            }
        }

        public void ApplyVertical(int x, TextureData2D src, TextureData2D dst)
        {

            int srcHeight = src.GetHeight();
            int channels = src.Channels;

            float scale = Length / (float)srcHeight;
            float iscale = 1.0f / scale;

	        for (int i = 0; i < Length; i++)
	        {
		        float center = (0.5f + i) * iscale;

                int left = (int)Math.Floor(center - Width);

                for (int c = 0; c < channels; c++)
                {
                    float sum = 0;
                    for (int j = 0; j < WindowSize; ++j)
                    {
                        int y = j + left;

                        if (y < 0) y = 0;
                        if (y >= srcHeight) y = srcHeight - 1;

                        sum += ValueAt(i, j) * src[x, y, c];
                    }

                    dst[x, i, c] = sum;
                }
	        }
        }

        public void ApplyVertical(int x, float[] src, int srcWidth, int srcHeight, int channels, TextureData2D dst)
        {

            int dstWidth = dst.GetWidth();

            float scale = Length / (float)srcHeight;
            float iscale = 1.0f / scale;
            int idx;

            for (int i = 0; i < Length; i++)
            {
                float center = (0.5f + i) * iscale;

                int left = (int)Math.Floor(center - Width);

                for (int c = 0; c < channels; c++)
                {
                    float sum = 0;
                    for (int j = 0; j < WindowSize; ++j)
                    {
                        int y = j + left;

                        if (y < 0) y = 0;
                        if (y >= srcHeight) y = srcHeight - 1;

                        idx = x + y * srcWidth;
                        sum += ValueAt(i, j) * src[idx * channels + c];
                    }

                    idx = x + i * dstWidth;
                    dst[x, i, c] = sum;
                }
            }
        }


    }
}

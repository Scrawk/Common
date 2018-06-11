using System;
using System.Collections.Generic;

using Common.Collections.Textures.Kernals;
using Common.Collections.Textures.Filters;

namespace Common.Collections.Textures.Data2D
{
    public class Mipmap2D : TextureData2D
    {

        /// <summary>
        /// The array of data that makes up the mipmap levels.
        /// </summary>
        protected List<TextureData2D> Data { get; set; }

        /// <summary>
        /// The mipmap filter.
        /// </summary>
        public Filter Filter {  get { return GetFilter(); } }

        /// Get a value from the data array at the mipmap level.
        /// Ignore m if not using mipmaps.
        /// </summary>
        public override float this[int x, int y, int c, int m = 0]
        {
            get { return Data[m][x, y, c]; }
            set { Data[m][x, y, c] = value; }
        }

        /// <summary>
        /// Create a new mipmap with the data all set to 0.
        /// </summary>
        public Mipmap2D(int width, int height, int channels, int bitDepth, TEXTURE_MIPMAP mode = TEXTURE_MIPMAP.BOX, int minLevel = -1)
            : base(width, height, channels, bitDepth)
        {

            MipmapMode = mode;

            int min = Math.Min(width, height);

            Levels = (int)(Math.Log(min) / Math.Log(2)) + 1;

            if (minLevel > -1 && Levels > minLevel)
                Levels = minLevel;

            Data = new List<TextureData2D>(Levels);

            int w = Width;
            int h = Height;

            for (int i = 0; i < Levels; i++)
            {
                Data.Add(TextureData2D.CreateData(w, h, channels, bitDepth));
                w /= 2;
                h /= 2;
            }

        }

        /// <summary>
        /// Clear the data in array to 0.
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].Clear();
            }
        }

        /// <summary>
        /// Gets the data at mipmap m.
        /// </summary>
        public TextureData2D GetData(int mipmap)
        {
            return Data[mipmap];
        }

        /// <summary>
        /// The size of the x dimension if dimensions > 0.
        /// </summary>
        public override int GetWidth(int mipmap = 0)
        {
            return Data[mipmap].GetWidth();
        }

        /// <summary>
        /// The size of the y dimension if dimensions > 1.
        /// </summary>
        public override int GetHeight(int mipmap = 0)
        {
            return Data[mipmap].GetHeight();
        }

        /// <summary>
        /// Returns a array containing the size of each
        /// mipmap level for a given size.
        /// </summary>
        public static int[] CalculateLevels(int size)
        {
            int levels = (int)(Math.Log(size) / Math.Log(2)) + 1;

            int[] levelSizes = new int[levels];

            int s = size;
            for (int i = 0; i < levels; i++)
            {
                levelSizes[i] = s;
                s /= 2;
            }

            return levelSizes;
        }

        /// <summary>
        /// Create the mipmaps by downsampling using a filter. 
        /// </summary>
        public override void GenerateMipmaps()
        {
            if (MipmapMode == TEXTURE_MIPMAP.NONE) return;

            GenerateMipmaps(Filter);
            
        }

        /// <summary>
        /// Create the mipmaps by downsampling using a filter. 
        /// </summary>
        public void GenerateMipmaps(Filter filter)
        {

            for (int i = 1; i < Levels; i++)
            {
                TextureData2D src = Data[i - 1];
                TextureData2D dst = Data[i];

                int srcWidth = src.GetWidth();
                int srcHeight = src.GetHeight();

                int dstWidth = dst.GetWidth();
                int dstHeight = dst.GetHeight();

                float[] tmp = new float[dstWidth * srcHeight * Channels];

                PolyphaseKernel xKernel = new PolyphaseKernel(filter, srcWidth, dstWidth, 32);
                PolyphaseKernel yKernel = new PolyphaseKernel(filter, srcHeight, dstHeight, 32);

                for (int y = 0; y < srcHeight; y++)
                    xKernel.ApplyHorizontal(y, src, tmp, dstWidth);

                for (int x = 0; x < dstWidth; x++)
                    yKernel.ApplyVertical(x, tmp, dstWidth, srcHeight, Channels, dst);

            }

        }

        private Filter GetFilter()
        {

            switch (MipmapMode)
            {
                case TEXTURE_MIPMAP.BOX:
                    return new BoxFilter();

                case TEXTURE_MIPMAP.TRIANGLE:
                    return new TriangleFilter();

                case TEXTURE_MIPMAP.QUADRATIC:
                    return new QuadraticFilter();

                case TEXTURE_MIPMAP.KAISER:
                    return new KaiserFilter();

                default:
                    return new BoxFilter();
            }

        }

    }
}

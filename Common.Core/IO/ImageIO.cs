using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Core.Mathematics;

namespace Common.Core.IO
{

    public class ImageProperties
    {
        public string FileName;
        public string Extension;
        public int Width;
        public int Height;
        public int Channels;
        public int ByteChannels;
        public int BitDepth;
        public bool BigEndian;
        public bool IsBGRA;
    }

    public static class ImageIO
    {

        public static void ToPixels(ImageProperties properties, byte[] bytes, ColorRGBA[,] pixels)
        {
            int width = properties.Width;
            int height = properties.Height;
            int channels = properties.Channels;
            int byteChannels = properties.ByteChannels;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ColorRGBA pixel = new ColorRGBA();
                    for (int c = 0; c < byteChannels; c++)
                    {
                        int i = (x + y * width) * byteChannels + c;
                        pixel[c] = Read(i, bytes, properties);
                    }

                    if (channels == 1)
                        pixel = pixel.rrra;
                    else if (properties.IsBGRA)
                        pixel = pixel.bgra;

                    pixels[x, y] = pixel;
                }
            }

        }

        public static void ToBytes(ImageProperties properties, ColorRGBA[,] pixels, byte[] bytes)
        {
            int width = properties.Width;
            int height = properties.Height;
            int channels = properties.Channels;
            int byteChannels = properties.ByteChannels;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    ColorRGBA pixel = pixels[x, y];

                    if (channels == 1)
                        pixel = pixel.rrra;
                    else if (properties.IsBGRA)
                        pixel = pixel.bgra;

                    for (int c = 0; c < byteChannels; c++)
                    {
                        int i = (x + y * width) * byteChannels + c;
                        Write(pixel[c], i, bytes, properties);
                    }
                }
            }

        }

        private static float Read(int i, byte[] bytes, ImageProperties properties)
        {
            switch (properties.BitDepth)
            {
                case 8:
                    return bytes[i] / (float)byte.MaxValue;

                case 16:
                    return ReadShort(i, bytes, properties.BigEndian) / (float)ushort.MaxValue;

                case 32:
                    return ReadFloat(i, bytes);

                default:
                    throw new ArgumentException("Unhandled bit depth");
            }
        }


        private static void Write(float c, int i, byte[] bytes, ImageProperties properties)
        {
            switch (properties.BitDepth)
            {
                case 8:
                    bytes[i] = (byte)FMath.Clamp(c * 255, 0, 255);
                    break;

                case 16:
                    WriteShort(c, i, bytes, properties.BigEndian);
                    break;

                default:
                    throw new ArgumentException("Unhandled bit depth");
            }
        }

        private static ushort ReadShort(int i, byte[] bytes, bool bigEndian)
        {
            int index = i * 2;

            if (bigEndian)
                return (ushort)(bytes[index] * 256 + bytes[index + 1]);
            else
                return (ushort)(bytes[index] + bytes[index + 1] * 256);
        }

        private static float ReadFloat(int i, byte[] bytes)
        {
            int index = i * 4;
            return BitConverter.ToSingle(bytes, index);
        }

        private static void WriteShort(float c, int i, byte[] bytes, bool bigEndian)
        {
            int index = i * 2;

            ushort v = (ushort)FMath.Clamp(c * ushort.MaxValue, 0, ushort.MaxValue);
            byte[] b = BitConverter.GetBytes(v);

            if (bigEndian)
            {
                bytes[index + 0] = b[1];
                bytes[index + 1] = b[0];
            }
            else
            {
                bytes[index + 0] = b[0];
                bytes[index + 1] = b[1];
            }
        }

        private static void WriteFloat(float c, int i, byte[] bytes)
        {
            int index = i * 4;

            byte[] b = BitConverter.GetBytes(c);
            bytes[index + 0] = b[0];
            bytes[index + 1] = b[1];
            bytes[index + 2] = b[2];
            bytes[index + 3] = b[3];
        }
    }
}

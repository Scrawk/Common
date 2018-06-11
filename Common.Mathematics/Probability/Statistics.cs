using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Core.LinearAlgebra;

namespace Common.Mathematics.Probability
{
    public static class Statistics
    {

        public static float Mean(IList<float> data)
        {
            int count = data.Count;

            float u = 0;
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static double Mean(IList<double> data)
        {
            int count = data.Count;

            double u = 0;
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static ColorRGB Mean(IList<ColorRGB> data)
        {
            int count = data.Count;

            ColorRGB u = new ColorRGB();
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static ColorRGB Mean(ColorRGB[,] data)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            ColorRGB u = new ColorRGB();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    u += data[i, j];

            return u / (width*height);
        }

        public static ColorRGBA Mean(IList<ColorRGBA> data)
        {
            int count = data.Count;

            ColorRGBA u = new ColorRGBA();
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static float[] Mean(float[,] data)
        {
            int count = data.GetLength(0);
            int dimension = data.GetLength(1);

            float[] u = new float[dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    u[j] += data[i, j];
            }

            for (int j = 0; j < dimension; j++)
                u[j] /= count;

            return u;
        }

        public static double[] Mean(double[,] data)
        {
            int count = data.GetLength(0);
            int dimension = data.GetLength(1);

            double[] u = new double[dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    u[j] += data[i, j];
            }

            for (int j = 0; j < dimension; j++)
                u[j] /= count;

            return u;
        }

        public static float[] Mean(IList<float[]> data)
        {
            int count = data.Count;
            int dimension = data[0].Length;

            float[] u = new float[dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    u[j] += data[i][j];
            }

            for (int j = 0; j < dimension; j++)
                u[j] /= count;

            return u;
        }

        public static double[] Mean(IList<double[]> data)
        {
            int count = data.Count;
            int dimension = data[0].Length;

            double[] u = new double[dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    u[j] += data[i][j];
            }

            for (int j = 0; j < dimension; j++)
                u[j] /= count;

            return u;
        }

        public static float Variance(IList<float> data, float mean)
        {
            int count = data.Count;

            float v = 0;
            for (int i = 0; i < count; i++)
            {
                float diff = data[i] - mean;
                v += diff * diff;
            }

            return v / count;
        }

        public static double Variance(IList<double> data, double mean)
        {
            int count = data.Count;

            double v = 0;
            for (int i = 0; i < count; i++)
            {
                double diff = data[i] - mean;
                v += diff * diff;
            }

            return v / count;
        }

        public static float[,] Covariance(float[,] data, float[] mean)
        {
            int count = data.GetLength(0);
            int dimension = data.GetLength(1);

            float[,] deviations = new float[count, dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    deviations[i, j] = data[i, j] - mean[j];
            }

            float[,] cv = new float[dimension, dimension];

            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k, i] * deviations[k, j];
                }
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static double[,] Covariance(double[,] data, double[] mean)
        {
            int count = data.GetLength(0);
            int dimension = data.GetLength(1);

            double[,] deviations = new double[count, dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    deviations[i, j] = data[i, j] - mean[j];
            }

            double[,] cv = new double[dimension, dimension];

            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k, i] * deviations[k, j];
                }
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static float[,] Covariance(IList<ColorRGB> data, ColorRGB mean)
        {
            int count = data.Count;
            int dimensions = 3; 

            ColorRGB[] deviations = new ColorRGB[count];

            for (int i = 0; i < count; i++)
                deviations[i] = data[i] - mean;

            float[,] cv = new float[dimensions, dimensions];

            for (int j = 0; j < dimensions; j++)
            {
                for (int i = 0; i < dimensions; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k][i] * deviations[k][j];
                }
            }

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static float[,] Covariance(ColorRGB[,] data, ColorRGB mean)
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int count = width * height;
            int dimensions = 3;

            ColorRGB[] deviations = new ColorRGB[count];

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    deviations[i + j * width] = data[i, j] - mean;

            float[,] cv = new float[dimensions, dimensions];

            for (int j = 0; j < dimensions; j++)
            {
                for (int i = 0; i < dimensions; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k][i] * deviations[k][j];
                }
            }

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static float[,] Covariance(IList<ColorRGBA> data, ColorRGBA mean)
        {
            int count = data.Count;
            int dimensions = 4;

            ColorRGBA[] deviations = new ColorRGBA[count];

            for (int i = 0; i < count; i++)
                deviations[i] = data[i] - mean;

            float[,] cv = new float[dimensions, dimensions];

            for (int j = 0; j < dimensions; j++)
            {
                for (int i = 0; i < dimensions; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k][i] * deviations[k][j];
                }
            }

            for (int i = 0; i < dimensions; i++)
            {
                for (int j = 0; j < dimensions; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static double[,] Covariance(IList<double[]> data, double[] mean)
        {
            int count = data.Count;
            int dimension = data[0].Length;

            double[,] deviations = new double[count, dimension];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < dimension; j++)
                    deviations[i, j] = data[i][j] - mean[j];
            }

            double[,] cv = new double[dimension, dimension];

            for (int j = 0; j < dimension; j++)
            {
                for (int i = 0; i < dimension; i++)
                {
                    for (int k = 0; k < count; k++)
                        cv[i, j] += deviations[k, i] * deviations[k, j];
                }
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                    cv[i, j] /= count;
            }

            return cv;
        }

        public static double[,] CovarianceToCorrelation(double[,] covariance)
        {
            int dimensions = covariance.GetLength(0);

            double[,] correlation = new double[dimensions, dimensions];

            for (int j = 0; j < dimensions; j++)
            {
                for (int i = 0; i < dimensions; i++)
                {
                    if (i != j) continue;
                    correlation[i, j] = Math.Sqrt(covariance[i, j]);
                }
            }

            double[,] inverse = MatrixMxN.Inverse(correlation);

            return MatrixMxN.MultiplyMatrix(MatrixMxN.MultiplyMatrix(inverse, covariance), inverse);

        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.Colors;
using Common.Core.Numerics;

namespace Common.Mathematics.Probability
{
    public static class Statistics
    {

        public static float Mean(IList<float> data)
        {
            int count = data.Count;
            if (count == 0) return 0;

            float u = 0;
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static double Mean(IList<double> data)
        {
            int count = data.Count;
            if (count == 0) return 0;

            double u = 0;
            for (int i = 0; i < count; i++)
                u += data[i];

            return u / count;
        }

        public static ColorRGBA Mean(IList<ColorRGBA> data)
        {
            int count = data.Count;
            if (count == 0) return new ColorRGBA();

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

        public static float Variance(IList<float> data, float mean)
        {
            int count = data.Count;
            if (count == 0) return 0;

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
            if (count == 0) return 0;

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
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Common.GraphTheory.MatrixGraphs
{
    public partial class MatrixGraph
    {

        public MatrixGraph(int count)
        {
            Matrix = new float[count, count];
        }

        public MatrixGraph(float[,] matrix)
        {
            Size = matrix.GetLength(0);

            if (Size != matrix.GetLength(1))
                throw new ArgumentException("Matrix must be square.");

            Matrix = new float[Size, Size];

            for(int i = 0; i < Size; i++)
                for(int j = 0; j < Size; j++)
                    Matrix[i, j] = matrix[i, j];
        }

        public int Size { get; private set; }

        private float[,] Matrix {  get; set; }  

        public float this[int i, int j]
        {
            get { return Matrix[i, j]; }
            set { Matrix[i, j] = value; }
        }

        public override string ToString()
        {
            return string.Format("[MatrixGraph: Size={0}]", Size);
        }

        public void Print()
        {
            var builder = new StringBuilder();
            Print(builder);
            Console.WriteLine(builder.ToString());
        }

        public void Print(StringBuilder builder)
        {
            builder.AppendLine(this.ToString());

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    float e = Matrix[x, y];

                    if(y == Size-1)
                        builder.Append(string.Format("{0}", e));
                    else
                        builder.Append(string.Format("{0}, ", e));

                }

                builder.AppendLine();
            }
        }

        public MatrixGraph Copy()
        {
            var copy = new MatrixGraph(this.Matrix);
            return copy;
        }

    }
}

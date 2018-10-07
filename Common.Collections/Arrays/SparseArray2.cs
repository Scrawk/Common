using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Collections.Arrays
{
    public class SparseArray2<T>
    {

        private class Grid
        {

            public Grid(int size)
            {
                Array = new T[size, size];
            }

            public int Count { get; private set; }

            public T[,] Array { get; private set; }

            public void Remove(int x, int y)
            {
                if(!Equals(Array[x,y], default(T)))
                {
                    Count--;
                    Array[x, y] = default(T);
                }
            }

            public void Add(int x, int y, T value)
            {
                if (!Equals(value, default(T)))
                {
                    Count++;
                    Array[x, y] = value;
                }
            }
        }

        private Grid[,] m_grids;

        public SparseArray2(int width, int height, int gridSize)
        {
            if (width % gridSize != 0)
                throw new ArgumentException("Width must be divisible by grid size.");

            if (height % gridSize != 0)
                throw new ArgumentException("Height must be divisible by grid size.");

            Width = width;
            Height = height;
            GridsX = width / gridSize;
            GridsY = height / gridSize;
            GridSize = gridSize;

            m_grids = new Grid[GridsX, GridsY];
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int GridsX { get; private set; }

        public int GridsY { get; private set; }

        public int GridSize { get; private set; }

        public int GridSize2 { get { return GridSize * GridSize; } }

        public int GridCount { get; private set; }

        public T this[int x, int y]
        {
            get
            {
                int i = x / GridSize;
                int j = y / GridSize;

                var grid = m_grids[i, j];

                int xi = x - i * GridSize;
                int yj = y - j * GridSize;

                if (grid == null)
                    return default(T);
                else
                    return grid.Array[xi, yj];
            }
            set
            {
                int i = x / GridSize;
                int j = y / GridSize;

                var grid = m_grids[i, j];

                int xi = x - i * GridSize;
                int yj = y - j * GridSize;

                if (grid == null)
                {
                    if (Equals(default(T), value))
                        return;
                    else
                    {
                        grid = new Grid(GridSize);
                        grid.Add(xi, yj, value);
                        m_grids[i, j] = grid;
                        GridCount++;
                    }
                }
                else
                {
                    grid.Remove(xi, yj);
                    grid.Add(xi, yj, value);

                    if (grid.Count < 0 || grid.Count > GridSize2)
                        throw new InvalidOperationException("Grid count in invalid state.");

                    if (grid.Count == 0)
                    {
                        m_grids[i, j] = null;
                        GridCount--;
                    }
                }

            }
        }

        public int Count
        {
            get
            {
                int count = 0;

                for (int y = 0; y < GridsY; y++)
                {
                    for (int x = 0; x < GridsX; x++)
                    {
                        if (m_grids[x, y] != null)
                            count += m_grids[x, y].Count;
                    }
                }

                return count;
            }
        }

        public void Clear()
        {
            for (int y = 0; y < GridsY; y++)
            {
                for (int x = 0; x < GridsX; x++)
                {
                    m_grids[x, y] = null;
                }
            }

            GridCount = 0;
        }

    }
}

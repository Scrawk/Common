using System;
using System.Collections;
using System.Collections.Generic;

namespace Common.Collections.Arrays
{
    /// <summary>
    /// A compact array for when most elements have a default value.
    /// Breaks the 2d space into grid. A grid is only allocated if 
    /// it contains a element.
    /// </summary>
    /// <typeparam name="T">The element type</typeparam>
    public class SparseArray2<T> : IArray2<T>
    {

        private Grid[,] m_grids;

        /// <summary>
        /// Create a new sparse array.
        /// </summary>
        /// <param name="width">The size of the arrays 1st dimention.</param>
        /// <param name="height">The size of the arrays 2st dimention.</param>
        /// <param name="gridSize">The size of the grids array is broken into.</param>
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

        /// <summary>
        /// The size of the arrays 1st dimention.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The size of the arrays 2nd dimention.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// The number of grids in the 1st dimension.
        /// </summary>
        public int GridsX { get; private set; }

        /// <summary>
        /// The number of grids in the 2nd dimension.
        /// </summary>
        public int GridsY { get; private set; }

        /// <summary>
        /// The size (width and height) of each grid.
        /// </summary>
        public int GridSize { get; private set; }

        /// <summary>
        /// The grid size squared. Equal to the grids area.
        /// </summary>
        public int GridSize2 { get { return GridSize * GridSize; } }

        /// <summary>
        /// The number of grids that are currently allocated.
        /// </summary>
        public int GridCount { get; private set; }

        /// <summary>
        /// Access a element at index x,y.
        /// </summary>
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

        /// <summary>
        /// The number of elements in the array.
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[SparseArray2: Width={0}, Height={1}, GridSize={2}, Count={3}, GridCount={4}]", 
                Width, Height, GridSize, Count, GridCount);
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach(var grid in m_grids)
            {
                if (grid == null) continue;

                foreach (var t in grid.Array)
                    yield return t;
            }
        }

        /// <summary>
        /// Enumerate all elements in the array.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Sets all elements in the array to default value.
        /// Means all grids are destroyed.
        /// </summary>
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

        /// <summary>
        /// The internal grid class that holds the elements.
        /// </summary>
        private class Grid
        {
            /// <summary>
            /// Create a new grid. Width and height must be the same.
            /// </summary>
            /// <param name="size">Size of 1st and second dimension</param>
            public Grid(int size)
            {
                Array = new T[size, size];
            }

            /// <summary>
            /// The number of elements added to grid.
            /// </summary>
            public int Count { get; private set; }

            /// <summary>
            /// The Array of elements.
            /// </summary>
            public T[,] Array { get; private set; }

            /// <summary>
            /// Remove the element at index x,y. Set element to default.
            /// </summary>
            public void Remove(int x, int y)
            {
                if (!Equals(Array[x, y], default(T)))
                {
                    Count--;
                    Array[x, y] = default(T);
                }
            }

            /// <summary>
            /// Adds element at index x,y. Only added if not default value.
            /// </summary>
            public void Add(int x, int y, T value)
            {
                if (!Equals(value, default(T)))
                {
                    Count++;
                    Array[x, y] = value;
                }
            }
        }

    }
}

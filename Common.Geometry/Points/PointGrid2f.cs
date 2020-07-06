using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

namespace Common.Geometry.Points
{

    /// <summary>
    /// A point collection seperated into spatial grids
    /// confined to a bounding box.
    /// </summary>
    public class PointGrid2f<T> : IPointCollection2f<T>
        where T : IPoint2f
    {

        private List<T>[,] m_grid;

        public PointGrid2f(float width, float height, float cellsize, IEnumerable<T> points = null)
            : this(new Box2f(0, width, 0, height), cellsize, points)
        {

        }

        public PointGrid2f(Box2f bounds, float cellsize, IEnumerable<T> points = null)
        {
            Bounds = bounds;
            int width = (int)Math.Ceiling(bounds.Width / cellsize) + 1;
            int height = (int)Math.Ceiling(bounds.Height / cellsize) + 1;
            GridSize = new Vector2i(width, height);
            CellSize = cellsize;
            InvCellSize = 1.0f / cellsize;

            m_grid = new List<T>[width, height];

            if (points != null)
                Add(points);
        }

        /// <summary>
        /// The number of points in the grid.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The number of cells in grid on each axis.
        /// </summary>
        public Vector2i GridSize { get; private set; }

        /// <summary>
        /// The are the grid bounds;
        /// </summary>
        public Box2f Bounds { get; private set; }

        /// <summary>
        /// The size of the cells in the grid.
        /// </summary>
        public float CellSize { get; private set; }

        /// <summary>
        /// The inverse size of the cells in the grid.
        /// </summary>
        public float InvCellSize { get; private set; }

        /// <summary>
        /// The grids as a string.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[PointGrid2f: Count={0}, GridSize={1}, CellSize={2}, Bounds={3}]",
                Count, GridSize, CellSize, Bounds);
        }

        /// <summary>
        /// Remove all points from the grid.
        /// Retains any allocated lists.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    var list = m_grid[x, y];
                    if (list != null)
                        list.Clear();
                }
            }
        }

        /// <summary>
        /// Adds a enumeration of points to the grid.
        /// Returns false if any of the points was 
        /// outside the grid bounds.
        /// </summary>
        public bool Add(IEnumerable<T> points)
        {
            bool allAdded = true;

            foreach (var p in points)
                if (!Add(p)) allAdded = false;

            return allAdded;
        }

        /// <summary>
        /// Add a point to the grid.
        /// Retruns false if point out of
        /// the grid bounds and will not be added.
        /// </summary>
        public bool Add(T point)
        {
            if (!PointOps2f<T>.Contains(Bounds, point)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index, true);

            cell.Add(point);
            Count++;
            return true;
        }

        /// <summary>
        /// Remove a point from the grid.
        /// </summary>
        public bool Remove(T point)
        {
            if (!PointOps2f<T>.Contains(Bounds, point)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index);
            if (cell == null) return false;

            for (int i = 0; i < cell.Count; i++)
            {
                if (cell[i].x == point.x && cell[i].y == point.y)
                {
                    cell.RemoveAt(i);
                    Count--;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return a list of all points in the grid.
        /// </summary>
        public List<T> ToList()
        {
            List<T> list = new List<T>(Count);

            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    var cell = m_grid[x, y];
                    if (cell != null)
                    {
                        for (int i = 0; i < cell.Count; i++)
                            list.Add(cell[i]);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Return a list of all points found 
        /// within the search region.
        /// </summary>
        public void Search(Circle2f region, List<T> points)
        {
            if (!region.Intersects(Bounds)) return;
            var box = ToCellSpace(region.Bounds);

            for (int y = box.Min.y; y <= box.Max.y; y++)
            {
                for (int x = box.Min.x; x <= box.Max.x; x++)
                {
                    var cell = m_grid[x, y];
                    if (cell == null) continue;

                    int count = cell.Count;
                    for (int k = 0; k < count; k++)
                    {
                        if (PointOps2f<T>.Contains(region, cell[k]))
                            points.Add(cell[k]);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the closest point in the grid.
        /// Grid will only search within a points nearest neighbour cells
        /// so this will fail if closest point not in that region.
        /// In this case returned point will be zero.
        /// </summary>
        public T Closest(T point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find nearest point if collection is empty.");

            T closest;
            Closest(point, out closest);
            return closest;
        }

        /// <summary>
        /// Returns the closest point in the grid.
        /// Grid will only search within a points nearest neighbour cells
        /// and will return false if no point is located in this range.
        /// </summary>
        public bool Closest(T point, out T closest)
        {
            closest = default(T);
            float dist2 = float.PositiveInfinity;
            bool found = false;

            var index = ToCellSpace(point);
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    int xi = index.x + x;
                    int yi = index.y + y;

                    if (xi < 0 || xi >= GridSize.x) continue;
                    if (yi < 0 || yi >= GridSize.y) continue;

                    var cell = m_grid[xi, yi];
                    if (cell == null) continue;

                    for (int k = 0; k < cell.Count; k++)
                    {
                        float d2 = PointOps2f<T>.SqrDistance(cell[k], point);
                        if (d2 < dist2)
                        {
                            closest = cell[k];
                            dist2 = d2;
                            found = true;
                        }
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// Enumerate the points in the grid.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    var cell = m_grid[x, y];
                    if (cell == null) continue;

                    for (int k = 0; k < cell.Count; k++)
                        yield return cell[k];
                }
            }
        }

        /// <summary>
        /// Enumerate the points in the grid.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the points cell space position.
        /// </summary>
        private Vector2i ToCellSpace(T p)
        {
            int x = (int)Math.Floor((p.x - Bounds.Min.x) * InvCellSize);
            int y = (int)Math.Floor((p.y - Bounds.Min.y) * InvCellSize);
            return new Vector2i(x, y);
        }

        /// <summary>
        /// Returns the boxes cell space position.
        /// </summary>
        private Box2i ToCellSpace(Box2f region)
        {
            var min = region.Min - Bounds.Min;
            var max = region.Max - Bounds.Min;
            int minx = (int)Math.Floor(min.x * InvCellSize);
            int miny = (int)Math.Floor(min.y * InvCellSize);
            int maxx = (int)Math.Floor(max.x * InvCellSize);
            int maxy = (int)Math.Floor(max.y * InvCellSize);

            var box = new Box2i(minx, maxx, miny, maxy);
            box.Min.Clamp(Vector2i.Zero, GridSize - 1);
            box.Max.Clamp(Vector2i.Zero, GridSize - 1);
            return box;
        }

        /// <summary>
        /// Returns the cells point list at this index.
        /// If create is true a new empty list will be 
        /// added to the grid and returned.
        /// </summary>
        private List<T> GetGridCell(Vector2i index, bool create = false)
        {
            if (create)
            {
                if (m_grid[index.x, index.y] == null)
                    m_grid[index.x, index.y] = new List<T>();

                return m_grid[index.x, index.y];
            }
            else
            {
                return m_grid[index.x, index.y];
            }
        }
    }
}

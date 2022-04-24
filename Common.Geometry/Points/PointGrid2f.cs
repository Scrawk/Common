using System;
using System.Collections;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Core.Shapes;

namespace Common.Geometry.Points
{

    /// <summary>
    /// A point collection seperated into spatial grids
    /// confined to a bounding box.
    /// </summary>
    public class PointGrid2f : IPointCollection2f
    {

        private List<Point2f>[,] m_grid;

        public PointGrid2f(float width, float height, float cellsize, IEnumerable<Point2f> points = null)
            : this(new Box2f(new Point2f(0, 0), new Point2f(width, height)), cellsize, points)
        {

        }

        public PointGrid2f(Box2f bounds, float cellsize, IEnumerable<Point2f> points = null)
        {
            Bounds = bounds;
            int width = (int)Math.Ceiling(bounds.Width / cellsize) + 1;
            int height = (int)Math.Ceiling(bounds.Height / cellsize) + 1;
            GridSize = new Point2i(width, height);
            CellSize = cellsize;
            InvCellSize = 1.0f / cellsize;

            m_grid = new List<Point2f>[width, height];

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
        public Point2i GridSize { get; private set; }

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
        public bool Add(IEnumerable<Point2f> points)
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
        public bool Add(Point2f point)
        {
            if (!Bounds.Contains(point, true)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index, true);

            cell.Add(point);
            Count++;
            return true;
        }

        /// <summary>
        /// Remove a point from the grid.
        /// </summary>
        public bool Remove(Point2f point)
        {
            if (!Bounds.Contains(point, true)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index);
            if (cell == null) return false;

            for (int i = 0; i < cell.Count; i++)
            {
                if (cell[i].x == point.x && 
                    cell[i].y == point.y)
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
        public List<Point2f> ToList()
        {
            List<Point2f> list = new List<Point2f>(Count);

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
        public void Search(Circle2f region, List<Point2f> points)
        {
            if (!region.Intersects(Bounds, true)) return;
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
                        if (region.Contains(cell[k], true))
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
        public Point2f Closest(Point2f point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find nearest point if collection is empty.");

            Point2f closest;
            Closest(point, out closest);
            return closest;
        }

        /// <summary>
        /// Returns the closest point in the grid.
        /// Grid will only search within a points nearest neighbour cells
        /// and will return false if no point is located in this range.
        /// </summary>
        public bool Closest(Point2f point, out Point2f closest)
        {
            closest = new Point2f();
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
                        float d2 = Point2f.SqrDistance(cell[k], point);
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
        public IEnumerator<Point2f> GetEnumerator()
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
        private Point2i ToCellSpace(Point2f p)
        {
            int x = (int)Math.Floor((p.x - Bounds.Min.x) * InvCellSize);
            int y = (int)Math.Floor((p.y - Bounds.Min.y) * InvCellSize);
            return new Point2i(x, y);
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

            var box = new Box2i(new Point2i(minx, miny), new Point2i(maxx, maxy));
            box.Min = Point2i.Clamp(box.Min, Point2i.Zero, GridSize - 1);
            box.Max = Point2i.Clamp(box.Max, Point2i.Zero, GridSize - 1);
            return box;
        }

        /// <summary>
        /// Returns the cells point list at this index.
        /// If create is true a new empty list will be 
        /// added to the grid and returned.
        /// </summary>
        private List<Point2f> GetGridCell(Point2i index, bool create = false)
        {
            if (create)
            {
                if (m_grid[index.x, index.y] == null)
                    m_grid[index.x, index.y] = new List<Point2f>();

                return m_grid[index.x, index.y];
            }
            else
            {
                return m_grid[index.x, index.y];
            }
        }
    }
}

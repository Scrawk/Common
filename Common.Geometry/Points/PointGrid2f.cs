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
    public class PointGrid2f : IPointCollection2f
    {
        /// <summary>
        /// The entry in the cell that holds 
        /// the point and the point count 
        /// when point was added. This would 
        /// be the point index in orginal source.
        /// If a point is removed and another added then
        /// these indices will no longer be correct.
        /// </summary>
        private struct PointEntry
        {
            public Vector2f point;
            public int index;
        }

        private List<PointEntry>[,] m_grid;

        public PointGrid2f(float width, float height, float cellsize, IEnumerable<Vector2f> points = null)
            : this(new Box2f(0, width, 0, height), cellsize, points)
        {

        }

        public PointGrid2f(Box2f bounds, float cellsize, IEnumerable<Vector2f> points = null)
        {
            Bounds = bounds;
            int width = (int)Math.Ceiling(bounds.Width / cellsize) + 1;
            int height = (int)Math.Ceiling(bounds.Height / cellsize) + 1;
            GridSize = new Vector2i(width, height);
            CellSize = cellsize;
            InvCellSize = 1.0f / cellsize;

            m_grid = new List<PointEntry>[width, height];

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
        public bool Add(IEnumerable<Vector2f> points)
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
        public bool Add(Vector2f point)
        {
            if (!Bounds.Contains(point)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index, true);

            var entry = new PointEntry();
            entry.point = point;
            entry.index = Count;

            cell.Add(entry);
            Count++;
            return true;
        }

        /// <summary>
        /// Remove a point from the grid.
        /// </summary>
        public bool Remove(Vector2f point)
        {
            if (!Bounds.Contains(point)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index);
            if (cell == null) return false;

            for (int i = 0; i < cell.Count; i++)
            {
                if (cell[i].point == point)
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
        public List<Vector2f> ToList()
        {
            List<Vector2f> list = new List<Vector2f>(Count);

            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    var cell = m_grid[x, y];
                    if (cell != null)
                    {
                        for (int i = 0; i < cell.Count; i++)
                            list.Add(cell[i].point);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Return a list of all points found 
        /// within the search region.
        /// </summary>
        public void Search(Circle2f region, List<Vector2f> points)
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
                        if (region.Contains(cell[k].point))
                            points.Add(cell[k].point);
                    }
                }
            }
        }

        /// <summary>
        /// Return a list of all point indices found 
        /// within the search region.
        /// </summary>
        public void Search(Circle2f region, List<int> indices)
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
                        if (region.Contains(cell[k].point))
                            indices.Add(cell[k].index);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the signed distance to the closest point in th grid.
        /// Grid will only search within a points nearest neighbour cells
        /// so this will fail if closest point not in that region.
        /// In this case returned dist will be zero.
        /// </summary>
        public float SignedDistance(Vector2f point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find signed distance if collection is empty.");

            return Vector2f.Distance(point, Closest(point));
        }

        /// <summary>
        /// Returns the closest point in the grid.
        /// Grid will only search within a points nearest neighbour cells
        /// so this will fail if closest point not in that region.
        /// In this case returned point will be zero.
        /// </summary>
        public Vector2f Closest(Vector2f point)
        {
            if (Count == 0)
                throw new InvalidOperationException("Can not find nearest point if collection is empty.");

            Vector2f closest;
            Closest(point, out closest);
            return closest;
        }

        /// <summary>
        /// Returns the closest point in the grid.
        /// Grid will only search within a points nearest neighbour cells
        /// and will return false if no point is located in this range.
        /// </summary>
        public bool Closest(Vector2f point, out Vector2f closest)
        {
            closest = new Vector2f();
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
                        float d2 = Vector2f.SqrDistance(point, cell[k].point);
                        if (d2 < dist2)
                        {
                            closest = cell[k].point;
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
        public IEnumerator<Vector2f> GetEnumerator()
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    var cell = m_grid[x, y];
                    if (cell == null) continue;

                    for (int k = 0; k < cell.Count; k++)
                        yield return cell[k].point;
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
        private Vector2i ToCellSpace(Vector2f p)
        {
            p -= Bounds.Min;
            int x = (int)Math.Floor(p.x * InvCellSize);
            int y = (int)Math.Floor(p.y * InvCellSize);
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
        private List<PointEntry> GetGridCell(Vector2i index, bool create = false)
        {
            if (create)
            {
                if (m_grid[index.x, index.y] == null)
                    m_grid[index.x, index.y] = new List<PointEntry>();

                return m_grid[index.x, index.y];
            }
            else
            {
                return m_grid[index.x, index.y];
            }
        }
    }
}

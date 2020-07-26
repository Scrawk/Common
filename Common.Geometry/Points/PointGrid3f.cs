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
    public class PointGrid3f<T> : IPointCollection3f<T>
            where T : IPoint3f
    {

        private List<T>[,,] m_grid;

        public PointGrid3f(float width, float height, float depth, float cellsize, IEnumerable<T> points = null)
            : this(new Box3f(0, width, 0, height, 0, depth), cellsize, points)
        {

        }

        public PointGrid3f(Box3f bounds, float cellsize, IEnumerable<T> points = null)
        {
            Bounds = bounds;
            var width = (int)Math.Ceiling(bounds.Width / cellsize) + 1;
            var height = (int)Math.Ceiling(bounds.Height / cellsize) + 1;
            var depth = (int)Math.Ceiling(bounds.Depth / cellsize) + 1;
            GridSize = new Vector3i(width, height, depth);
            CellSize = cellsize;
            InvCellSize = 1.0f / cellsize;

            m_grid = new List<T>[width, height, depth];

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
        public Vector3i GridSize { get; private set; }

        /// <summary>
        /// The are the grid bounds;
        /// </summary>
        public Box3f Bounds { get; private set; }

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
            return string.Format("[PointGrid3f: Count={0}, GridSize={1}, CellSize={2}, Bounds={3}]",
                Count, GridSize, CellSize, Bounds);
        }

        /// <summary>
        /// Remove all points from the grid.
        /// Retains any allocated lists.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            for (int z = 0; z < GridSize.z; z++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    for (int x = 0; x < GridSize.x; x++)
                    {
                        var list = m_grid[x, y, z];
                        if (list != null)
                            list.Clear();
                    }
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
            if (!PointOps3f<T>.Contains(Bounds, point)) return false;

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
            if (!PointOps3f<T>.Contains(Bounds, point)) return false;

            var index = ToCellSpace(point);
            var cell = GetGridCell(index);
            if (cell == null) return false;

            for (int i = 0; i < cell.Count; i++)
            {
                if (cell[i].x == point.x && 
                    cell[i].y == point.y && 
                    cell[i].z == point.z)
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

            for (int z = 0; z < GridSize.z; z++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    for (int x = 0; x < GridSize.x; x++)
                    {
                        var cell = m_grid[x, y, z];
                        if (cell != null)
                        {
                            for (int i = 0; i < cell.Count; i++)
                                list.Add(cell[i]);
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Return a list of all points found 
        /// within the search region.
        /// </summary>
        public void Search(Sphere3f region, List<T> points)
        {
            if (!region.Intersects(Bounds)) return;
            var box = ToCellSpace(region.Bounds);

            for (int z = box.Min.z; z <= box.Max.z; z++)
            {
                for (int y = box.Min.y; y <= box.Max.y; y++)
                {
                    for (int x = box.Min.x; x <= box.Max.x; x++)
                    {
                        var cell = m_grid[x, y, z];
                        if (cell == null) continue;

                        int count = cell.Count;
                        for (int k = 0; k < count; k++)
                        {
                            if (PointOps3f<T>.Contains(region, cell[k]))
                                points.Add(cell[k]);
                        }
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
            for (int z = -1; z <= 1; z++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        int xi = index.x + x;
                        int yi = index.y + y;
                        int zi = index.z + z;

                        if (xi < 0 || xi >= GridSize.x) continue;
                        if (yi < 0 || yi >= GridSize.y) continue;
                        if (zi < 0 || zi >= GridSize.z) continue;

                        var cell = m_grid[xi, yi, zi];
                        if (cell == null) continue;

                        for (int k = 0; k < cell.Count; k++)
                        {
                            float d2 = PointOps3f<T>.SqrDistance(point, cell[k]);
                            if (d2 < dist2)
                            {
                                closest = cell[k];
                                dist2 = d2;
                                found = true;
                            }
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
            for (int z = 0; z < GridSize.z; z++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    for (int x = 0; x < GridSize.x; x++)
                    {
                        var cell = m_grid[x, y, z];
                        if (cell == null) continue;

                        for (int k = 0; k < cell.Count; k++)
                            yield return cell[k];
                    }
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
        private Vector3i ToCellSpace(T p)
        {
            int x = (int)Math.Floor((p.x - Bounds.Min.x) * InvCellSize);
            int y = (int)Math.Floor((p.y - Bounds.Min.y) * InvCellSize);
            int z = (int)Math.Floor((p.z - Bounds.Min.z) * InvCellSize);
            return new Vector3i(x, y, z);
        }

        /// <summary>
        /// Returns the boxes cell space position.
        /// </summary>
        private Box3i ToCellSpace(Box3f region)
        {
            var min = region.Min - Bounds.Min;
            var max = region.Max - Bounds.Min;
            int minx = (int)Math.Floor(min.x * InvCellSize);
            int miny = (int)Math.Floor(min.y * InvCellSize);
            int minz = (int)Math.Floor(min.z * InvCellSize);
            int maxx = (int)Math.Floor(max.x * InvCellSize);
            int maxy = (int)Math.Floor(max.y * InvCellSize);
            int maxz = (int)Math.Floor(max.z * InvCellSize);

            var box = new Box3i(minx, maxx, miny, maxy, minz, maxz);
            box.Min.Clamp(Vector3i.Zero, GridSize - 1);
            box.Max.Clamp(Vector3i.Zero, GridSize - 1);
            return box;
        }

        /// <summary>
        /// Returns the cells point list at this index.
        /// If create is true a new empty list will be 
        /// added to the grid and returned.
        /// </summary>
        private List<T> GetGridCell(Vector3i index, bool create = false)
        {
            if (create)
            {
                if (m_grid[index.x, index.y, index.z] == null)
                    m_grid[index.x, index.y, index.z] = new List<T>();

                return m_grid[index.x, index.y, index.z];
            }
            else
            {
                return m_grid[index.x, index.y, index.z];
            }
        }
    }
}

using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Collections.Sets
{
    public class ConnectedGridSet2
    {
        public const int UNASSIGNED = -1;
        public const int BACKGROUND = 0;
        public const int FOREGROUND = 1;

        private static readonly int[,] OFFSETS = new int[,]
        {
                {-1,0},{-1,-1},{0,-1},{1,-1},
                {1,0},{1,1},{0,1}, {-1,1}
        };

        private int[,] m_labels;

        private DisjointSet m_set;

        private Func<int, int, bool> IsForeGround;

        public ConnectedGridSet2(int width, int height, Func<int, int, bool> isForeGround)
        {
            Width = width;
            Height = height;
            m_labels = new int[width, height];
            m_set = new DisjointSet();
            IsForeGround = isForeGround;

            Clear();
        }

        /// <summary>
        /// The width of the grid.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// The height of the grid.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("[ConnectedGridSet2: Width={0}, Height={1}]", Width, Height);
        }

        public void Clear()
        {
            m_set.Clear();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    m_labels[x, y] = UNASSIGNED;
                }
            }
        }

        public bool InGrid(int x, int y)
        {
            if (x < 0 || x >= Width) return false;
            if (y < 0 || y >= Height) return false;
            return true;
        }

        public int GetLabel(int x, int y)
        {
            return m_labels[x, y];
        }

        private void SetLabel(int x, int y, int label)
        {
            m_labels[x, y] = label;
        }

        public void Label()
        {
            int[,] OFFSETS = new int[,]
            {
                {-1,0},{-1,-1},{0,-1},{1,-1},
                {1,0},{1,1},{0,1}, {-1,1}
            };

            int currentLabel = 0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (!IsForeGround(x, y)) continue;

                    bool found = false;

                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + OFFSETS[i, 0];
                        int yi = y + OFFSETS[i, 1];

                        if (!InGrid(xi, yi)) continue;
                        if (!IsForeGround(xi, yi)) continue;
                        if (GetLabel(xi, yi) == UNASSIGNED) continue;

                        SetLabel(x, y, GetLabel(xi, yi));
                        found = true;
                        break;
                    }

                    if(!found)
                    {
                        SetLabel(x, y, currentLabel);
                        currentLabel++;
                    }

                }
            }

            m_set.AddRange(currentLabel);
            var neighbours = new List<Vector2i>();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (GetLabel(x, y) == UNASSIGNED) continue;

                    neighbours.Clear();
                    for (int i = 0; i < 8; i++)
                    {
                        int xi = x + OFFSETS[i, 0];
                        int yi = y + OFFSETS[i, 1];

                        if (!InGrid(xi, yi)) continue;
                        if (GetLabel(xi, yi) == UNASSIGNED) continue;

                        neighbours.Add(new Vector2i(xi, yi));
                    }

                    if(neighbours.Count > 0)
                    {
                        int label = GetLabel(x, y);
   
                        for (int i = 0; i < neighbours.Count; i++)
                        {
                            var n = neighbours[i];
                            m_set.Union(GetLabel(n.x, n.y), label);
                            SetLabel(n.x, n.y, label);
                        }
                            
                    }

                }
            }

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int label = GetLabel(x, y);
                    if (label == UNASSIGNED) continue;

                    SetLabel(x, y, m_set.FindParent(label));
                }
            }
        }

    }
}

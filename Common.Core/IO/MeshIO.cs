using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Common.Core.Numerics;
using Common.Core.Colors;

namespace Common.Core.IO
{
    public class MeshProperties3d
    {
        public string FileName;
        public string Extension;
        public bool FlipTriangles;
        public List<Vector3d> Positions = new List<Vector3d>();
        public List<int> PositionIndices = new List<int>();
        public List<Vector3d> Normals = new List<Vector3d>();
        public List<int> NormalIndices = new List<int>();
        public List<Vector2d> TexCoords = new List<Vector2d>();
        public List<int> TexCoordIndices = new List<int>();
    }

    public class MeshProperties3f
    {
        public string FileName;
        public string Extension;
        public bool FlipTriangles;
        public List<Vector3f> Positions = new List<Vector3f>();
        public List<int> PositionIndices = new List<int>();
        public List<Vector3f> Normals = new List<Vector3f>();
        public List<int> NormalIndices = new List<int>();
        public List<Vector2f> TexCoords = new List<Vector2f>();
        public List<int> TexCoordIndices = new List<int>();
    }

    public static class MeshIO
    {
        public static void FromOBJ(MeshProperties3d properties, StreamReader streamReader)
        {
            var space = new char[] { ' ', '\t' };
            var slash = new char[] { '/' };
            var options = StringSplitOptions.RemoveEmptyEntries;

            var positions = properties.Positions;
            var pIndices = properties.PositionIndices;
            var normals = properties.Normals;
            var nIndices = properties.NormalIndices;
            var uvs = properties.TexCoords;
            var uvIndices = properties.TexCoordIndices;
            var flip = properties.FlipTriangles;

            using (streamReader)
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line[0] == 'f')
                    {
                        string[] elements = line.Split(space, options);

                        if (elements.Length == 4)
                        {
                            string[] face1 = elements[1].Split(slash, StringSplitOptions.None);
                            string[] face2 = elements[2].Split(slash, StringSplitOptions.None);
                            string[] face3 = elements[3].Split(slash, StringSplitOptions.None);

                            if (flip)
                            {
                                var tmp = face1;
                                face1 = face3;
                                face3 = tmp;
                            }

                            pIndices.Add(int.Parse(face1[0]) - 1);
                            pIndices.Add(int.Parse(face2[0]) - 1);
                            pIndices.Add(int.Parse(face3[0]) - 1);
                        }
                    }
                    else if (line[0] == 'v')
                    {
                        string[] elements = line.Split(space, options);

                        if (line[1] == ' ')
                        {
                            Vector3d v;
                            v.x = double.Parse(elements[1]);
                            v.y = double.Parse(elements[2]);
                            v.z = double.Parse(elements[3]);

                            positions.Add(v);
                        }
                        else if (line[1] == 'n')
                        {
                            Vector3d v;
                            v.x = double.Parse(elements[1]);
                            v.y = double.Parse(elements[2]);
                            v.z = double.Parse(elements[3]);

                            normals.Add(v.Normalized);
                        }
                        else if (line[1] == 't')
                        {
                            Vector2d v;
                            v.x = double.Parse(elements[1]);
                            v.y = double.Parse(elements[2]);

                            uvs.Add(v);
                        }
                    }

                }
            }
        }

        public static void FromOBJ(MeshProperties3f properties, StreamReader streamReader)
        {
            var space = new char[] { ' ', '\t' };
            var slash = new char[] { '/' };
            var options = StringSplitOptions.RemoveEmptyEntries;

            var positions = properties.Positions;
            var pIndices = properties.PositionIndices;
            var normals = properties.Normals;
            var nIndices = properties.NormalIndices;
            var uvs = properties.TexCoords;
            var uvIndices = properties.TexCoordIndices;
            var flip = properties.FlipTriangles;

            using (streamReader)
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    if (line[0] == 'f')
                    {
                        string[] elements = line.Split(space, options);

                        if (elements.Length == 4)
                        {
                            string[] face1 = elements[1].Split(slash, StringSplitOptions.None);
                            string[] face2 = elements[2].Split(slash, StringSplitOptions.None);
                            string[] face3 = elements[3].Split(slash, StringSplitOptions.None);

                            if (flip)
                            {
                                var tmp = face1;
                                face1 = face3;
                                face3 = tmp;
                            }

                            pIndices.Add(int.Parse(face1[0]) - 1);
                            pIndices.Add(int.Parse(face2[0]) - 1);
                            pIndices.Add(int.Parse(face3[0]) - 1);
                        }
                    }
                    else if (line[0] == 'v')
                    {
                        string[] elements = line.Split(space, options);

                        if (line[1] == ' ')
                        {
                            Vector3f v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);
                            v.z = float.Parse(elements[3]);

                            positions.Add(v);
                        }
                        else if (line[1] == 'n')
                        {
                            Vector3f v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);
                            v.z = float.Parse(elements[3]);

                            normals.Add(v.Normalized);
                        }
                        else if (line[1] == 't')
                        {
                            Vector2f v;
                            v.x = float.Parse(elements[1]);
                            v.y = float.Parse(elements[2]);

                            uvs.Add(v);
                        }
                    }

                }
            }
        }

        public static string ToObj(MeshProperties3d properties)
        {
            var positions = properties.Positions;
            var pIndices = properties.PositionIndices;
            var normals = properties.Normals;
            var nIndices = properties.NormalIndices;
            var uvs = properties.TexCoords;
            var uvIndices = properties.TexCoordIndices;
            var flip = properties.FlipTriangles;

            bool hasUVs = uvs != null && uvs.Count != 0 && uvIndices.Count == pIndices.Count;
            bool hasNormals = normals != null && normals.Count != 0 && nIndices.Count == pIndices.Count;

            StringBuilder sb = new StringBuilder();

            foreach (Vector3d v in positions)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");

            foreach (Vector3d v in normals)
            {
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");

            foreach (Vector2d v in uvs)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }
            sb.Append("\n");

            for (int i = 0; i < pIndices.Count; i += 3)
            {
                int i0 = i + 0;
                int i1 = i + 1;
                int i2 = i + 2;

                if (flip)
                {
                    int tmp = i0;
                    i0 = i2;
                    i2 = tmp;
                }

                if (hasUVs && hasNormals)
                {
                    sb.Append(string.Format("f {0}/{1}/{2} {3}/{4}/{4} {6}/{7}/{8}\n",
                        pIndices[i0] + 1, uvIndices[i0] + 1, nIndices[i0] + 1,
                        pIndices[i1] + 1, uvIndices[i1] + 1, nIndices[i1] + 1,
                        pIndices[i2] + 1, uvIndices[i2] + 1, nIndices[i2] + 1));
                }
                else if (hasUVs)
                {
                    sb.Append(string.Format("f {0}/{1} {2}/{3} {4}/{5}\n",
                        pIndices[i0] + 1, uvIndices[i0] + 1,
                        pIndices[i1] + 1, uvIndices[i1] + 1,
                        pIndices[i2] + 1, uvIndices[i2] + 1));
                }
                else if (hasNormals)
                {
                    sb.Append(string.Format("f {0}//{1} {2}//{3} {4}//{5}\n",
                        pIndices[i0] + 1, nIndices[i0] + 1,
                        pIndices[i1] + 1, nIndices[i1] + 1,
                        pIndices[i2] + 1, nIndices[i2] + 1));
                }
                else
                {
                    sb.Append(string.Format("f {0} {1} {2}\n",
                        pIndices[i0] + 1,
                        pIndices[i1] + 1,
                        pIndices[i2] + 1));
                }
            }

            return sb.ToString();
        }

        public static string ToObj(MeshProperties3f properties)
        {
            var positions = properties.Positions;
            var pIndices = properties.PositionIndices;
            var normals = properties.Normals;
            var nIndices = properties.NormalIndices;
            var uvs = properties.TexCoords;
            var uvIndices = properties.TexCoordIndices;
            var flip = properties.FlipTriangles;

            bool hasUVs = uvs != null && uvs.Count != 0 && uvIndices.Count == pIndices.Count;
            bool hasNormals = normals != null && normals.Count != 0 && nIndices.Count == pIndices.Count;

            StringBuilder sb = new StringBuilder();

            foreach (Vector3f v in positions)
            {
                sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");

            foreach (Vector3f v in normals)
            {
                sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
            }
            sb.Append("\n");

            foreach (Vector2f v in uvs)
            {
                sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
            }
            sb.Append("\n");

            for (int i = 0; i < pIndices.Count; i += 3)
            {
                int i0 = i + 0;
                int i1 = i + 1;
                int i2 = i + 2;

                if (flip)
                {
                    int tmp = i0;
                    i0 = i2;
                    i2 = tmp;
                }

                if (hasUVs && hasNormals)
                {
                    sb.Append(string.Format("f {0}/{1}/{2} {3}/{4}/{4} {6}/{7}/{8}\n",
                        pIndices[i0] + 1, uvIndices[i0] + 1, nIndices[i0] + 1,
                        pIndices[i1] + 1, uvIndices[i1] + 1, nIndices[i1] + 1,
                        pIndices[i2] + 1, uvIndices[i2] + 1, nIndices[i2] + 1));
                }
                else if (hasUVs)
                {
                    sb.Append(string.Format("f {0}/{1} {2}/{3} {4}/{5}\n",
                        pIndices[i0] + 1, uvIndices[i0] + 1,
                        pIndices[i1] + 1, uvIndices[i1] + 1,
                        pIndices[i2] + 1, uvIndices[i2] + 1));
                }
                else if (hasNormals)
                {
                    sb.Append(string.Format("f {0}//{1} {2}//{3} {4}//{5}\n",
                        pIndices[i0] + 1, nIndices[i0] + 1,
                        pIndices[i1] + 1, nIndices[i1] + 1,
                        pIndices[i2] + 1, nIndices[i2] + 1));
                }
                else
                {
                    sb.Append(string.Format("f {0} {1} {2}\n",
                        pIndices[i0] + 1,
                        pIndices[i1] + 1,
                        pIndices[i2] + 1));
                }
            }

            return sb.ToString();
        }

    }
}

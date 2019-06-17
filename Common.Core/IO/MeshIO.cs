using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Common.Core.LinearAlgebra;

namespace Common.Core.IO
{
    public class MeshProperties3d
    {
        public string FileName;
        public string Extension;
        public bool FilpTriangles;
        public List<Vector3d> Positions = new List<Vector3d>();
        public List<Vector3d> Normals = new List<Vector3d>();
        public List<int> Indices = new List<int>();
        public List<Vector2d> TexCoords = new List<Vector2d>();
    }

    public static class MeshIO
    {
        public static void FromOBJ(MeshProperties3d properties, StreamReader streamReader)
        {
            var space = new char[] { ' ', '\t' };
            var slash = new char[] { '/' };
            var options = StringSplitOptions.RemoveEmptyEntries;

            var positions = properties.Positions;
            var normals = properties.Normals;
            var indices = properties.Indices;
            var uvs = properties.TexCoords;
            var flip = properties.FilpTriangles;

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
                                indices.Add(int.Parse(face3[0]) - 1);
                                indices.Add(int.Parse(face2[0]) - 1);
                                indices.Add(int.Parse(face1[0]) - 1);
                            }
                            else
                            {
                                indices.Add(int.Parse(face1[0]) - 1);
                                indices.Add(int.Parse(face2[0]) - 1);
                                indices.Add(int.Parse(face3[0]) - 1);
                            }
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

        public static string ToObj(MeshProperties3d properties)
        {
            var positions = properties.Positions;
            var normals = properties.Normals;
            var indices = properties.Indices;
            var uvs = properties.TexCoords;
            var flip = properties.FilpTriangles;

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

            for (int i = 0; i < indices.Count; i += 3)
            {
                if(flip)
                    sb.Append(string.Format("f {0} {1} {2}\n", indices[i + 2] + 1, indices[i + 1] + 1, indices[i + 0] + 1));
                else
                    sb.Append(string.Format("f {0} {1} {2}\n", indices[i + 0] + 1, indices[i + 1] + 1, indices[i + 2] + 1));
            }

            return sb.ToString();
        }

    }
}

using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Nurbs;

using Common.Console.Collections;
using Common.Console.Geometry;

using CONSOLE = System.Console;

namespace Common.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new Vector3d[]
            {
                new Vector3d(-10,0),
                new Vector3d(10,0),
                new Vector3d(10,10),
                new Vector3d(0,10),
                new Vector3d(5,5)
            };

            var curve = NurbsMake.FromPoints(3, points);

            for(int i = 0; i < curve.ControlPoints.Length; i++)
                CONSOLE.WriteLine(curve.ControlPoints[i]);


        }
    }
}

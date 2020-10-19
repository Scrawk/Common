using System;
using System.Collections.Generic;

using Common.Core.Numerics;
using Common.Geometry.Shapes;

using REAL = System.Single;
using VECTOR2 = Common.Core.Numerics.Vector2f;
using BOX2 = Common.Geometry.Shapes.Box2f;

namespace Common.Geometry.Collections
{
    public interface IStaticShapeCollection2f<T> : IEnumerable<T>
        where T : class, IShape2f
    {
        /// <summary>
        /// The number of shapes in the collection.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Does a shape contain point.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        T Contains(VECTOR2 point);

        /// <summary>
        /// Find all the shapes that contain the point and 
        /// add them to the list
        /// </summary>
        /// <param name="point"></param>
        /// <param name="shapes"></param>
        void Containing(VECTOR2 point, List<T> shapes);

        /// <summary>
        /// Does a shape intersect the box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        T Intersects(BOX2 box);

        /// <summary>
        /// Find all the shapes that intersect the box and 
        /// add them to the list.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="shapes"></param>
        void Intersecting(BOX2 box, List<T> shapes);

        /// <summary>
        /// Return the signed distance field from 
        /// the union of all shapes in the collection.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        REAL SignedDistance(VECTOR2 point);

        /// <summary>
        /// Return a list of all the shapes in the collection.
        /// </summary>
        /// <returns></returns>
        List<T> ToList();
    }

    public interface IShapeCollection2f<T> : IStaticShapeCollection2f<T>
        where T : class, IShape2f
    {

        /// <summary>
        /// Clear the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Add the shapes to the collection.
        /// </summary>
        /// <param name="shapes"></param>
        void Add(IEnumerable<T> shapes);

        /// <summary>
        /// Add a shape to the collection.
        /// </summary>
        /// <param name="shape"></param>
        void Add(T shape);

        /// <summary>
        /// Remove a shape from the collection.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        bool Remove(T shape);

    }
}

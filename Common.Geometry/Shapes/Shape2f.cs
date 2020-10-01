using System;
using System.Collections.Generic;

using Common.Core.Numerics;

namespace Common.Geometry.Shapes
{

    public interface IShape2f
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        Box2f Bounds { get; }

        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool Contains(Vector2f p);

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        bool Intersects(Box2f box);

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Vector2f Closest(Vector2f p);

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        float SignedDistance(Vector2f p);
    }

    public abstract class Shape2f : IShape2f
    {
        /// <summary>
        /// The bounding box that contains the shape.
        /// </summary>
        public abstract Box2f Bounds { get; }


        /// <summary>
        /// Does the shape contain the point.
        /// Points on the shapes surface count as 
        /// being contained in the shape.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contains(Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Does the shape intersect the box.
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public virtual bool Intersects(Box2f box)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The closest point to the shape.
        /// If point inside shape return the same point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual Vector2f Closest(Vector2f p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The signed distance between the shapes surface and the point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Positive if outside shape, negative if inside and 0 on boundary</returns>
        public virtual float SignedDistance(Vector2f p)
        {
            throw new NotImplementedException();
        }
    }
}

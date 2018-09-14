﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Core.LinearAlgebra;
using Common.Geometry.Intersections;
using Common.Geometry.Shapes;

namespace Common.Geometry.Test.Intersections
{
    [TestClass]
    public class Geometry_Intersections_SegmentIntersetions2fTest
    {
        [TestMethod]
        public void SegmentIntersectsSegment()
        {
            Vector2f point;

            Segment2f seg0, seg1, seg2;

            seg0.A = new Vector2f(0, 0);
            seg0.B = new Vector2f(1, 0);

            seg1.A = new Vector2f(0, 0);
            seg1.B = new Vector2f(0, 1);

            seg2.A = new Vector2f(1, 0);
            seg2.B = new Vector2f(1, 1);

            //parallel
            Assert.IsFalse(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg1, out point));
            Assert.IsFalse(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg2, out point));

            seg1.A = new Vector2f(0, -1);
            seg1.B = new Vector2f(0, 1);

            seg2.A = new Vector2f(1, -1);
            seg2.B = new Vector2f(1, 1);

            //intersect start/end
            Assert.IsFalse(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg1, out point));
            Assert.IsFalse(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg2, out point));

            seg1.A = new Vector2f(0.25f, -1);
            seg1.B = new Vector2f(0.25f, 1);

            seg2.A = new Vector2f(0.75f, -1);
            seg2.B = new Vector2f(0.75f, 1);

            //intersect
            Assert.IsTrue(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg1, out point));
            Assert.IsTrue(SegmentIntersections2f.SegmentIntersectsSegment(seg0, seg2, out point));

        }

    }

}

// PolyBoolCS is a C# port of the polybooljs library
// polybooljs is (c) Copyright 2016, Sean Connelly (@voidqk), http://syntheti.cc
// MIT License


namespace PolyBoolCS
{
	using System;
	using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Provides the raw computation functions that takes epsilon into account.
    /// zero is defined to be between (-epsilon, epsilon) exclusive
    /// </summary>
    public class Epsilon
	{
		#region Static variables

		private const float eps = 1e-10f;

		#endregion

		#region Public functions

		public static bool pointAboveOrOnLine( Vector2 pt, Vector2 left, Vector2 right )
		{
			var Ax = left.X;
			var Ay = left.Y;
			var Bx = right.X;
			var By = right.Y;
			var Cx = pt.X;
			var Cy = pt.Y;

			return ( Bx - Ax ) * ( Cy - Ay ) - ( By - Ay ) * ( Cx - Ax ) >= -eps;
		}

		public static bool pointBetween( Vector2 pt, Vector2 left, Vector2 right )
		{
			// p must be collinear with left->right
			// returns false if p == left, p == right, or left == right
			var d_py_ly = pt.Y - left.Y;
			var d_rx_lx = right.X - left.X;
			var d_px_lx = pt.X - left.X;
			var d_ry_ly = right.Y - left.Y;

			var dot = d_px_lx * d_rx_lx + d_py_ly * d_ry_ly;

			// if `dot` is 0, then `p` == `left` or `left` == `right` (reject)
			// if `dot` is less than 0, then `p` is to the left of `left` (reject)
			if( dot < eps )
				return false;

			var sqlen = d_rx_lx * d_rx_lx + d_ry_ly * d_ry_ly;

			// if `dot` > `sqlen`, then `p` is to the right of `right` (reject)
			// therefore, if `dot - sqlen` is greater than 0, then `p` is to the right of `right` (reject)
			if( dot - sqlen > -eps )
				return false;

			return true;
		}

		public static bool pointsSameX( Vector2 p1, Vector2 p2 )
		{
			return Math.Abs( p1.X - p2.X ) < eps;
		}

		public static bool pointsSameY( Vector2 p1, Vector2 p2 )
		{
			return Math.Abs( p1.Y - p2.Y ) < eps;
		}

		public static bool pointsSame( Vector2 p1, Vector2 p2 )
		{
			return
				Math.Abs( p1.X - p2.X ) < eps &&
				Math.Abs( p1.Y - p2.Y ) < eps;
		}

		public static int pointsCompare( Vector2 p1, Vector2 p2 )
		{
			// returns -1 if p1 is smaller, 1 if p2 is smaller, 0 if equal
			if( pointsSameX( p1, p2 ) )
				return pointsSameY( p1, p2 ) ? 0 : ( p1.Y < p2.Y ? -1 : 1 );

			return p1.X < p2.X ? -1 : 1;
		}

		public static bool pointsCollinear( Vector2 p1, Vector2 p2, Vector2 p3 )
		{
			// does pt1->pt2->pt3 make a straight line?
			// essentially this is just checking to see if the slope(pt1->pt2) === slope(pt2->pt3)
			// if slopes are equal, then they must be collinear, because they share pt2
			var dx1 = p1.X - p2.X;
			var dy1 = p1.Y - p2.Y;
			var dx2 = p2.X - p3.X;
			var dy2 = p2.Y - p3.Y;

			return Math.Abs( dx1 * dy2 - dx2 * dy1 ) < eps;
		}

		public static bool linesIntersect( Vector2 a0, Vector2 a1, Vector2 b0, Vector2 b1, out Intersection intersection )
		{
			// returns false if the lines are coincident (e.g., parallel or on top of each other)
			//
			// returns an object if the lines intersect:
			//   {
			//     pt: [x, y],    where the intersection point is at
			//     alongA: where intersection point is along A,
			//     alongB: where intersection point is along B
			//   }
			//
			//  alongA and alongB will each be one of: -2, -1, 0, 1, 2
			//
			//  with the following meaning:
			//
			//    -2   intersection point is before segment's first point
			//    -1   intersection point is directly on segment's first point
			//     0   intersection point is between segment's first and second points (exclusive)
			//     1   intersection point is directly on segment's second point
			//     2   intersection point is after segment's second point

			var adx = a1.X - a0.X;
			var ady = a1.Y - a0.Y;
			var bdx = b1.X - b0.X;
			var bdy = b1.Y - b0.Y;

			var axb = adx * bdy - ady * bdx;
			if( Math.Abs( axb ) < eps )
			{
				intersection = Intersection.Empty;
				return false; // lines are coincident
			}

			var dx = a0.X - b0.X;
			var dy = a0.Y - b0.Y;

			var A = ( bdx * dy - bdy * dx ) / axb;
			var B = ( adx * dy - ady * dx ) / axb;

			intersection = new Intersection()
			{
				alongA = 0,
				alongB = 0,
				pt = new Vector2()
				{
					X = a0.X + A * adx,
					Y = a0.Y + A * ady
				}
			};

			// categorize where intersection point is along A and B

			if( A <= -eps )
				intersection.alongA = -2;
			else if( A < eps )
				intersection.alongA = -1;
			else if( A - 1 <= -eps )
				intersection.alongA = 0;
			else if( A - 1 < eps )
				intersection.alongA = 1;
			else
				intersection.alongA = 2;

			if( B <= -eps )
				intersection.alongB = -2;
			else if( B < eps )
				intersection.alongB = -1;
			else if( B - 1 <= -eps )
				intersection.alongB = 0;
			else if( B - 1 < eps )
				intersection.alongB = 1;
			else
				intersection.alongB = 2;

			return true;
		}

		#endregion
	}
}
using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// A set of helpful methods for working with rectangles.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Calculates the signed depth of intersection between two rectangles.
        /// </summary>
        /// <returns>
        /// The amount of overlap between two intersecting rectangles. These
        /// depth values can be negative depending on which wides the rectangles
        /// intersect. This allows callers to determine the correct direction
        /// to push objects in order to resolve collisions.
        /// If the rectangles are not intersecting, Vector2.Zero is returned.
        /// </returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        public static Point RightTop(this Rectangle rect)
        {
            return new Point(rect.Right, rect.Y);
        }

        public static Point LeftTop(this Rectangle rect)
        {
            return new Point(rect.Left, rect.Y);
        }

        public static Point RightBottom(this Rectangle rect)
        {
            return new Point(rect.Right, rect.Bottom);
        }

        public static Point LeftBottom(this Rectangle rect)
        {
            return new Point(rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Gets a flattened collision normal between rectA and rectB
        /// </summary>
        /// <param name="rectA">hitter</param>
        /// <param name="rectB">hitte</param>
        /// <returns>a collision normal</returns>
        public static Vector3 GetCollisionNormal(this Rectangle rectA, Rectangle rectB)
        {
            Vector3 collisionNormal = Vector3.Zero;

            // Compute the most valid collision side
            float top = Math.Abs(rectA.Top - rectB.Bottom);
            float bottom = Math.Abs(rectA.Bottom - rectB.Top);
            float left = Math.Abs(rectA.Left - rectB.Right);
            float right = Math.Abs(rectA.Right - rectB.Left);

            // Hit top
            if (top <= bottom && top <= left && top < right)
                collisionNormal += Vector3.Forward;

            // hit bottom
            if (bottom <= top && bottom < left && bottom < right)
                collisionNormal += Vector3.Backward;

            // hit left
            if (left <= top && left < right && left <= bottom)
                collisionNormal += Vector3.Left;

            // hit right
            if (right <= top && right <= left && right <= bottom)
                collisionNormal += Vector3.Right;

            return collisionNormal;
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        public static Vector2 GetBottomCenter(this Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
        }

        /// <summary>
        /// You can't do rectangle manipulation in side of this, so no point in doing a (this Rectangle rect) 
        /// since its a value type
        /// </summary>
        /// <param name="rect">The rectangle you want to center</param>
        /// <param name="position">The point you want to center it on </param>
        public static void CenterOn(ref Rectangle rect,ref  Point position )
        {
            rect.X = position.X - (int)(rect.Width / 2.0f);
            rect.Y = position.Y - (int)(rect.Height / 2.0f);
        }

        public static void CenterOn( ref Rectangle rect, Vector3 point )
        {
            Point position = point.ToPoint();
            rect.X = position.X - ( int ) ( rect.Width / 2.0f );
            rect.Y = position.Y - ( int ) ( rect.Height / 2.0f );
        }
    }
}

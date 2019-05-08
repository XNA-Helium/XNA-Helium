using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class PointExtensions
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2((float) point.X, (float) point.Y);
        }
        public static Vector3 ToVector3(this Point point)
        {
            return new Vector3((float)point.X, 0.0f, (float)point.Y);
        }
        public static void Add(ref Point point,ref Point p2 )
        {
            point.X += p2.X;
            point.Y += p2.Y;
        }
        public static void Subract(ref Point point, ref Point p2 )
        {
            point.X -= p2.X;
            point.Y -= p2.Y;
        }
    }
}

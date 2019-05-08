using System;
using Microsoft.Xna.Framework;


namespace Engine
{
    public static class Vector2Extensions
    {
        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X,(int)vector.Y);
        }
        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.X, 0.0f, vector.Y);
        }
    }
}

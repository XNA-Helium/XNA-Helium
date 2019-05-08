using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public static class Vector3Extensions
    {
        public static Point ToPoint(this Vector3 vector)
        {
            return new Point((int)vector.X, (int)vector.Z);
        }
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Z);
        }
    }
}

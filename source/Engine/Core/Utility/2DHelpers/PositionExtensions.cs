using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Engine;
namespace Engine.Core
{
    public static class PositionExtensions
    {
        #region Get
        public static Point GetPointPosition( this Placeable placeable )
        {
            return placeable.Position.ToPoint();
        }
        public static Vector2 Get2DPosition(this Placeable placeable )
        {
            return placeable.Position.ToVector2();
        }
        public static float Get2DDepth(this Placeable placeable)
        {
            return placeable.Position.Y;
        }
        public static float Get2DRotation(this Placeable placeable)
        {
            return placeable.Rotation.Y;
        }
        public static Vector2 Get2DScale(this Placeable placeable)
        {
            return placeable.Scale.ToVector2();
        }
        #endregion
        #region Set
        public static void Set2DPosition(this Placeable placeable, Point Position)
        {
            Vector3 p = Position.ToVector3();
            p.Y = placeable.Position.Y;
            placeable.Position = p;
        }
        public static void Set2DPosition(this Placeable placeable, Vector2 Position)
        {
            Vector3 p = Position.ToVector3();
            p.Y = placeable.Position.Y;
            placeable.Position = p;
        }
        public static void Set2DDepth(this Placeable placeable, float Depth)
        {
            Vector3 p = placeable.Position;
            p.Y = Depth;
            placeable.Position = p;
        }
        public static void Set2DRotation(this Placeable placeable, float Rotation)
        {
            Vector3 p = placeable.Rotation;
            p.Y = Rotation;
            placeable.Rotation = p;
        }
        public static void Set2DScale(this Placeable placeable, Vector2 Scale)
        {
            Vector3 p = Scale.ToVector3();
            p.Y = placeable.Position.Y;
            placeable.Scale = p;
        }
        #endregion
    }
}

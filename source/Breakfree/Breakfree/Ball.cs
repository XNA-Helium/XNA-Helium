using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine;
using Engine.Core;

namespace Breakfree
{
    public class Ball : BaseObjectStreamingHelper<Ball>
    {
        Drawable2D drawable;
        Placeable placeable;
        Physics2D phys;

        public Ball()
        {
            placeable = new Placeable(true, -1);
            this.AddComponent(placeable);
            drawable = new Drawable2D(true, -1);
            base.AddComponent(drawable);
            drawable.DrawLayer = 80.0f / 100.0f;

            phys = new Physics2D(new Rectangle(0, 0, 20, 20));
            base.AddComponent(phys);
            phys.Callback = OnCollision;

            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Ball"), "Default");
            drawable.PlayAnimation("Default");
        }

        public void OnCollision(BaseObject collider)
        {
            if (collider.InWorld == false)
                return;


            // Determin collision resolution
            Rectangle hitRect = collider.GetComponent<Physics2D>().Rectangle;

            Rectangle intersection = Rectangle.Intersect(hitRect, phys.Rectangle);
            Vector2 depth = new Vector2(intersection.Width, intersection.Height);

            // this is an issue with the Rectangle VS FRect, QuadTree uses FRect, baseObject uses Rectangle
            // so sometimes a collide occurs without any whole number intersection and causes issues.
            // blocking the occurance completely fixes the problem but a better solution should likely be worked out.
            if (depth == Vector2.Zero) return; 

            Vector3 collisionNormal = RectangleExtensions.GetCollisionNormal(hitRect, phys.Rectangle);

            // resolve collision interesection
            Vector3 Resolution = (depth.ToVector3() * collisionNormal);
            placeable.Position += Resolution;

            // get the vector between the two colliders
            Vector3 vec = Vector3.Normalize(placeable.Position - collider.GetComponent<Placeable>().Position);
            // determin reflection off collided object
            Vector3 escapeVec = Vector3.Reflect(phys.Velocity, collisionNormal);

            float force = phys.Velocity.Length();

            // Tweak the escape vector if the ball hits the paddle
            if (collider is Paddle)
            {
                // offset the X escape vector by how centered the hit is on the paddle
                float centered = Math.Abs(hitRect.Center.X - placeable.Position.X);
                escapeVec.X += centered * -0.1f;
                force = 8;
            }

            // give the reflection alittle push and reset the velocity
            phys.Velocity = Vector3.Normalize(escapeVec) * force;
        }
    }
}

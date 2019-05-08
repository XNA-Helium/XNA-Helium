using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Engine;
using Engine.Core;

namespace Breakfree
{
    class Paddle : BaseObjectStreamingHelper<Paddle>
    {

        Drawable2D drawable;
        Placeable placeable;
        Physics2D phys;

        public Paddle(Vector3 position)
        {
            placeable = new Placeable(true, -1);
            placeable.Position = position;

            this.AddComponent(placeable);
            drawable = new Drawable2D(true, -1);
            base.AddComponent(drawable);
            drawable.DrawLayer = 80.0f / 100.0f;

            phys = new Physics2D(new Rectangle(0, 0, 75, 25));
            base.AddComponent(phys);

            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Paddle"), "Default");
            drawable.PlayAnimation("Default");
        }

        public void ProcessInput(ref Engine.TouchCollection touches)
        {
            Vector2 Target = touches[0].Position.ToVector2();
            Vector2 vec = new Vector2(Target.X - placeable.Position.X, 0) * 0.1f;

            if (vec.Length() > 15)
                vec = Vector2.Normalize(vec) * 15;

            phys.Velocity = vec.ToVector3();
        }
    }
}

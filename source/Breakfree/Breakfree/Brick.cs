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
using Engine.Core;

namespace Breakfree
{
    public class Brick : BaseObjectStreamingHelper<Brick>
    {
        Placeable placeable;
        Drawable2D drawable;
        Physics2D phys;

        public Brick(Vector3 position)
        {
            placeable = new Placeable(true, -1);
            placeable.Position = position;
            this.AddComponent(placeable);

            drawable = new Drawable2D(true, -1);
            base.AddComponent(drawable);
            drawable.DrawLayer = 80.0f / 100.0f;

            phys = new Physics2D(new Rectangle(0, 0, 64, 32));
            base.AddComponent(phys);
            phys.Static = true;
            phys.Callback = OnCollision;

            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\purpleBrick"), "Default");
            drawable.PlayAnimation("Default");
        }

        public void OnCollision(BaseObject hit)
        {
            // Determin collision resolution
            Rectangle hitRect = hit.GetComponent<Physics2D>().Rectangle;
            Rectangle intersection = Rectangle.Intersect(hitRect, phys.Rectangle);

            PlayingState state = Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState;
            state.Score++;

            Engine.GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromObjectListLater(this);
        }
    }
}

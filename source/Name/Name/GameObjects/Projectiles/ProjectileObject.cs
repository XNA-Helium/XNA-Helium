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

namespace Name
{
    public abstract class ProjectileObject<T> : BaseObjectStreamingHelper<T> , IProjectile where T : Factoryable
    {
        protected bool collided;
        protected Facing facing;

        public ProjectileObject(bool InWorld, float uID)
            : base(InWorld, uID)
        {

            collided = false;
            facing = Facing.Right; ;
            Placeable r = new Placeable(true, -1);
            Drawable2D d = new Drawable2D(true, -1);
            Physics2D p = new Physics2D(new Rectangle(0, 0, 16, 16), OnCollision);
            AddComponent(r);
            AddComponent(d);
            AddComponent(p);
            d.Visible = true;
            d.FaceVelocity = true;
            d.DrawLayer = (float)DrawLayer.LayerDepth.Projectiles / 100.0f;

            d.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\RocketLeft"), new Rectangle(0, 0, 32, 32), "Left", 1);
            d.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\RocketRight"), new Rectangle(0, 0, 32, 32), "Right", 1);
            d.PlayAnimation("Right");
        
        }

        public ProjectileObject(Facing direction):base()
        {
            collided = false;
            facing = direction;
            Placeable r = new Placeable(true,-1);
            Drawable2D d = new Drawable2D(true,-1);
            Physics2D p = new Physics2D(new Rectangle(0, 0, 16, 16), OnCollision);
            AddComponent(r);
            AddComponent(d);
            AddComponent(p);
            d.Visible = true;
            d.FaceVelocity = true;
            d.DrawLayer = (float) DrawLayer.LayerDepth.Projectiles / 100.0f;

            d.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\RocketLeft" ), new Rectangle( 0, 0, 32, 32 ), "Left", 1 );
            d.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\RocketRight" ), new Rectangle( 0, 0, 32, 32 ), "Right", 1 );
            if (direction == Facing.Left)
            {
                d.PlayAnimation( "Left" );
            }
            if (direction == Facing.Right)
            {
                d.PlayAnimation( "Right" );
            }
        }

        public virtual void OnCollision(BaseObject rhs)
        {
            if(!collided)
            {
                collided = true;
                //set off explosion
            }
        }
    }
}

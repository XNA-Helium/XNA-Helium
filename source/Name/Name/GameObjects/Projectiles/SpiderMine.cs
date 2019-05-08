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
    
    class SpiderMine : ProjectileObject<SpiderMine>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new SpiderMine(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected SpiderMine( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\SpiderMine" ), "Static" );
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 32 );

            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\SpiderMineRight" ), new Rectangle( 0, 0, 32, 64 ), "SMR", 4 );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\SpiderMineLeft" ), new Rectangle( 0, 0, 32, 64 ), "SML", 4 );
            SoundManager.Instance.Play( "jump" );
            SoundManager.Instance.CurrentlyPlaying["jump"].Stop();
        }

        public SpiderMine(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D);
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\SpiderMine"),"Static");
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 16, 32 );

            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\SpiderMineRight" ), new Rectangle( 0, 0, 32, 64 ), "SMR", 4 );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\SpiderMineLeft" ), new Rectangle( 0, 0, 32, 64 ), "SML", 4 );
            SoundManager.Instance.Play("jump");
            SoundManager.Instance.CurrentlyPlaying["jump"].Stop();
        }

        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new SpiderMine(facing);
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (!collided)
            {
                if (rhs == this)
                {
                     Facing f = facing;
                    int multiplier = 1;
                    if(f == Facing.Left)
                    {
                        multiplier = -1;
                    }
                    (this[Physics2D.TypeStatic] as Physics2D).AddVelocity = new Vector2(multiplier * 1,1).ToVector3();
                     if(facing == Facing.Left)
                    {
                        ( this[Drawable2D.TypeStatic] as Drawable2D ).PlayAnimation( "SML" );
                    }else{
                        ( this[Drawable2D.TypeStatic] as Drawable2D ).PlayAnimation( "SMR" );
                    };
                    collided = true;
                        
                }
            }
            else if ( rhs != this && ( rhs is IProjectile || rhs is IExplosion || rhs is PlayerObject || rhs is LandMine ) )
            {
                if (rhs[HitPoints.TypeStatic] != null)
                {
                    (rhs[HitPoints.TypeStatic] as HitPoints).Hitpoints -= 20;
                }
                Vector2 position = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
                WeaponExplosion we = new WeaponExplosion(new Vector2(position.X, position.Y));
                if (SoundManager.Instance.Playing("jump"))
                    SoundManager.Instance.CurrentlyPlaying["jump"].Stop();
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(we);
                base.OnCollision(rhs);
                Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
            }
            else if(rhs == this)
            {
                if (collided)
                {
                    int velocity = 0;
                    if (facing == Facing.Left)
                    {
                        velocity = -1;
                    }
                    else if (facing == Facing.Right)
                    {
                        velocity = 1;
                    }

                    ( this[Physics2D.TypeStatic] as Physics2D ).AddVelocity = new Vector2( velocity, 0 ).ToVector3();
                }
            }
        }

        /*
        bool ShouldPlay = true;
        public override void Update(GameTime p_time)
        {
            if (collided)
            {
                if (!SoundManager.Instance.Playing("jump") && ShouldPlay && (this[Drawable2D.TypeStatic] as Drawable2D).Frame == 0)
                {
                    SoundManager.Instance.Play("jump", 0.5f);
                    ShouldPlay = false;
                }
                else if ((this[Drawable2D.TypeStatic] as Drawable2D).Frame != 0)
                {
                    ShouldPlay = true;
                }
            }
            base.Update(p_time);
        }
        */
    }
    
}

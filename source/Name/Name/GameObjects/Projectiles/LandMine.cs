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
using Name.GameState;

namespace Name
{
    class LandMine : ProjectileObject<LandMine>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new LandMine(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected LandMine( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\Mine" ), "Static" );
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 26, 32 );

        }

        public LandMine(Facing direction)
            : base(direction)
        {
            Drawable2D dss = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Projectiles\Mine" ), "Static" );
            dss.PlayAnimation( "Static" );
            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 26, 32 );

        }

        public static IProjectile ReturnNewProjectile( Facing facing )
        {
            return new LandMine(facing);
        }

        public override void OnCollision(BaseObject rhs)
        {
            if (!collided)
            {
                if (rhs == this)
                {
                    collided = true;

                    SoundManager.Instance.Play("land", 0.5f);

                    Physics2D c = this[Physics2D.TypeStatic] as Physics2D;
                    c.Static = true;

                    Drawable2D d = this[Drawable2D.TypeStatic] as Drawable2D;
                    d.Visible = true;

                    d.FaceVelocity = false;
                    int height = (this[Placeable.TypeStatic] as Placeable).Position.ToPoint().Y;

                    //int left = (Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState).Terrain[height - 5];
                    //int right = ( Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState ).Terrain[height + 5];
                    //( this[Placeable.TypeStatic] as Placeable ).Set2DRotation( Utility.ArcTanAngle( left, right ) );
                }
            }
            else if ( rhs != this && ( rhs is IProjectile || rhs is IExplosion || rhs is PlayerObject || rhs is SpiderMine ) )
            {
                Vector2 position = ( this[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                MineExplosion we = new MineExplosion( new Vector2( position.X, position.Y ) );
                Engine.GameState.GameStateSystem.Instance.RemoveObjectLater( this );
                base.OnCollision( rhs );
            }
        }

    }
}

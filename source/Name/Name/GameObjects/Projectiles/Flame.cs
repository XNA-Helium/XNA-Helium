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

    class Flame : ProjectileObject<Flame>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new Flame(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected Flame( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            /*
            weaponObject = weapon;

            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 26, 32 );
            ( this[Physics2D.TypeStatic] as Physics2D ).Static = true;
            float angle = ( weapon[Placeable.TypeStatic] as Placeable ).Get2DRotation();
            Vector2 offset = Vector2.Zero;
            if ( facing == Facing.Right )
            {
                offset = new Vector2( ( float ) 128 * ( float ) Math.Cos( angle ), ( float ) 128 * ( float ) Math.Sin( angle ) );
            }
            if ( facing == Facing.Left )
            {
                offset = new Vector2( ( float ) -128 * ( float ) Math.Cos( angle ), ( float ) -128 * ( float ) Math.Sin( angle ) );
            }

            Vector2 StartPosition = ( weapon[Placeable.TypeStatic] as Placeable ).Position.ToVector2() + offset;
            FlameCollision = new FlameExplosion( StartPosition );
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater( FlameCollision );

            Drawable2D dsa = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dsa.Visible = true;
            dsa.FaceVelocity = false;

            base.AddComponent( dsa );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\FlameLeft" ), new Rectangle( 0, 0, 192, 64 ), "FlameLeft", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\FlameRight" ), new Rectangle( 0, 0, 192, 64 ), "FlameRight", 5 );
            if ( direction == Facing.Left )
            {
                dsa.PlayAnimation( "FlameLeft" );
                dsa.SetOrigin( new Vector2( 96, 32 ) );
            }
            else
            {
                dsa.PlayAnimation( "FlameRight" );
                dsa.SetOrigin( new Vector2( 96, 32 ) );
            }
            EventManager.Instance.AddEvent( RemoveMe, ( float ) framesleft / 30.0f );
            */ 
        }

        FlameExplosion FlameCollision;
        int framesleft = 35;
        IWeapon weaponObject;
        public Flame(Facing direction,IWeapon weapon)
            : base(direction)
        {
            weaponObject = weapon;

            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 26, 32 );
            ( this[Physics2D.TypeStatic] as Physics2D ).Static = true;
            float angle = (weapon[Placeable.TypeStatic] as Placeable).Get2DRotation();
            Vector2 offset = Vector2.Zero;
            if (facing == Facing.Right)
            {
                offset = new Vector2( ( float ) 128 * ( float ) Math.Cos( angle ), ( float ) 128 * ( float ) Math.Sin( angle ) );
            }
            if (facing == Facing.Left)
            {
                offset = new Vector2( ( float ) -128 * ( float ) Math.Cos( angle ), ( float ) -128 * ( float ) Math.Sin( angle ) );
            }

            Vector2 StartPosition = (weapon[Placeable.TypeStatic] as Placeable).Position.ToVector2() + offset;
            FlameCollision = new FlameExplosion(StartPosition);
            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(FlameCollision);
            
            Drawable2D dsa = ( this[Drawable2D.TypeStatic] as Drawable2D );
            dsa.Visible = true;
            dsa.FaceVelocity = false;
            
            base.AddComponent(dsa);
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\FlameLeft" ), new Rectangle( 0, 0, 192, 64 ), "FlameLeft", 5 );
            dsa.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Explosions\FlameRight" ), new Rectangle( 0, 0, 192, 64 ), "FlameRight", 5 );
            if (direction == Facing.Left)
            {
                dsa.PlayAnimation("FlameLeft");
                dsa.SetOrigin( new Vector2( 96, 32 ) );
            }
            else
            {
                dsa.PlayAnimation( "FlameRight" );
                dsa.SetOrigin( new Vector2( 96, 32 ) );
            }
            EventManager.Instance.AddEvent( RemoveMe, ( float ) framesleft / 30.0f );
        }

        protected void RemoveMe( )
        {
            Engine.GameState.GameStateSystem.Instance.RemoveObjectLater( this );
            Engine.GameState.GameStateSystem.Instance.RemoveObjectLater( FlameCollision );
        }

        public static IProjectile ReturnNewProjectile( Facing facing, IWeapon weapon )
        {
            return new Flame( facing, weapon );
        }

        /*
        public Flame(Facing direction)
            : base(direction)
        {
            Drawable2D dsa = ( this[Drawable2D.TypeStatic] as Drawable2D );
           
            dsa.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\FlameLeft"), new Rectangle(0, 0, 96, 32), "FlameLeft", 5);
            dsa.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Projectiles\FlameRight"), new Rectangle(0, 0, 96, 32), "FlameRight", 5);
            
            if (direction == Facing.Left)
            {
                dsa.PlayAnimation("FlameLeft");
            }
            else
            {
                dsa.PlayAnimation("FlameRight");
            }

            ( this[Physics2D.TypeStatic] as Physics2D ).Rectangle = new Rectangle( 0, 0, 32, 32 );

            EventManager.Instance.AddEvent( RemoveMe, ( float ) framesleft / 30.0f );
        }
        public static ProjectileObject ReturnNew( Facing facing )
        {
            return new Flame(facing);
        }
        */
    }
}

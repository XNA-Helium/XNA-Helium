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
    class FlameThrower : WeaponObject<FlameThrower>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new FlameThrower(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected FlameThrower( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            ProjectileHolder hp = new ProjectileHolder( LandMine.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\FlamethrowerLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\FlamethrowerRight" ), "Right" );
            base.AddComponent( dss );
            if ( facing == Facing.Left )
            {
                dss.PlayAnimation( "Left" );
            }
            if ( facing == Facing.Right )
            {
                dss.PlayAnimation( "Right" );
            }
        }

        public FlameThrower(PlayerObject _po)
            : base(_po)
        {

            ProjectileHolder hp = new ProjectileHolder( LandMine.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\FlamethrowerLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\FlamethrowerRight" ), "Right" );
            base.AddComponent( dss );
            if ( facing == Facing.Left )
            {
                dss.PlayAnimation( "Left" );
            }
            if ( facing == Facing.Right )
            {
                dss.PlayAnimation( "Right" );
            }

        }

        public override IProjectile Fire( float Velocity )
        {
            //return base.Fire( Velocity ); // Don't use the base
         
            float angle = ( this[Placeable.TypeStatic] as Placeable ).Get2DRotation();
            Vector2 offset = Vector2.Zero;
            if ( facing == Facing.Right )
            {
                offset = new Vector2( ( float ) 64 * ( float ) Math.Cos( angle ), ( float ) 64 * ( float ) Math.Sin( angle ) );
            }
            if ( facing == Facing.Left )
            {
                offset = new Vector2( ( float ) -64 * ( float ) Math.Cos( angle ), ( float ) -64 * ( float ) Math.Sin( angle ) );
            }

            Vector2 StartPosition = ( this[Placeable.TypeStatic] as Placeable ).Position.ToVector2() + offset;
            Flame rocket = new Flame( facing, this );
            ( rocket[Placeable.TypeStatic] as Placeable ).Position = StartPosition.ToVector3();

            ( rocket[Placeable.TypeStatic] as Placeable ).Set2DRotation( angle );

            return rocket;
        }
    }
}

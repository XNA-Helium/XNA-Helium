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
    class GrenadeLauncher : WeaponObject<GrenadeLauncher>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new GrenadeLauncher(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected GrenadeLauncher( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            ProjectileHolder hp = new ProjectileHolder( Grenade.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\GrenadeLauncherLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\GrenadeLauncherRight" ), "Right" );
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

        public GrenadeLauncher(PlayerObject _po)
            : base(_po)
        {

            ProjectileHolder hp = new ProjectileHolder( Grenade.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\GrenadeLauncherLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\GrenadeLauncherRight" ), "Right" );
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

    }
}

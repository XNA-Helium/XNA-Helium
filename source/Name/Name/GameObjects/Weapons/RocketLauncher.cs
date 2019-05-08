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
    class RocketLauncher : WeaponObject<RocketLauncher>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new RocketLauncher(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected RocketLauncher(bool InWorld, float uID)
            : base(InWorld, uID)
        {
            ProjectileHolder hp = new ProjectileHolder( Rocket.ReturnNewProjectile );
            base.AddComponent(hp);

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\RocketLauncherLeft"),"Left");
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\RocketLauncherRight"),"Right");
            base.AddComponent(dss);
            if (facing == Facing.Left)
            {
                dss.PlayAnimation( "Left" );
            }
            if (facing == Facing.Right)
            {
                dss.PlayAnimation( "Right" );
            }
        }

        public RocketLauncher(PlayerObject _po)
            : base(_po)
        {

            ProjectileHolder hp = new ProjectileHolder( Rocket.ReturnNewProjectile );
            base.AddComponent(hp);

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\RocketLauncherLeft"),"Left");
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\RocketLauncherRight"),"Right");
            base.AddComponent(dss);
            if (facing == Facing.Left)
            {
                dss.PlayAnimation( "Left" );
            }
            if (facing == Facing.Right)
            {
                dss.PlayAnimation( "Right" );
            }
        }
    }
}

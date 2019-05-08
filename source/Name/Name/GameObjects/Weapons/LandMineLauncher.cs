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
    class LandMineLauncher : WeaponObject<LandMineLauncher>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new LandMineLauncher(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected LandMineLauncher( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            ProjectileHolder hp = new ProjectileHolder( LandMine.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\MineLauncherLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\MineLauncherRight" ), "Right" );
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

        public LandMineLauncher(PlayerObject _po)
            : base(_po)
        {

            ProjectileHolder hp = new ProjectileHolder( LandMine.ReturnNewProjectile );
            base.AddComponent(hp);

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\MineLauncherLeft"),"Left");
            dss.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Weapons\MineLauncherRight"),"Right");
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
        /*
        public override ProjectileObject Fire(float Velocity)
        {
            AvailableWeaponList weaponlist = (Game1.Instance.GameManager as GameManagerTeamedInterface).CurrentUTP().AvailableWeapons;
            if (weaponlist.LandMine != AvailableWeaponList.INFINITY)
            {
                weaponlist.LandMine -= 1;
            }

            Vector2 StartPosition = (this[Placeable.TypeStatic] as Placeable).Position.ToVector2();
            
            Rocket rocket = new Rocket(facing);
            (rocket[Placeable.TypeStatic] as Placeable).Position = StartPosition.ToVector3();
            
            float angle = (this[Placeable.TypeStatic] as Placeable).Get2DRotation();
            if (facing == Facing.Right)
            {
                (rocket[Physics2D.TypeStatic] as Physics2D).AddVelocity = new Vector2((float)Velocity * (float)Math.Cos(angle), (float)Velocity * (float)Math.Sin(angle)).ToVector3();
            }
            if (facing == Facing.Left)
            {
                (rocket[Physics2D.TypeStatic] as Physics2D).AddVelocity = new Vector2((float)-Velocity * (float)Math.Cos(angle), (float)-Velocity * (float)Math.Sin(angle)).ToVector3();
            }

            Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(rocket);
            return rocket;
        }
        */
    }
}

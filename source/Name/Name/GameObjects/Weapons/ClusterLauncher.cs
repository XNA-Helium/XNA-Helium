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
    class ClusterLauncher : WeaponObject<ClusterLauncher>
    {
        
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new ClusterLauncher(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        protected ClusterLauncher( bool InWorld, float uID )
            : base(InWorld, uID)
        {
            ProjectileHolder hp = new ProjectileHolder( GrenadeCluster.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\ClusterLauncherLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\ClusterLauncherRight" ), "Right" );
            base.AddComponent( dss );
            if ( facing == Facing.Left )
            {
                dss.PlayAnimation( "Left" );
            }
            if ( facing == Facing.Right )
            {
                dss.PlayAnimation( "Right" );
            }
            dss.Visible = false;
        }

        public ClusterLauncher(PlayerObject _po)
            : base(_po)
        {

            ProjectileHolder hp = new ProjectileHolder( GrenadeCluster.ReturnNewProjectile );
            base.AddComponent( hp );

            Drawable2D dss = this[Drawable2D.TypeStatic] as Drawable2D;
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\ClusterLauncherLeft" ), "Left" );
            dss.SetupAnimation( Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Weapons\ClusterLauncherRight" ), "Right" );
            base.AddComponent( dss );
            if ( facing == Facing.Left )
            {
                dss.PlayAnimation( "Left" );
            }
            if ( facing == Facing.Right )
            {
                dss.PlayAnimation( "Right" );
            }
            dss.Visible = false;
        }

        /*
            public override ProjectileObject Fire(float Velocity)
            {
                AvailableWeaponList weaponlist = (Game1.Instance.GameManager as GameManagerTeamedInterface).CurrentUTP().AvailableWeapons;
                if (weaponlist.Cluster != AvailableWeaponList.INFINITY)
                {
                    weaponlist.Cluster -= 1;
                }
                Vector2 StartPosition = (this[Placeable.TypeStatic] as Placeable).Position;
                GrenadeCluster rocket = new GrenadeCluster(facing);
                (rocket[Placeable.TypeStatic] as Placeable).Position = StartPosition;
                float angle = (this[Placeable.TypeStatic] as Placeable).Get2DRotation().Rotation;
                if (facing == Facing.Right)
                {
                    (rocket[Physics2D.TypeStatic] as Physics2D).Velocity = new Vector2((float)Velocity * (float)Math.Cos(angle), (float)Velocity * (float)Math.Sin(angle));
                }
                if (facing == Facing.Left)
                {
                    (rocket[Physics2D.TypeStatic] as Physics2D).Velocity = new Vector2((float)-Velocity * (float)Math.Cos(angle), (float)-Velocity * (float)Math.Sin(angle));
                }

                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(rocket);
                Engine.GameState.GameStateSystem.Instance.GetCurrentState.AddToObjectListLater(rocket.GraphicalOverlay);
                return rocket;
            }
        */
    }
}

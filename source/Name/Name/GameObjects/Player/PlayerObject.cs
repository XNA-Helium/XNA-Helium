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
    public class PlayerObject : BaseObjectStreamingHelper<PlayerObject>
    {
        public static Object ReturnsNew(bool InWorld, float uID)
        {
            return new PlayerObject(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public static string IdleLeft = "IdleLeft";
        public static string IdleRight = "IdleRight";
        public static string WalkLeft = "WalkLeft";
        public static string WalkRight = "WalkRight";
        public static string AimLeft = "AimLeft";
        public static string AimRight = "AimRight";
        public static string ShootLeft = "ShootLeft";
        public static string ShootRight = "ShootRight";

        Drawable2D drawable;
        protected Facing facing;
        protected LaunchingRod rod;
        protected PlayerHitpoints hp;
        protected String myName = "Hired Goon";
        
        public PlayerObject(String name)
            : this()
        {
            myName = name;
        }

        protected PlayerObject(bool InWorld, float uID)
            : base(InWorld, uID)
        {
            TeamMember gt = new TeamMember(Name.Team.NoTeam);
            base.AddComponent(gt);

            HitPoints gh = new HitPoints(100);
            base.AddComponent(gh);

            Placeable p = new Placeable(true, -1);
            this.AddComponent(p);
            drawable = new Drawable2D(true, -1);
            base.AddComponent(drawable);
            this.AddComponent(new WeaponHolder(new RocketLauncher(this)));
            drawable.DrawLayer = (int)DrawLayer.LayerDepth.Players / 100.0f;
            int fps = 3;

            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\StandLeft"), IdleLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\StandRight"), IdleRight);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\WalkLeft"), new Rectangle(0, 0, 64, 64), WalkLeft, fps);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\WalkRight"), new Rectangle(0, 0, 64, 64), WalkRight, fps);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\AimLeft"), AimLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\AimRight"), AimRight);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\ShootLeft"), ShootLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\ShootRight"), ShootRight);

            Physics2D phy = new Physics2D(new Rectangle(0, 0, 40, 55));
            AddComponent(phy);

            System.Random rand = new Random();

            if (rand.Next(1, 3) == 1)
            {
                drawable.PlayAnimation("IdleLeft");
                facing = Facing.Left;
            }
            else
            {
                drawable.PlayAnimation("IdleRight");
                facing = Facing.Right;
            }
            rod = new LaunchingRod(this);
            hp = new PlayerHitpoints(this);
            (rod[Drawable2D.TypeStatic] as Drawable2D).Visible = false;
        }

        public PlayerObject()
        {

            TeamMember gt = new TeamMember( Name.Team.NoTeam );
            base.AddComponent( gt );

            HitPoints gh = new HitPoints( 100 );
            base.AddComponent( gh );

            Placeable p = new Placeable(true,-1);
            this.AddComponent( p );
            drawable = new Drawable2D(true,-1);
            base.AddComponent( drawable );
            this.AddComponent( new WeaponHolder( new RocketLauncher( this ) ) );
            drawable.DrawLayer = (int) DrawLayer.LayerDepth.Players / 100.0f;
            int fps = 3;

            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\StandLeft"), IdleLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\StandRight"), IdleRight);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Sprites\Gir\WalkLeft" ), new Rectangle(0, 0, 64, 64), WalkLeft, fps );
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\WalkRight"), new Rectangle(0, 0, 64, 64), WalkRight, fps);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\AimLeft"), AimLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\AimRight"),  AimRight);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\ShootLeft"), ShootLeft);
            drawable.SetupAnimation(Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Sprites\Gir\ShootRight"),  ShootRight);

            Physics2D phy = new Physics2D( new Rectangle( 0, 0, 40, 55 ) );
            AddComponent( phy );

            System.Random rand = new Random();

            if (rand.Next(1,3) == 1)
            {
                drawable.PlayAnimation( "IdleLeft" );
                facing = Facing.Left;
            }
            else
            {
                drawable.PlayAnimation( "IdleRight" );
                facing = Facing.Right;
            }
            rod = new LaunchingRod(this);
            hp = new PlayerHitpoints(this);
            (rod[Drawable2D.TypeStatic] as Drawable2D).Visible = false;
        }

        public override void CleanUp( bool RemoveFromObjectList )
        {
            if ( RemoveFromObjectList )
            {
                if(hp != null)
                    hp.CleanUp();
                if(rod != null)
                    rod.CleanUp();
            }
            hp = null;
            rod = null;
            base.CleanUp( RemoveFromObjectList );
        }

        public void RemoveMe()
        {
            CleanUp();
            Engine.GameState.GameStateSystem.Instance.RemoveObjectLater(this);
        }

        public void Shoot( Facing NewFacing )
        {
            if ( NewFacing == Engine.Core.Facing.Right )
                drawable.PlayAnimation( ShootRight, AimRight);
            else if ( NewFacing == Engine.Core.Facing.Left )
                drawable.PlayAnimation( ShootLeft, AimLeft );

            facing = NewFacing;
        }

        public void Shoot( )
        {
            Shoot( facing );
        }

        public void ShowRod()
        {
            if(rod != null)
                rod.Deploy(facing);
        }
        public void HideRod()
        {
            if(rod != null)
                rod.Retract(facing);
        }

        public void ShowWeapon()
        {
            if ( Weapon != null )
            {
                Weapon.SetFacing( facing );
                Weapon.Visible = true;
            }
        }
        public void HideWeapon()
        {
            if(Weapon != null)
                Weapon.Visible = false;
        }
        EventManager.GameEvent CurrentDeployEvent;
        public void DeployAndShot()
        {
            Shoot();
            // If not Jetpack
            if(Weapon is JetPack)
            {
                ShowWeapon();
            }else{
                ShowRod();
                CurrentDeployEvent = EventManager.Instance.AddEvent( ShowWeapon, 1.25f );
            }
            // else
        }
        public void Retract( )
        {
            Retract( true );
        }
        public void Retract(bool ThrowEvent )
        {
            if (Weapon != null && Weapon is JetPack )
            {
                HideWeapon();
            }
            else if ( Weapon != null )
            {
                HideWeapon();
                if ( CurrentDeployEvent != null )
                {
                    EventManager.Instance.RemoveEvent( CurrentDeployEvent );
                    CurrentDeployEvent = null;
                }
                HideRod();
                if ( ThrowEvent )
                    EventManager.Instance.AddEvent( Stand, drawable.PlayTime / 30.0f );
            }
            else
            {
                Engine.DebugHelper.Break( Weapon == null, DebugHelper.DebugLevels.Curious );
            }
        }
        public void Stand( Facing NewFacing )
        {
            if ( NewFacing == Engine.Core.Facing.Right )
                drawable.PlayAnimation( IdleRight );
            else if ( NewFacing == Engine.Core.Facing.Left )
                drawable.PlayAnimation( IdleLeft );

            facing = NewFacing;
        }

        public void Stand( )
        {
            Stand( facing );
        }

        public void Aim( Facing NewFacing )
        {
            if ( NewFacing == Engine.Core.Facing.Right )
                drawable.PlayAnimation( AimRight );
            else if ( NewFacing == Engine.Core.Facing.Left )
                drawable.PlayAnimation( AimLeft );

            facing = NewFacing;
        }

        public void Aim( )
        {
            Aim( facing );
        }

        public void Walk( Facing NewFacing )
        {
            if(NewFacing == Engine.Core.Facing.Right)
                drawable.PlayAnimation( WalkRight );
            else if ( NewFacing == Engine.Core.Facing.Left )
                drawable.PlayAnimation( WalkLeft );

            facing = NewFacing;
        }

        public void Walk( )
        {
            Walk( facing );
        }
        public Facing Facing
        {
            get
            {
                return facing;
            }
        }

        public String MyName
        {
            set
            {
                myName = value;
            }
            get
            {
                return myName;
            }
        }

        public String Team
        {
            get
            {
                return (this[TeamMember.TypeStatic] as TeamMember).Team;
            }
        }

        public Vector2 Position
        {
            get
            {
                return ( this[Placeable.TypeStatic] as Placeable ).Get2DPosition();
            }
        }

        public void SwapWeapon( IWeapon w )
        {
            ( this[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon = w;
        }

        public PlayerHitpoints HP
        {
            get
            {
                return hp;
            }
        }

        public IWeapon Weapon
        {
            get
            {
                WeaponHolder wh = (this[WeaponHolder.TypeStatic] as WeaponHolder );
                if ( wh != null )
                    return wh.Weapon;
                else
                    return null;
            }
            set
            {
                SwapWeapon(value);
            }
        }

    }
}

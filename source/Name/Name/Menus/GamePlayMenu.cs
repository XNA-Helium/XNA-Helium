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
using Engine.UI;
using Engine.Core;

using Name.GameState;

namespace Name.Menus
{
    class GamePlayMenu : TimeDisplay
    {
        bool UseInput = false;
        PlayerObject player;
        bool AnimationPicked;
        UIButton retbutton;
        Texture2D MiniMap = Engine.ContentManager.Instance.GetObject<Texture2D>("MiniMap");
        SpriteFont sf = Engine.ContentManager.Instance.GetObject<SpriteFont>(@"SharedContent\TextboxFont");
        NameTeam Team;
        EventManager.GameEvent e;
        bool ThisRemoved = false;

        UIButton ShootRect;
        GameOptions Options;

        public GamePlayMenu( PlayerObject Player, NameTeam team, double time, GameOptions options )
            : base( @"Content\Menu\BottomBackground", CameraManager.GetPosition( 0.0f, 0.8125f ),time, options.TurnLength )
        {
            Team = team;
            player = Player;

            if ( !Player.InWorld )
                return;

            Options = options;
            Point p = Player.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref p );
            UIButton b = new UIButton( @"Content\Menu\Previous", CameraManager.GetPosition(0.04f,0.9f) );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Left, b );
            Point nextp = CameraManager.GetPosition( 0.96f, 0.9f );
            nextp.X -= Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Next_active" ).Width;
            b = new UIButton( @"Content\Menu\Next", nextp );
            b.ValidStates = ( int ) TouchStates.ANY;
            Add( Right, b );

            e = EventManager.Instance.AddEvent( TimesUp, TURNLENGTH );


            IWeapon o = ( player[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon;

            if ( o is RocketLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\RocketActive", @"Content\Menu\RocketInactive", CameraManager.GetPosition(0.20f,0.86f) );
            }
            else if ( o is GrenadeLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\GrenadeActive", @"Content\Menu\GrenadeInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else if ( o is ClusterLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\ClusterActive", @"Content\Menu\ClusterInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else if ( o is JetPack )
            {
                retbutton = new UIButton( @"Content\Menu\JetPackActive", @"Content\Menu\JetPackInactive", CameraManager.GetPosition( 0.20f, 0.86f ) ); 
            }
            else if ( o is LandMineLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\MineActive", @"Content\Menu\MineInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else if ( o is SpiderMineLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\SpiderMineActive", @"Content\Menu\SpiderMineInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else if ( o is CarpetBombLauncher )
            {
                retbutton = new UIButton( @"Content\Menu\NapalmActive", @"Content\Menu\NapalmInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else if ( o is FlameThrower )
            {
                retbutton = new UIButton( @"Content\Menu\FlamethrowerActive", @"Content\Menu\FlamethrowerInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }
            else
            {
                retbutton = new UIButton( @"Content\Menu\RocketActive", @"Content\Menu\RocketInactive", CameraManager.GetPosition( 0.20f, 0.86f ) );
            }

            retbutton.ValidStates = ( int ) TouchStates.Released;
            Add( WeaponSelection, retbutton );

            Point shootpoint = CameraManager.GetPosition(0.80f,0.89f);
            int width = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\Reticle_active" ).Width;
            shootpoint.X -= width + (width/4);
            ShootRect = new UIButton( @"Content\Menu\Reticle", shootpoint);
            ShootRect.ValidStates = ( int ) TouchStates.Released;
            Add( LaunchShootMenu, ShootRect );

            AnimationPicked = false;
        }
        public override void OnShow( )
        {
            base.OnShow();

            if ( !player.InWorld )
            {
                NoLongerInWorld();
                return;
            }

            IWeapon o = ( player[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon;

            ShootRect.Enabled =  (Team.WeaponList.GetAvailable( o ) != 0);

            if ( o is RocketLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\RocketActive", @"Content\Menu\RocketInactive" );
            }
            else if ( o is GrenadeLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\GrenadeActive", @"Content\Menu\GrenadeInactive" );
            }
            else if ( o is ClusterLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\ClusterActive", @"Content\Menu\ClusterInactive" );
            }
            else if ( o is JetPack )
            {
                retbutton.SwapTextures( @"Content\Menu\JetPackActive", @"Content\Menu\JetPackInactive" ); 
            }
            else if ( o is LandMineLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\MineActive", @"Content\Menu\MineInactive" );
            }
            else if ( o is SpiderMineLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\SpiderMineActive", @"Content\Menu\SpiderMineInactive" );
            }
            else if ( o is CarpetBombLauncher )
            {
                retbutton.SwapTextures( @"Content\Menu\NapalmActive", @"Content\Menu\NapalmInactive" );
            }
            else if ( o is FlameThrower )
            {
                retbutton.SwapTextures( @"Content\Menu\FlamethrowerActive", @"Content\Menu\FlamethrowerInactive" );
            }
            else
            {
                retbutton.SwapTextures( @"Content\Menu\RocketActive", @"Content\Menu\RocketInactive" );
            }
        }

        public void NoLongerInWorld( )
        {
            if( MenuSystemInstance.CurrentMenu == this)
                MenuSystemInstance.PopTopMenu();
        }

        public void WeaponSelection( )
        {
            if ( !player.InWorld )
            {
                NoLongerInWorld();
                return;
            }

            Name.GameState.AvailableWeaponList WeaponList = Team.WeaponList;
            MenuSystemInstance.AddMenu( new WeaponSelectionMenu(player, WeaponList) );
        }

        public void LaunchShootMenu( )
        {

            if ( !player.InWorld )
            {
                NoLongerInWorld();
                return;
            }

            Name.GameState.AvailableWeaponList WeaponList = ( Engine.GameState.GameStateSystem.Instance.GetCurrentState as Name.GameState.PlayingState ).CurrentTeam.WeaponList;
            if ( WeaponList.GetAvailable( ( player[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon ) == 0 )
            {

            }
            else if ( ( player[WeaponHolder.TypeStatic] as WeaponHolder ).Weapon is JetPack )
            {
                Team.WeaponList.Decriment( player.Weapon );
                player.Stand();
                MenuSystemInstance.AddMenu( new JetpackMenu( player, starttime, TimeLeft ) );
            }
            else
            {
                player.Shoot();
                bool ClassicMode = false;
                if ( Options.ShootMode == ShootMode.Classic )
                {
                    ClassicMode = true;
                }
                MenuSystemInstance.AddMenu( new AimAndShootMenu( ClassicMode, player, Team, starttime, TimeLeft ) );
            }
        }

        public void Left( )
        {
            if ( !player.InWorld )
            {
                NoLongerInWorld();
                return;
            }
            player.Walk( Facing.Left ); AnimationPicked = true;
            ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity =  -2 * Vector2.UnitX.ToVector3() ;
            Point point = player.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref point );
        }

        public void Right( )
        {
            if ( !player.InWorld )
            {
                NoLongerInWorld();
                return;
            }
            player.Walk( Facing.Right ); AnimationPicked = true;
            ( player[Physics2D.TypeStatic] as Physics2D ).AddVelocity = 2 * Vector2.UnitX.ToVector3();
            Point point = player.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn( ref point );
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );
            if ( !player.InWorld )
                return;

            if (!AnimationPicked )
            {
                player.Stand();
            }
            if ( !UseInput && AnimationPicked)
            {
                Point point = player.Position.ToPoint();
                CameraManager.Instance.Current.CenterOn( ref point );
            }
            AnimationPicked = false;
        }
        //double starttime;

        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            base.Draw( gameTime, spriteBatch );
            Vector2 ZeroPoint = CameraManager.GetPosition( 0.5f, 0.675f ).ToVector2();

            ZeroPoint.X -= Engine.ContentManager.Instance.GetObject<Texture2D>( "MiniMap" ).Width / 2;

            Texture2D YouDot = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\MiniMapYou" );
            Texture2D pack = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\HealthPack" );
            Texture2D fire = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\fire" );
            Texture2D mine = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\mine" );
            Texture2D WhiteDot = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\WhiteDot" );
            String timestring = ((int)TimeLeft).ToString();
            spriteBatch.DrawString( sf, timestring, CameraManager.GetPosition( 0.5f, 0.075f ).ToVector2() - (sf.MeasureString(timestring)/2), Team.Color);
            spriteBatch.Draw( MiniMap, ZeroPoint, null, Name.GameState.Terrain.BackGround, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f );

            foreach ( BaseObject o in Engine.GameState.GameStateSystem.Instance.GetCurrentState.ObjectList )
            {
                if ( o.InWorld == false )
                {

                }
                else if ( o == player )
                    Draw( o, YouDot, ZeroPoint, spriteBatch );
                else if ( o is PlayerObject )
                    Draw( o, WhiteDot, ZeroPoint, spriteBatch, ( o[Drawable2D.TypeStatic] as Drawable2D ).Color );
                else if ( o is HealthPack )
                    Draw( o, pack, ZeroPoint, spriteBatch );
                else if ( o is Napalm )
                    Draw( o, fire, ZeroPoint, spriteBatch );
                else if ( o is LandMine )
                    Draw( o, mine, ZeroPoint, spriteBatch );
            }
        }
        protected void Draw( BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch, Color c )
        {
            Vector2 p = ( o[Placeable.TypeStatic] as Placeable ).Position.ToVector2() / 10;
            spriteBatch.Draw( texture, basePoint + p, null, c, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f );
        }
        protected void Draw( BaseObject o, Texture2D texture, Vector2 basePoint, SpriteBatch spriteBatch )
        {
            Vector2 p = (o[Placeable.TypeStatic] as Placeable).Position.ToVector2() / 10;
            spriteBatch.Draw( texture, basePoint + p, null, Color.White, 0.0f, Vector2.One, 1.0f, SpriteEffects.None, 0.1f );
        }
        public override void ProcessInput( GameTime gameTime, ref Engine.TouchCollection touches )
        {
            UseInput = false;
            base.ProcessInput( gameTime, ref touches );
            if( !MenuUsedInput && touches.Count > 0)
            {
                CameraManager.Instance.Current.ProcesInput( gameTime,ref touches );
                UseInput = true;
            }
        }
        public override bool ValidationCallback( )
        {
            return player.InWorld;
        }
        public override void OnRemove( )
        {
            if (player != null && player.InWorld)
            {
                player.Stand();
            }

            if ( !player.InWorld || player == Team.CurrentlyActive )
            {
                Team.Next();
            }
            ThisRemoved = true;

            EventManager.Instance.RemoveEvent( e );

            base.OnRemove();
        }
        public void TimesUp( )
        {
            if (ThisRemoved)
                return;
            BaseMenu m = MenuSystemInstance.CurrentMenu;
            if ( m == this )
            {
                MenuSystemInstance.PopTopMenu();
            }
            if ( m is WeaponSelectionMenu )
            {
                MenuSystemInstance.PopTopMenu();
                if( MenuSystemInstance.CurrentMenu == this)
                    MenuSystemInstance.PopTopMenu();
            }
            if ( m is AimAndShootMenu )
            {
                MenuSystemInstance.PopTopMenu();
                if ( MenuSystemInstance.CurrentMenu == this )
                    MenuSystemInstance.PopTopMenu();
            }
            if ( m is ProjectileTracker )
            {
                MenuSystemInstance.PopTopMenu();
                if ( MenuSystemInstance.CurrentMenu == this )
                    MenuSystemInstance.PopTopMenu();
            }
            if (m is JetpackMenu)
            {
                MenuSystemInstance.PopTopMenu();
                if (MenuSystemInstance.CurrentMenu == this)
                    MenuSystemInstance.PopTopMenu();
            }
        }
    }
}

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

namespace Name.Menus
{
    class WeaponSelectionMenu : UIBaseMenu
    {
        PlayerObject po;
        Name.GameState.AvailableWeaponList wl;
        
        public WeaponSelectionMenu(PlayerObject playerObject, Name.GameState.AvailableWeaponList weaponList )
            : base ()
        {

            int width = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\RocketInactive" ).Width + 16;
            int QuarterHeight = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\RocketInactive" ).Height / 4;
            int QuarterWidth = width / 4;

            wl = weaponList;
            SpriteFont sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"Content\Menu\TitleFont" );
            Texture2D infinity = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\Menu\infinity" );
            po = playerObject;

            
            Point p = CameraManager.GetPosition( 0.1f, 0.125f );
            UIButton b = new UIButton( @"Content\Menu\RocketActive", @"Content\Menu\RocketInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Rocket, b );
            p.X += width;
            p.Y += QuarterHeight;
            if ( weaponList.Rocket == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.Rocket.ToString() ) );
            }
        
            p = CameraManager.GetPosition( 0.5f, 0.125f );
            b = new UIButton( @"Content\Menu\ClusterActive", @"Content\Menu\ClusterInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Cluster, b );
            p.X += width;
            p.Y += QuarterHeight;
            if ( weaponList.Cluster == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.Cluster.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.1f, 0.275f );
            b = new UIButton( @"Content\Menu\GrenadeActive", @"Content\Menu\GrenadeInactive", p);
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Grenade, b );
            p.X += width;
            p.Y += QuarterHeight;
            if ( weaponList.Grenade == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.Grenade.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.5f, 0.275f );
            b = new UIButton( @"Content\Menu\JetPackActive", @"Content\Menu\JetPackInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( JetPack, b );
            p.X += width;
            p.Y += QuarterHeight; 
            if ( weaponList.JetPack == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.JetPack.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.1f, 0.425f );
            b = new UIButton( @"Content\Menu\NapalmActive", @"Content\Menu\NapalmInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Napalm, b );
            p.X += width;
            p.Y += QuarterHeight; 
            if ( weaponList.CarpetBomb == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.CarpetBomb.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.5f, 0.425f );
            b = new UIButton( @"Content\Menu\SpiderMineActive", @"Content\Menu\SpiderMineInactive", p);
            b.ValidStates = ( int ) TouchStates.Released;
            Add( SpiderMine, b );
            p.X += width;
            p.Y += QuarterHeight;
            if ( weaponList.SpiderMine == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.SpiderMine.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.1f, 0.575f );
            b = new UIButton( @"Content\Menu\FlameThrowerActive", @"Content\Menu\FlameThrowerInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( FlameThrower, b );
            p.X += width;
            p.Y += QuarterHeight; 
            if ( weaponList.FlameThrower == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.FlameThrower.ToString() ) );
            }

            p = CameraManager.GetPosition( 0.5f, 0.575f );
            b = new UIButton( @"Content\Menu\MineActive", @"Content\Menu\MineInactive", p );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Mine, b );
            p.X += width;
            p.Y += QuarterHeight;
            if ( weaponList.LandMine == -1 )
            {
                Add( new UIImageElement( infinity, p ) );
            }
            else
            {
                p.X += QuarterWidth;
                Add( new UITextElement( p, sf, weaponList.LandMine.ToString() ) );
            }

        }

        void Mine( )
        {
            if ( wl.LandMine == 0 )
            {
                return;
            }
            po.SwapWeapon( new LandMineLauncher( po ) );
            MenuSystemInstance.PopTopMenu();
        }
        void Rocket( )
        {
            if ( wl.Rocket == 0 )
            {
                return;
            }
            po.SwapWeapon( new RocketLauncher( po ) );
            MenuSystemInstance.PopTopMenu();
        }
        void Cluster( )
        {
            if ( wl.Cluster == 0 )
            {
                return;
            }
            po.SwapWeapon( new ClusterLauncher( po ) );
            MenuSystemInstance.PopTopMenu();
        }
        void Grenade( )
        {
            if ( wl.Grenade == 0 )
            {
                return;
            }
            po.SwapWeapon( new GrenadeLauncher( po ) );
            MenuSystemInstance.PopTopMenu();
        }
        void JetPack( )
        {
            if ( wl.JetPack == 0 )
            {
                return;
            }
            po.SwapWeapon( new JetPack( po ) );    
            MenuSystemInstance.PopTopMenu();
        }
        void Napalm( )
        {
            if ( wl.CarpetBomb == 0 )
            {
                return;
            }
            po.SwapWeapon( new  CarpetBombLauncher(po) );
            MenuSystemInstance.PopTopMenu();
        }
        void SpiderMine( )
        {
            if ( wl.SpiderMine == 0 )
            {
                return;
            }
            po.SwapWeapon( new SpiderMineLauncher( po ) );
            MenuSystemInstance.PopTopMenu();
        }
        void FlameThrower( )
        {
            if ( wl.FlameThrower == 0 )
            {
                return;
            }
            po.SwapWeapon( new FlameThrower( po ) );
            MenuSystemInstance.PopTopMenu();
        }
    }
}

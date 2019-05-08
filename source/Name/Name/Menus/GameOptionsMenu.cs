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
using Engine.UI;


namespace Name.Menus
{
    class GameOptionsMenu : UIBaseMenu
    {
        SpriteFont sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
        GameOptions Options;
        GameOptions DefaultOptions;

        UITextOption Shoot;
        UITextOption Display;
        UITextOption HP;
        UITextOption TL;
        UITextOption BC;

        public GameOptionsMenu( GameOptions options )
            : base( @"SharedContent\Background", Point.Zero )
        {
            DefaultOptions = new GameOptions();
            System.IO.StreamReader sr = FileMananger.Instance.ReadFile( FileMananger.OptionsFile );
            DefaultOptions.Load( new System.IO.BinaryReader( sr.BaseStream ) );
            sr.Close();

            Options = options;
            AddUIElement( new UITextElement( CameraManager.GetPosition( 0.025f, 0.187f ), sf, "Game Options Menu" ) );

            UIButton b = new UIButton( @"Content\Menu\Back", CameraManager.GetPosition( 0.025f, 0.8125f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );

            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                ItemList.Add( "Any:" + DefaultOptions.DisplayModeString );
                ItemList.Add( "Upright" );
                ItemList.Add( "LandScape" );
                for ( int i = 0; i < ( int ) Options.DisplayMode; ++i )
                {
                    ItemList.Next();
                }
                Display = new UITextOption( "Display: \n     ", ItemList, CameraManager.GetPosition( 0.025f, 0.31f ) );
                Display.ValidStates = ( int ) TouchStates.Released;
                Add( DisplayChanged, Display );
            }
            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                ItemList.Add( "Any:" + DefaultOptions.ShootModeString  );
                ItemList.Add( "Standard" );
                ItemList.Add( "Classic" );
                for ( int i = 0; i < ( int ) Options.ShootMode; ++i )
                {
                    ItemList.Next();
                }
                Shoot = new UITextOption( "Shoot: \n     ", ItemList, CameraManager.GetPosition( 0.025f, 0.4f ) );
                Shoot.ValidStates = ( int ) TouchStates.Released;
                Add( ShootChanged, Shoot );
            }
            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                ItemList.Add( "50");
                ItemList.Add( "75" );
                ItemList.Add( "100" );
                ItemList.Add( "150" );
                ItemList.Add( "200" );
                ItemList.Next();
                ItemList.Next();
                HP = new UITextOption( "HitPoints:  ", ItemList, CameraManager.GetPosition( 0.025f, 0.49f ) );
                HP.ValidStates = ( int ) TouchStates.Released;
                Add( HitPoints, HP );
            }
            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                ItemList.Add( "20" );
                ItemList.Add( "30" );
                ItemList.Add( "45" );
                ItemList.Add( "60" );
                ItemList.Add( "90" );
                ItemList.Next();
                TL = new UITextOption( "Turn Length:  ", ItemList, CameraManager.GetPosition( 0.025f, 0.58f ) );
                TL.ValidStates = ( int ) TouchStates.Released;
                Add( TurnLength, TL );
            }
            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                ItemList.Add( "1" );
                ItemList.Add( "2" );
                ItemList.Add( "3" );
                ItemList.Add( "4" );
                ItemList.Add( "5" );
                ItemList.Add( "6" );
                ItemList.Next();
                ItemList.Next();
                ItemList.Next();
                BC = new UITextOption( "Number of Units: ", ItemList, CameraManager.GetPosition( 0.025f, 0.67f ) );
                BC.ValidStates = ( int ) TouchStates.Released;
                Add( BotCount, BC );
            }
        }

        public void TurnLength( )
        { // 20,30,45,60,90
            TL.Next();
            switch ( TL.Current )
            {
                case "20":
                    Options.TurnLength = 20;
                    break;
                case "30":
                    Options.TurnLength = 30;
                    break;
                case "45":
                    Options.TurnLength = 45;
                    break;
                case "60":
                    Options.TurnLength = 60;
                    break;
                case "90":
                    Options.TurnLength = 90;
                    break;
                default:
                    DebugHelper.Break( DebugHelper.DebugLevels.Important );
                    break;
            }
        }

        public void BotCount( )
        { // 1 -> 6
            BC.Next();
            switch ( BC.Current )
            {
                case "1":
                    Options.NumberOfPlayers = 1;
                    break;
                case "2":
                    Options.NumberOfPlayers = 2;
                    break;
                case "3":
                    Options.NumberOfPlayers = 3;
                    break;
                case "4":
                    Options.NumberOfPlayers = 4;
                    break;
                case "5":
                    Options.NumberOfPlayers = 5;
                    break;
                case "6":
                    Options.NumberOfPlayers = 6;
                    break;
                default:
                    DebugHelper.Break( DebugHelper.DebugLevels.Important );
                    break;

            }
        }

        public void HitPoints( )
        { // 50,75,100,150,200
            HP.Next();
            switch ( HP.Current )
            {
                case "50":
                    Options.NumHitPoints = 50;
                    break;
                case "75":
                    Options.NumHitPoints = 75;
                    break;
                case "100":
                    Options.NumHitPoints = 100;
                    break;
                case "150":
                    Options.NumHitPoints = 150;
                    break;
                case "200":
                    Options.NumHitPoints = 200;
                    break;
                default:
                    DebugHelper.Break( DebugHelper.DebugLevels.Important );
                    break;
            }
        }

        public void DisplayChanged( )
        {
            Display.Next();
            String current = Display.Current;
            if ( current == "Upright" )
            {
                Options.DisplayMode = DisplayMode.Normal;
            }
            else if ( current == "LandScape" )
            {
                Options.DisplayMode = DisplayMode.Landscape;
            }
            else
            {
                Options.DisplayMode = DisplayMode.UserChoice;
            }
        }

        public void ShootChanged( )
        {
            Shoot.Next();
            String current = Shoot.Current;
            if ( current == "Standard" )
            {
                Options.ShootMode = ShootMode.Normal;
            }
            else if ( current == "Classic" )
            {
                Options.ShootMode = ShootMode.Classic;
            }
            else
            {
                Options.ShootMode = ShootMode.UserChoice;
            }
        }
    }
}

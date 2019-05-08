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
    class OptionsMenu : UIBaseMenu
    {
        SpriteFont sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
        GameOptions Options;

        UITextOption Shoot;
        UITextOption Display;
        public OptionsMenu()
            : base( @"SharedContent\Background", Point.Zero )
        {
            Options = new GameOptions();
            System.IO.StreamReader sr = FileMananger.Instance.ReadFile( FileMananger.OptionsFile );
            Options.Load( new System.IO.BinaryReader( sr.BaseStream ) );
            sr.Close();

            AddUIElement(new UITextElement(CameraManager.GetPosition(0.025f, 0.187f),sf,"Options Menu" ));
            UIButton b = new UIButton(@"Content\Menu\Back", CameraManager.GetPosition(0.025f, 0.8125f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );

            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                if ( Options.DisplayMode == DisplayMode.Normal )
                {
                    ItemList.Add( "Upright" );
                    ItemList.Add( "LandScape" );
                }
                else if(Options.DisplayMode == DisplayMode.Landscape)
                {
                    ItemList.Add( "LandScape" );
                    ItemList.Add( "Upright" );
                }
                Display = new UITextOption( "Display: ", ItemList, CameraManager.GetPosition( 0.025f, 0.31f ) );
                Display.ValidStates = ( int ) TouchStates.Released;
                Add( DisplayChanged, Display );
            }
            {
                CircularObjectList<String> ItemList = new CircularObjectList<string>();
                if ( Options.ShootMode == ShootMode.Normal )
                {
                    ItemList.Add( "Standard" );
                    ItemList.Add( "Classic" );
                }
                else
                {
                    ItemList.Add( "Classic" );
                    ItemList.Add( "Standard" );
                }
                Shoot = new UITextOption( "Shoot: ", ItemList, CameraManager.GetPosition( 0.025f, 0.4f ) );
                Shoot.ValidStates = ( int ) TouchStates.Released;
                Add( ShootChanged, Shoot );
            }
            // Turn Length  - 15, 30, 45, 60
            // Number Players - 1,2,3,4, 5, 6
            // Default HP - 50, 100, 150, 200
            // Weapon Loadouts  - Standard, Customize 
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
                DebugHelper.Break( DebugHelper.DebugLevels.Important );
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
                DebugHelper.Break( DebugHelper.DebugLevels.Important );
            }
        }

        public override void OnHide( )
        {
            base.OnHide();
            System.IO.StreamWriter br = FileMananger.Instance.OpenFileForWrite( FileMananger.OptionsFile );
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter( br.BaseStream );
            Options.Save( bw );
            bw.Close();
        }
    }
}

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

namespace TBS.Menus
{
    public class MainMenu : UIBaseMenu
    {
        public MainMenu( )
            : base( @"SharedContent\Background", Point.Zero )
        {
            UIButton b = new UIButton( @"Content\Menu\Exit", new Point( 100, 400 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            b = new UIButton( @"Content\Menu\Credits", new Point( 100, 300 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Credits, b );
            b = new UIButton( @"Content\Menu\Options", new Point( 100, 200 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Options, b );
            b = new UIButton( @"Content\Menu\Start", new Point( 100, 100 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Start, b );

        }

        public override void Back( )
        {
            //@@ Terminate the Application
        }

        protected void Credits( )
        {
            MenuSystemInstance.PushMenu( new CreditsMenu() );
        }

        protected void Start( )
        {
            MenuSystemInstance.PushMenu( new HumanPlayersMenu() );
        }

        protected void Options( )
        {
            MenuSystemInstance.PushMenu( new OptionsMenu() );
        }
    }
}

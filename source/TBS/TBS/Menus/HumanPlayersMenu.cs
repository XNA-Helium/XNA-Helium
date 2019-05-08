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
    class HumanPlayersMenu : UIBaseMenu
    {
        public HumanPlayersMenu( )
            : base( @"SharedContent\Background", Point.Zero )
        {
            UIButton b = new UIButton( @"Content\Menu\Back", new Point( 100, 600 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            b = new UIButton( @"Content\Menu\4p", new Point( 100, 500 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( FourP, b );
            b = new UIButton( @"Content\Menu\3p", new Point( 100, 400 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ThreeP, b );
            b = new UIButton( @"Content\Menu\2p", new Point( 100, 300 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( TwoP, b );
            b = new UIButton( @"Content\Menu\1p", new Point( 100, 200 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( OneP, b );
            b = new UIButton( @"Content\Menu\0p", new Point( 100, 100 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ZeroP, b );
        }

        void FourP( )
        { 
            MenuSystemInstance.PushMenu( new AIPlayersMenu( 4 ) );
        }
        void ThreeP( )
        {
            MenuSystemInstance.PushMenu( new AIPlayersMenu( 3 ) );
        }
        void TwoP( )
        {
            MenuSystemInstance.PushMenu( new AIPlayersMenu( 2 ) );
        }
        void OneP( )
        {
            MenuSystemInstance.PushMenu( new AIPlayersMenu( 1 ) );
        }
        void ZeroP( )
        {
            MenuSystemInstance.PushMenu( new AIPlayersMenu( 0 ) );
        }
    
    }
}

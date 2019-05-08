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
    class AIPlayersMenu : UIBaseMenu
    {
        protected int Human;
        public AIPlayersMenu(int current )
            : base( @"SharedContent\Background", Point.Zero )
        {
            AddUIElement(new UIImageElement(@"Content\Menu\AIplayers", CameraManager.GetPosition(0.025f, 0.187f)));
            Human = current;
            UIButton b = new UIButton(@"Content\Menu\Back", CameraManager.GetPosition(0.025f, 0.8125f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            b = new UIButton(@"Content\Menu\0p", CameraManager.GetPosition(0.025f, 0.31f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ZeroP, b );
            if ( current == 4 ) return;
            b = new UIButton(@"Content\Menu\1p", CameraManager.GetPosition(0.025f, 0.4f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( OneP, b );
            if ( current == 3 ) return;
            b = new UIButton(@"Content\Menu\2p", CameraManager.GetPosition(0.025f, 0.49f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( TwoP, b );
            if ( current == 2 ) return;
            b = new UIButton(@"Content\Menu\3p", CameraManager.GetPosition(0.025f, 0.58f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ThreeP, b );
            if ( current == 1 ) return;
            b = new UIButton(@"Content\Menu\4p", CameraManager.GetPosition(0.025f, 0.67f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( FourP, b );
        }
    
        void FourP( )
        {
            MenuSystemInstance.PushMenu(new LevelSelectionMenu( Human, 4 ) );
        }
        void ThreeP( )
        {
            MenuSystemInstance.PushMenu( new LevelSelectionMenu( Human, 3 ) );
        }
        void TwoP( )
        {
            MenuSystemInstance.PushMenu( new LevelSelectionMenu( Human, 2 ) );
        }
        void OneP( )
        {
            MenuSystemInstance.PushMenu( new LevelSelectionMenu( Human, 1 ) );
        }
        void ZeroP( )
        {
            MenuSystemInstance.PushMenu( new LevelSelectionMenu( Human, 0 ) );
        }

    }
}
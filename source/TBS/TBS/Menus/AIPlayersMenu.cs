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
    class AIPlayersMenu : UIBaseMenu
    {
        protected int Human;
        public AIPlayersMenu(int current )
            : base( @"SharedContent\Background", Point.Zero )
        {
            Human = current;
            UIButton b = new UIButton( @"Content\Menu\Back", new Point( 100, 600 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            b = new UIButton( @"Content\Menu\0p", new Point( 100, 100 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ZeroP, b );
            if ( current == 4 ) return;
            b = new UIButton( @"Content\Menu\1p", new Point( 100, 200 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( OneP, b );
            if ( current == 3 ) return;
            b = new UIButton( @"Content\Menu\2p", new Point( 100, 300 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( TwoP, b );
            if ( current == 2 ) return;
            b = new UIButton( @"Content\Menu\3p", new Point( 100, 400 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( ThreeP, b );
            if ( current == 1 ) return;
            b = new UIButton( @"Content\Menu\4p", new Point( 100, 500 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( FourP, b );
        }
    
        void FourP( )
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState());
            MenuSystemInstance.PushMenu(new GamePlayMenu( Human, 4 ) );
        }
        void ThreeP( )
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState());
            MenuSystemInstance.PushMenu( new GamePlayMenu( Human, 3 ) );
        }
        void TwoP( )
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState());
            MenuSystemInstance.PushMenu( new GamePlayMenu( Human, 2 ) );
        }
        void OneP( )
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState());
            MenuSystemInstance.PushMenu( new GamePlayMenu( Human, 1 ) );
        }
        void ZeroP( )
        {
            Engine.GameState.GameStateSystem.Instance.AddGameState(new GameState.PlayingState());
            MenuSystemInstance.PushMenu( new GamePlayMenu( Human, 0 ) );
        }

    }
}

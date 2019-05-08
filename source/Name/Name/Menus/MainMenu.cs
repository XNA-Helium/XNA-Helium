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

    // UPRIGHT MENU ONLY. ONLY GAMEPLAY WORKS IN LANDSCAPE

    public class MainMenu : UIBaseMenu
    {
        bool FirstRun = true;
        public MainMenu( )
            : base( @"SharedContent\Title_Background", new Point(0,0) )
        {
            UIButton b = new UIButton( @"Content\Menu\Exit",CameraManager.GetPosition(0.025f, 0.75f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            b = new UIButton( @"Content\Menu\Credits", CameraManager.GetPosition(0.025f, 0.60f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Credits, b );
            b = new UIButton( @"Content\Menu\Options",CameraManager.GetPosition(0.025f, 0.45f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( OptionsMenu, b );
            b = new UIButton( @"Content\Menu\Start",CameraManager.GetPosition(0.025f, 0.30f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Start, b );

        }

        public override void OnShow( )
        {
#if DEBUG
            DebugHelper.Break(Engine.GameState.GameStateSystem.Instance.Count > 0, DebugHelper.DebugLevels.Important);
#endif
            {
                EngineGame.SwitchToResolution(DisplayOrientation.Portrait, 480, 800, false);
            }
            if ( FirstRun )
            {
                FirstRun = false;
                MenuSystemInstance.PushMenu( new LoadingMenu(@"Content\Sprites\Projectiles\Grenade") );
            }
            base.OnShow();
        }

        public override void Back( )
        {
            EngineGame.EndGame();
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

        protected void OptionsMenu( )
        {
            MenuSystemInstance.PushMenu( new OptionsMenu() );
        }

    }
}

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

using Name;
using Name.GameState;

namespace Name.Menus
{
    public class GameOverMenu : UIBaseMenu
    {
        SpriteFont sf;  
        NameTeam team;
        public GameOverMenu( PlayingState ps )
        //: base( @"SharedContent\Background", new Point(0,0) )
        {
            CircularObjectList<NameTeam> o = ps.NameTeams;
            int Tie = 0;
            foreach ( NameTeam nt in o )
            {
                if ( !nt.DeadTeam )
                {
                    ++Tie;
                }
            }
            if ( Tie == 1 )
            {
                foreach ( NameTeam nt in o )
                {
                    if ( !nt.DeadTeam )
                    {
                        team = nt;
                    }
                }
            }


            UIButton b = new UIButton(@"Content\Menu\Back", CameraManager.GetPosition(0.025f, 0.8125f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
            if ( Tie != 1 )
            {
                UITextElement t = new UITextElement(CameraManager.GetPosition(0.25f, 0.375f), sf, "Tie Game!");
                AddUIElement( t );
            }
            else
            {
                UITextElement t = new UITextElement(CameraManager.GetPosition(0.25f, 0.375f), sf, team.TeamName);
                AddUIElement( t );
            }

            EventManager.Instance.ClearAll();
        }

        public override void OnRemove( )
        {
            Engine.GameState.GameStateSystem.Instance.RemoveGameState( Engine.GameState.GameStateSystem.Instance.GetCurrentState );
            base.OnRemove();
        }
    }
}

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
using Name;

namespace Name.Menus
{
    public class LocalGameMenu : UIBaseMenu
    {
        // Flow control
        // Turn announcement 
        SpriteFont sf;
        String Top = "";
        String bottom = "";
        Vector2 TopPosition = Vector2.Zero;
        Vector2 BottomPosition = Vector2.Zero;
        Color color = Color.White;
        PlayingState ps;
        bool FirstTurn = true;
        public static float SECONDS = 1.75f;
        protected bool EventSet = false;

        bool GameOver = false;
        GameOptions options;
        public LocalGameMenu(Name.GameState.PlayingState PS )
            : base()
        {
            options = PS.GameOptions;
            GameOptions DefaultOptions = new GameOptions( true );
            if ( options.ShootMode == ShootMode.UserChoice )
            {
                options.ShootMode = DefaultOptions.ShootMode;
            }
            if ( options.DisplayMode == DisplayMode.UserChoice )
            {
                options.DisplayMode = DefaultOptions.DisplayMode;
            }

            ps = PS;
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );

            if ( !EventSet )
            {
                EventSet = true;
                System.Random rand = new Random();
                if ( rand.Next( 0, 10 ) == 1 )
                {
                    int width = (Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState).Terrain.m_BoundingRectangle.Width;
                    HealthPack hp = new HealthPack( 25, new Vector2( rand.Next( Terrain.EdgeOffset, width - Terrain.EdgeOffset ), 0 ) );
                    Engine.MenuSystem.Instance.PushMenu( new Name.Menus.Tracker(hp));
                    return;
                }
                if ( FirstTurn )
                {
                    FirstTurn = false;
                    EventManager.Instance.AddEvent( StartTurn, SECONDS );
                }
                else
                {
                    AdvanceTeam();
                    if ( GameOver )
                        return;
                }
                {
                    Top = ps.CurrentTeam.TeamName;
                    if ( ps.CurrentTeam.Human )
                        bottom = "Human Team - " + ps.CurrentTeam.HP.ToString();
                    else if ( ps.CurrentTeam.AITeam )
                        bottom = "AI Team - " + ps.CurrentTeam.HP.ToString();
                    else
                        bottom = "";

                    TopPosition = sf.MeasureString( Top ) / 2.0f;
                    BottomPosition = sf.MeasureString( bottom ) / 2.0f;
                    color = ps.CurrentTeam.Color;
                    Point p = ps.Current.Position.ToPoint();
                    CameraManager.Instance.Current.CenterOn(ref p);
                }
            }
        }

        Vector2 TopStringPosition = CameraManager.GetPosition( 0.5f, 0.2f ).ToVector2();
        Vector2 BottomStringPosition = CameraManager.GetPosition( 0.5f, 0.375f ).ToVector2();
        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            base.Draw( gameTime, spriteBatch );
            spriteBatch.DrawString( sf, Top, TopStringPosition - TopPosition, color );
            spriteBatch.DrawString( sf, bottom, BottomStringPosition - BottomPosition, color );
        }

        public override void OnHide( )
        {
            base.OnHide();
        }

        public override void OnShow( )
        {
            base.OnShow();
            EventSet = false;
        }

        public override void OnRemove( )
        {
            base.OnRemove();
        }

        public void AdvanceTeam( )
        {
            if ( !ps.AdvanceTeam() )
            {
                GameOver = true;
                MenuSystemInstance.PopTopMenu();
                MenuSystemInstance.AddMenu( new GameOverMenu( ps ) );

                return;
            }
            NameTeam team = ps.CurrentTeam;
            EventManager.Instance.AddEvent( StartTurn, SECONDS );
        }

        public void StartTurn( )
        {
            NameTeam team = ps.CurrentTeam;
            if ( team.AITeam )
            {
                StartAITurn( team.CurrentlyActive, team );
            }
            else
            {
                StartHumanTurn( team.CurrentlyActive, team );
            }
        }

        public void StartHumanTurn( PlayerObject po, NameTeam team )
        {
            //@@ Switch Display Mode if we are in classic mode
            Engine.MenuSystem.Instance.PushMenu( new Name.Menus.GamePlayMenu(po, team, 0.0f, options ) );
        }

        public void StartAITurn( PlayerObject po, NameTeam team )
        {
            //@@ Switch Display Mode if we are in classic mode
            Engine.MenuSystem.Instance.PushMenu( new Name.Menus.AITeamMenu(po, team, 0.0f, options ) );
        }
    }

}
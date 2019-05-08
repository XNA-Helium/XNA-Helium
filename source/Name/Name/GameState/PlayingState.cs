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
using Engine.GameState;


namespace Name.GameState
{
    public class PlayingState : Engine.GameState.PrimaryGameState
    {
        protected Terrain terrain;
        Texture2D BlackBox;
        protected GameOptions Options;
        public GameOptions GameOptions
        {
            get
            {
                return Options;
            }
        }

        public PlayerObject Current
        {
            get
            {
                return Teams.Current.CurrentlyActive;
            }
        }

        public NameTeam CurrentTeam
        {
            get
            {
                return Teams.Current;
            }
        }
        CircularObjectList<NameTeam> Teams = new CircularObjectList<NameTeam>();
        public CircularObjectList<NameTeam> NameTeams
        {
            get
            {
                return Teams;
            }
        }
        public Terrain Terrain
        {
            get
            {
                return terrain;
            }
        }
        public PlayingState(GameOptions options, int HumanPlayers, int ComputerPlayers,TerrainSaver ts ):base()
        {
            Options = options;
            objectList = new List<Engine.Core.BaseObject>( 128 );
            terrain = new Terrain( @"Content\MiniMap\BackgroundL", @"Content\MiniMap\BackgroundR", ts );
            Rectangle World = new Rectangle(0, 0, terrain.m_BoundingRectangle.Width, terrain.m_BoundingRectangle.Height + 500 );
            Rectangle CameraWorld = new Rectangle( -Terrain.EdgeOffset, 0, terrain.m_BoundingRectangle.Width + ( 2 * Terrain.EdgeOffset ), terrain.m_BoundingRectangle.Height + 500 );
            Physic2DManagerWithTerrain.SetupPhysics2DManager( terrain, World );
            CameraManager.Instance.Current.SetConstrainRectangle( ref CameraWorld );
            BlackBox = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\TerrainPiece" );
            
            int teamcount = 0;
            // Setup Human Players
            for ( int i = 0; i < HumanPlayers; ++i )
            {
                Teams.Add( new NameTeam( Team.TeamName( teamcount ), false, teamcount, true, Team.TeamColor(teamcount) ) );
                ++teamcount;
            }
            // Setup AI Players
            for (int i = 0; i < ComputerPlayers; ++i)
            {
                Teams.Add(new NameTeam(Team.TeamName(teamcount), true, teamcount, true, Team.TeamColor(teamcount)));
                ++teamcount;
            }
        }

        public void SetupPlayers()
        {
            int total = Teams.Count * Options.NumberOfPlayers;
            Point[] points = new Point[total];
            int width = terrain.m_BoundingRectangle.Width - (2 * Terrain.EdgeOffset);

            int piece = ( width / total + 2 );
            for ( int i = 0; i < total; ++i )
            {
                int w = (1 + i ) * piece;
                points[i] = new Point( w, terrain[w] - 32);
            }

            for ( int i = 0; i < Teams.Count; ++i )
            {
                for ( int j = 0; j < Options.NumberOfPlayers; ++j )
                {
                    PlayerObject p = Teams[i].AddPlayer( points[i + ( j * Teams.Count )] );
                    objectList.Add( p );
                    HitPoints hp = ( p[HitPoints.TypeStatic] as HitPoints );
                    hp.Hitpoints = Options.NumHitPoints;
                }
            }

            Point point = (  Teams.Current.CurrentlyActive[Engine.Core.Placeable.TypeStatic] as Engine.Core.Placeable ).Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref point );
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );
            Physic2DManagerWithTerrain.Instance.Update( gameTime );
        }

        public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
        {
            Rectangle r = CameraManager.Instance.Current.Rectangle;
            Vector2 LeftUpperLeft = Vector2.Zero - new Vector2( Terrain.EdgeOffset, 0 ) - new Vector2( r.X, r.Y );
            Vector2 RightUpperLeft = LeftUpperLeft + new Vector2( terrain.BackgroundLeft.Width, 0 );
            spriteBatch.Begin();
            spriteBatch.Draw( terrain.BackgroundLeft, LeftUpperLeft, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.99f );
            spriteBatch.Draw( terrain.BackgroundRight, RightUpperLeft, null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.99f );
            int start = r.Left;
            int end = r.Right;
            for ( int i = start; i < end; ++i )
            {
                spriteBatch.Draw( BlackBox, new Vector2( i - r.Left, terrain[i] - r.Top ), null, Color.White, 0.0f, Vector2.Zero, new Vector2( 1.0f, 1.0f ), SpriteEffects.None, 0.97f );
            }
            spriteBatch.End( );
            base.Draw( gameTime, spriteBatch );
        }
        protected bool ValidateTeam( )
        {
            if ( !CurrentTeam.DeadTeam && CurrentTeam.HP > 0 )
                return true;
            else
                return false;
        }

        protected bool FindNextValidTeam( )
        {
            int Tie = 0;
            foreach ( NameTeam nt in Teams )
            {
                if ( nt.DeadTeam )
                {
                    ++Tie;
                }
            }
            if ( Tie >= Teams.Count - 1 )
            {
                return true; // We have 1 or 0 teams still alive, the game is over
            }

            NameTeam c = Teams.Current;
            int totaliterations = 0;
            do
            {
                Teams.Next();
                ++totaliterations;
            } while ( ( Teams.Current != c ) && Teams.Current.DeadTeam );

            return totaliterations >= Teams.Count;
        }

        public bool AdvanceTeam( )
        {
            bool OnlyOneTeamLeft = FindNextValidTeam();
            if ( ValidateTeam() && !OnlyOneTeamLeft )
                return true;
            else
                return false;
        }
        public bool GameOver()
        {
            int deadteams = 0;
            foreach (NameTeam t in Teams)
            {
                if (t.DeadTeam)
                {
                    ++deadteams;
                }
            }
            return ((deadteams + 1) >= Teams.Count);
        }
    }
}

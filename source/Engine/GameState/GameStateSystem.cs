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

using Engine.Core;

namespace Engine.GameState
{
    public class GameStateSystem :  IEngineUpdateable, IEngineDebugDrawable
    {

        List<PrimaryGameState> gameStates = new List<PrimaryGameState>( 16 );
        List<PrimaryGameState> gameStatesAdded = new List<PrimaryGameState>(16);
        List<PrimaryGameState> gameStatesRemoved = new List<PrimaryGameState>(16);

        public PrimaryGameState GetCurrentState
        {
            get
            {
                DebugHelper.Break(gameStates.Count == 0, DebugHelper.DebugLevels.Explosive);
                return gameStates[gameStates.Count - 1];
            }
        }
        public int Count
        {
            get
            {
                return gameStates.Count;
            }
        }
        public PrimaryGameState this[int index]
        {
            get
            {
                return gameStates[index];
            }
        }

        protected static GameStateSystem instance = null;
        public static GameStateSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateSystem();
                }
                return instance;
            }
        }
        bool IteratingState = false;
        public virtual void Update(GameTime gameTime)
        {
            IteratingState = true;
            foreach ( BaseGameState g in gameStates )
            {
                if (!g.Paused)
                    g.Update( gameTime );
            }
            IteratingState = false;

            foreach (PrimaryGameState g in gameStatesAdded)
            {
                AddGameState(g);
            }
            gameStatesAdded.Clear();

            foreach (PrimaryGameState g in gameStatesRemoved)
            {
                RemoveGameState(g);
            }
            gameStatesRemoved.Clear();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            IteratingState = true;
            foreach ( BaseGameState g in gameStates )
            {
                g.Draw( gameTime, spriteBatch );
            }
            IteratingState = false;
        }

        public virtual void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            IteratingState = true;
            foreach ( BaseGameState g in gameStates )
            {
                g.DebugDraw( gameTime, spriteBatch );
            }
            IteratingState = false;
        }

        public virtual BaseGameState FindObject( ref BaseObject FindMe )
        {
            foreach ( BaseGameState g in gameStates )
            {
                if ( g.FindObject( ref FindMe ) )
                {
                    return g;
                } 
            }
            return null;
        }

        public virtual void AddGameState( PrimaryGameState state )
        {
            if (!IteratingState)
            {
                gameStates.Add(state);
                state.Initialize();
            }
            else
                Engine.DebugHelper.Break(new Exception("Write Delayed GameState Add/Remove system"), DebugHelper.DebugLevels.Critical);
        }

        public virtual void RemoveGameState( PrimaryGameState state )
        {
            if (!IteratingState)
            {
                gameStates.Remove(state);
                state.Shutdown();
            }
            else
                Engine.DebugHelper.Break( new Exception( "Write Delayed GameState Add/Remove system" ), DebugHelper.DebugLevels.Critical );
        }

        public virtual void AddGameStateLater(PrimaryGameState state)
        {
            gameStatesAdded.Add(state);
        }

        public virtual void RemoveGameStateLater(PrimaryGameState state)
        {
            gameStatesRemoved.Add(state);
        }

        public virtual void RemoveObjectLater(BaseObject baseObject)
        {
            foreach ( BaseGameState g in gameStates )
            {
                g.RemoveFromObjectListLater( baseObject );
            }
        }
    }
}

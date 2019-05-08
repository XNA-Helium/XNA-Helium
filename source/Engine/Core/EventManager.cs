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

namespace Engine
{
    public class EventManager
    {
        private GameTime gt;
        private static EventManager instance;
        public bool IteratingList = false;
        public class GameEvent
        {
            private float ElapsedTime;
            private Callback eh;
            private float Seconds;
            bool Passed;

            public void Save(Engine.Persistence.PersistenceManager sw)
            {
                // Custom Save code goes here
                sw.BinaryWriter.Write(Seconds);
                sw.BinaryWriter.Write(Passed);
                sw.BinaryWriter.Write(ElapsedTime);
            }
            protected GameEvent() { }
            public static GameEvent Load(Callback p_eh, Engine.Persistence.PersistenceManager sr)
            {
                // Custom Load code goes here
                GameEvent g = new GameEvent();
                g.eh = p_eh;
                g.Seconds = sr.BinaryReader.ReadSingle();
                g.Passed = sr.BinaryReader.ReadBoolean();
                g.ElapsedTime = sr.BinaryReader.ReadSingle();
                return g;
            }

            public GameEvent(Callback p_eh, float p_Seconds)
            {
                eh = p_eh;
                Seconds = p_Seconds;
                Passed = false;
                //Repeating = false;
                ElapsedTime = 0;
            }
            public void Update( GameTime gt )
            {
                if (!Passed)
                {
                    ElapsedTime += gt.ElapsedGameTime.Milliseconds / 1000.0f;
                    if (ElapsedTime > Seconds && !Passed)
                    {
                        eh.Invoke();
                        Passed = true;
                    }
                }
            }
            public bool HasPassed
            {
                internal set
                {
                    Passed = value;
                }
                get { return Passed; }
            }
        }
        List<GameEvent> Events;
        List<GameEvent> RemoveUS;

        private EventManager()
        {
            Events = new List<GameEvent>();
            gt = new GameTime(TimeSpan.Zero, TimeSpan.Zero);
            RemoveUS = new List<GameEvent>();
        }

        public static EventManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }
        public void RemoveEvent( GameEvent e )
        {
            if ( IteratingList )
            {
                RemoveUS.Add( e );
                e.HasPassed = true;
            }
            else
                Events.Remove( e );
        }
        public GameEvent AddEvent(GameEvent e)
        {
            Events.Add(e);
            return e;
        }
        public GameEvent AddEvent(Callback p_eh, float p_seconds)
        {
            float blah = gt.ElapsedGameTime.Seconds + (gt.ElapsedGameTime.Milliseconds / 1000.0f);
            GameEvent e = new GameEvent(p_eh, p_seconds);
            Events.Add(e);
            return e;
        }
        public void ClearAll( )
        {
            Engine.DebugHelper.Break( IteratingList , DebugHelper.DebugLevels.HighlyCritical );
            Events.Clear();
        }
        public void Update(GameTime time)
        {
            if (GameState.GameStateSystem.Instance.Count > 0)
            {
                if (GameState.GameStateSystem.Instance.GetCurrentState.Paused)
                {
                    return;
                }
            }
            gt = time;
            IteratingList = true;
            for (int i = 0; i < Events.Count; ++i)
            {
                Events[i].Update( time );
                if (Events[i].HasPassed)
                {
                    RemoveUS.Add(Events[i]);
                }
            }
            IteratingList = false;
            for (int i = 0; i < RemoveUS.Count; ++i)
            {
                Events.Remove(RemoveUS[i]);
            }
            RemoveUS.Clear();
        }

    }
}

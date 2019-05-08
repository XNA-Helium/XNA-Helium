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

namespace Engine
{
    public class TimeTracker : IEngineUpdateable
    {
        protected double starttime = 0.0f;
        protected double TargetTime = 0.0f;
        protected double Total = 0.0f;
        /// <summary>
        /// Counts down
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="Delta"></param>
        public TimeTracker(double Delta)
        {
            starttime = Delta;
            TargetTime = Delta;
            Total = Delta;
            GameState.GameStateSystem.Instance.GetCurrentState.AddToUpdateList(this);
        }
        /// <summary>
        /// For Save/Load use only
        /// </summary>
        public TimeTracker()
        {
            GameState.GameStateSystem.Instance.GetCurrentState.AddToUpdateList(this);
        }
        public void Save(Engine.Persistence.PersistenceManager sw)
        {
            sw.BinaryWriter.Write(starttime);
            sw.BinaryWriter.Write(TargetTime);
            sw.BinaryWriter.Write(Total);
        }
        public void Load(Engine.Persistence.PersistenceManager sr)
        {
            starttime = sr.BinaryReader.ReadDouble();
            TargetTime = sr.BinaryReader.ReadDouble();
            Total = sr.BinaryReader.ReadDouble();
        }
        public void Update(GameTime gameTime)
        {
            TargetTime -= ((double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            if (TargetTime < 0.0f)
            {
                GameState.GameStateSystem.Instance.GetCurrentState.RemoveFromUpdateList(this);
            }
        }
        public double TotalTime
        {
            get
            {
                return Total;
            }
        }
        public double TimeLeft
        {
            get
            {
                return TargetTime;
            }
        }
        public bool Passed
        {
            get
            {
                return TargetTime <= 0.0f;
            }
        }
    }
}

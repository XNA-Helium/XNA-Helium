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
using Engine.GameState;
using Engine;

namespace Engine
{
    public class Settings
    {
        protected static Settings settings = new Settings(Color.Black,30.0f);
        public static Settings DefaultSettings
        {
            get
            {
                return settings;
            }
        }

        public Settings(Color clear, float framerate)
        {
            Background = clear;
            Framerate = framerate;
        }

        protected Color Background;
        protected float Framerate;

        public Color BackgroundColor
        {
            get
            {
                return Background;
            }
        }
        public float FPS
        {
            get
            {
                return Framerate;
            }
        }
    }
}

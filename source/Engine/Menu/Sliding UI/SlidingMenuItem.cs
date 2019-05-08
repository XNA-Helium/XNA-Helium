using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Engine.UI
{
    class SlidingMenuItem : UIImageElement
    {
        protected bool ShowStrip;
        protected SlidingMenuStrip strip;
        protected Callback myDelegate;

        protected SlidingMenuItem(String imagename,Point position, SlidingMenuStrip slidingMenuStrip):base(imagename,position)
        {
            myDelegate = null;
            strip = slidingMenuStrip;
        }
        public SlidingMenuItem(String imagename, Point position, Callback callback):base(imagename,position)
        {
            myDelegate = callback;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (ShowStrip && strip != null)
                strip.Draw(gameTime,spriteBatch);
        }

        public bool ProcessInput(Point point)
        {
            if (Extents().Contains(point))
            {
                if (myDelegate != null)
                {
                    myDelegate();
                }
                else
                {
                    ShowStrip = true;
                }
                return true;
            }
            else if (strip != null && ShowStrip == true)
            {
                ShowStrip = strip.ProcessInput(point);
                return ShowStrip;
            }
            else
            {
                ShowStrip = false;
                return false;
            }
        }
    }    
}



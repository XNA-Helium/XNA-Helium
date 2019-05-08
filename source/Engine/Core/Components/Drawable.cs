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


namespace Engine.Core
{
    public abstract class Drawable : BaseComponent , IEngineDrawable
    {
        protected bool visible = true;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                HasChanged = true;
                visible = value;
            }
        }
        public Drawable(bool InWorld,float uID ):base(InWorld,uID)
        {
            //Position is the CENTER 
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused);
    }
}

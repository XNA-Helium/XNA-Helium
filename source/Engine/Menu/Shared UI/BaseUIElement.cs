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



namespace Engine.UI
{
    public abstract class BaseUIElement : IExtents
    {
        protected bool enabled = true;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
            }
        }

        protected Rectangle extents;
        protected Point absolutionPosition;
        public Point Position
        {
            get
            {
                return absolutionPosition;
            }
            set
            {
                absolutionPosition = value;
            }
        }
        protected bool visible;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }
        protected float _DrawLayer;
        public BaseUIElement(Point position, float drawLayer)
        {
            _DrawLayer = drawLayer;
            visible = true;
            absolutionPosition = position;
        }
        public BaseUIElement(Point position)
            : this( position, 0.1f )
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public Rectangle Extents()
        {
            return extents;
        }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

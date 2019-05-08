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

    public class UIElementContainer<T> : BaseUIElement
    {
        protected List<KeyValuePair<T, BaseUIElement>> _Elements;

        public UIElementContainer()
            : base(Point.Zero)
        {
            _Elements = new List<KeyValuePair<T, BaseUIElement>>();
            extents = new Rectangle();
        }

        public virtual void Remove(T value, BaseUIElement Element)
        {
            KeyValuePair<T, BaseUIElement> p = new KeyValuePair<T, BaseUIElement>(value, Element);
            if (_Elements.Contains(p))
            {
                _Elements.Remove(p);
            }
        }
        public virtual void Add(T value, BaseUIElement Element)
        {
            _Elements.Add(new KeyValuePair<T, BaseUIElement>(value, Element));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<T, BaseUIElement> pairs in _Elements)
            {
                pairs.Value.Draw(gameTime,spriteBatch);
            }
        }
    }
}

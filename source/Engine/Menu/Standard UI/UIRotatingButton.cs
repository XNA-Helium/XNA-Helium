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
    public class UIRotatingButton : UIElementContainer<Callback>
    {
        protected int si;
        Point MiddlePosition;

        public UIRotatingButton(Point BasePosition)
            : base()
        {
            si = 0;
            MiddlePosition = BasePosition;
            Engine.DebugHelper.Break( "Not implimented", DebugHelper.DebugLevels.Curious );
        }
        //has a left button
        //has a right button
        //middle button rotates
        public override void Add(Callback value, BaseUIElement Element)
        {
            Element.Position = MiddlePosition;
            base.Add(value, Element);
        }
        public virtual void Add(Callback value, String Element)
        {
            UIImageElement i = new UIImageElement(Element, MiddlePosition);
            base.Add(value, i);
        }
        public virtual void Setup(int visible)
        {
            foreach (KeyValuePair<Callback, BaseUIElement> e in base._Elements)
            {
                e.Value.Visible = false;
            }
            base._Elements[visible].Value.Visible = true;
        }

        public virtual void ProcessInput()
        {
            if (si == 0)
            {
            }
            else if (si == (_Elements.Count - 1))
            {
            }
            else
            {
            }
            if (si > 0 && si < _Elements.Count - 1)
            {
            }
        }

        public bool AtFront
        {
            get
            {
                return (si == 0);
            }
        }

        public bool AtBack
        {
            get
            {
                return (si == (_Elements.Count - 1));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
        }
    }
}

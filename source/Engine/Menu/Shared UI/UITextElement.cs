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
    public class UITextElement : BaseUIElement
    {
        Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        float _scale = 1.0f;
        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        SpriteFont _sf;
        string _text;
        Vector2 hw;
        public String Text
        {
            get 
            { 
                return _text;
            }
            set
            {
                _text = value;
                hw = _sf.MeasureString(_text);
                extents = new Rectangle((int)Position.X, (int)Position.Y, (int)hw.X, (int)hw.Y);
                if ( MenuSystem.PointIsCenter )
                {
                    Engine.RectangleExtensions.CenterOn(ref extents,ref absolutionPosition );
                }
            }
        }
        public UITextElement(Point Position, SpriteFont sf, string text)
            : base(Position)
        {
            _sf = sf;
            _text = text;
            _color = Color.White;

            hw = _sf.MeasureString(_text);
            extents = new Rectangle((int)Position.X, (int)Position.Y, (int)hw.X, (int)hw.Y);
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
        }
        public UITextElement(Point Position, SpriteFont sf, string text, Color color)
            : base(Position)
        {
            _sf = sf;
            _text = text;
            _color = color;
            hw = _sf.MeasureString(_text);
            extents = new Rectangle((int)Position.X, (int)Position.Y, (int)hw.X, (int)hw.Y);
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
        }
        public UITextElement( Point Position, SpriteFont sf, string text, Color color, float drawlayer )
            : base( Position )
        {
            _sf = sf;
            _text = text;
            _color = color;
            hw = _sf.MeasureString( _text );
            extents = new Rectangle( ( int ) Position.X, ( int ) Position.Y, ( int ) hw.X, ( int ) hw.Y );
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
            this._DrawLayer = drawlayer;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (base.visible)
            {
                spriteBatch.DrawString(_sf, _text,absolutionPosition.ToVector2(), _color, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, _DrawLayer);
            }
        }
    }
}

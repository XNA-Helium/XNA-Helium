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
    public class UITextButton : UIButton
    {
        public static Point TextOffset = new Point( 20, 20 );
        public static Color color = new Color(59,79,58);

        public string Text
        {
            get
            {
                return textElement.Text;
            }
            set
            {
                textElement.Text = value;
            }
        }

        UITextElement textElement;
        
        public UITextButton(string text, SpriteFont font, Point textOffset, string ImageName, Point Position)
            : base(ImageName, Position)
        {
            _Active.DrawLayer = 0.2f;
            _Inactive.DrawLayer = 0.2f;
            textElement = new UITextElement( new Point( Position.X + textOffset.X, Position.Y + textOffset.Y ), font, text, color, 0.1f );
        }

        public UITextButton( string text,Point position ):base(@"SharedContent\button",position)
        {
            _Active.DrawLayer = 0.2f;
            _Inactive.DrawLayer = 0.2f;
            this._DrawLayer = 0.2f;
            textElement = new UITextElement( new Point( Position.X + TextOffset.X, Position.Y + TextOffset.Y ), Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" ), text, color, 0.1f );
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            if (this.Active)
                textElement.Color = UIButton.ActiveColor;
            else
                textElement.Color = UIButton.InActiveColor;
            textElement.Draw(gameTime, spriteBatch);
        }
    }

}
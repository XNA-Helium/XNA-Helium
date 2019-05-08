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
using Engine;

namespace Engine.UI
{
    public class UIImageElement : BaseUIElement
    {
        protected Texture2D _image;
        public Texture2D Texture
        {
            internal set
            {
                _image = value;
            }
            get
            {
                return _image;
            }
        }
        public float DrawLayer
        {
            get
            {
                return _DrawLayer;
            }
            set
            {
                _DrawLayer = value;
            }
        }

        public UIImageElement( Texture2D image, Point Position, float drawLayer )
            : base(Position,drawLayer)
        {
            _image = image;
            extents = new Rectangle( ( int ) Position.X, ( int ) Position.Y, ( int ) _image.Width, ( int ) _image.Height );
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn(ref extents,ref absolutionPosition);
            }
        }
        public UIImageElement( Texture2D image, Point Position)
            : this(image,Position,0.1f)
        {

        }
        public UIImageElement(String imageName, Point Position)
            : this(imageName,Position, 0.1f)
        {
        }
        public UIImageElement(String imageName, Point Position,float drawLayer)
            : base(Position,drawLayer)
        {
            _image = ContentManager.Instance.GetObject<Texture2D>(imageName);
            extents = new Rectangle((int)Position.X, (int)Position.Y, (int)_image.Width, (int)_image.Height);
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (base.visible)
            {
                spriteBatch.Draw(_image, absolutionPosition.ToVector2(), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, _DrawLayer);
            }
        }
        public Texture2D Image
        {
            get
            {
                return _image;
            }
        }
    }
}

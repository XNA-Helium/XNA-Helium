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
    public class UIButton : BaseUIElement
    {
        public static Color ActiveColor = new Color(179, 243, 176); //@@ put these somewhere else, easier to set them per game
        public static Color InActiveColor = new Color(59, 79, 58); //@@ put these somewhere else, easier to set them per game

        protected UIImageElement _Active;
        protected UIImageElement _Inactive;
        bool IsActive;
        public bool Active
        {
            get
            {
                return IsActive;
            }
            set
            {
                IsActive = value;

                if ( IsActive )
                {
                    extents = new Rectangle( ( int ) Position.X, ( int ) Position.Y, ( int ) _Active.Image.Width, ( int ) _Active.Image.Height );
                }
                else
                {
                    extents = new Rectangle( Position.X, Position.Y, ( int ) _Inactive.Image.Width, ( int ) _Inactive.Image.Height );
                }
                if ( MenuSystem.PointIsCenter )
                {
                    Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
                }
            }
        }

        protected int validStates;
        public int ValidStates
        {
            get
            {
                return validStates;
            }
            set
            {
                validStates = value;
            }
        }
        public UIButton(String ImageName, Vector2 Position)
            : this(ImageName, CameraManager.GetPosition(Position))
        {

        }
        public UIButton(String ActiveImage, String InactiveImage, Vector2 Position)
            : this(ActiveImage, InactiveImage, CameraManager.GetPosition(Position))
        {

        }

        public UIButton(String ImageName, Point Position)
            : base(Position)
        {

            validStates = (int)TouchStates.ANY;

            _Active = new UIImageElement(ImageName + "_active", Position);
            _Inactive = new UIImageElement(ImageName + "_inactive", Position);
            IsActive = false;
            extents = new Rectangle((int)Position.X, (int)Position.Y, (int)_Inactive.Image.Width, (int)_Inactive.Image.Height);
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
        }
        public UIButton(String ActiveImage, String InactiveImage, Point Position)
            : base(Position)
        {

            validStates = (int)TouchStates.ANY;

            _Active = new UIImageElement(ActiveImage, Position);
            _Inactive = new UIImageElement(InactiveImage, Position);
            IsActive = false;
            extents = new Rectangle((int)Position.X, (int)Position.Y, (int)_Inactive.Image.Width, (int)_Inactive.Image.Height);
            if ( MenuSystem.PointIsCenter )
            {
                Engine.RectangleExtensions.CenterOn( ref extents, ref absolutionPosition );
            }
        }

        public virtual void SwapTextures( String Active, String InActive )
        {
            _Active.Texture = Engine.ContentManager.Instance.GetObject<Texture2D>( Active );
            _Inactive.Texture = Engine.ContentManager.Instance.GetObject<Texture2D>( InActive );
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if ( base.enabled && base.visible )
            {
                if (IsActive)
                {
                    _Active.Draw(gameTime, spriteBatch);
                }
                else
                {
                    _Inactive.Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}

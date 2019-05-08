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
using Engine.UI;
namespace Engine.UI
{
    public class LoadingMenu : UIBaseMenu
    {
        UITextElement textele;
        Texture2D Image;
        Vector2 ImgPos;
        Vector2 ImageCenter;
        float rotation;
        int Total = Engine.ContentManager.Instance.LoadCount(0);
        char[] array = {'L','o','a','d','i','n','g','.','.','.', ' ', ' ', ' '};
        public LoadingMenu(string CenterImage, string background, Point position )
            : this( background, position )
        {
            Image = Engine.ContentManager.Instance.GetObject<Texture2D>(CenterImage);
            ImgPos = CameraManager.GetPosition(0.5f, 0.5f).ToVector2();
            ImageCenter = Image.Bounds.Center.ToVector2();
        }
        public LoadingMenu(string background, Point position)
            : base(background, position)
        {
            textele = new UITextElement(CameraManager.GetPosition(0.2f, 0.35f), Engine.ContentManager.Instance.GetObject<SpriteFont>(@"SharedContent\TitleFont"), "");
            AddUIElement(textele);
        }
        public LoadingMenu(string CenterImage)
            : this()
        {
            Image = Engine.ContentManager.Instance.GetObject<Texture2D>(CenterImage);
            ImgPos = CameraManager.GetPosition(0.5f, 0.575f).ToVector2();
            ImageCenter = Image.Bounds.Center.ToVector2();
        }
        public LoadingMenu( )
            : this( @"SharedContent\Background", Point.Zero )
        {

        }

        public override void Update( GameTime gameTime )
        {
            int ItemsLeft = Engine.ContentManager.Instance.LoadCount( 1 );
            if ( ItemsLeft == 0 )
            {
                MenuSystemInstance.PopTopMenu();
            }
            string str = "";
            int NumChar = (int)((float) (Total - ItemsLeft) / (float)Total * (float)array.Length);
            for ( int i = 0; i < NumChar; ++i )
            {
                str += array[i];
            }
            textele.Text = str;
            base.Update( gameTime );
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            rotation += MathHelper.PiOver4 / 8;
            base.Draw(gameTime, spriteBatch);
            if (Image != null)
                spriteBatch.Draw(Image, ImgPos, null, Color.White, rotation, ImageCenter, 4.0f, SpriteEffects.None, 0.0f);
        }

        public override void Back()
        {
            //Back on this screen ends the game.
            EngineGame.EndGame();
        }
    }
}

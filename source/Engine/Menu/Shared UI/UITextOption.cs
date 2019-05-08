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
using Engine.Core;

namespace Engine.UI
{
    public class UITextOption : UITextButton
    {
        protected CircularObjectList<String> ItemList;
        protected String PrimaryString;
        public String Current
        {
            get
            {
                return ItemList.Current;
            }
        }

        public UITextOption(String Primary_text,CircularObjectList<String> list, Point position):base(Primary_text + list.Current,position)
        {
            ItemList = list;
            PrimaryString = Primary_text;
        }

        public void Next( )
        {
            ItemList.Next();
            Text = PrimaryString + ItemList.Current;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw( gameTime, spriteBatch );
        }
    }
}
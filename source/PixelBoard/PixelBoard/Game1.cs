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

namespace PixelBoard
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : EngineGame
    {
        public Game1()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
        }

        protected override void LoadGameContent()
        {
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\BoardTiles");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Pawn");
            Engine.ContentManager.Instance.Load<SpriteFont>(@"Content\Arial");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\BoardGameBackground");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\BoardGameFloor");
        }

        protected override Engine.UI.BaseMenu FirstMenu()
        {
            SwitchToResolution(DisplayOrientation.Portrait, 800, 600, false);
            return new MainMenu();
        }
    }
}

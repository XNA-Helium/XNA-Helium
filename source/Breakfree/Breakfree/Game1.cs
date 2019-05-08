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


namespace Breakfree
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : EngineGame
    {
        public Game1()
            : base()
        {
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            base.LoadContent();
        }

        protected override void LoadGameContent()
        {
            DebugHelper.MinDebugLevel = DebugHelper.DebugLevels.IgnoreAll;


            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\MainMenuBackground");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Start_active");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Start_inactive");

            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Ball");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\gamebackground");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\gamemenu");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Paddle");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\purpleBrick");

            Engine.ContentManager.Instance.Load<SpriteFont>(@"Content\Arial");
        }

        protected override Engine.UI.BaseMenu FirstMenu()
        {
            SwitchToResolution(DisplayOrientation.Portrait, 320, 480, false);
            return new MainMenu();
        }
    }
}

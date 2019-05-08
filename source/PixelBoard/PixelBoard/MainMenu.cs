using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelBoard
{
    class MainMenu : UIBaseMenu
    {
        public MainMenu()
        {
            UITextButton btn = new UITextButton("START", CameraManager.GetPosition(new Vector2(0.40f, 0.5f)));
            Add(Start, btn);
        }

        public void Start()
        {
            GameSetupState gss = new GameSetupState();
            Engine.GameState.GameStateSystem.Instance.AddGameState(gss);
            gss.Initalize();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkBlue);
            base.Draw(gameTime, spriteBatch);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.Core;
using Engine.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PixelBoard
{
    class TestMiniGame : MiniGameState
    {
        double Timer = 0;

        public override void StartGame()
        {
            base.StartGame();
        }

        public override void Update(GameTime gameTime)
        {
            Timer += gameTime.ElapsedGameTime.Milliseconds;
            if (Timer >= 1000)
                GameOver();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.Green);
            base.Draw(gameTime, spriteBatch);
        }
    }
}

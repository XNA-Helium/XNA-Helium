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
using Engine.Core;

namespace Breakfree
{
    class GameMenu : UIBaseMenu
    {
        UITextElement Score;
        public GameMenu()
            : base(@"Content\gamemenu", new Point(0, 0))
        {
            Score = new UITextElement(new Point(20, 15), Engine.ContentManager.Instance.GetObject<SpriteFont>(@"Content\Arial"), "0");
            AddUIElement(Score);
        }
  
        public void SetScore(int score)
        {
            Score.Text = score.ToString();
        }

        public override void ProcessInput(GameTime gameTime, ref Engine.TouchCollection touches)
        {
            if (touches.Count > 0)
            {
                PlayingState state = Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState;
                Paddle Paddle = state.Paddle;

                Paddle.ProcessInput(ref touches);
            }

            base.ProcessInput(gameTime, ref touches);
        }
    }
}

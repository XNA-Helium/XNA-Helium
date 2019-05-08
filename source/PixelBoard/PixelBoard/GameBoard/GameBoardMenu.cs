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
    class GameBoardMenu : UIBaseMenu
    {
        UITextElement CurrentPlayer, Rolled;

        public GameBoardMenu()
            : base(@"Content\BoardGameBackground", Point.Zero)
        {
            UITextButton rollDice = new UITextButton("Roll", Point.Zero);
            rollDice.ValidStates = (int)TouchStates.Released;
            Add(RollDice, rollDice);

            CurrentPlayer = new UITextElement(CameraManager.GetPosition(0.05f, 0.9f), 
                Engine.ContentManager.Instance.GetObject<SpriteFont>(@"Content\Arial"), "Player 1");
            AddUIElement(CurrentPlayer);

            Rolled = new UITextElement(CameraManager.GetPosition(0.8f, 0.9f),
                Engine.ContentManager.Instance.GetObject<SpriteFont>(@"Content\Arial"), "");
            AddUIElement(Rolled);
        }

        public string RolledAmount
        {
            set 
            {
                if (Rolled.Text != value)
                    Rolled.Scale = 2.0f;
                Rolled.Text = value; 
            } 
        }

        public string SetCurrentPlayer
        {
            set { CurrentPlayer.Text = value; }
        }

        public override void Update(GameTime gameTime)
        {
            if (Rolled.Scale != 1.0f)
            {
                Rolled.Scale -= 0.1f;
                if (Rolled.Scale < 1.0f)
                    Rolled.Scale = 1.0f;
            } 

            base.Update(gameTime);
        }

        public void RollDice()
        {
            GameBoardState gbs = Engine.GameState.GameStateSystem.Instance.GetCurrentState as GameBoardState;
            if (gbs != null)
                gbs.RollDice();
        }
    }
}

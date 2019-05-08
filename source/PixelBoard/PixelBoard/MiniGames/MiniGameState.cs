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
    class GameResult
    {
        public GameResult(int winner) { Winner = winner; }
        int Winner;
    }

    class MiniGameState : PrimaryGameState
    {
        public delegate void MiniGameOverCallback(GameResult result);
        public MiniGameOverCallback GameOverCallback;
        public MiniGameState()
        {
            objectList = new List<BaseObject>();
        }

        public virtual void StartGame()
        {
        }


        public virtual void GameResults()
        {
        }

        public virtual void GameOver()
        {
            GameStateSystem.Instance.RemoveGameStateLater(this);
            if (GameOverCallback != null)
                GameOverCallback(new GameResult(0));
        }
    }
}

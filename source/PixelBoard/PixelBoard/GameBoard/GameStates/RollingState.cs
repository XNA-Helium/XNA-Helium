using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.GameState;

using Microsoft.Xna.Framework;

namespace PixelBoard
{
    class RollingState : WatchableState 
    {
        float RollTicker, RollStudder = 0;
        Random rand = new Random();
        GameBoardMenu Menu;
        Player CurrentPlayer;
        GameBoard GameBoard;

        public RollingState(GameBoardMenu menu, Player currentPlayer, GameBoard gameBoard)
        {
            Menu = menu;
            CurrentPlayer = currentPlayer;
            GameBoard = gameBoard;
        }

        public override void Update(GameTime gameTime)
        {
            RollTicker += gameTime.ElapsedGameTime.Milliseconds;

            int num = 0;
            if (RollTicker >= 300)
            {
                RollTicker = 0;
                num = rand.Next(1, 6);

                RollStudder++;
                Menu.RolledAmount = num.ToString();
            }

            if (RollStudder >= 10)
            {
                SetPlayersNextMoves(num);
                RollStudder = 0;
                RemoveSelf();
                Paused = true;
            }

            base.Update(gameTime);
        }

        public void SetPlayersNextMoves(int numberOfMoves)
        {
            Vector2[] Position = new Vector2[numberOfMoves];

            for (int i = 0; i < numberOfMoves; i++)
                Position[i] = GameBoard.GetSpaceByIndex(CurrentPlayer.Location + i + 1, Vector2.Zero);

            CurrentPlayer.MoveForward(Position, numberOfMoves);
        }
    }
}

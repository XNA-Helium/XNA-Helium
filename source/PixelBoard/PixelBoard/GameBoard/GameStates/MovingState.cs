using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.GameState;

using Microsoft.Xna.Framework;

namespace PixelBoard
{
    class MovingState : WatchableState
    {
        Player CurrentPlayer;
        GameBoardState GBS;

        public MovingState(GameBoardState gbs, Player currentPlayer)
        {
            GBS = gbs;
            CurrentPlayer = currentPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentPlayer.UpdateMovement(gameTime);
            if (CurrentPlayer.Moving)
                GBS.SetCameraOffset(CurrentPlayer.Position);

            base.Update(gameTime);
        }

        public void PlayerArrived()
        {
            GBS.EndTurn();
            RemoveSelf();
        }
    }
}

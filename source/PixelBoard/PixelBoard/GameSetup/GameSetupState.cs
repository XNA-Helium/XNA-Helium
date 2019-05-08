using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.Core;
using Engine.GameState;
using Microsoft.Xna.Framework;

namespace PixelBoard
{
    /// <summary>
    /// This is the state inwhich the amount of players is picked, 
    /// the colors of there pieces and board is picked before the 
    /// actual game starts. This is an actual game state because
    /// the players actually see there pieces rendered to the 
    /// screen and are able to edit there color/name and player 
    /// count in real-time rather then from just an interface.
    /// </summary>
    public class GameSetupState : PrimaryGameState
    {
        Player[] Players = new Player[10];
        Player[] LeavingPlayers = new Player[10];
        int PlayerCount, LeavingCount = 0;

        public Player[] GetPlayers
        {
            get { return Players.Clone() as Player[]; }
        }

        public GameSetupState()
        {
            objectList = new List<BaseObject>();
            MenuSystem.Instance.PopTopMenu();
            MenuSystem.Instance.PushMenu(new GameSetupMenu());
        }

        public void Initalize()
        {
            Point Home = CameraManager.GetPosition(new Vector2(0.5f, 0.5f));
            CameraManager.Instance.Current.CenterOn(ref Home);
        }

        public void PushPlayer()
        {
            if (PlayerCount == Players.Length) return;

            Players[PlayerCount] = new Player(Microsoft.Xna.Framework.Color.Red);
            Players[PlayerCount].Position = CameraManager.GetPosition(new Vector2(0.5f, 0.1f)).ToVector2();
            PlayerCount++;

            UpdatePlayerAlignment();
        }

        public void PopPlayer()
        {
            if (PlayerCount == 0) return;

            PlayerCount--;

            LeavingPlayers[LeavingCount] = Players[PlayerCount];
            LeavingPlayers[LeavingCount].MovesComplete = PlayerLeft;

            Players[PlayerCount] = null;

            Vector2[] Position = new Vector2[1];
            Position[0] = CameraManager.GetPosition(new Vector2(0.8f, 1.2f)).ToVector2();
            LeavingPlayers[LeavingCount].MoveForward(Position, 1);
            LeavingCount++;

            UpdatePlayerAlignment();
        }

        public void PlayerLeft()
        {
            if (LeavingCount == 0) return;

            LeavingCount--;
            LeavingPlayers[LeavingCount].RemoveMeInTime();
            LeavingPlayers[LeavingCount] = null;

            UpdatePlayerAlignment();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Player player in Players)
            {
                if (player == null) break;
                player.UpdateMovement(gameTime);
            }

            foreach (Player player in LeavingPlayers)
            {
                if (player == null) break;
                player.UpdateMovement(gameTime);
            }

            base.Update(gameTime);
        }

        public void UpdatePlayerAlignment()
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                float xOffset = (1.0f / (PlayerCount + 1)) * (i + 1);
                Vector2[] Position = new Vector2[1];
                Position[0] = CameraManager.GetPosition(new Vector2(xOffset, 0.5f)).ToVector2();
                Players[i].MoveForward(Position, 1);
            }
        }
    }
}

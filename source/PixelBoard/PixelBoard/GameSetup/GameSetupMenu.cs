using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.UI;

using Microsoft.Xna.Framework;

namespace PixelBoard
{
    class GameSetupMenu : UIBaseMenu
    {
        public GameSetupMenu()
        {
            UITextButton addPlayer = new UITextButton("Add Player", CameraManager.GetPosition(new Vector2(0.10f, 0.1f)));
            addPlayer.ValidStates = (int)TouchStates.Released;
            Add(PushPlayer, addPlayer);

            UITextButton removePlayer = new UITextButton("Remove Player", CameraManager.GetPosition(new Vector2(0.10f, 0.2f)));
            removePlayer.ValidStates = (int)TouchStates.Released;
            Add(PopPlayer, removePlayer);

            UITextButton startGame = new UITextButton("Start Game", CameraManager.GetPosition(new Vector2(0.10f, 0.8f)));
            removePlayer.ValidStates = (int)TouchStates.Released;
            Add(StartGame, startGame);
        }

        public override void ProcessInput(GameTime gameTime, ref TouchCollection touches)
        {
            base.ProcessInput(gameTime, ref touches);

            GameSetupState gss = Engine.GameState.GameStateSystem.Instance.GetCurrentState as GameSetupState;

            if (touches.Count > 0 && touches[0].State == TouchStates.Released)
            {
                foreach (Player player in gss.GetPlayers)
                {
                    if (player == null) return;

                    Point Touched = touches[0].Position;
                    if (player.Rectangle.Contains(Touched))
                    {
                        player.ColorChange();
                    }
                }
            }
        }

        public void StartGame()
        {
            GameSetupState gss = Engine.GameState.GameStateSystem.Instance.GetCurrentState as GameSetupState;
            if (gss.GetPlayers[0] == null) return;


            Engine.GameState.GameStateSystem.Instance.RemoveGameState(gss);
            MenuSystem.Instance.PopTopMenu();

            GameBoardState gbs = new GameBoardState();
            Engine.GameState.GameStateSystem.Instance.AddGameState(gbs);

            gbs.Setup(gss.GetPlayers);
        }

        public void PushPlayer()
        {
            GameSetupState gss = Engine.GameState.GameStateSystem.Instance.GetCurrentState as GameSetupState;
            gss.PushPlayer();
        }

        public void PopPlayer()
        {
            GameSetupState gss = Engine.GameState.GameStateSystem.Instance.GetCurrentState as GameSetupState;
            gss.PopPlayer();
        }
    }
}

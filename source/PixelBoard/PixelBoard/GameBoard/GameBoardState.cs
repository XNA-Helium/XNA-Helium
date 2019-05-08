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
    /// <summary>
    /// This state manages all of the other states that run
    /// to produce all of the board game logic, MovingState
    /// and RollingState. For simplisity this state also
    /// manages the camera movement operations via a few
    /// variables
    /// </summary>
    class GameBoardState : PrimaryGameState
    {
        enum RoundState
        {
            Ready,
            Rolling,
            Moving,
            CameraMoving,
            MiniGame,
            ResolvingGame,
            StateCount
        }

        GameBoard gameBoard;
        Vector2 CameraOffset = Vector2.Zero;
        Player[] Players;
        int PlayerCount, CurrentPlayer, GameRound = 0;
        static Random rand = new Random();
        Player Winner;
        GameBoardMenu Menu;
        RoundState CurrentRoundState = RoundState.Ready;
        public Vector2 CameraMove = Vector2.Zero;
        MiniGameState CurrentMiniGame;
        Texture2D Background;

        public GameBoardState()
        {
            gameBoard = new GameBoard();
            objectList = new List<BaseObject>();

            Menu = new GameBoardMenu();
            MenuSystem.Instance.AddMenu(Menu);

            Background = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\BoardGameFloor"); ;
        }

        public void Setup(Player[] players)
        {
            Players = new Player[players.Length];
            Vector2 Start = gameBoard.GetSpaceByIndex(0, Vector2.Zero);
            foreach (Player player in players)
            {
                if (player == null) break;
                Player clone = Player.ClonePlayer(player, gameBoard.Spaces);
                clone.Position = Start;

                Players[PlayerCount] = clone;
                PlayerCount++;
            }


            CameraOffset = CameraManager.GetPosition(0.5f, 0.5f).ToVector2() - Start;
            Point Home = (CameraManager.GetPosition(0.5f, 0.5f).ToVector2() - CameraOffset).ToPoint();
            CameraManager.Instance.Current.CenterOn(ref Home);
        }

        public void RoundEnd()
        {
            CurrentRoundState = RoundState.MiniGame;

            CurrentMiniGame = new TestMiniGame();
            CurrentMiniGame.GameOverCallback = MiniGameOver;
            GameStateSystem.Instance.AddGameStateLater(CurrentMiniGame);
            CurrentMiniGame.StartGame();
        }

        public void MiniGameOver(GameResult result)
        {
            CurrentRoundState = RoundState.CameraMoving;
        }

        public void RollDice()
        {
            if (CurrentRoundState != RoundState.Ready)
                return;
            CurrentRoundState = RoundState.Rolling;
        }

        public void ProcessStateChange(WatchableState state)
        {
            int Round = (int)CurrentRoundState + 1;
            if (Round > (int)RoundState.StateCount)
                Round = 0;

            CurrentRoundState = (RoundState)Round;
            Paused = false;
        }

        public void SetWinner(Player winner)
        {
            Winner = winner;
        }

        public void SetCameraOffset(Vector2 offset)
        {
            CameraOffset = CameraManager.GetPosition(0.5f, 0.5f).ToVector2() - offset;
            Point Home = (CameraManager.GetPosition(0.5f, 0.5f).ToVector2() - CameraOffset).ToPoint();
            CameraManager.Instance.Current.CenterOn(ref Home);
        }

        /// <summary>
        /// Ends the turn of the current player
        /// Ends the round if there are no more players
        /// </summary>
        public void EndTurn()
        {
            Menu.RolledAmount = "";
            CameraMove = Players[CurrentPlayer].Position;

            if (Players[CurrentPlayer].Location == gameBoard.Spaces - 1)
            {
                SetWinner(Players[CurrentPlayer]);
                return;
            }

            CurrentPlayer++;
            if (CurrentPlayer == PlayerCount)
            {
                GameRound++;
                CurrentPlayer = 0;
                RoundEnd();
            }
            Menu.SetCurrentPlayer = "Player " + (CurrentPlayer + 1);
        }

        public override void Update(GameTime gameTime)
        {

            switch (CurrentRoundState)
            {
                case RoundState.Ready:
                    break;
                case RoundState.Moving:
                    Paused = true;

                    MovingState MovingState = new MovingState(this, Players[CurrentPlayer]);
                    MovingState.GameStateEnded = ProcessStateChange;

                    foreach (Player player in Players)
                    {
                        if (player == null) break;
                        player.MovesComplete = MovingState.PlayerArrived;
                    }

                    GameStateSystem.Instance.AddGameStateLater(MovingState);
                    break;
                case RoundState.CameraMoving:
                    if (Players[CurrentPlayer].Position == CameraMove)
                    {
                        CurrentRoundState = RoundState.Ready;
                        return;
                    }

                    Vector2 interp = Vector2.Normalize(Players[CurrentPlayer].Position - CameraMove);
                    CameraMove += interp*10;

                    if ((CameraMove - Players[CurrentPlayer].Position).Length() <= 10)
                    {
                        CurrentRoundState = RoundState.Ready;
                        CameraMove = Players[CurrentPlayer].Position;
                    }

                    SetCameraOffset(CameraMove);
                    break;
                case RoundState.MiniGame:
                    break;
                case RoundState.ResolvingGame:
                    break;
                case RoundState.Rolling:
                    Paused = true;

                    RollingState RollingState = new RollingState(Menu, Players[CurrentPlayer], gameBoard);
                    RollingState.GameStateEnded = ProcessStateChange;

                    GameStateSystem.Instance.AddGameStateLater(RollingState);
                    return;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Player player = Players[CurrentPlayer];

            spriteBatch.Begin();
            spriteBatch.Draw(Background, Vector2.Zero, player.Color);
            gameBoard.Draw(spriteBatch, CameraOffset);
            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}

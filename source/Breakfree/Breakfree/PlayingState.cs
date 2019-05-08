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
using Engine.Core;
using Engine.GameState;

namespace Breakfree
{
    class PlayingState : PrimaryGameState
    {
        public Ball Ball;
        public Paddle Paddle;
        Texture2D GameBackground;
        public PlayingState()
            : base()
        {
            GameBackground = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\gamebackground");
        }

        private int score = 0;
        public int Score
        {
            set
            {
                GameMenu gameMenu = Engine.MenuSystem.Instance.CurrentMenu as GameMenu;
                gameMenu.SetScore(value);

                score = value;

                // if the score is mod 6, then all of them have been destroyed, reset the game.
                if (score % 6 == 0)
                    PopulateBricks();
            }
            get
            {
                return score;
            }
        }

        public void Setup()
        {
            PongPhysics.SetupPhysics2DManager(new Rectangle(0, 0, 320, 480));

            objectList = new List<BaseObject>(128);
            Paddle = new Paddle(new Vector3(160, 0, 450));

            Ball = new Ball();
            ResetBall(Ball);
            PopulateBricks();


            Point Home = new Point(320/2, 480/2);
            CameraManager.Instance.Current.CenterOn(ref Home);
        }

        public void PopulateBricks()
        {
            ResetBall(Ball);
            CreateBrick(new Vector2(320 / 6 * 2, 100));
            CreateBrick(new Vector2(320 / 6 * 4, 100));

            CreateBrick(new Vector2(320 / 6 * 2, 140));
            CreateBrick(new Vector2(320 / 6 * 4, 140));

            CreateBrick(new Vector2(320 / 6 * 2, 180));
            CreateBrick(new Vector2(320 / 6 * 4, 180));
        }

        public void CreateBrick(Vector2 Position)
        {
            Brick brick = new Brick(Position.ToVector3());
        }

        public void ResetBall(Ball ball)
        {
            (ball[Physics2D.TypeStatic] as Physics2D).Velocity = new Vector3(1, 0, -8.0f);
            (ball[Placeable.TypeStatic] as Placeable).Set2DPosition(new Vector2(320/2, 400));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            PongPhysics.Instance.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(GameBackground, Vector2.Zero, Color.White);
            spriteBatch.End();

            base.Draw(gameTime, spriteBatch);
        }
    }
}

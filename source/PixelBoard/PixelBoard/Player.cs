using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelBoard
{
    public class Player : BaseObjectStreamingHelper<Player>
    {
        protected Placeable placeable;
        protected Drawable2D drawable;
        protected int BoardLocation = 0;
        protected int colorIndex = 0;
        protected bool MovingToNext = false;
        protected Vector2[] NextPosition;
        protected int NumMoves = 0;
        protected int MovesMade = 0;
        protected int BoardSize = 0;

        int moveWave = 0;

        public Callback MovesComplete;

        public bool Moving
        {
            get { return MovingToNext; }
        }

        public int Location
        {
            get { return BoardLocation; }
        }

        public Color Color
        {
            get { return drawable.Color; }
        }

        static public Player ClonePlayer(Player player, int boardSize)
        {
            Player clone = new Player(player.drawable.Color);
            clone.colorIndex = player.colorIndex;
            clone.BoardSize = boardSize;
            return clone;
        }

        public Player(Color color)
        {
            placeable = new Placeable(true, -1);
            AddComponent(placeable);

            Texture2D pawn = ContentManager.Instance.GetObject<Texture2D>(@"Content\Pawn");
            drawable = new Drawable2D(true, -1);
            AddComponent(drawable);

            drawable.SetupAnimation(pawn, "Default");
            drawable.PlayAnimation("Default");
            drawable.Color = color;
        }

        public void UpdateMovement(GameTime gametime)
        {
            if (MovingToNext)
            {
                moveWave++;

                if (NextPosition[MovesMade] != Position)
                {
                    Vector2 vec = Vector2.Normalize(NextPosition[MovesMade] - Position);
                    vec.Y -= (float)Math.Cos((float)moveWave / 10);
                    Position += vec * 4;
                }

                if ((Position - NextPosition[MovesMade]).Length() <= 4)
                {
                
                    moveWave = 0;
                    MovesMade++;
                    if (MovesMade == NumMoves)
                    {
                        BoardLocation += NumMoves;
                        NumMoves = 0;
                        MovesMade = 0;
                        MovingToNext = false;

                        if (MovesComplete != null)
                            MovesComplete();
                    }
                }
            }

        }

        public void ColorChange()
        {
            switch (colorIndex)
            {
                case 0:
                    drawable.Color = Color.Teal;
                    break;
                case 1:
                    drawable.Color = Color.Green;
                    break;
                case 2:
                    drawable.Color = Color.Purple;
                    break;
                case 3:
                    drawable.Color = Color.Orange;
                    break;
                case 4:
                    drawable.Color = Color.Blue;
                    break;
                case 5:
                    drawable.Color = Color.Pink;
                    break;
                case 6:
                    drawable.Color = Color.SeaGreen;
                    break;
                case 7:
                    drawable.Color = Color.Silver;
                    break;
                case 8:
                    drawable.Color = Color.Snow;
                    break;
                case 9:
                    drawable.Color = Color.SpringGreen;
                    break;
                case 10:
                    drawable.Color = Color.SteelBlue;
                    break;
                case 11:
                    drawable.Color = Color.Red;
                    break;
            }

            colorIndex++;
            if (colorIndex > 11)
                colorIndex = 0;
        }

        public Vector2 Position
        {
            get { return placeable.Get2DPosition(); }
            set { placeable.Set2DPosition(value.ToPoint()); }
        }

        public Rectangle Rectangle
        {
            get { return drawable.Rectangle; }
        }

        public void MoveForward(Vector2[] position, int amount)
        {
            if (BoardSize != 0 && amount + BoardLocation > BoardSize - 1)
                amount = BoardSize - 1;

            MovingToNext = true;
            NextPosition = position;
            NumMoves = amount;
            moveWave = 0;
        }

        public void SetLocation(int loc)
        {
            BoardLocation = loc;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelBoard
{
    public enum SpaceRule
    {
        Start, Good, Bad, Left, Right, Win
    }

    public class GameBoard
    {
        Texture2D BoardTilesText;
        int TileSize = 64;

        int BoardSize = 29;
        SpaceRule[] BoardSpaces;

        public int Spaces
        {
            get { return BoardSize; }
        }   

        public GameBoard()
        {
            BoardTilesText = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\BoardTiles");
            BoardSpaces = new SpaceRule[BoardSize];
            GenerateMap();
        }

        public void DrawTile(SpriteBatch spriteBatch, SpaceRule rule, Vector2 Position, float Rotation)
        {
            int index = (int)rule;
            Rectangle rect = new Rectangle(index * TileSize, 0, TileSize, TileSize);

            spriteBatch.Draw(BoardTilesText, Position, rect, Color.White, Rotation, new Vector2(TileSize*0.5f, TileSize*0.5f), 1.0f, SpriteEffects.None, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            float Spin = 0;
            Vector2 Position = offset + new Vector2(TileSize*0.5f);
            for (int i = 0; i < BoardSpaces.Length; i++)
            {
                if (BoardSpaces[i] == SpaceRule.Left)
                    Spin -= (float)Math.PI * 0.5f;
                if (BoardSpaces[i] == SpaceRule.Right)
                    Spin += (float)Math.PI * 0.5f;

                DrawTile(spriteBatch, BoardSpaces[i], Position, Spin);

                Position += new Vector2((float)Math.Cos(Spin), (float)Math.Sin(Spin)) * TileSize;
            }
        }

        public Vector2 GetSpaceByIndex(int index, Vector2 offset)
        {
            float Spin = 0;
            Vector2 Position = offset + new Vector2(TileSize * 0.5f);
            for (int i = 0; i < BoardSpaces.Length; i++)
            {
                if (BoardSpaces[i] == SpaceRule.Left)
                    Spin -= (float)Math.PI * 0.5f;
                if (BoardSpaces[i] == SpaceRule.Right)
                    Spin += (float)Math.PI * 0.5f;

                if (i == index)
                    return Position;

                Position += new Vector2((float)Math.Cos(Spin), (float)Math.Sin(Spin)) * TileSize;
            }

            return Vector2.Zero;
        }

        public void GenerateMap()
        {
            int Count = 0;
            BoardSpaces[Count++] = SpaceRule.Start;

            BoardSpaces[Count++] = SpaceRule.Bad;
            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;

            BoardSpaces[Count++] = SpaceRule.Right;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;
            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;

            BoardSpaces[Count++] = SpaceRule.Right;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;

            BoardSpaces[Count++] = SpaceRule.Right;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;

            BoardSpaces[Count++] = SpaceRule.Left;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;
            BoardSpaces[Count++] = SpaceRule.Good;

            BoardSpaces[Count++] = SpaceRule.Left;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;
            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;

            BoardSpaces[Count++] = SpaceRule.Left;

            BoardSpaces[Count++] = SpaceRule.Good;
            BoardSpaces[Count++] = SpaceRule.Bad;
            BoardSpaces[Count++] = SpaceRule.Good;

            BoardSpaces[Count++] = SpaceRule.Win;
        }
    }
}

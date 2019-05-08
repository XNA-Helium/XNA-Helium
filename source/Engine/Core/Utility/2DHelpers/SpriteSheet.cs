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

namespace Engine.Core
{

    public class SpriteSheet
    {
        internal Texture2D spriteSheet;
        internal Rectangle boxSize;
        internal Rectangle currentSquare;
        internal String sheetName;
        internal int boxesWide;
        internal int boxesTall;
        internal int currentIndex;
        internal int maxIndex;
        internal int framesPerSprite; //number of seconds to show each sprite
        internal int counter;

        internal bool OneFrame;
        public int Time
        {
            get
            {
                return framesPerSprite * maxIndex;
            }
        }
        public Texture2D Sheet
        {
            internal set
            {
                spriteSheet = value;
            }
            get
            {
                return spriteSheet;
            }
        }
        public Rectangle BoxSize
        {
            internal set
            {
                boxSize = value;
            }
            get
            {
                return boxSize;
            }
        }

        public bool OnlyOneFrame
        {
            get
            {
                return OneFrame;
            }
        }

        public String name
        {
            get
            {
                return sheetName;
            }
        }
        protected bool IndexChanged;

        public SpriteSheet( Texture2D p_spritesheet, String p_Name)
        {
            framesPerSprite = 1;
            counter = 0;
            spriteSheet = p_spritesheet;
            boxSize = new Rectangle(0,0,p_spritesheet.Width,p_spritesheet.Height);
            currentSquare = boxSize; 
            sheetName = p_Name;
            maxIndex = 0;
            boxesWide = 1;
            boxesTall = 1;
            maxIndex = 1;
            currentIndex = 0;
            OneFrame = true;
        }
        public SpriteSheet( Texture2D p_spritesheet, Rectangle p_BoxSize, String p_Name, int fps )
        {
            framesPerSprite = fps;
            counter = 0;
            spriteSheet = p_spritesheet;
            boxSize = p_BoxSize;
            sheetName = p_Name;
            maxIndex = 0;
            boxesWide = p_spritesheet.Width / p_BoxSize.Width;
            boxesTall = p_spritesheet.Height / p_BoxSize.Height;
            maxIndex = boxesWide * boxesTall;
            currentIndex = 0;
            OneFrame = ( boxesWide == 1 && boxesTall == 1 );
            currentSquare = boxSize; 
        }

        public void Reset(bool randomize)
        {
            if ( randomize )
            {
                System.Random rand = new Random();
                currentIndex = rand.Next( maxIndex );
            }
            counter = 0;
        }
        public void Reset( )
        {
            currentIndex = 0;
            counter = 0;
        }

        public bool AtLastFrame( )
        {
            return ( currentIndex == maxIndex - 1 );
        }

        public void Update( GameTime p_time )
        {
            if ( OneFrame )
                return;

            int x = ( currentIndex / boxesTall ) * boxSize.Width;
            int y = ( currentIndex % boxesTall ) * boxSize.Height;
            currentSquare = new Rectangle( x, y, boxSize.Width, boxSize.Height );
            if ( counter++ == framesPerSprite )
            {
                counter = 0;
                IndexChanged = true;
                if ( ++currentIndex >= maxIndex )
                {
                    currentIndex = 0;
                }
            }
        }

        public bool FrameChanged
        {
            get
            {
                return IndexChanged;
            }
        }
        public int Frame
        {
            internal set
            {
               currentIndex = value;
            }
            get
            {
                return currentIndex;
            }
        }

        Vector2 target;
        Vector2 origin;
        public void Draw( SpriteBatch spriteBatch, GameTime p_GameTime,ref Rectangle p_Screen, Color DrawColor, float Scale, float DrawLayer,ref Rectangle Position, float Rotation )
        {
            target = new Vector2( ( int ) Position.Center.X - p_Screen.X, ( int ) Position.Center.Y - p_Screen.Y );
            origin = new Vector2( ( int ) boxSize.Width / 2, ( int ) boxSize.Height / 2 );
            spriteBatch.Draw( spriteSheet, target, currentSquare, DrawColor, Rotation, origin , Scale, SpriteEffects.None, DrawLayer );
        }
        public void Draw( SpriteBatch spriteBatch, GameTime p_GameTime, ref Rectangle p_Screen, Color DrawColor, float Scale, float DrawLayer, ref Rectangle Position, float Rotation, Vector2 origin )
        {
            target = new Vector2( ( int ) Position.Center.X - p_Screen.X, ( int ) Position.Center.Y - p_Screen.Y );
            spriteBatch.Draw( spriteSheet, target, currentSquare, DrawColor, Rotation, origin, Scale, SpriteEffects.None, DrawLayer );
        }
    }


}

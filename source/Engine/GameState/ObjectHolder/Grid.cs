using System;
using System.Collections;
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

// http://support.microsoft.com/kb/307484

namespace Engine.GameState
{
    public class Grid<T> : List<T>
    {
        int height, width;
        T[,] ObjectArray;
        internal Grid():this(128,128)
        {

        }

        public Grid(int Height,int Width):base(Height *  Width)
        {
            height = Height;
            width = Width;
            ObjectArray = new T[Height, Width];
        }
    
        public new void Add( T item ) 
        {
            base.Add(item);
        }

        public new void Clear()
        {
            base.Clear();
            ObjectArray = new T[height, width];
        }

        public new bool Remove(T item) 
        {
            return base.Remove(item); 
        }

        public T this[int x, int y]
        {
            get
            {
                return ObjectArray[x, y];
            }
        }

    }
}

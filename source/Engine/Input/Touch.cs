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


namespace Engine
{
    public class Touch
    {
        protected TouchLocation previousTouchLocation;
        protected bool ignore;
        public bool Ignore
        {
            get { return ignore; }
            internal set { ignore = value; }
        }

        protected Point point;
        public Point Position
        {
            get{ return point;}
            internal set{ point = value; }
        }

        protected int id;
        public int Id 
        {
            get{return id;} 
            internal set {id = value;} 
        }
        protected bool interactedWith;
        public bool InteractedWith
        {
            get
            {
                return interactedWith;
            }
            set
            {
                interactedWith = value;
            }
        }
        
        /*
        protected float pressure;
        public float Pressure
        {
            get { return pressure; }
            internal set { pressure = value; }
        }
        */ 

        public TouchStates state;
        public TouchStates State 
        {
            get{return state;}
            internal set
            {
                state = value;
            }
        }

        protected Touch touch;
        public Touch PreviousTouch
        {
            get
            {
                return touch;
            }
            internal set
            {
                touch = value;
            }
        }

        public void SetInfo(Microsoft.Xna.Framework.Input.Touch.TouchLocation touch)
        {
            SetInfo(touch, true);
        }
        public void SetInfo(Microsoft.Xna.Framework.Input.Touch.TouchLocation touch, bool recurse)
        {
            this.Position = touch.Position.ToPoint();
            this.Id = touch.Id;
            //this.Pressure = touch.Pressure;
            this.interactedWith = false;
            switch (touch.State)
            {
                case TouchLocationState.Invalid:
                    this.State = TouchStates.Invalid;
                    break;
                case TouchLocationState.Released:
                    this.State = TouchStates.Released;
                    break;
                case TouchLocationState.Pressed:
                    this.State = TouchStates.Pressed;
                    break;
                case TouchLocationState.Moved:
                    this.State = TouchStates.Moved;
                    break;
            }
            if(recurse)
            { 
                if( touch.TryGetPreviousLocation(out previousTouchLocation) )
                {
                    if ( PreviousTouch == null )
                        PreviousTouch = new Touch();

                    PreviousTouch.SetInfo(previousTouchLocation, false);
                }
            }
        }
    }
}

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
    // Add Helper functions
    public enum TouchStates : int { Invalid = 0, Pressed = 1, Moved = 2, Released = 4, ANY = 7 };

    public class InputSystem : IEngineUpdateable, IEngineDebugDrawable
    {
        Microsoft.Xna.Framework.Input.Touch.TouchCollection touchState;
        Engine.Input.ButtonWrapper BackButton;

        protected TouchCollection TouchCollection = new TouchCollection();
        public TouchCollection Touches
        {
            get
            {
                return TouchCollection;
            }
        }
        public virtual Touch this[int Index]
        {
            get
            {
                return TouchCollection[Index];
            }
        }

        protected static InputSystem instance = null;
        public static InputSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputSystem();
                }
                return instance;
            }
        }
        protected InputSystem()
        {
            TouchCount = 0;
            BackButton = new Input.ButtonWrapper(Buttons.Back, Input.PlayerIndex.One);
        }

        int TouchCount;
        public virtual void Update(GameTime gameTime)
        {
            touchState = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();
            // Update the input
            TouchCount = touchState.Count();
            int i = 0;
            for (/* i = 0 */; i < TouchCount; ++i)
            {
                TouchCollection[i].Ignore = false;
                TouchCollection[i].SetInfo(touchState[i] );
            }
            for (/* i */; i < TouchCollection.MaxTouches; ++i)
            {
                TouchCollection[i].Ignore = true;
            }
#if WINDOWS || XBOX
            if ( TouchCount == 0 && Mouse.GetState().LeftButton == ButtonState.Pressed )
            {
                TouchCollection[0].PreviousTouch = null;
                Touch temp = TouchCollection[0];
                TouchCollection[0] = new Touch();
                TouchCollection[0].PreviousTouch = temp;

                TouchCollection[0].Position = new Point( Mouse.GetState().X, Mouse.GetState().Y );
                //TouchCollection[0].Pressure = 1.0f;
                TouchCollection[0].Ignore = false;
                TouchCollection[0].State = TouchStates.Pressed;
                TouchCount = 1;
                if ( TouchCollection[0].PreviousTouch != null && TouchCollection[0].PreviousTouch.state == TouchStates.Pressed || TouchCollection[0].PreviousTouch.state == TouchStates.Moved )
                {
                    TouchCollection[0].state = TouchStates.Moved;
                }
                //@@ come back and add state for moving
            }
            else
            {
                if ( TouchCollection[0].state == TouchStates.Moved || TouchCollection[0].state == TouchStates.Pressed )
                {
                    TouchCollection[0].PreviousTouch = null;
                    TouchCollection[0].PreviousTouch = TouchCollection[0];
                    TouchCollection[0].State = TouchStates.Released;
                    TouchCollection[0].Ignore = false;
                    TouchCount = 1;
                }
                else if(TouchCollection[0].State == TouchStates.Released)
                {
                    TouchCollection[0].PreviousTouch = null;
                    TouchCollection[0].State = TouchStates.Invalid;
                }
                TouchCollection[0].Position = new Point( Mouse.GetState().X, Mouse.GetState().Y );
            }
#endif
            TouchCollection.Count = TouchCount;
            BackButton.Update();
        }


        public virtual void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
#if WINDOWS || XBOX
            if ( TouchCollection[0].Ignore == false )
            {
                spriteBatch.Begin();
                Rectangle r = new Rectangle(TouchCollection[0].Position.X - 32,TouchCollection[0].Position.Y - 32, 64,64);
                spriteBatch.Draw( ContentManager.Instance.GetObject<Texture2D>( @"SharedContent\Pointer" ), r, Color.Red );
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                Rectangle r = new Rectangle( TouchCollection[0].Position.X - 32, TouchCollection[0].Position.Y - 32, 64, 64 );
                spriteBatch.Draw( ContentManager.Instance.GetObject<Texture2D>( @"SharedContent\Pointer" ), r, Color.DarkBlue );
                spriteBatch.End();
            }
#endif
        }
        public virtual bool Back()
        {
            return BackButton.SinglePress;
        }
    }
}

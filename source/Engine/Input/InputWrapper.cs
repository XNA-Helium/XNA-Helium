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


namespace Engine.Input
{

    public abstract class InputWrapper
    {
        protected bool RegisterDown = true;
        protected ButtonState _State;
        public enum ButtonState : byte { Held, Pressed, None };

        public abstract void Update();
        public ButtonState State
        {
            get
            {
                return _State;
            }
        }

        public bool SinglePress
        {
            get
            {
                if (_State == ButtonState.Held && RegisterDown)
                {
                    RegisterDown = false;
                    _State = ButtonState.None;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsRegisterDown
        {
            get
            {
                return RegisterDown;
            }
            set
            {
                RegisterDown = value;
            }
        }
    }


}

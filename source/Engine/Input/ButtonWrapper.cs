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

    public class ButtonWrapper : InputWrapper
    {
        protected Microsoft.Xna.Framework.Input.Buttons T;
        protected PlayerIndex P;

        public ButtonWrapper(Microsoft.Xna.Framework.Input.Buttons button, PlayerIndex Player)
        {
            T = button;
            P = Player;
            _State = ButtonState.None;
            SetStateFromInput();
        }

        protected virtual void SetStateFromInput()
        {
#if WINDOWS_PHONE
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
#else
            switch (T)
            {
                case Buttons.Back:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.A:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.B:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.DPadDown:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.DPadLeft:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.DPadRight:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.DPadUp:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
#if !ZUNE
                case Buttons.LeftShoulder:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.LeftShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.LeftStick:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.LeftStick == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.LeftTrigger:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Triggers.Right == 1.0f)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.RightShoulder:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.RightShoulder == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.RightStick:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.RightStick == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.RightTrigger:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Triggers.Right == 1.0f)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.Start:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.Start == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.X:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
                case Buttons.Y:
                    if (Microsoft.Xna.Framework.Input.GamePad.GetState((Microsoft.Xna.Framework.PlayerIndex)P).Buttons.Y == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                    {
                        if (RegisterDown)
                            _State = ButtonState.Held;
                    }
                    else
                    {
                        if (RegisterDown)
                        {
                            if (_State == ButtonState.Held)
                            {
                                _State = ButtonState.Pressed;
                            }
                            else
                            {
                                _State = ButtonState.None;
                            }
                        }
                        else //RegisterDown == false
                        {
                            RegisterDown = true;
                        }
                    }
                    break;
#endif
            }
#endif
        }

        public override void Update()
        {
            SetStateFromInput();
        }

        public byte GetStateForStreaming()
        {
            return (byte)_State;
        }
    }
}

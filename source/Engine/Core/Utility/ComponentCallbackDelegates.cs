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

namespace Engine.Core
{
    public delegate void OnCollision(BaseObject Collider);
    // Shoudl these be full template classes, with a delegate method, so that 

    public delegate bool SetterValidatorDelegate<T,U>(ref T InValue, out T NewValue, SetterValidatorDelegate<T,U> AdditionalValidator);
    public delegate void SetterCallback<T,U>(ref T InValue, U Component);

    public delegate bool GetterValidator<T,U>(ref T InValue, out T RequiredOffset, GetterValidator<T,U> AdditionalValidator);
    public delegate void GetterCallback<T, U>(ref T InValue, U Component);

}

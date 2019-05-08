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
using Engine.Core;
using Engine.GameState;

namespace Engine.GameState
{
    public class BoardGameState : PrimaryGameState
    {
        public BoardGameState():base()
        {
            objectList = new  Grid< BaseObject >();
        }

    }
}

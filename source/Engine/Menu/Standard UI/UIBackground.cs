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

namespace Engine.UI
{
    public class UIBackground : UIImageElement
    {
        public UIBackground( String Imagename, Point Position, float layer )
            : base( Imagename, Position, layer )
        {
        }
        public UIBackground(String Imagename, Point Position)
            : this(Imagename, Position, 0.99f)
        {
        }
    }
}

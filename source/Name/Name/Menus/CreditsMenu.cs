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
using Engine.UI;

namespace Name.Menus
{
    class CreditsMenu : UIBaseMenu
    {
        public CreditsMenu( )
            : base( @"SharedContent\Credits_Background", Point.Zero )
        {
            UIButton b = new UIButton(@"Content\Menu\Back", CameraManager.GetPosition(0.025f, 0.8125f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );

        }
    }
}

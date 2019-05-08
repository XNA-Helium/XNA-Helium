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

using Engine.Core;
using Engine;
using Engine.UI;
using Name.GameState;

namespace Name.Menus
{
    public class Tracker : UIBaseMenu
    {
        BaseObject po;
        public Tracker( BaseObject o )
            : base()
        {
            po = o;
            EventManager.Instance.AddEvent( RemoveMenu, 2.75f );
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );
            if ( po.InWorld == false )
            {
                RemoveMenu();
                return;
            }
            Point point = ( po[Engine.Core.Placeable.TypeStatic] as Engine.Core.Placeable ).Position.ToPoint();
            CameraManager.Instance.Current.CenterOn( ref point );

        }

        bool removed = false;

        public override void OnRemove( )
        {
            removed = true;
            base.OnRemove();
        }

        public void RemoveMenu( )
        {
            if ( !removed )
            {
                if( MenuSystemInstance.CurrentMenu == this)
                    MenuSystemInstance.PopTopMenu();
                removed = true;
            }
        }
    }
}

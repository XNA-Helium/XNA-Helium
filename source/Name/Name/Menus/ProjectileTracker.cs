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
    public class ProjectileTracker : UIBaseMenu
    {
        IProjectile po;
        bool FirstTrigger = true;
        public ProjectileTracker( IProjectile o )
            : base()
        {
            po = o;
        }

        public override void Update( GameTime gameTime )
        {
            base.Update( gameTime );
             if ( po.InWorld == false || (po[Physics2D.TypeStatic] as Physics2D).Static)
            {
                if ( FirstTrigger )
                {
                    EventManager.Instance.AddEvent( RemoveMenu, 1.75f );
                    FirstTrigger = false;
                }
            }
            else
            {
                Point point = ( po[Engine.Core.Placeable.TypeStatic] as Engine.Core.Placeable ).Position.ToPoint();
                CameraManager.Instance.Current.CenterOn(ref point);
            }
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
                if( MenuSystemInstance.CurrentMenu is GamePlayMenu)
                    MenuSystemInstance.PopTopMenu();
                removed = true;
            }
        }
    }
}

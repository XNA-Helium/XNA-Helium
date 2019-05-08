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
using Engine.Core;

using Name.GameState;

namespace Name.Menus
{
    public class SharedGamePlayMenu : TimeDisplay
    {
        protected PlayerObject player;
        protected NameTeam Team;

        protected Texture2D MiniMap;
        protected SpriteFont sf;

        protected bool ThisRemoved = false;

        EventManager.GameEvent e;

        public SharedGamePlayMenu( PlayerObject Player, NameTeam team, double time, float TurnLength )
            : base(time,TurnLength)
        {
            Point p = Player.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref p);

            Team = team;
            sf = Engine.ContentManager.Instance.GetObject<SpriteFont>( @"SharedContent\TextboxFont" );
            player = Player;
            MiniMap = Engine.ContentManager.Instance.GetObject<Texture2D>( "MiniMap" );

            e = EventManager.Instance.AddEvent( TimesUp, TURNLENGTH );
        }

        public virtual void NoLongerInWorld( )
        {
            if ( MenuSystemInstance.CurrentMenu == this )
                MenuSystemInstance.PopTopMenu();
        }


        public override void OnRemove( )
        {
            if ( player == Team.CurrentlyActive )
            {
                Team.Next();
            }
            ThisRemoved = true;
            EventManager.Instance.RemoveEvent( e );
            base.OnRemove();
        }


        public virtual void TimesUp( )
        {
            if ( ThisRemoved )
                return;
            BaseMenu m = MenuSystemInstance.CurrentMenu;
            if ( m == this )
            {
                MenuSystemInstance.PopTopMenu();
            }
            if ( m is ProjectileTracker )
            {
                MenuSystemInstance.PopTopMenu();
                if ( MenuSystemInstance.CurrentMenu == this )
                    MenuSystemInstance.PopTopMenu();
            }
        }
    }
}
